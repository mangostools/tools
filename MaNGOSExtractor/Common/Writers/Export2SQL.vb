'Imports System.ComponentModel.Composition
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Reflection

Namespace Core
    Public Module Export2SQL
        Public Property Finished() As Action(Of Integer)
            Get
                Return m_Finished
            End Get
            Set(value As Action(Of Integer))
                m_Finished = value
            End Set
        End Property
        Private m_Finished As Action(Of Integer)


        Public Function exportSQL(ByRef Filename As String, ByRef DBCDataTable As DataTable) As Boolean
            Dim intMaxRows As Integer = DBCDataTable.Rows.Count() - 1
            Dim intMaxCols As Integer = DBCDataTable.Columns.Count() - 1

            '            Dim sqlWriter As New StreamWriter(Filename.Substring(0, Filename.Length - 4) & ".dbc.sql")
            Dim sqlWriter As New StreamWriter(Filename & ".sql")

            WriteSqlStructure(sqlWriter, DBCDataTable, Path.GetFileNameWithoutExtension(Filename.Substring(0, Filename.Length - 4)))

            Try
                Dim intCounterRows As Integer = (intMaxRows - 1)

                For rows = 0 To intMaxRows - 1
                    If intCounterRows Mod (intCounterRows / 100) = (intCounterRows / 100) Then Alert("+", True)

                    Dim result As New StringBuilder()
                    result.AppendFormat("INSERT INTO `dbc_{0}` VALUES (", Path.GetFileNameWithoutExtension(Filename))

                    Dim flds As Integer = 0

                    Try
                        For cols As Integer = 0 To intMaxCols
                            Dim thisColData As String
                            If Not IsDBNull(DBCDataTable.Rows(rows)(cols)) Then
                                thisColData = DBCDataTable.Rows(rows)(cols)
                            Else
                                thisColData = "1"
                            End If

                            Dim thisLastColData As String
                            If Not IsDBNull(DBCDataTable.Rows(intMaxRows)(cols)) Then
                                thisLastColData = DBCDataTable.Rows(intMaxRows)(cols)
                            Else
                                thisLastColData = "1"
                            End If

                            'Last Year contains the field type.. 0 = String, 1 = Int32, 2 = Long, 3 = Float
                            Select Case thisLastColData
                                Case "2"    '"Long"
                                    result.Append(thisColData)
                                    Exit Select
                                Case "1"    '"Int32"
                                    result.Append(thisColData)
                                    Exit Select
                                Case "3"    '"Single", "Float"
                                    result.Append(CSng(thisColData).ToString(CultureInfo.InvariantCulture))
                                    Exit Select
                                Case "0"    '"String"
                                    result.Append("""" & StripBadCharacters(DirectCast(thisColData.ToString, String)) & """")
                                    Exit Select
                                Case Else
                                    Core.Alert([String].Format("Unknown field type {0}!", thisColData), False)
                            End Select

                            If flds < intMaxCols Then
                                result.Append(",")
                            End If

                            flds += 1
                        Next
                    Catch ex As Exception
                        Alert(ex.Message & " - 1", False)
                    End Try

                    result.Append(");")
                    sqlWriter.WriteLine(result)
                    Threading.Thread.Sleep(0)
                Next

                sqlWriter.Flush()
                sqlWriter.Close()

                Return True
            Catch ex As Exception
                Alert(ex.Message & " - 2", False)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Generates the SQL File header to drop the existing table and recreate it
        ''' </summary>
        ''' <param name="sqlWriter"></param>
        ''' <param name="data"></param>
        ''' <param name="tablename"></param>
        ''' <remarks></remarks>
        Private Sub WriteSqlStructure(sqlWriter As StreamWriter, data As DataTable, tablename As String)
            sqlWriter.WriteLine("DROP TABLE IF EXISTS `dbc_{0}`;", tablename)

            If data.Rows.Count() > 0 Then
                sqlWriter.WriteLine("CREATE TABLE `dbc_{0}` (", tablename)
                Dim strDataType As String = ""
                For i As Integer = 0 To data.Columns.Count - 1
                    sqlWriter.Write(vbTab & [String].Format("`{0}`", data.Columns(i).ColumnName))
                    If Not IsDBNull(data.Rows(data.Rows.Count - 1)(i)) Then
                        strDataType = data.Rows(data.Rows.Count - 1)(i)
                    Else
                        strDataType = "1"
                    End If

                    Select Case strDataType
                        Case "2"    '"Int64"
                            sqlWriter.Write(" BIGINT NOT NULL DEFAULT '0'")
                            Exit Select
                        Case "1"  '"Int32"
                            sqlWriter.Write(" INT NOT NULL DEFAULT '0'")
                            Exit Select
                        Case "3"    '"Single"
                            sqlWriter.Write(" FLOAT NOT NULL DEFAULT '0'")
                            Exit Select
                        Case "0"    '"String"
                            sqlWriter.Write(" TEXT NOT NULL")
                            Exit Select
                        Case Else
                            Alert("Unknown field type " & data.Columns(i).DataType.Name & "!", False)
                    End Select

                    If i < data.Columns.Count - 1 Then sqlWriter.WriteLine(",")
                Next

                For Each index As DataColumn In data.PrimaryKey
                    sqlWriter.WriteLine(vbTab & "PRIMARY KEY (`{0}`)", index.ColumnName)
                Next

                sqlWriter.WriteLine(")")
                sqlWriter.WriteLine(" ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci COMMENT='Export of {0}';", tablename)
                sqlWriter.WriteLine(" SET NAMES UTF8;")
            End If

            sqlWriter.WriteLine()
        End Sub

    End Module
End Namespace
