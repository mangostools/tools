Imports System.IO
Imports System.Text
Imports MpqLib

Namespace Core
    Module MaNGOSExtractorCore
        Private m_Version As String = " v1.2"

        ''' <summary>
        ''' Returns the version number as a string - currently predefined, but may come from application properties in the future
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Version As String
            Get
                Return m_Version
            End Get
            'Private Set(value As Integer)
            '    m_Version = value
            'End Set
        End Property

        Public Property runningAsGui As Boolean
            Get
                Return m_runningAsGui
            End Get
            Set(value As Boolean)
                m_runningAsGui = value
            End Set
        End Property
        Private m_runningAsGui As Boolean = False

        Public Property alertlist As ListBox
            Get
                Return m_alertlist
            End Get
            Set(value As ListBox)
                m_alertlist = value
            End Set
        End Property
        Private m_alertlist As ListBox

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

                            'Console.WriteLine("Destination Folder: {0}", DestinationFolder)
                            'Console.WriteLine("Sub Folder: {0}", strSubFolder)

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


                        Else    'File is something else
                            'As I am matching on *.db* rather than *.dbc or *.db2, one .db file is found as well - so this check ignores it
                            If thisFile.FileName.EndsWith(".db") = False Then
                                sbOutput.AppendLine("Strange File Type: " & thisFile.FileName)
                            End If
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
                If testI > 0 Then
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
                        OutData = "Float"
                    End If
                End If
            Else
                OutData = "String"
            End If
            Return OutData
        End Function

        ''' <summary>
        ''' Sends a message to either a gui listbox or console
        ''' </summary>
        ''' <param name="AlertMessage"></param>
        ''' <param name="runningAsGui"></param>
        ''' <param name="resultList"></param>
        ''' <remarks></remarks>
        Public Sub Alert(ByRef AlertMessage As String, ByRef runningAsGui As Boolean, Optional append As Boolean = False)
            If runningAsGui = True Then 'running as a Gui App
                If Not IsNothing(m_alertlist) Then
                    If append = False Then
                        m_alertlist.Items.Add(AlertMessage)
                        m_alertlist.SelectedIndex = m_alertlist.Items.Count() - 1
                    Else
                        Dim Temp As String = m_alertlist.Items(m_alertlist.Items.Count() - 1)
                        AlertMessage = Temp & AlertMessage
                        m_alertlist.Items.Remove(m_alertlist.Items.Count() - 1)

                        m_alertlist.Items.Add(AlertMessage)
                        m_alertlist.SelectedIndex = m_alertlist.Items.Count() - 1
                    End If
                End If
                Else 'Running as console
                    If append = False Then
                        Console.WriteLine(AlertMessage)
                    Else
                        Console.Write(AlertMessage)
                    End If
                End If
        End Sub
    End Module
End Namespace