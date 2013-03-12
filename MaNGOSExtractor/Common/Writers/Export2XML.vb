'Imports System.ComponentModel.Composition
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports System.Xml
Imports System.Xml.Linq

Namespace Core
    Public Module Export2XML
        Public Property Finished() As Action(Of Integer)
            Get
                Return m_Finished
            End Get
            Set(value As Action(Of Integer))
                m_Finished = value
            End Set
        End Property
        Private m_Finished As Action(Of Integer)


        Public Function exportXML(ByRef SourceFolder As String, ByRef tableName As String, ByRef DBCDataTable As DataTable) As Boolean
            Dim intMaxRows As Integer = DBCDataTable.Rows.Count() - 1
            Dim intMaxCols As Integer = DBCDataTable.Columns.Count() - 1

            'Dim sqlWriter As New StreamWriter(Filename.Substring(0, Filename.Length - 4) & ".dbc.sql")
            'Dim sqlWriter As New StreamWriter(Filename & ".xml")
            Dim myXMLDoc As New Xml.XmlDocument()
            Dim XMLFilename As String = ""
            XMLFilename = returnDBCXMLfilename()

            If My.Computer.FileSystem.FileExists(SourceFolder & "\" & XMLFilename) = True Then
                myXMLDoc.Load(SourceFolder & "\" & XMLFilename)
                'Alert(" External XML Definitions used", Core.AlertNewLine.AddCRLF)
            Else
                Dim _textStreamReader As StreamReader
                Dim _assembly As [Assembly]
                Try
                    _assembly = [Assembly].GetExecutingAssembly()
                    _textStreamReader = New StreamReader(_assembly.GetManifestResourceStream(_assembly.GetName.Name.ToString() & "." & XMLFilename))
                    myXMLDoc.Load(_textStreamReader)
                    'Alert(" Internal XML Definitions used", Core.AlertNewLine.AddCRLF)
                Catch ex As Exception
                End Try
            End If

            WriteXMLStructure(myXMLDoc, DBCDataTable, Path.GetFileNameWithoutExtension(tableName.Substring(0, tableName.Length - 4)), XMLFilename)

            'Try
            '    Dim intCounterRows As Integer = (intMaxRows - 1)

            '    For rows = 0 To intMaxRows - 1
            '        If intCounterRows Mod (intCounterRows / 100) = (intCounterRows / 100) Then Alert("+", Core.runningAsGui, True)

            '        Dim result As New StringBuilder()
            '        result.AppendFormat("INSERT INTO `dbc_{0}` VALUES (", Path.GetFileNameWithoutExtension(Filename))

            '        Dim flds As Integer = 0

            '        Try
            '            For cols As Integer = 0 To intMaxCols
            '                Dim thisColData As String
            '                If Not IsDBNull(DBCDataTable.Rows(rows)(cols)) Then
            '                    thisColData = DBCDataTable.Rows(rows)(cols)
            '                Else
            '                    thisColData = "1"
            '                End If

            '                Dim thisLastColData As String
            '                If Not IsDBNull(DBCDataTable.Rows(intMaxRows)(cols)) Then
            '                    thisLastColData = DBCDataTable.Rows(intMaxRows)(cols)
            '                Else
            '                    thisLastColData = "1"
            '                End If

            '                'Last Year contains the field type.. 0 = String, 1 = Int32, 2 = Long, 3 = Float
            '                Select Case thisLastColData
            '                    Case "2"    '"Long"
            '                        result.Append(thisColData)
            '                        Exit Select
            '                    Case "1"    '"Int32"
            '                        result.Append(thisColData)
            '                        Exit Select
            '                    Case "3"    '"Single", "Float"
            '                        result.Append(CSng(thisColData).ToString(CultureInfo.InvariantCulture))
            '                        Exit Select
            '                    Case "0"    '"String"
            '                        result.Append("""" & StripBadCharacters(DirectCast(thisColData.ToString, String)) & """")
            '                        Exit Select
            '                    Case Else
            '                        Core.Alert([String].Format("Unknown field type {0}!", thisColData), MaNGOSExtractorCore.runningAsGui)
            '                End Select

            '                If flds < intMaxCols Then
            '                    result.Append(",")
            '                End If

            '                flds += 1
            '            Next
            '        Catch ex As Exception
            '            Alert(ex.Message & " - 1", Core.runningAsGui)
            '        End Try

            '        result.Append(");")
            '        sqlWriter.WriteLine(result)
            '        Threading.Thread.Sleep(0)
            '    Next

            '    sqlWriter.Flush()
            '    sqlWriter.Close()

            '    Return True
            'Catch ex As Exception
            '    Alert(ex.Message & " - 2", Core.runningAsGui)
            '    Return False
            'End Try
            Return True
        End Function

        ''' <summary>
        ''' Generates the SQL File header to drop the existing table and recreate it
        ''' </summary>
        ''' <param name="sqlWriter"></param>
        ''' <param name="data"></param>
        ''' <param name="tablename"></param>
        ''' <remarks></remarks>
        Private Sub WriteXMLStructure(myXML As Xml.XmlDocument, data As DataTable, tablename As String, ByRef XMLFilename As String)
            'Check whether the tablename exists in the XML
            'If it does, bail out and dont do anything

            'If it doesnt exist, need to create node and add it

            Dim thisNode As Xml.XmlNode
            Dim thisSubNode As Xml.XmlNode
            '<root>
            '    <Files>
            '        <Achievement>
            thisNode = myXML.SelectSingleNode("root")
            Alert("", Core.AlertNewLine.AddCRLF)
            If Not IsNothing(thisNode) Then
                thisNode = thisNode.SelectSingleNode("Files")
                If Not IsNothing(thisNode) Then
                    thisSubNode = thisNode.SelectSingleNode(tablename)
                    If IsNothing(thisSubNode) Then 'Not found, time to create it
                        Dim newNode As Xml.XmlElement
                        Dim newIncludeNode As Xml.XmlElement
                        Dim newtableNode As Xml.XmlElement
                        Dim newfieldNode As Xml.XmlElement
                        Dim newtableNameNode As Xml.XmlElement
                        Dim maxCols As Integer = data.Columns.Count - 1
                        Dim intMaxRows As Integer = data.Rows.Count() - 1
                        If intMaxRows = -1 Then intMaxRows = 0

                        If maxCols > 0 And intMaxRows > 0 Then
                            newtableNode = myXML.CreateNode(XmlNodeType.Element, tablename, "")

                            newIncludeNode = myXML.CreateNode(XmlNodeType.Element, "include", "")
                            newIncludeNode.InnerText = "Y"
                            newtableNode.AppendChild(newIncludeNode)

                            newtableNameNode = myXML.CreateNode(XmlNodeType.Element, "tablename", "")
                            newtableNameNode.InnerText = "dbc_" & tablename

                            newtableNode.AppendChild(newtableNameNode)

                            newNode = myXML.CreateNode(XmlNodeType.Element, "fieldcount", "")
                            newNode.InnerText = maxCols + 1
                            newtableNode.AppendChild(newNode)

                            For allColumns As Integer = 0 To maxCols
                                Dim thisLastColData As String
                                If Not IsDBNull(data.Rows(intMaxRows)(allColumns)) Then
                                    thisLastColData = data.Rows(intMaxRows)(allColumns)
                                Else
                                    thisLastColData = "1"
                                End If

                                Dim OutfieldType As String = ""
                                'Last Year contains the field type.. 0 = String, 1 = Int32, 2 = Long, 3 = Float
                                Select Case thisLastColData
                                    Case "2"    '"Long"
                                        OutfieldType = "bigint"
                                        Exit Select
                                    Case "1"    '"Int32"
                                        OutfieldType = "int"
                                        Exit Select
                                    Case "3"    '"Single", "Float"
                                        OutfieldType = "float"
                                        Exit Select
                                    Case "0"    '"String"
                                        OutfieldType = "text"
                                        Exit Select
                                    Case Else
                                        Core.Alert([String].Format("Unknown field type {0}!", thisLastColData), Core.AlertNewLine.AddCRLF)
                                End Select


                                newfieldNode = myXML.CreateNode(XmlNodeType.Element, "field", "")
                                newfieldNode.SetAttribute("type", OutfieldType)
                                newfieldNode.SetAttribute("name", "col" & allColumns.ToString())
                                newfieldNode.SetAttribute("include", "y")

                                newtableNode.AppendChild(newfieldNode)
                                '       thisNode.AppendChild(newtableNode)
                            Next


                            thisNode.AppendChild(newtableNode)
                            'For Each tables As XmlNode In thisNode.ChildNodes
                            myXML.Save(XMLFilename) '"D:\wtt\TEST.XML")


                            '    Alert(tables.Name, False)
                            'Next


                            '	    <index>
                            '	      <primary>id</primary>
                            '	    </index>
                            '            <include>Y</include>
                            '            <tablename>dbc_Achievement</tablename>
                            '            <fieldcount>15</fieldcount>
                            '            <field type="bigint" name="id" include="y" />




                            'If data.Rows.Count() > 0 Then
                            '    sqlWriter.WriteLine("CREATE TABLE `dbc_{0}` (", tablename)
                            '    Dim strDataType As String = ""
                            '    For i As Integer = 0 To data.Columns.Count - 1
                            '        sqlWriter.Write(vbTab & [String].Format("`{0}`", data.Columns(i).ColumnName))
                            '        If Not IsDBNull(data.Rows(data.Rows.Count - 1)(i)) Then
                            '            strDataType = data.Rows(data.Rows.Count - 1)(i)
                            '        Else
                            '            strDataType = "1"
                            '        End If

                            '        Select Case strDataType
                            '            Case "2"    '"Int64"
                            '                sqlWriter.Write(" BIGINT NOT NULL DEFAULT '0'")
                            '                Exit Select
                            '            Case "1"  '"Int32"
                            '                sqlWriter.Write(" INT NOT NULL DEFAULT '0'")
                            '                Exit Select
                            '            Case "3"    '"Single"
                            '                sqlWriter.Write(" FLOAT NOT NULL DEFAULT '0'")
                            '                Exit Select
                            '            Case "0"    '"String"
                            '                sqlWriter.Write(" TEXT NOT NULL")
                            '                Exit Select
                            '            Case Else
                            '                Alert("Unknown field type " & data.Columns(i).DataType.Name & "!", runningAsGui)
                            '        End Select

                            '        If i < data.Columns.Count - 1 Then sqlWriter.WriteLine(",")
                            '    Next

                            '    '    For Each index As DataColumn In data.PrimaryKey
                            '    '         sqlWriter.WriteLine(vbTab & "PRIMARY KEY (`{0}`)", index.ColumnName)
                            '    '     Next

                            '    '   sqlWriter.WriteLine(")")
                            '    '   sqlWriter.WriteLine(" ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci COMMENT='Export of {0}';", tablename)
                            '    '  sqlWriter.WriteLine(" SET NAMES UTF8;")
                            'End If

                            ''sqlWriter.WriteLine()
                        End If
                    End If
                End If
            End If
        End Sub

    End Module
End Namespace
