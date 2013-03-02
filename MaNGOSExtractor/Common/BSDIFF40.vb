Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Namespace Blizzard
    Public Class Patch
        Implements IDisposable
        Private m_PTCH As PTCH
        Private m_MD5 As MD5_
        Private m_XFRM As XFRM
        Private m_BSDIFF40 As BSDIFF40
        Private m_WDBC As WDBC

        ' BSD0
        Private m_unpackedSize As UInteger
        Private m_compressedDiff As Byte()

        ' BSDIFF40
        Private m_ctrlBlock As Byte(), m_diffBlock As Byte(), m_extraBlock As Byte()

        Private m_type As String

        Public Sub New(patchFile As String)
            Using fs As New FileStream(patchFile, FileMode.Open, FileAccess.Read)
                Using br As New BinaryReader(fs)
                    m_PTCH = br.ReadStruct(Of PTCH)()
                    Debug.Assert(m_PTCH.m_magic.FourCC() = "PTCH")

                    m_MD5 = br.ReadStruct(Of MD5_)()
                    Debug.Assert(m_MD5.m_magic.FourCC() = "MD5_")

                    m_XFRM = br.ReadStruct(Of XFRM)()
                    Debug.Assert(m_XFRM.m_magic.FourCC() = "XFRM")

                    m_type = m_XFRM.m_type.FourCC()

                    Select Case m_type
                        Case "BSD0"
                            m_unpackedSize = br.ReadUInt32()
                            m_compressedDiff = br.ReadRemaining()
                            BSDIFFParse()
                            Exit Select
                        Case "COPY"
                            m_compressedDiff = br.ReadRemaining()
                            Return
                        Case Else
                            Debug.Assert(False, [String].Format("Unknown patch type: {0}", m_type))
                            Exit Select
                    End Select
                End Using
            End Using
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' TODO
        End Sub

        Public Sub PrintHeaders()
            Console.WriteLine("PTCH: patchSize {0}, sizeBefore {1}, sizeAfter {2}", m_PTCH.m_patchSize, m_PTCH.m_sizeBefore, m_PTCH.m_sizeAfter)
            Console.WriteLine("MD5_: md5BlockSize {0}" & vbLf & " md5Before {1}" & vbLf & " md5After {2}", m_MD5.m_md5BlockSize, m_MD5.m_md5Before.ToHexString(), m_MD5.m_md5After.ToHexString())
            Console.WriteLine("XFRM: xfrmBlockSize {0}, patch type: {1}", m_XFRM.m_xfrmBlockSize, m_XFRM.m_type.FourCC())
        End Sub

        Private Sub BSDIFFParseHeader(br As BinaryReader)
            m_BSDIFF40 = br.ReadStruct(Of BSDIFF40)()

            Debug.Assert(m_BSDIFF40.m_magic.FourCC() = "BSDIFF40")

            Debug.Assert(m_BSDIFF40.m_ctrlBlockSize > 0 AndAlso m_BSDIFF40.m_diffBlockSize > 0)

            Debug.Assert(m_BSDIFF40.m_sizeAfter = m_PTCH.m_sizeAfter)
        End Sub

        Private Sub BSDIFFParse()
            Dim diff() As Byte = RLEUnpack()

            Using ms As New MemoryStream(diff)
                Using br As New BinaryReader(ms)
                    BSDIFFParseHeader(br)

                    m_ctrlBlock = br.ReadBytes(CInt(m_BSDIFF40.m_ctrlBlockSize))
                    m_diffBlock = br.ReadBytes(CInt(m_BSDIFF40.m_diffBlockSize))
                    m_extraBlock = br.ReadRemaining()
                End Using
            End Using
        End Sub

        Private Function RLEUnpack() As Byte()
            Dim ret As New List(Of Byte)()

            Using ms As New MemoryStream(m_compressedDiff)
                Using br As New BinaryReader(ms, Encoding.ASCII)
                    While br.PeekChar() >= 0
                        Dim b As Byte = br.ReadByte()
                        If (b And &H80) <> 0 Then
                            ret.AddRange(br.ReadBytes((b And &H7F) + 1))
                        Else
                            ret.AddRange(New Byte(b) {})
                        End If
                    End While
                End Using
            End Using

            Debug.Assert(ret.Count = m_unpackedSize)

            Return ret.ToArray()
        End Function

        Public Sub Apply(oldFileName As String, newFileName As String, validate As Boolean)
            If m_type = "COPY" Then
                File.WriteAllBytes(newFileName, m_compressedDiff)
                Return
            End If

            Dim oldFile As Byte() = File.ReadAllBytes(oldFileName)

            If validate Then
                ' pre-validate
                Debug.Assert(oldFile.Length = m_PTCH.m_sizeBefore)
                Dim md5 As New MD5CryptoServiceProvider()
                Dim hash() As Byte = md5.ComputeHash(oldFile)
                Debug.Assert(hash.Compare(m_MD5.m_md5Before), "Input MD5 mismatch!")
            End If

            Dim ctrlBlock As BinaryReader = m_ctrlBlock.ToBinaryReader()
            Dim diffBlock As BinaryReader = m_diffBlock.ToBinaryReader()
            Dim extraBlock As BinaryReader = m_extraBlock.ToBinaryReader()

            Dim newFile As Byte() = New Byte(m_PTCH.m_sizeAfter - 1) {}

            Dim newFileOffset As Integer = 0, oldFileOffset As Integer = 0

            While newFileOffset < m_PTCH.m_sizeAfter
                Dim diffChunkSize As Integer = ctrlBlock.ReadInt32()

                Dim extraChunkSize As Integer = ctrlBlock.ReadInt32()
                Dim extraOffset As UInteger = ctrlBlock.ReadUInt32()

                Debug.Assert(newFileOffset + diffChunkSize <= m_PTCH.m_sizeAfter)

                newFile.SetBytes(diffBlock.ReadBytes(diffChunkSize), newFileOffset)

                For i As Integer = 0 To diffChunkSize - 1
                    If (oldFileOffset + i >= 0) AndAlso (oldFileOffset + i < m_PTCH.m_sizeBefore) Then
                        Dim nb As UInt32 = newFile(newFileOffset + i)
                        Dim ob As UInt32 = oldFile(oldFileOffset + i)
                        newFile(newFileOffset + i) = CByte((nb + ob) Mod 256)
                    End If
                Next

                newFileOffset += diffChunkSize
                oldFileOffset += diffChunkSize

                Debug.Assert(newFileOffset + extraChunkSize <= m_PTCH.m_sizeAfter)

                newFile.SetBytes(extraBlock.ReadBytes(extraChunkSize), newFileOffset)

                newFileOffset += extraChunkSize
                oldFileOffset += CInt(xsign(extraOffset))
            End While

            ctrlBlock.Close()
            diffBlock.Close()
            extraBlock.Close()

            If validate Then
                ' post-validate
                Debug.Assert(newFile.Length = m_PTCH.m_sizeAfter)
                Dim md5 As New MD5CryptoServiceProvider()
                Dim hash() As Byte = md5.ComputeHash(newFile)
                Debug.Assert(hash.Compare(m_MD5.m_md5After), "Output MD5 mismatch!")
            End If

            File.WriteAllBytes(newFileName, newFile)
        End Sub

        Private Shared Function xsign(i As UInteger) As Long
            If (i And &H80000000UI) <> 0 Then
                Return (CLng(&H80000000UI) - i)
            End If
            Return i
        End Function


        Public Sub ReadDBC(ByRef Filename As String)
            Dim thisRecord() As Byte

            Using fs As New FileStream(Filename, FileMode.Open, FileAccess.Read)
                Using br As New BinaryReader(fs)
                    m_WDBC = br.ReadStruct(Of WDBC)()
                    Debug.Assert(m_WDBC.m_magic.FourCC() = "WDBC")

                    'm_WDBC.m_fieldCount
                    'm_WDBC.m_recordCount
                    'm_WDBC.m_recSize
                    'm_WDBC.m_stringSize

                    For CurrentRecord As Byte = 0 To m_WDBC.m_recordCount
                        thisRecord = br.ReadBytes(m_WDBC.m_recSize)

                    Next
                End Using
            End Using
        End Sub


    End Class
End Namespace