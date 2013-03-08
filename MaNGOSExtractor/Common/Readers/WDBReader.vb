Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text

Namespace FileReader
    Class WDBReader
        Implements IWowClientDBReader
        Private Const HeaderSize As Integer = 24
        ' creaturecache.wdb
        ' gameobjectcache.wdb
        ' itemcache.wdb
        ' itemnamecache.wdb
        ' itemtextcache.wdb
        ' npccache.wdb
        ' pagetextcache.wdb
        ' questcache.wdb
        ' wowcache.wdb
        Private WDBSigs As UInteger() = New UInteger() {&H574D4F42, &H57474F42, &H57494442, &H574E4442, &H57495458, &H574E5043, _
            &H57505458, &H57515354, &H5752444E}

        ReadOnly Property RecordsCount() As Integer Implements IWowClientDBReader.RecordsCount
            Get
                Return m_RecordsCount
            End Get
            'Private Set(value As Integer)
            '    m_RecordsCount = Value
            'End Set
        End Property
        Private m_RecordsCount As Integer
        ReadOnly Property FieldsCount() As Integer Implements IWowClientDBReader.FieldsCount
            Get
                Return m_FieldsCount
            End Get
            'Private Set(value As Integer)
            '    m_FieldsCount = value
            'End Set
        End Property
        Private m_FieldsCount As Integer
        ReadOnly Property RecordSize() As Integer Implements IWowClientDBReader.RecordSize
            Get
                Return m_RecordSize
            End Get
            'Private Set(value As Integer)
            '    m_RecordSize = value
            'End Set
        End Property
        Private m_RecordSize As Integer
        ReadOnly Property StringTableSize() As Integer Implements IWowClientDBReader.StringTableSize
            Get
                Return m_StringTableSize
            End Get
            'Private Set(value As Integer)
            '    m_StringTableSize = Value
            'End Set
        End Property
        Private m_StringTableSize As Integer

        ReadOnly Property StringTable() As Dictionary(Of Integer, String) Implements IWowClientDBReader.StringTable
            Get
                Return m_StringTable
            End Get
            'Private Set(value As Dictionary(Of Integer, String))
            '    m_StringTable = value
            'End Set
        End Property
        Private m_StringTable As Dictionary(Of Integer, String)

        Private m_rows As Dictionary(Of Integer, Byte())

        Public Function GetRowAsByteArray(row As Integer) As Byte() Implements IWowClientDBReader.GetRowAsByteArray
            Return m_rows(m_rows.ElementAt(row).Key)
        End Function

        Default Public ReadOnly Property Item(row As Integer) As BinaryReader Implements IWowClientDBReader.Item
            Get
                Return New BinaryReader(New MemoryStream(m_rows.ElementAt(row).Value), Encoding.UTF8)
            End Get
        End Property

        Public Sub New(fileName As String)
            Using reader = BinaryReaderExtensions.FromFile(fileName)
                If reader.BaseStream.Length < HeaderSize Then
                    Throw New InvalidDataException([String].Format("File {0} is corrupted!", fileName))
                End If

                Dim signature = reader.ReadUInt32()

                If Not WDBSigs.Contains(signature) Then
                    Throw New InvalidDataException([String].Format("File {0} isn't valid WDB file!", fileName))
                End If

                Dim build As UInteger = reader.ReadUInt32()
                Dim locale As UInteger = reader.ReadUInt32()
                Dim unk1 = reader.ReadInt32()
                Dim unk2 = reader.ReadInt32()
                Dim version = reader.ReadInt32()

                m_rows = New Dictionary(Of Integer, Byte())()

                Try
                    While reader.BaseStream.Position <> reader.BaseStream.Length


                        Dim entry = reader.ReadInt32()
                        Dim size = reader.ReadInt32()
                        If entry = 0 AndAlso size = 0 AndAlso reader.BaseStream.Position = reader.BaseStream.Length Then
                            Exit While
                        End If
                        Dim row = New Byte(-1) {}.Concat(BitConverter.GetBytes(entry)).Concat(reader.ReadBytes(size)).ToArray()
                        m_rows.Add(entry, row)
                    End While
                Catch ex As Exception

                End Try

                m_RecordsCount = m_rows.Count
            End Using
        End Sub
    End Class
End Namespace