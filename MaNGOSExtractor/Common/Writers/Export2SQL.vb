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

        Public Function loadDBCtoDataTable(ByRef Filename As String, ByRef dbcDataTable As DataTable) As DataTable
            Dim m_reader As FileReader.IWowClientDBReader

            Dim entireRow() As Byte
            Dim thisRow As DataRow

            m_reader = FileReader.DBReaderFactory.GetReader(Filename)
            Try
                entireRow = m_reader.GetRowAsByteArray(0)
                Dim TotalRows As Integer = entireRow.Length - 1
                Dim ColType(TotalRows / 4) As String

                If TotalRows > 0 Then

                    Try

                        For cols As Integer = 0 To TotalRows Step 4
                            dbcDataTable.Columns.Add("Col" & (cols / 4).ToString(), GetType(String))
                        Next

                    Catch ex As Exception
                        Core.Alert(ex.Message, MaNGOSExtractorCore.runningAsGui)
                    End Try

                    Try
                        For rows = 0 To m_reader.RecordsCount() - 1
                            entireRow = m_reader.GetRowAsByteArray(rows)

                            thisRow = dbcDataTable.NewRow()
                            For cols As Integer = 0 To TotalRows Step 4
                                Dim TempCol As Object '= entireRow(cols)
                                Try
                                    If entireRow(cols + 3) > 127 Then 'And entireRow(cols + 2) = 255 And entireRow(cols + 1) = 255 And entireRow(cols + 0) = 255 Then
                                        TempCol = -1
                                    Else
                                        TempCol = (entireRow(cols + 3) * 16777216) + (entireRow(cols + 2) * 65536) + (entireRow(cols + 1) * 256) + (entireRow(cols + 0))
                                    End If
                                Catch ex As Exception
                                    TempCol = -1
                                End Try
                                thisRow(CInt(cols / 4)) = TempCol
                            Next
                            'Threading.Thread.Sleep(500)
                            dbcDataTable.Rows.Add(thisRow)
                        Next
                    Catch ex As Exception
                        Core.Alert(ex.Message, MaNGOSExtractorCore.runningAsGui)
                    End Try

                    'Create a new row at the end to store the datatype
                    thisRow = dbcDataTable.NewRow()

                    dbcDataTable.Rows.Add(thisRow)
                    Try
                        For cols As Integer = 0 To TotalRows Step 4
                            Dim blnFoundString As Boolean = True
                            For thisScanRow As Integer = 0 To dbcDataTable.Rows.Count - 1

                                If IsDBNull(dbcDataTable.Rows(thisScanRow)(CInt(cols / 4))) = False Then
                                    If m_reader.StringTable.ContainsKey(dbcDataTable.Rows(thisScanRow)(CInt(cols / 4))) = False Then
                                        blnFoundString = False
                                        'Dim testL As Long
                                        'Dim testI As Integer
                                        'Dim testD As Double
                                        'Try
                                        '    Long.TryParse(dbcDataTable.Rows(thisScanRow)(CInt(cols / 4)), testL)
                                        '    Integer.TryParse(dbcDataTable.Rows(thisScanRow)(CInt(cols / 4)), testI)
                                        '    Double.TryParse(dbcDataTable.Rows(thisScanRow)(CInt(cols / 4)), testD)
                                        'Catch ex As Exception
                                        '    Core.Alert(ex.Message, MaNGOSExtractorCore.runningAsGui)
                                        'End Try
                                        Dim strDataType As String = ""
                                        Dim strCurDataType As String = "Int32"
                                        If Not IsDBNull(dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols / 4))) Then
                                            strDataType = dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols / 4))
                                        Else
                                            strDataType = "Int32"
                                        End If
                                        Select Case strCurDataType
                                            Case "0"
                                                strDataType = "String"
                                            Case "1"
                                                strDataType = "Int32"
                                            Case "2"
                                                strDataType = "Long"
                                            Case "3"
                                                strDataType = "Float"
                                        End Select
                                        strDataType = Core.getObjectType(dbcDataTable.Rows(thisScanRow)(CInt(cols / 4)), strCurDataType)
                                        Try
                                            If strDataType = "Int32" Then 'Integer
                                                dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols / 4)) = 1
                                            ElseIf strDataType = "float" Then 'Float
                                                dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols / 4)) = 3
                                            ElseIf strDataType = "String" Then 'Float
                                                dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols / 4)) = 0
                                            Else 'Long
                                                dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols / 4)) = 2
                                            End If
                                        Catch ex As Exception
                                            Core.Alert(ex.Message, MaNGOSExtractorCore.runningAsGui)
                                        End Try
                                    End If
                                End If
                            Next

                            If blnFoundString = True Then
                                Try
                                    For thisScanRow As Integer = 0 To dbcDataTable.Rows.Count - 1
                                        dbcDataTable.Rows(thisScanRow)(CInt(cols / 4)) = m_reader.StringTable(dbcDataTable.Rows(thisScanRow)(CInt(cols / 4)))
                                        dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols / 4)) = 0
                                    Next
                                Catch ex As Exception
                                End Try
                            End If
                        Next

                    Catch ex As Exception
                        Core.Alert(ex.Message, MaNGOSExtractorCore.runningAsGui)
                    End Try
                End If

            Catch ex As Exception
                Core.Alert(ex.Message, MaNGOSExtractorCore.runningAsGui)
            End Try

            Return dbcDataTable
        End Function

        Public Function exportSQL(ByRef Filename As String, ByRef DBCDataTable As DataTable) As Boolean
            Dim intMaxRows As Integer = DBCDataTable.Rows.Count() - 1
            Dim intMaxCols As Integer = DBCDataTable.Columns.Count() - 1

            Dim sqlWriter As New StreamWriter(Filename.Substring(0, Filename.Length - 4) & ".dbc.sql")

            WriteSqlStructure(sqlWriter, DBCDataTable, Path.GetFileNameWithoutExtension(Filename.Substring(0, Filename.Length - 4)))

            Try
                'Alert("- Exporting Row:" & "+", Core.runningAsGui)
                Dim intCounterRows As Integer = (intMaxRows - 1)

                For rows = 0 To intMaxRows - 1
                    If intCounterRows Mod (intCounterRows / 100) = (intCounterRows / 100) Then Alert("+", Core.runningAsGui, True)

                    Dim result As New StringBuilder()
                    result.AppendFormat("INSERT INTO `dbc_{0}` VALUES (", Path.GetFileNameWithoutExtension(Filename))

                    Dim flds As Integer = 0

                    Try

                        For cols As Integer = 0 To intMaxCols
                            Dim thisColData As String = DBCDataTable.Rows(rows)(cols)
                            'Last Year contains the field type.. 0 = String, 1 = Int32, 2 = Long, 3 = Float
                            Select Case DBCDataTable.Rows(intMaxRows)(cols)
                                '    Case "Int64"
                                '        result.Append(TempCol)
                                '        Exit Select
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
                                    Core.Alert([String].Format("Unknown field type {0}!", thisColData), MaNGOSExtractorCore.runningAsGui)
                            End Select

                            If flds < intMaxCols Then
                                result.Append(",")
                            End If

                            flds += 1
                        Next
                    Catch ex As Exception
                        Stop
                    End Try

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

        Public Function exportSQLold(ByRef Filename As String) As Boolean
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
                        Dim TempCol As Object
                        Try
                            TempCol = (entireRow(cols + 3) * 16777216) + (entireRow(cols + 2) * 65536) + (entireRow(cols + 1) * 256) + (entireRow(cols + 0))
                        Catch ex As Exception
                            TempCol = -1
                        End Try

                        ColType(cols / 4) = Core.getObjectType(TempCol, ColType(cols / 4))
                        Try
                            If TempCol > 0 Then
                                If m_reader.StringTable.ContainsKey(TempCol) = True Then
                                    ColType(cols / 4) = "String"
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
                                If m_reader.StringTable.ContainsKey(TempCol) = True Then
                                    result.Append("""" & StripBadCharacters(m_reader.StringTable.Values(TempCol)) & """")
                                    Exit Select
                                Else
                                    result.Append(TempCol)
                                    ColType(cols / 4) = "Int32"
                                    Exit Select
                                End If
                            Case Else
                                Core.Alert([String].Format("Unknown field type {0}!", Core.getObjectType(entireRow(cols), ColType(cols))), MaNGOSExtractorCore.runningAsGui)
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

                Return True
            Catch ex As Exception
                Core.Alert(ex.Message, MaNGOSExtractorCore.runningAsGui)

                Return False
            End Try
        End Function

        'Public Sub Run(data As DataTable)
        '    Dim sqlWriter As New StreamWriter(Path.GetFileNameWithoutExtension(data.TableName) & ".sql")

        '    WriteSqlStructure(sqlWriter, data)

        '    For Each row As DataRow In data.Rows
        '        Dim result As New StringBuilder()
        '        result.AppendFormat("INSERT INTO `dbc_{0}` VALUES (", Path.GetFileNameWithoutExtension(data.TableName))

        '        Dim flds As Integer = 0

        '        For i As Integer = 0 To data.Columns.Count - 1
        '            Select Case data.Columns(i).DataType.Name
        '                Case "Int64"
        '                    result.Append(row(i))
        '                    Exit Select
        '                Case "UInt64"
        '                    result.Append(row(i))
        '                    Exit Select
        '                Case "Int32"
        '                    result.Append(row(i))
        '                    Exit Select
        '                Case "UInt32"
        '                    result.Append(row(i))
        '                    Exit Select
        '                Case "Int16"
        '                    result.Append(row(i))
        '                    Exit Select
        '                Case "UInt16"
        '                    result.Append(row(i))
        '                    Exit Select
        '                Case "SByte"
        '                    result.Append(row(i))
        '                    Exit Select
        '                Case "Byte"
        '                    result.Append(row(i))
        '                    Exit Select
        '                Case "Single"
        '                    result.Append(CSng(row(i)).ToString(CultureInfo.InvariantCulture))
        '                    Exit Select
        '                Case "Double"
        '                    result.Append(CDbl(row(i)).ToString(CultureInfo.InvariantCulture))
        '                    Exit Select
        '                Case "String"
        '                    result.Append("""" & StripBadCharacters(DirectCast(row(i), String)) & """")
        '                    Exit Select
        '                Case Else
        '                    Throw New Exception([String].Format("Unknown field type {0}!", data.Columns(i).DataType.Name))
        '            End Select

        '            If flds <> data.Columns.Count - 1 Then
        '                result.Append(", ")
        '            End If

        '            flds += 1
        '        Next

        '        result.Append(");")
        '        sqlWriter.WriteLine(result)
        '    Next

        '    sqlWriter.Flush()
        '    sqlWriter.Close()

        '    ' Finished(data.Rows.Count)
        'End Sub

        Private Sub WriteSqlStructure(sqlWriter As StreamWriter, data As DataTable, tablename As String)
            sqlWriter.WriteLine("DROP TABLE IF EXISTS `dbc_{0}`;", tablename)
            sqlWriter.WriteLine("CREATE TABLE `dbc_{0}` (", tablename)
            Dim strDataType As String = ""
            For i As Integer = 0 To data.Columns.Count - 1
                sqlWriter.Write(vbTab & [String].Format("`{0}`", data.Columns(i).ColumnName))
                strDataType = data.Rows(data.Rows.Count - 1)(i)
                Select Case strDataType 'data.Columns(i).DataType.Name
                    Case "2"    '"Int64"
                        sqlWriter.Write(" BIGINT NOT NULL DEFAULT '0'")
                        Exit Select
                        'Case "UInt64"
                        '    sqlWriter.Write(" BIGINT UNSIGNED NOT NULL DEFAULT '0'")
                        '    Exit Select
                    Case "1"  '"Int32"
                        sqlWriter.Write(" INT NOT NULL DEFAULT '0'")
                        Exit Select
                        'Case "UInt32"
                        '    sqlWriter.Write(" INT UNSIGNED NOT NULL DEFAULT '0'")
                        '    Exit Select
                        'Case "Int16"
                        '    sqlWriter.Write(" SMALLINT NOT NULL DEFAULT '0'")
                        '    Exit Select
                        'Case "UInt16"
                        '    sqlWriter.Write(" SMALLINT UNSIGNED NOT NULL DEFAULT '0'")
                        '    Exit Select
                        'Case "SByte"
                        '    sqlWriter.Write(" TINYINT NOT NULL DEFAULT '0'")
                        '    Exit Select
                        'Case "Byte"
                        '    sqlWriter.Write(" TINYINT UNSIGNED NOT NULL DEFAULT '0'")
                        '    Exit Select
                    Case "3"    '"Single"
                        sqlWriter.Write(" FLOAT NOT NULL DEFAULT '0'")
                        Exit Select
                        'Case "2"    '"Double"
                        '    sqlWriter.Write(" DOUBLE NOT NULL DEFAULT '0'")
                        '    Exit Select
                    Case "0"    '"String"
                        sqlWriter.Write(" TEXT NOT NULL")
                        Exit Select
                    Case Else
                        Throw New Exception([String].Format("Unknown field type {0}!", data.Columns(i).DataType.Name))
                End Select

                If i < data.Columns.Count - 1 Then sqlWriter.WriteLine(",")
            Next

            For Each index As DataColumn In data.PrimaryKey
                sqlWriter.WriteLine(vbTab & "PRIMARY KEY (`{0}`)", index.ColumnName)
            Next

            sqlWriter.WriteLine(")")
            sqlWriter.WriteLine(" ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci COMMENT='Export of {0}';", tablename)
            sqlWriter.WriteLine(" SET NAMES UTF8;")
            sqlWriter.WriteLine()
        End Sub


        Private Function StripBadCharacters(input As String) As String
            input = Regex.Replace(input, "'", "\'")
            input = Regex.Replace(input, "\""", "\""")
            Return input
        End Function
    End Module
End Namespace
