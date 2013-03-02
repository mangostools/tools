Imports System.Collections.Generic
Imports System.IO
Imports System.Text

Namespace FileReader
    Class ADBReader
        Implements IWowClientDBReader
        Private Const HeaderSize As Integer = 48
        Private Const ADBFmtSig As UInteger = &H32484357
        ' WCH2
        ReadOnly Property RecordsCount() As Integer Implements IWowClientDBReader.RecordsCount
            Get
                Return m_RecordsCount
            End Get
            'Private Set(value As Integer)
            '    m_RecordsCount = value
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
            '    m_StringTableSize = value
            'End Set
        End Property
        Private m_StringTableSize As Integer

        ReadOnly Property StringTable() As Dictionary(Of Integer, String) Implements IWowClientDBReader.StringTable
            Get
                Return m_StringTable
            End Get
            'Private Set(value As Dictionary(Of Integer, String))
            '    m_StringTable = Value
            'End Set
        End Property
        Private m_StringTable As Dictionary(Of Integer, String)

        Private m_rows As Byte()()

        Public Function GetRowAsByteArray(row As Integer) As Byte() Implements IWowClientDBReader.GetRowAsByteArray
            Return m_rows(row)
        End Function

        Default Public ReadOnly Property Item(row As Integer) As BinaryReader Implements IWowClientDBReader.Item
            Get
                Return New BinaryReader(New MemoryStream(m_rows(row)), Encoding.UTF8)
            End Get
        End Property

        Public Sub New(fileName As String)
            Using reader = BinaryReaderExtensions.FromFile(fileName)
                If reader.BaseStream.Length < HeaderSize Then
                    Throw New InvalidDataException([String].Format("File {0} is corrupted!", fileName))
                End If

                Dim signature = reader.ReadUInt32()

                If signature <> ADBFmtSig Then
                    Throw New InvalidDataException([String].Format("File {0} isn't valid DBC file!", fileName))
                End If

                m_RecordsCount = reader.ReadInt32()
                m_FieldsCount = reader.ReadInt32()
                ' not fields count in WCH2
                m_RecordSize = reader.ReadInt32()
                m_StringTableSize = reader.ReadInt32()

                ' WCH2 specific fields
                Dim tableHash As UInteger = reader.ReadUInt32()
                ' new field in WCH2
                Dim build As UInteger = reader.ReadUInt32()
                ' new field in WCH2
                Dim unk1 As Integer = reader.ReadInt32()
                ' Unix time in WCH2
                Dim unk2 As Integer = reader.ReadInt32()
                ' new field in WCH2
                Dim unk3 As Integer = reader.ReadInt32()
                ' new field in WCH2 (index table?)
                Dim locale As Integer = reader.ReadInt32()
                ' new field in WCH2
                Dim unk5 As Integer = reader.ReadInt32()
                ' new field in WCH2
                If unk3 <> 0 Then
                    reader.ReadBytes(unk3 * 4 - HeaderSize)
                    ' an index for rows
                    ' a memory allocation bank
                    reader.ReadBytes(unk3 * 2 - HeaderSize * 2)
                End If

                m_rows = New Byte(RecordsCount - 1)() {}

                For i As Integer = 0 To RecordsCount - 1
                    m_rows(i) = reader.ReadBytes(RecordSize)
                Next

                Dim stringTableStart As Integer = CInt(reader.BaseStream.Position)

                m_StringTable = New Dictionary(Of Integer, String)()

                While reader.BaseStream.Position <> reader.BaseStream.Length
                    Dim index As Integer = CInt(reader.BaseStream.Position) - stringTableStart
                    StringTable(index) = reader.ReadStringNull()
                End While
            End Using
        End Sub
    End Class
End Namespace