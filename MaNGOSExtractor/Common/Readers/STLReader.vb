Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.IO
Imports System.Runtime.InteropServices

Namespace FileReader
    Structure MPQHeader
        ' sizeof 0x10
        Public mpqMagicNumber As UInteger
        ' MPQ file magic number: 0xDEADBEEF
        Public fileTypeId As UInteger
        ' file type or version id (same for all *.gam files)
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> _
        Public unused As UInteger()
        ' always 0x00000000
    End Structure

    Structure StlHeader
        ' sizeof 0x28
        Public stlFileId As UInteger
        ' Stl file Id
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=5)> _
        Public unknown1 As UInteger()
        ' always 0x00000000
        Public headerSize As UInteger
        ' size (in bytes) of the StlHeader? (always 0x00000028)
        Public entriesSize As Integer
        ' size (in bytes) of the StlEntries
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> _
        Public unknown2 As UInteger()
        ' always 0x00000000
    End Structure

    Structure StlEntry
        ' sizeof 0x50
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> _
        Public unknown1 As UInteger()
        ' always 0x00000000
        Public string1offset As UInteger
        ' file offset for string1 (non-NLS key)
        Public string1size As UInteger
        ' size of string1
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> _
        Public unknown2 As UInteger()
        ' always 0x00000000
        Public string2offset As UInteger
        ' file offset for string2
        Public string2size As UInteger
        ' size of string2
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> _
        Public unknown3 As UInteger()
        ' always 0x00000000
        Public string3offset As UInteger
        ' file offset for string3
        Public string3size As UInteger
        ' size of string3
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=2)> _
        Public unknown4 As UInteger()
        ' always 0x00000000
        Public string4offset As UInteger
        ' file offset for string4
        Public string4size As UInteger
        ' size of string4
        Public unknown5 As UInteger
        ' always 0xFFFFFFFF
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> _
        Public unknown6 As UInteger()
        ' always 0x00000000
    End Structure

    Class STLReader
        Implements IWowClientDBReader
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
            '    m_StringTable = value
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

        Private reader As BinaryReader

        Public Sub New(fileName As String)
            reader = BinaryReaderExtensions.FromFile(fileName)

            Dim mHdr As MPQHeader = reader.ReadStruct(Of MPQHeader)()
            Dim sHdr As StlHeader = reader.ReadStruct(Of StlHeader)()
            'StlEntry sEntry = reader.ReadStruct<StlEntry>();

            m_RecordsCount = sHdr.entriesSize \ &H50

            m_rows = New Byte(RecordsCount - 1)() {}

            For i As Integer = 0 To RecordsCount - 1
                m_rows(i) = reader.ReadBytes(&H50)

                'StringTable = new Dictionary<int, string>();

                'if (reader.BaseStream.Position != reader.BaseStream.Length)
                '{
                '    while (reader.BaseStream.Position != reader.BaseStream.Length)
                '    {
                '        if (reader.PeekChar() == 0)
                '        {
                '            reader.BaseStream.Position++;
                '            continue;
                '        }

                '        int offset = (int)reader.BaseStream.Position;
                '        StringTable[offset] = reader.ReadStringNull();
                '    }
                '}
            Next
        End Sub

        Public Function ReadString(offset As Integer) As String
            reader.BaseStream.Position = offset

            While reader.PeekChar() = 0
                reader.BaseStream.Position += 1
            End While

            Return reader.ReadStringNull()
            '    while (reader.BaseStream.Position != reader.BaseStream.Length)
            '    {
            '        if (reader.PeekChar() == 0)
            '        {
            '            reader.BaseStream.Position++;
            '            continue;
            '        }

            '        int offset = (int)reader.BaseStream.Position;
            '        StringTable[offset] = reader.ReadStringNull();
            '    }
        End Function
    End Class
End Namespace