Imports System.IO
Imports System.Text
Imports MpqLib
Imports System.Data
Imports System.Text.RegularExpressions
Imports System.Reflection

Namespace Core
    Module MaNGOSExtractorCore
        'Private m_Version As String = " v1.3"
        Private m_BuildNo As Integer
        Private m_MajorVersion As Integer
        Private m_FullVersion As String

        Property BuildNo As Integer
            Get
                Return m_BuildNo
            End Get
            Set(value As Integer)
                m_BuildNo = value
            End Set
        End Property

        Property MajorVersion As Integer
            Get
                Return m_MajorVersion
            End Get
            Set(value As Integer)
                m_MajorVersion = value
            End Set
        End Property

        Property FullVersion As String
            Get
                Return m_FullVersion
            End Get
            Set(value As String)
                m_FullVersion = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the version number as a string which is pulled from the application properties
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Version As String
            Get

                Return " v" & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor    'm_Version
            End Get
            'Private Set(value As Integer)
            '    m_Version = value
            'End Set
        End Property

        ''' <summary>
        ''' A boolean value which indicates whether the app is running as a gui or console (false=console)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property runAsGui As Boolean
            Get
                Return m_runningAsGui
            End Get
            Set(value As Boolean)
                m_runningAsGui = value
            End Set
        End Property
        Private m_runningAsGui As Boolean = False

        ''' <summary>
        ''' Defines the Listbox which messages are sent to in gui mode
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property alertlist As ListBox
            Get
                Return m_alertlist
            End Get
            Set(value As ListBox)
                m_alertlist = value
            End Set
        End Property
        Private m_alertlist As ListBox

        Public Sub ReadWarcraftExe(ByRef Filename As String)
            Try
                Dim Version As String = FileVersionInfo.GetVersionInfo(Filename).FileVersion
                BuildNo = Version.Substring(Version.LastIndexOf(", ") + 2)
                FullVersion = Version.Substring(0, Version.LastIndexOf(", ")).Replace(" ", "").Replace(",", ".")
                MajorVersion = Version.Substring(0, 1)
            Catch ex As Exception

            End Try

        End Sub


        ''' <summary>
        ''' Recursively reads the directory structure from the StartFolder down
        ''' </summary>
        ''' <param name="StartFolder"></param>
        ''' <param name="FolderList"></param>
        ''' <remarks></remarks>
        Public Function ReadFolders(ByRef StartFolder As System.IO.DirectoryInfo, ByRef FolderList As Collection) As String
            Dim sbOutput As New StringBuilder
            If System.IO.Directory.Exists(StartFolder.FullName) = True Then
                Try
                    For Each thisFolder As System.IO.DirectoryInfo In StartFolder.GetDirectories()
                        Try
                            'Skip the cache and updates folders if they exist
                            If thisFolder.FullName.ToLower.Contains("cache") = False And thisFolder.FullName.ToLower.Contains("updates") = False Then
                                FolderList.Add(thisFolder, thisFolder.FullName)
                                ReadFolders(thisFolder, FolderList)
                            End If
                        Catch ex As Exception
                            sbOutput.AppendLine("Error reading folder '" & thisFolder.FullName & "'")
                        End Try
                    Next
                Catch ex As Exception
                    sbOutput.AppendLine("Error reading folder '" & StartFolder.FullName & "'")
                End Try
            Else
                sbOutput.AppendLine("Warcraft folder '" & StartFolder.FullName & "' can not be located")
            End If
            Return sbOutput.ToString()
        End Function

        ''' <summary>
        ''' Extracts DBC Files including Patch files (MPQLib Version)
        ''' </summary>
        ''' <param name="MPQFilename"></param>
        ''' <param name="FileFilter"></param>
        ''' <param name="DestinationFolder"></param>
        ''' <remarks></remarks>
        Public Function ExtractDBCFiles(ByVal MPQFilename As String, ByVal FileFilter As String, ByVal DestinationFolder As String) As String
            Dim Archive As MpqLib.Mpq.CArchive
            Dim FileList As System.Collections.Generic.IEnumerable(Of MpqLib.Mpq.CFileInfo)
            Dim sbOutput As New StringBuilder

            Try
                'Open the Archive Folder
                Archive = New MpqLib.Mpq.CArchive(MPQFilename)

                'Get a list of all files matching FileFilter
                FileList = Archive.FindFiles(FileFilter)

                'Process each file found
                For Each thisFile As MpqLib.Mpq.CFileInfo In FileList
                    Dim inbyteData(thisFile.Size - 1) As Byte
                    Dim intFileType As Integer = 0
                    'intFileType = 0  = Unknown
                    'intFileType = 1  = WDBC
                    'intFileType = 2  = WDB2
                    'intFileType = 3  = PTCH
                    'Create the output directory tree, allowing for additional paths contained within the filename

                    Dim strSubFolder As String
                    If thisFile.FileName.Contains("\") = True Then
                        strSubFolder = thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\")))
                        If My.Computer.FileSystem.DirectoryExists(DestinationFolder & strSubFolder) = False Then
                            Directory.CreateDirectory(DestinationFolder & strSubFolder)
                        End If
                    Else
                        strSubFolder = ""
                        If My.Computer.FileSystem.DirectoryExists(DestinationFolder) = False Then
                            Directory.CreateDirectory(DestinationFolder)
                        End If
                    End If

                    Dim strOriginalName As String = thisFile.FileName.Substring(thisFile.FileName.LastIndexOf("\") + 1, thisFile.FileName.Length - (thisFile.FileName.LastIndexOf("\") + 1))
                    Dim strPatchName As String = strOriginalName & "_" & MPQFilename.Substring(MPQFilename.LastIndexOf("\") + 1, MPQFilename.Length - (MPQFilename.LastIndexOf("\") + 1) - 4) & ".patch"
                    Dim strNewName As String = strOriginalName & ".New"
                    If DestinationFolder.EndsWith("\") = False Then DestinationFolder = DestinationFolder & "\"

                    'Skip corrupt files (Length < 21)
                    If inbyteData.Length > 20 Then

                        'We perform this export so that we can get the header bytes
                        Archive.ExportFile(thisFile.FileName, inbyteData)
                        If (inbyteData(0) = 87 And inbyteData(1) = 68 And inbyteData(2) = 66 And inbyteData(3) = 67) Then intFileType = 1 'WDBC HEader
                        If (inbyteData(0) = 87 And inbyteData(1) = 68 And inbyteData(2) = 66 And inbyteData(3) = 50) Then intFileType = 2 'WDB2 Header
                        If (inbyteData(0) = 80 And inbyteData(1) = 84 And inbyteData(2) = 67 And inbyteData(3) = 72) Then intFileType = 3 'PTCH File

                        If intFileType = 1 Or intFileType = 2 Then 'Is a WDBC/WDB2 File

                            'Create the output directory tree, allowing for additional paths contained within the filename
                            If thisFile.FileName.Contains("\") = True Then
                                If My.Computer.FileSystem.DirectoryExists(DestinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\")))) = False Then
                                    Directory.CreateDirectory(DestinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\"))))
                                End If
                            Else
                                If My.Computer.FileSystem.DirectoryExists(DestinationFolder) = False Then
                                    Directory.CreateDirectory(DestinationFolder)
                                End If
                            End If

                            'If the file already exists, delete it and recreate it
                            If My.Computer.FileSystem.FileExists(DestinationFolder & "\" & thisFile.FileName) = True Then
                                My.Computer.FileSystem.DeleteFile(DestinationFolder & "\" & thisFile.FileName)
                            End If
                            Archive.ExportFile(thisFile.FileName, DestinationFolder & "\" & thisFile.FileName)
                        ElseIf intFileType = 3 Then   'PTCH File

                            '###############################################################################
                            '## Patch Files are a special case and are only present in Cata and Mop       ##
                            '## - The current Implementation has been split into two stages               ##
                            '###############################################################################
                            '## Stage 1 - Saves the files out with a .patch extension                     ##
                            '###############################################################################
                            '## Stage 2 - will attempt to process the patch files and apply them to the   ##
                            '##           original file                                                   ##
                            '###############################################################################

                            '###############################################################################
                            '## Stage 1 - Saves the files out with a .patch extension                     ##
                            '###############################################################################

                            'If the file already exists, delete it and recreate it
                            If My.Computer.FileSystem.FileExists(DestinationFolder & strSubFolder & "\" & strPatchName) = True Then
                                My.Computer.FileSystem.DeleteFile(DestinationFolder & strSubFolder & "\" & strPatchName)
                            End If
                            Archive.ExportFile(thisFile.FileName, DestinationFolder & strSubFolder & "\" & strPatchName)

                            'Copy the patch to .new
                            If My.Computer.FileSystem.FileExists(DestinationFolder & strSubFolder & "\" & strNewName) = False Then
                                System.IO.File.Copy(DestinationFolder & strSubFolder & "\" & strPatchName, DestinationFolder & strSubFolder & "\" & strNewName)
                            End If


                            '###############################################################################
                            '## Stage 2 - will attempt to process the patch files and apply them to the   ##
                            '##           original file                                                   ##
                            '###############################################################################
                            Using p As New Blizzard.Patch(DestinationFolder & strSubFolder & "\" & strPatchName)
                                p.PrintHeaders(strOriginalName)
                                p.Apply(DestinationFolder & strSubFolder & "\" & strOriginalName, DestinationFolder & strSubFolder & "\" & strNewName, True)
                            End Using

                            'Move the original and the patch
                            My.Computer.FileSystem.DeleteFile(DestinationFolder & strSubFolder & "\" & strOriginalName)
                            My.Computer.FileSystem.DeleteFile(DestinationFolder & strSubFolder & "\" & strPatchName)

                            'Rename the .new as the Original Name
                            My.Computer.FileSystem.RenameFile(DestinationFolder & strSubFolder & "\" & strNewName, strOriginalName)


                            'Else    'File is something else
                            '    'As I am matching on *.db* rather than *.dbc or *.db2, one .db file is found as well - so this check ignores it
                            '    If thisFile.FileName.EndsWith(".db") = False Then
                            '        sbOutput.AppendLine("Strange File Type: " & thisFile.FileName)
                            'End If
                        End If
                    End If
                    '                    Core.exportSQL(DestinationFolder & strSubFolder & "\" & strOriginalName)

                Next
            Catch ex As Exception
                sbOutput.AppendLine(ex.Message)
            End Try
            Return sbOutput.ToString()
        End Function

        ''' <summary>
        ''' Generic Extraction Routine
        ''' </summary>
        ''' <param name="MPQFilename"></param>
        ''' <param name="FileFilter"></param>
        ''' <param name="DestinationFolder"></param>
        ''' <remarks></remarks>
        Public Function ExtractFilesGeneric(ByVal MPQFilename As String, ByVal FileFilter As String, ByVal DestinationFolder As String) As String
            Dim Archive As MpqLib.Mpq.CArchive
            Dim FileList As System.Collections.Generic.IEnumerable(Of MpqLib.Mpq.CFileInfo)
            Dim sbOutput As New StringBuilder

            Try
                Archive = New MpqLib.Mpq.CArchive(MPQFilename)

                FileList = Archive.FindFiles(FileFilter)

                For Each thisFile As MpqLib.Mpq.CFileInfo In FileList
                    If thisFile.FileName.Contains("\") = True Then
                        If My.Computer.FileSystem.DirectoryExists(DestinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\")))) = False Then
                            Directory.CreateDirectory(DestinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\"))))
                        End If
                    Else
                        If My.Computer.FileSystem.DirectoryExists(DestinationFolder) = False Then
                            Directory.CreateDirectory(DestinationFolder)
                        End If
                    End If

                    If My.Computer.FileSystem.FileExists(DestinationFolder & "\" & thisFile.FileName) = True Then
                        My.Computer.FileSystem.DeleteFile(DestinationFolder & "\" & thisFile.FileName)
                    End If
                    Archive.ExportFile(thisFile.FileName, DestinationFolder & "\" & thisFile.FileName)

                Next
            Catch ex As Exception
                sbOutput.AppendLine(ex.Message)
            End Try
            Return sbOutput.ToString()
        End Function

        ''' <summary>
        ''' Returns a value based on object type
        ''' </summary>
        ''' <param name="InputData"></param>
        ''' <param name="test"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getObjectType(ByRef InputData As Object, ByRef test As String) As String
            Dim OutData As String = ""
            Dim testL As Long
            Long.TryParse(InputData, testL)
            Dim testI As Integer
            Integer.TryParse(InputData, testI)
            Dim testD As Double
            Double.TryParse(InputData, testD)

            If IsNumeric(InputData) = True Then
                If testI > 0 And testI < 65536 Then
                    If test = "Float" Or test = "Long" Or test = "String" Then
                        OutData = test
                    Else
                        OutData = "Int32"
                    End If
                ElseIf testI = 0 And testL = 0 Then
                    If test = "Long" Or test = "String" Then
                        OutData = test
                    Else
                        OutData = "Float"
                    End If
                Else
                    If test = "String" Then
                        OutData = test
                    Else
                        OutData = "Long"
                    End If
                End If
            Else
                OutData = "String"
            End If
            Return OutData
        End Function

        ''' <summary>
        ''' Loads a DBC file data into a datatable 
        ''' </summary>
        ''' <param name="Filename"></param>
        ''' <param name="dbcDataTable"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function loadDBCtoDataTable(ByRef Filename As String, ByRef dbcDataTable As DataTable) As DataTable
            Dim m_reader As FileReader.IWowClientDBReader

            Dim entireRow() As Byte
            Dim thisRow As DataRow

            m_reader = FileReader.DBReaderFactory.GetReader(Filename)
            Try
                entireRow = m_reader.GetRowAsByteArray(0)
            Catch ex As Exception
                entireRow = Nothing
            End Try
            Dim intMaxcols As Integer
            If IsNothing(entireRow) = True Then
                intMaxcols = 0
            Else
                intMaxcols = entireRow.Length - 1
            End If
            Dim ColType(intMaxcols / 4) As String
            'Dim ColType(intMaxcols) As String

            If intMaxcols > 0 Then
                For cols As Integer = 0 To intMaxcols Step 4
                    dbcDataTable.Columns.Add("Col" & (cols / 4).ToString(), GetType(String))
                    'dbcDataTable.Columns.Add("Col" & (cols).ToString(), GetType(String))
                Next

                Dim intMaxRows As Integer = 0
                Try
                    intMaxRows = m_reader.RecordsCount() - 1
                Catch
                    intMaxRows = 0
                End Try

                'Try
                If intMaxRows >= 0 Then
                    Dim strValuecounter As String = "0%---------50%--------100%"
                    Dim intblockcountersize As Integer = strValuecounter.Length()
                    If intMaxRows > 0 Then
                        Alert("", Core.AlertNewLine.AddCRLF)
                        Alert("         Loading DBC into memory " & strValuecounter & " Records: " & intMaxRows, Core.AlertNewLine.AddCRLF)
                        Alert("", Core.AlertNewLine.AddCRLF)
                        Alert("                                 ", Core.AlertNewLine.NoCRLF)
                    Else
                        Alert("         Loading DBC into memory " & strValuecounter & " Records: 0", Core.AlertNewLine.AddCRLF)
                        Alert("", Core.AlertNewLine.AddCRLF)
                    End If

                    For rows As Integer = 0 To intMaxRows
                        'Try
                        If CInt(intMaxRows / intblockcountersize) > 4 Then
                            If rows Mod CInt(intMaxRows / intblockcountersize) = 0 Then
                                Alert(".", Core.AlertNewLine.NoCRLF)
                            End If
                        End If

                        entireRow = m_reader.GetRowAsByteArray(rows)

                        thisRow = dbcDataTable.NewRow()
                        Dim cols As Integer = 0
                        While cols < entireRow.Length() - 1 '(dbcDataTable.Columns.Count()) * 4 '((intMaxcols + 1) * 4)
                            '                       For cols As Integer = 0 To ((intMaxcols - 1) * 4) Step 4
                            Dim TempCol As Object '= entireRow(cols)
                            ' Try
                            If IsNothing(entireRow) = True Then
                                TempCol = -1
                            Else
                                Try
                                    If entireRow(cols + 3) > 127 Then 'And entireRow(cols + 2) = 255 And entireRow(cols + 1) = 255 And entireRow(cols + 0) = 255 Then
                                        TempCol = -1
                                    Else
                                        TempCol = (entireRow(cols + 3) * 16777216) + (entireRow(cols + 2) * 65536) + (entireRow(cols + 1) * 256) + (entireRow(cols + 0))
                                    End If
                                Catch ex As Exception
                                    TempCol = -1
                                End Try
                            End If
                            '                            thisRow(CInt(cols / 4)) = TempCol
                            thisRow(CInt(cols / 4)) = TempCol
                            '                        Next
                            cols = cols + 4
                        End While
                        dbcDataTable.Rows.Add(thisRow)
                        Threading.Thread.Sleep(0)
                    Next
                Else 'Empty file
                    Alert("", Core.AlertNewLine.AddCRLF)
                End If

                Alert("", Core.AlertNewLine.AddCRLF)
                'Create a new row at the end to store the datatype
                If intMaxRows > 0 Then
                    thisRow = dbcDataTable.NewRow()

                    dbcDataTable.Rows.Add(thisRow)
                    'Try
                    Dim strValuecounter As String = "0%---------50%--------100%"
                    Dim intblockcountersize As Integer = strValuecounter.Length()
                    'If CInt(Fix(TotalRows / 4) / intblockcountersize) > 0 Then
                    Alert("   Determining Column Data Types " & strValuecounter & " Cols: " & dbcDataTable.Columns.Count() - 1, Core.AlertNewLine.AddCRLF)
                    Alert("", Core.AlertNewLine.AddCRLF)
                    Alert("                                 ", Core.AlertNewLine.NoCRLF)
                    'End If
                    Dim totalCols As Integer = dbcDataTable.Columns.Count() - 1
                    For cols As Integer = 0 To totalCols 'TotalRows Step 4
                        'Try

                        If CInt((totalCols / intblockcountersize)) > 0 Then
                            If cols Mod CInt((totalCols / intblockcountersize)) = 0 Then

                                Alert(".", Core.AlertNewLine.NoCRLF)

                            End If
                        Else
                            If (cols + 1) Mod CInt((intblockcountersize / (cols + 1))) = 0 Then

                                Alert(".", Core.AlertNewLine.NoCRLF)

                            End If
                        End If
                        '                Catch ex As Exception
                        '    Core.Alert("Error: " & ex.Message, MaNGOSExtractorCore.runningAsGui)

                        'End Try

                        Dim blnFoundString As Boolean = True
                        Dim SteppingAmount As Integer = 1 'dbcDataTable.Rows.Count() - 1 / 100
                        If dbcDataTable.Rows.Count() > 5000 Then SteppingAmount = 100
                        If SteppingAmount < 1 Then SteppingAmount = 1
                        For thisScanRow As Integer = 0 To dbcDataTable.Rows.Count - 1 Step SteppingAmount
                            If IsDBNull(dbcDataTable.Rows(thisScanRow)(CInt(cols))) = False Then
                                If m_reader.StringTable.ContainsKey(dbcDataTable.Rows(thisScanRow)(CInt(cols))) = False Then
                                    blnFoundString = False

                                    Dim strDataType As String = ""
                                    Dim strCurDataType As String = "Int32"
                                    If Not IsDBNull(dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols))) Then
                                        strCurDataType = dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols))
                                    Else
                                        strCurDataType = "1"
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
                                    strDataType = Core.getObjectType(dbcDataTable.Rows(thisScanRow)(CInt(cols)), strDataType)
                                    'Try
                                    If strDataType = "Int32" Then 'Integer
                                        dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols)) = 1
                                    ElseIf strDataType = "Float" Then 'Float
                                        dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols)) = 3
                                    ElseIf strDataType = "String" Then 'Float
                                        dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols)) = 0
                                    Else 'Long
                                        dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols)) = 2
                                    End If
                                    'Catch ex As Exception
                                    '    Core.Alert("Error: " & ex.Message, MaNGOSExtractorCore.runningAsGui)
                                    'End Try
                                End If
                            End If
                            Threading.Thread.Sleep(0)
                        Next
                        'Catch ex As Exception
                        '    Core.Alert("Error: " & ex.Message, MaNGOSExtractorCore.runningAsGui)
                        'End Try

                        If blnFoundString = True And m_reader.StringTableSize > 0 Then
                            'Try
                            For thisScanRow As Integer = 0 To dbcDataTable.Rows.Count - 1 Step SteppingAmount
                                If Not IsDBNull(dbcDataTable.Rows(thisScanRow)(CInt(cols))) Then
                                    dbcDataTable.Rows(thisScanRow)(CInt(cols)) = m_reader.StringTable(dbcDataTable.Rows(thisScanRow)(CInt(cols)))
                                    dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(cols)) = 0
                                End If
                                Threading.Thread.Sleep(0)
                            Next
                            'Catch ex As Exception
                            'End Try
                        End If
                        Threading.Thread.Sleep(0)
                    Next

                    'Catch ex As Exception
                    '    Core.Alert("Error: " & ex.Message, MaNGOSExtractorCore.runningAsGui)
                    'End Try
                End If
                Alert("", Core.AlertNewLine.AddCRLF)
            Else 'No Rows
                Alert("", Core.AlertNewLine.AddCRLF)
            End If
            'Catch ex As Exception
            '    Core.Alert("Error: " & ex.Message, MaNGOSExtractorCore.runningAsGui)
            'End Try

            Return dbcDataTable
        End Function


        ''' <summary>
        ''' Remove characters that mess with MySQL by escaping them with a leading \
        ''' </summary>
        ''' <param name="input"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function StripBadCharacters(input As String) As String
            input = input.Replace("\", "\\")
            input = input.Replace("'", "\'")
            input = input.Replace("_", "\_")
            input = input.Replace("%", "\%")
            input = input.Replace(Chr(34), "\" & Chr(34))
            Return input
        End Function

        Public Enum AlertNewLine
            NoCRLF = 1
            AddCRLF = 2
        End Enum


        ''' <summary>
        ''' Sends a message to either a gui listbox or console
        ''' </summary>
        ''' <param name="AlertMessage"></param>
        ''' <param name="resultList"></param>
        ''' <remarks></remarks>
        Public Sub Alert(ByRef AlertMessage As String, ByRef LineEnding As AlertNewLine)
            If m_runningAsGui = True Then 'running as a Gui App

                If Not IsNothing(Core.alertlist) Then
                    If LineEnding = AlertNewLine.AddCRLF Then
#If _MyType <> "Console" Then
                        Core.alertlist.Items.Add(AlertMessage)
#Else
                        Core.alertlist.Items.Add(AlertMessage, AlertMessage)
#End If
                        Core.alertlist.SelectedIndex = Core.alertlist.Items.Count() - 1
                    Else
                        Dim Temp As String = Core.alertlist.Items(Core.alertlist.Items.Count() - 1)
                        AlertMessage = Temp & AlertMessage


                        Core.alertlist.Items.RemoveAt(Core.alertlist.Items.Count() - 1)
#If _MyType <> "Console" Then
                        Core.alertlist.Items.Add(AlertMessage)
#Else
                        Core.alertlist.Items.Add(AlertMessage, AlertMessage)
#End If
                        Core.alertlist.SelectedIndex = Core.alertlist.Items.Count() - 1
                        Core.alertlist.SelectedIndex = -1
                    End If
                End If
            Else 'Running as console
                If LineEnding = AlertNewLine.AddCRLF Then
                    Console.WriteLine(AlertMessage)
                Else
                    Console.Write(AlertMessage)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Main Export routine, this reads all the dbc's from the output folder and calls the selected export routines on each one
        ''' </summary>
        ''' <param name="BaseFolder"></param>
        ''' <param name="OutputFolder"></param>
        ''' <param name="ExportCSV"></param>
        ''' <param name="ExportSQL"></param>
        ''' <param name="ExportXML"></param>
        ''' <remarks></remarks>
        Public Sub ExportFiles(ByRef BaseFolder As String, ByRef OutputFolder As String, ByRef ExportCSV As Boolean, ByRef ExportSQL As Boolean, ByRef ExportXML As Boolean)
            'Now that we have all the DBC's extracted and patched, we need to check the export options and export data
            If OutputFolder.EndsWith("\") = False Then OutputFolder = OutputFolder & "\"
            If My.Computer.FileSystem.DirectoryExists(OutputFolder & "DBFilesClient\") = False Then
                Directory.CreateDirectory(OutputFolder & "DBFilesClient\")
            End If
            Dim myFolders As System.IO.DirectoryInfo
            myFolders = New System.IO.DirectoryInfo(OutputFolder & "\DBFilesClient")




            Dim Files() As System.IO.FileInfo = myFolders.GetFiles("*.DB?")
            Dim FilelistSorted As New SortedList()
            
            For Each thisFile As System.IO.FileInfo In Files
                FilelistSorted.Add(thisFile.Name, thisFile.Name)
            Next

            ' For Each file As System.IO.FileInfo In Files 'myFolders.GetFiles("*.DB?")
            For Each fileItem As DictionaryEntry In FilelistSorted 'myFolders.GetFiles("*.DB?")
                Dim dbcDataTable As New DataTable

                'Load the entire DBC into a DataTable to be processed by all exports
                If ExportCSV = True Or ExportSQL = True Or ExportXML = True Then
                    Alert("", Core.AlertNewLine.AddCRLF)
                    Alert(fileItem.Value, Core.AlertNewLine.NoCRLF)
                    loadDBCtoDataTable(OutputFolder & "\DBFilesClient" & "\" & fileItem.Value, dbcDataTable)
                End If

                'Export to SQL Files
                If ExportSQL = True Then
                    Alert("Creating SQL for " & fileItem.Value, Core.AlertNewLine.AddCRLF)
                    Core.exportSQL(OutputFolder & "\DBFilesClient" & "\" & fileItem.Value, dbcDataTable, BaseFolder)
                    Alert("", Core.AlertNewLine.NoCRLF)
                End If

                'Export to CSV
                If ExportCSV = True Then
                    Alert("Creating CSV for " & fileItem.Value, Core.AlertNewLine.AddCRLF)
                    Core.exportCSV(OutputFolder & "\DBFilesClient" & "\" & fileItem.Value, dbcDataTable)
                    Alert("", Core.AlertNewLine.NoCRLF)
                End If

                'Export to XML
                If ExportXML = True Then
                    Alert("Creating XML for " & fileItem.Value, Core.AlertNewLine.AddCRLF)
                    Core.exportXML(BaseFolder, OutputFolder & "\DBFilesClient" & "\" & fileItem.Value, dbcDataTable)
                    Alert("", Core.AlertNewLine.NoCRLF)
                End If

                Threading.Thread.Sleep(0)
                dbcDataTable = Nothing
            Next
        End Sub

        ''' <summary>
        ''' This function returns the dbc fieldnames xml config file
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function returnDBCXMLfilename() As String
            Dim XMLFilename As String = ""
            Select Case Core.MajorVersion
                Case "1"
                    XMLFilename = "dbc_classic.xml"
                Case "2"
                    XMLFilename = "dbc_tbc.xml"
                Case "3"
                    XMLFilename = "dbc_wotlk.xml"
                Case "4"
                    XMLFilename = "dbc_cata.xml"
                Case "5"
                    XMLFilename = "dbc_mop.xml"
            End Select
            Return XMLFilename
        End Function

        ''' <summary>
        ''' Loads the fieldnames from the config xml for the specified file and returns a Dictionary of all field names
        ''' </summary>
        ''' <param name="sourceFolder"></param>
        ''' <param name="Tablename"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadXMLDefinitions(ByRef sourceFolder As String, ByRef Tablename As String) As Dictionary(Of Integer, String)
            Dim thisCollection As New Dictionary(Of Integer, String) ' Collection
            Dim myXMLDoc As New Xml.XmlDocument()
            Dim XMLFilename As String = ""
            XMLFilename = returnDBCXMLfilename()

            If My.Computer.FileSystem.FileExists(sourceFolder & "\" & XMLFilename) = True Then
                myXMLDoc.Load(sourceFolder & "\" & XMLFilename)
                ' Alert(" External XML Definitions used", Core.AlertNewLine.AddCRLF)
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

            Dim maxCols As Integer = 0
            Dim thisNode As Xml.XmlNode
            Dim thisFieldCountNode As Xml.XmlNode
            Dim thisFieldNode As Xml.XmlNode
            '<root>
            '    <Files>
            '        <Achievement>
            thisNode = myXMLDoc.SelectSingleNode("root")
            If Not IsNothing(thisNode) Then thisNode = thisNode.SelectSingleNode("Files")
            If Not IsNothing(thisNode) Then thisNode = thisNode.SelectSingleNode(Tablename)

            If Not IsNothing(thisNode) Then 'found, time to read it
                thisFieldCountNode = thisNode.SelectSingleNode("fieldcount") '<fieldcount>15</fieldcount>
                If Not IsNothing(thisFieldCountNode) Then
                    maxCols = thisFieldCountNode.InnerText '<field type="bigint" name="id" include="y" />
                    thisFieldNode = thisFieldCountNode.NextSibling

                    '                thisFieldNode = thisNode.SelectSingleNode("field")
                    For thisCol As Integer = 0 To maxCols - 1
                        If Not IsNothing(thisFieldNode) Then
                            thisCollection.Add(thisCol, thisFieldNode.Attributes.GetNamedItem("name").InnerText)
                        End If
                        thisFieldNode = thisFieldNode.NextSibling
                    Next
                End If

            End If
            Return thisCollection
        End Function

    End Module
End Namespace