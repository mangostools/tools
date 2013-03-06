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

        Public Function exportSQL(ByRef Filename As String) As Boolean
            Dim sqlWriter As New StreamWriter(Filename.Substring(0, Filename.Length - 4) & ".sql")

            'WriteSqlStructure(sqlWriter, ColType, Filename.Substring(0, Filename.Length - 4))

            ' Need to rethink how to do this as its too slow !!
            ' I'm thinking about trying the following: 
            ' 1) Preload everything into memory
            ' 2) Perform checks for stringtable matches
            ' 3) Replace values for stringtable columns with text
            ' 4) Write data out to SQL

            Dim m_reader As FileReader.IWowClientDBReader
            Try
                m_reader = FileReader.DBReaderFactory.GetReader(Filename)
                Dim entireRow() As Byte
                entireRow = m_reader.GetRowAsByteArray(0)
                Dim TotalRows As Integer = entireRow.Count() - 1
                Dim ColType(TotalRows / 4) As String
                'Dim ColStrings(m_reader.RecordsCount() - 1, TotalRows / 4) As String
                For rows = 0 To m_reader.RecordsCount() - 1
                    entireRow = m_reader.GetRowAsByteArray(rows)

                    Dim result As New StringBuilder()
                    result.AppendFormat("INSERT INTO `dbc_{0}` VALUES (", Path.GetFileNameWithoutExtension(Filename))

                    Dim flds As Integer = 0

                    For cols As Integer = 0 To TotalRows Step 4
                        Dim TempCol As Object '= entireRow(cols)
                        Try
                            TempCol = (entireRow(cols + 3) * 16777216) + (entireRow(cols + 2) * 65536) + (entireRow(cols + 1) * 256) + (entireRow(cols + 0))
                        Catch ex As Exception
                            TempCol = -1
                        End Try
                        'If cols / 4 = 6 Then Stop
                        ColType(cols / 4) = Core.getObjectType(TempCol, ColType(cols / 4))


                        ' Had to diable this string checking as it was finding too many entries
                        Try
                            If TempCol > 0 Then
                                If m_reader.StringTable.ContainsKey(TempCol) = True Then
                                    ColType(cols / 4) = "String"
                                    'ColStrings(rows, cols / 4) = m_reader.StringTable.ContainsValue(TempCol)
                                End If
                            End If
                        Catch
                        End Try
                        Select Case ColType(cols / 4)
                            Case "Int64"
                                result.Append(TempCol)
                                Exit Select
                            Case "Long"
                                result.Append(TempCol)
                                Exit Select
                            Case "UInt64"
                                result.Append(TempCol)
                                Exit Select
                            Case "Int32"
                                result.Append(TempCol)
                                Exit Select
                            Case "UInt32"
                                result.Append(TempCol)
                                Exit Select
                            Case "Int16"
                                result.Append(TempCol)
                                Exit Select
                            Case "UInt16"
                                result.Append(TempCol)
                                Exit Select
                            Case "SByte"
                                result.Append(TempCol)
                                Exit Select
                            Case "Byte"
                                result.Append(TempCol)
                                Exit Select
                            Case "Single", "Float"
                                result.Append(CSng(TempCol).ToString(CultureInfo.InvariantCulture))
                                Exit Select
                            Case "Double"
                                result.Append(CDbl(TempCol).ToString(CultureInfo.InvariantCulture))
                                Exit Select
                            Case "String"
                                'result.Append("""" & StripBadCharacters(DirectCast(entireRow(cols).ToString, String)) & """")
                                If m_reader.StringTable.ContainsKey(TempCol) = True Then
                                    result.Append("""" & StripBadCharacters(m_reader.StringTable.Values(TempCol)) & """")
                                    Exit Select
                                Else
                                    result.Append(TempCol)
                                    ColType(cols / 4) = "Int32"
                                    Exit Select
                                End If
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

        Private Sub WriteSqlStructure(sqlWriter As StreamWriter, ColTypes As Collection, Filename As String)
            sqlWriter.WriteLine("DROP TABLE IF EXISTS `dbc_{0}`;", Path.GetFileNameWithoutExtension(Filename))
            sqlWriter.WriteLine("CREATE TABLE `dbc_{0}` (", Path.GetFileNameWithoutExtension(Filename))

            For i As Integer = 0 To ColTypes.Count - 1
                sqlWriter.Write(vbTab & [String].Format("`{0}`", ColTypes(i).ColumnName))

                Select Case ColTypes(i).DataType.Name
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
                        Throw New Exception([String].Format("Unknown field type {0}!", ColTypes(i).DataType.Name))
                End Select

                sqlWriter.WriteLine(",")
            Next

            'For Each index As DataColumn In Data.PrimaryKey
            '    sqlWriter.WriteLine(vbTab & "PRIMARY KEY (`{0}`)", index.ColumnName)
            'Next

            sqlWriter.WriteLine(") ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='Export of {0}';", Filename)
            sqlWriter.WriteLine()
        End Sub

        Private Function StripBadCharacters(input As String) As String
            input = Regex.Replace(input, "'", "\'")
            input = Regex.Replace(input, "\""", "\""")
            Return input
        End Function
    End Module
End Namespace
