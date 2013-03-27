'Imports System.ComponentModel.Composition
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports System.Xml
'Imports System.Xml.Linq

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

        ''' <summary>
        ''' Export data to the XML file
        ''' </summary>
        ''' <param name="SourceFolder"></param>
        ''' <param name="tableName"></param>
        ''' <param name="DBCDataTable"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
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
                'Else
                '    Stop
                '    'TODO: Need to create base structure here
            End If

            WriteXMLStructure(myXMLDoc, DBCDataTable, Path.GetFileNameWithoutExtension(tableName.Substring(0, tableName.Length - 4)), SourceFolder & "\" & XMLFilename)

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

                    If Not IsNothing(thisSubNode) Then
                        'Stop
                        'Delete the existing node and recreate it

                        thisNode.RemoveChild(thisSubNode)
                        '    thisSubNode = thisNode.PreviousSibling
                        'Else
                        '    thisSubNode = thisNode.LastChild
                    End If 'Not found, time to create it

                    Dim newNode As Xml.XmlElement
                    Dim newIncludeNode As Xml.XmlElement
                    Dim newtableNode As Xml.XmlElement
                    Dim newfieldNode As Xml.XmlElement
                    Dim newtableNameNode As Xml.XmlElement
                    Dim maxCols As Integer = data.Columns.Count - 1
                    Dim intMaxRows As Integer = data.Rows.Count() - 1
                    If intMaxRows = -1 Then intMaxRows = 0

                    If maxCols > 0 And intMaxRows >= 0 Then
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

                        Dim blnOverrideOk As Boolean = False
                        Dim ColumnNameOverride As New Dictionary(Of Integer, String)
                        ColumnNameOverride = LoadXMLDefinitions(XMLFilename, tablename)
                        If ColumnNameOverride.Count() = data.Columns.Count() Then blnOverrideOk = True

                        For allColumns As Integer = 0 To maxCols
                            Dim thisLastColData As String
                            If Not IsDBNull(data.Rows(intMaxRows)(allColumns)) Then
                                thisLastColData = data.Rows(intMaxRows)(allColumns)
                            Else
                                thisLastColData = "1"
                            End If

                            Dim OutfieldType As String = ""
                            'Last Row contains the field type.. 0 = String, 1 = Int32, 2 = Long, 3 = Float
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
                                    OutfieldType = "int"
                                    Core.Alert([String].Format("Unknown field type {0}!, int assumed", thisLastColData), Core.AlertNewLine.AddCRLF)
                            End Select


                            newfieldNode = myXML.CreateNode(XmlNodeType.Element, "field", "")
                            newfieldNode.SetAttribute("type", OutfieldType)

                            If blnOverrideOk = False Then
                                newfieldNode.SetAttribute("name", "col" & allColumns.ToString())
                            Else
                                newfieldNode.SetAttribute("name", ColumnNameOverride(allColumns).ToString())
                            End If

                            newfieldNode.SetAttribute("include", "y")
                            newtableNode.AppendChild(newfieldNode)
                        Next

                        thisNode.InsertAfter(newtableNode, thisNode.LastChild)

                        myXML.Save(XMLFilename) '"D:\wtt\TEST.XML")
                        '    End If
                    End If
                End If
            End If
        End Sub
    End Module
End Namespace
