Imports System.Collections.Generic
Imports System.IO

Namespace FileReader
    Interface IWowClientDBReader
        ReadOnly Property RecordsCount As Integer
        ReadOnly Property FieldsCount As Integer
        ReadOnly Property RecordSize As Integer
        ReadOnly Property StringTableSize As Integer
        ReadOnly Property StringTable() As Dictionary(Of Integer, String)
        Function GetRowAsByteArray(row As Integer) As Byte()
        Default ReadOnly Property Item(row As Integer) As BinaryReader
    End Interface
End Namespace
