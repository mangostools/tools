'Imports System.ComponentModel.Composition
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Reflection

Namespace Core
    Public Module Export2MD
        Public Property Finished() As Action(Of Integer)
            Get
                Return m_Finished
            End Get
            Set(value As Action(Of Integer))
                m_Finished = value
            End Set
        End Property
        Private m_Finished As Action(Of Integer)


        Public Function exportMD(ByRef Filename As String, ByRef DBCDataTable As DataTable, ByRef sourceFolder As String) As Boolean
            Dim intMaxRows As Integer = DBCDataTable.Rows.Count() - 1
            Dim intMaxCols As Integer = DBCDataTable.Columns.Count() - 1

            '            Dim sqlWriter As New StreamWriter(Filename.Substring(0, Filename.Length - 4) & ".dbc.sql")
            Dim sqlWriter As New StreamWriter(Path.GetDirectoryName(Filename) & "\dbc_" & Path.GetFileNameWithoutExtension(Filename) & "_" & Core.returnMangosCoreVersion & ".md")

            WriteMDStructure(sqlWriter, DBCDataTable, Path.GetFileNameWithoutExtension(Filename.Substring(0, Filename.Length - 4)), sourceFolder)

            Try
                'Dim intCounterRows As Integer = (intMaxRows - 1)
                'If intMaxCols > 0 Then
                '    '    For rows = 0 To intMaxRows - 1
                '    '        'If intCounterRows Mod (intCounterRows / 100) = (intCounterRows / 100) Then Alert("+", Core.AlertNewLine.NoCRLF)

                '    '        Dim result As New StringBuilder()
                '    '        result.AppendFormat("INSERT INTO `dbc_{0}` VALUES (", Path.GetFileNameWithoutExtension(Filename))

                '    Dim flds As Integer = 0

                '    Try
                '        For cols As Integer = 0 To intMaxCols
                '            Dim thisColData As String
                '            If Not IsDBNull(DBCDataTable.Rows(0)(cols)) Then
                '                thisColData = DBCDataTable.Rows(0)(cols)
                '            Else
                '                thisColData = "1"
                '            End If

                '            Dim thisLastColData As String
                '            If Not IsDBNull(DBCDataTable.Rows(intMaxRows)(cols)) Then
                '                thisLastColData = DBCDataTable.Rows(intMaxRows)(cols)
                '            Else
                '                thisLastColData = "1"
                '            End If

                '            sqlWriter.WriteLine("<td>")
                '            'Last Year contains the field type.. 0 = String, 1 = Int32, 2 = Long, 3 = Float
                '            Select Case thisLastColData
                '                Case "2"    '"Long"
                '                    sqlWriter.WriteLine(thisColData)
                '                    Exit Select
                '                Case "1"    '"Int32"
                '                    sqlWriter.WriteLine(thisColData)
                '                    Exit Select
                '                Case "3"    '"Single", "Float"
                '                    sqlWriter.WriteLine(CSng(thisColData).ToString(CultureInfo.InvariantCulture))
                '                    Exit Select
                '                Case "0"    '"String"
                '                    sqlWriter.WriteLine("""" & StripBadCharacters(DirectCast(thisColData.ToString, String)) & """")
                '                    Exit Select
                '                Case Else
                '                    Core.Alert([String].Format("Unknown field type {0}!", thisColData), Core.AlertNewLine.AddCRLF)
                '            End Select

                '            flds += 1
                '        Next
                '    Catch ex As Exception
                '        Alert(ex.Message & " - 1", Core.AlertNewLine.AddCRLF)
                '    End Try

                '        result.Append(");")
                '        sqlWriter.WriteLine(result)
                '        Threading.Thread.Sleep(0)
                '    Next
                'End If
                sqlWriter.Flush()
                sqlWriter.Close()

                Return True
            Catch ex As Exception
                Alert(ex.Message & " - 2", Core.AlertNewLine.AddCRLF)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Generates the Wiki MD File header page
        ''' </summary>
        ''' <param name="sqlWriter"></param>
        ''' <param name="data"></param>
        ''' <param name="tablename"></param>
        ''' <remarks></remarks>
        Private Sub WriteMDStructure(sqlWriter As StreamWriter, data As DataTable, tablename As String, ByRef sourceFolder As String)
            sqlWriter.WriteLine("Back to [[Known DBC Files|Dbc-files]] summary")
            sqlWriter.WriteLine("##### Description of the DBC file " & tablename & " for v" & Core.FullVersion & " (Build " & Core.BuildNo & ")")
            sqlWriter.WriteLine("")
            sqlWriter.Write("Other Versions: ")
            sqlWriter.Write(" [[*MaNGOSZero*|dbc_" & tablename & "_MaNGOSZero]] ")
            sqlWriter.Write(" [[*MaNGOSOne*|dbc_" & tablename & "_MaNGOSOne]] ")
            sqlWriter.Write(" [[*MaNGOSTwo*|dbc_" & tablename & "_MaNGOSTwo]] ")
            sqlWriter.Write(" [[*MaNGOSThree*|dbc_" & tablename & "_MaNGOSThree]] ")
            sqlWriter.Write(" [[*MaNGOSFour*|dbc_" & tablename & "_MaNGOSFour]] ")
            sqlWriter.WriteLine("")
            sqlWriter.WriteLine("<p>The purpose of this file needs to be documented</p>")
            sqlWriter.WriteLine("")

            If data.Rows.Count() >= 0 Then
                sqlWriter.WriteLine("##### The Field definitions follow, No. of columns: {0}", data.Columns.Count())
                Dim strDataType As String = ""
                Dim blnOverrideOk As Boolean = False
                Dim ColumnNameOverride As New Dictionary(Of Integer, String)
                ColumnNameOverride = LoadXMLDefinitions(sourceFolder, tablename)
                If ColumnNameOverride.Count() = data.Columns.Count() Then blnOverrideOk = True
                sqlWriter.WriteLine("<table border='1' cellpadding='5' cellspacing='0'>")
                sqlWriter.WriteLine("<tr bgcolor='#dedede'>")
                sqlWriter.WriteLine("<th>Name</th>")
                sqlWriter.WriteLine("<th>Type</th>")
                sqlWriter.WriteLine("<th>Include</th>")
                sqlWriter.WriteLine("<th>Comments</th>")
                sqlWriter.WriteLine("</tr>")
                For i As Integer = 0 To data.Columns.Count - 1
                    sqlWriter.WriteLine("<tr>")
                    If blnOverrideOk = False Then
                        sqlWriter.WriteLine("<td>{0}</td>", data.Columns(i).ColumnName)
                    Else
                        sqlWriter.WriteLine("<td>{0}</td>", ColumnNameOverride(i).ToString)
                    End If

                    If Not IsDBNull(data.Rows(data.Rows.Count - 1)(i)) Then
                        strDataType = data.Rows(data.Rows.Count - 1)(i)
                    Else
                        strDataType = "1"
                    End If

                    Select Case strDataType
                        Case "2"    '"Int64"
                            sqlWriter.WriteLine("<td align='center'>BIGINT</td>")
                            Exit Select
                        Case "1"  '"Int32"
                            sqlWriter.WriteLine("<td align='center'>INT</td>")
                            Exit Select
                        Case "3"    '"Single"
                            sqlWriter.WriteLine("<td align='center'>FLOAT</td>")
                            Exit Select
                        Case "0"    '"String"
                            sqlWriter.WriteLine("<td align='center'>TEXT</td>")
                            Exit Select
                        Case Else
                            sqlWriter.WriteLine("<td align='center'>INT</td>")
                            Exit Select
                    End Select
                    sqlWriter.WriteLine("<td align='center'>Y</td>")
                    sqlWriter.WriteLine("<td align='center'>&nbsp;</td>")
                    sqlWriter.WriteLine("</tr>")

                Next
                sqlWriter.WriteLine("</table>")
                sqlWriter.WriteLine("###### auto-generated by MaNGOSExtractor") ' by the Mangos Foundation")
            End If
            sqlWriter.WriteLine()
        End Sub

    End Module
End Namespace
