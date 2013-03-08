Imports System.IO

Namespace FileReader
    Class DBReaderFactory
        Public Shared Function GetReader(file As String) As IWowClientDBReader
            Dim reader As IWowClientDBReader

            Dim ext = Path.GetExtension(file).ToUpperInvariant()
            If ext = ".DBC" Then
                reader = New DBCReader(file)
            ElseIf ext = ".DB2" Then
                reader = New DB2Reader(file)
            ElseIf ext = ".ADB" Then
                reader = New ADBReader(file)
            ElseIf ext = ".WDB" Then
                reader = New WDBReader(file)
            ElseIf ext = ".STL" Then
                reader = New STLReader(file)
            Else
                reader = Nothing
                'Throw New InvalidDataException([String].Format("Unknown file type {0}", ext))
            End If

            Return reader
        End Function
    End Class
End Namespace