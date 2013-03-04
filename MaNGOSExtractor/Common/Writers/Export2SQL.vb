'Imports System.ComponentModel.Composition
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

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

        Public Function exportSQL(ByRef Filename As String) As Boolean
            Dim sqlWriter As New StreamWriter(Filename.Substring(0, Filename.Length - 4) & ".sql")

            'WriteSqlStructure(sqlWriter, Data)

            Dim m_reader As FileReader.IWowClientDBReader
            Try
                m_reader = FileReader.DBReaderFactory.GetReader(Filename)
                Dim entireRow() As Byte
                entireRow = m_reader.GetRowAsByteArray(0)
                Dim ColType(entireRow.Count()) As String
                For rows = 0 To m_reader.RecordsCount() - 1
                    entireRow = m_reader.GetRowAsByteArray(rows)

                    Dim result As New StringBuilder()
                    result.AppendFormat("INSERT INTO `dbc_{0}` VALUES (", Path.GetFileNameWithoutExtension(Filename))

                    Dim flds As Integer = 0

                    For cols As Integer = 0 To entireRow.Count() - 1

                        ColType(cols) = Core.getObjectType(entireRow(cols), ColType(cols))
                        Try
                            If m_reader.StringTable(((entireRow.Count() - 1) * rows) + cols).Trim.Length > 0 Then
                                ColType(cols) = "String"
                            End If
                        Catch
                        End Try
                        Select Case ColType(cols)
                            Case "Int64"
                                result.Append(entireRow(cols))
                                Exit Select
                            Case "UInt64"
                                result.Append(entireRow(cols))
                                Exit Select
                            Case "Int32"
                                result.Append(entireRow(cols))
                                Exit Select
                            Case "UInt32"
                                result.Append(entireRow(cols))
                                Exit Select
                            Case "Int16"
                                result.Append(entireRow(cols))
                                Exit Select
                            Case "UInt16"
                                result.Append(entireRow(cols))
                                Exit Select
                            Case "SByte"
                                result.Append(entireRow(cols))
                                Exit Select
                            Case "Byte"
                                result.Append(entireRow(cols))
                                Exit Select
                            Case "Single"
                                result.Append(CSng(entireRow(cols)).ToString(CultureInfo.InvariantCulture))
                                Exit Select
                            Case "Double"
                                result.Append(CDbl(entireRow(cols)).ToString(CultureInfo.InvariantCulture))
                                Exit Select
                            Case "String"
                                'result.Append("""" & StripBadCharacters(DirectCast(entireRow(cols).ToString, String)) & """")
                                Try
                                    result.Append("""" & StripBadCharacters(m_reader.StringTable(((entireRow.Count() - 1) * rows) + cols)) & """")
                                    Exit Select
                                Catch
                                    result.Append("""""""")
                                    Exit Select
                                End Try
                            Case Else
                                Throw New Exception([String].Format("Unknown field type {0}!", Core.getObjectType(entireRow(cols), ColType(cols))))
                        End Select

                        If flds <> entireRow.Count() - 1 Then
                            result.Append(", ")
                        End If

                        flds += 1
                    Next

                    result.Append(");")
                    sqlWriter.WriteLine(result)

                Next

                sqlWriter.Flush()
                sqlWriter.Close()

                '    For cols = 0 To entireRow.Count() - 1
                '        Console.Write(ColType(cols))
                '    Next
                '    Console.Write(vbCrLf)

                Return True
            Catch ex As Exception
                'MessageBox.Show(ex.Message)
                'e.Cancel = True
                Return False
            End Try
        End Function


        Public Sub Run(data As DataTable)
            Dim sqlWriter As New StreamWriter(Path.GetFileNameWithoutExtension(data.TableName) & ".sql")

            WriteSqlStructure(sqlWriter, data)

            For Each row As DataRow In data.Rows
                Dim result As New StringBuilder()
                result.AppendFormat("INSERT INTO `dbc_{0}` VALUES (", Path.GetFileNameWithoutExtension(data.TableName))

                Dim flds As Integer = 0

                For i As Integer = 0 To data.Columns.Count - 1
                    Select Case data.Columns(i).DataType.Name
                        Case "Int64"
                            result.Append(row(i))
                            Exit Select
                        Case "UInt64"
                            result.Append(row(i))
                            Exit Select
                        Case "Int32"
                            result.Append(row(i))
                            Exit Select
                        Case "UInt32"
                            result.Append(row(i))
                            Exit Select
                        Case "Int16"
                            result.Append(row(i))
                            Exit Select
                        Case "UInt16"
                            result.Append(row(i))
                            Exit Select
                        Case "SByte"
                            result.Append(row(i))
                            Exit Select
                        Case "Byte"
                            result.Append(row(i))
                            Exit Select
                        Case "Single"
                            result.Append(CSng(row(i)).ToString(CultureInfo.InvariantCulture))
                            Exit Select
                        Case "Double"
                            result.Append(CDbl(row(i)).ToString(CultureInfo.InvariantCulture))
                            Exit Select
                        Case "String"
                            result.Append("""" & StripBadCharacters(DirectCast(row(i), String)) & """")
                            Exit Select
                        Case Else
                            Throw New Exception([String].Format("Unknown field type {0}!", data.Columns(i).DataType.Name))
                    End Select

                    If flds <> data.Columns.Count - 1 Then
                        result.Append(", ")
                    End If

                    flds += 1
                Next

                result.Append(");")
                sqlWriter.WriteLine(result)
            Next

            sqlWriter.Flush()
            sqlWriter.Close()

            ' Finished(data.Rows.Count)
        End Sub

        Private Sub WriteSqlStructure(sqlWriter As StreamWriter, data As DataTable)
            sqlWriter.WriteLine("DROP TABLE IF EXISTS `dbc_{0}`;", Path.GetFileNameWithoutExtension(data.TableName))
            sqlWriter.WriteLine("CREATE TABLE `dbc_{0}` (", Path.GetFileNameWithoutExtension(data.TableName))

            For i As Integer = 0 To data.Columns.Count - 1
                sqlWriter.Write(vbTab & [String].Format("`{0}`", data.Columns(i).ColumnName))

                Select Case data.Columns(i).DataType.Name
                    Case "Int64"
                        sqlWriter.Write(" BIGINT NOT NULL DEFAULT '0'")
                        Exit Select
                    Case "UInt64"
                        sqlWriter.Write(" BIGINT UNSIGNED NOT NULL DEFAULT '0'")
                        Exit Select
                    Case "Int32"
                        sqlWriter.Write(" INT NOT NULL DEFAULT '0'")
                        Exit Select
                    Case "UInt32"
                        sqlWriter.Write(" INT UNSIGNED NOT NULL DEFAULT '0'")
                        Exit Select
                    Case "Int16"
                        sqlWriter.Write(" SMALLINT NOT NULL DEFAULT '0'")
                        Exit Select
                    Case "UInt16"
                        sqlWriter.Write(" SMALLINT UNSIGNED NOT NULL DEFAULT '0'")
                        Exit Select
                    Case "SByte"
                        sqlWriter.Write(" TINYINT NOT NULL DEFAULT '0'")
                        Exit Select
                    Case "Byte"
                        sqlWriter.Write(" TINYINT UNSIGNED NOT NULL DEFAULT '0'")
                        Exit Select
                    Case "Single"
                        sqlWriter.Write(" FLOAT NOT NULL DEFAULT '0'")
                        Exit Select
                    Case "Double"
                        sqlWriter.Write(" DOUBLE NOT NULL DEFAULT '0'")
                        Exit Select
                    Case "String"
                        sqlWriter.Write(" TEXT NOT NULL")
                        Exit Select
                    Case Else
                        Throw New Exception([String].Format("Unknown field type {0}!", data.Columns(i).DataType.Name))
                End Select

                sqlWriter.WriteLine(",")
            Next

            For Each index As DataColumn In data.PrimaryKey
                sqlWriter.WriteLine(vbTab & "PRIMARY KEY (`{0}`)", index.ColumnName)
            Next

            sqlWriter.WriteLine(") ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='Export of {0}';", data.TableName)
            sqlWriter.WriteLine()
        End Sub

        Private Function StripBadCharacters(input As String) As String
            input = Regex.Replace(input, "'", "\'")
            input = Regex.Replace(input, "\""", "\""")
            Return input
        End Function
    End Module
End Namespace
