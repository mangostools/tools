Imports System
Imports System.IO
'Imports ICSharpCode.SharpZipLib.BZip2
'Imports ICSharpCode.SharpZipLib.Zip.Compression.Streams
'Imports ICSharpCode.SharpZipLib
'Imports StormLibSharp

Public Class MaNGOSExtractor
    Public Locales As String = "enGB enUS deDE esES frFR koKR zhCN zhTW enCN enTW esMX ruRU"

    ''' <summary>
    ''' Starts the DBC Extraction process
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnStartDBC_Click(sender As Object, e As EventArgs) Handles btnStartDBC.Click
        ListBox1.Items.Clear()
        Dim colBaseFiles As New SortedSet(Of String)    'Collection containing all the base files
        Dim colMainFiles As New SortedSet(Of String)    'Collection containing all the main files
        Dim colUpdateFiles As New SortedSet(Of String)  'Collection containing any update or patch files

        Dim colFolders As New Collection                'Collection to hold for the folders to be processed
        Dim myFolders As System.IO.DirectoryInfo

        If System.IO.Directory.Exists(txtBaseFolder.Text) = False Then
            MessageBox.Show("Warcraft folder '" & txtBaseFolder.Text & "' can not be located")
            Exit Sub
        End If


        'Set the Top level as {Wow Folder}\data
        myFolders = New System.IO.DirectoryInfo(txtBaseFolder.Text & "\data")

        'Add the Data folder to the collection before we start walking down the tree
        colFolders.Add(myFolders, myFolders.FullName)

        'Build a list of all the subfolders under data
        ReadFolders(myFolders, colFolders)

        'Now we need to walk through the folders, getting the MPQ files along the way
        For t As Integer = 1 To colFolders.Count()
            myFolders = colFolders.Item(t)
            For Each file As System.IO.FileInfo In myFolders.GetFiles("*.MPQ")
                If file.FullName.ToLower.Contains("update") = True Or file.FullName.ToLower.Contains("patch") = True Then
                    colUpdateFiles.Add(file.FullName)
                ElseIf file.FullName.ToLower.Contains("base") = True Then
                    colBaseFiles.Add(file.FullName)
                Else
                    colMainFiles.Add(file.FullName)
                End If
            Next
        Next

        If My.Computer.FileSystem.DirectoryExists(txtBaseFolder.Text & "\Adbc\") = False Then
            Directory.CreateDirectory(txtBaseFolder.Text & "\Adbc\")
        End If


        For Each strItem As String In colBaseFiles
            ListBox1.Items.Add("  BASE: " & strItem)
            Try
                ExtractDBCFiles(strItem, "*.db*", txtBaseFolder.Text & "\Adbc\")
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Next

        For Each strItem As String In colMainFiles
            ListBox1.Items.Add("  FILE: " & strItem)
            Try
                ExtractDBCFiles(strItem, "*.db*", txtBaseFolder.Text & "\Adbc\")
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Next

        For Each strItem As String In colUpdateFiles
            ListBox1.Items.Add("UPDATE: " & strItem)

            Try
                ExtractDBCFiles(strItem, "*.db*", txtBaseFolder.Text & "\Adbc\")
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Next
        MessageBox.Show("Finished")
    End Sub

    ''' <summary>
    ''' Exits the Application
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnQuit_Click(sender As Object, e As EventArgs) Handles BtnQuit.Click
        End
    End Sub


    ''' <summary>
    ''' Recursively reads the directory structure from the StartFolder down
    ''' </summary>
    ''' <param name="StartFolder"></param>
    ''' <param name="FolderList"></param>
    ''' <remarks></remarks>
    Private Sub ReadFolders(ByRef StartFolder As System.IO.DirectoryInfo, ByRef FolderList As Collection)
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
                        MessageBox.Show("Error reading folder '" & thisFolder.FullName & "'")
                    End Try
                Next
            Catch ex As Exception
                MessageBox.Show("Error reading folder '" & StartFolder.FullName & "'")
            End Try
        Else
            MessageBox.Show("Warcraft folder '" & StartFolder.FullName & "' can not be located")
        End If
    End Sub


    ''' <summary>
    ''' Extracts DBC Files including Patch files (MPQLib Version)
    ''' </summary>
    ''' <param name="MPQFilename"></param>
    ''' <param name="FileFilter"></param>
    ''' <param name="DestinationFolder"></param>
    ''' <remarks></remarks>
    Private Sub ExtractDBCFiles(ByVal MPQFilename As String, ByVal FileFilter As String, ByVal DestinationFolder As String)
        Me.Text = MPQFilename
        Dim Archive As MpqLib.Mpq.CArchive
        Dim FileList As System.Collections.Generic.IEnumerable(Of MpqLib.Mpq.CFileInfo)

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
                            p.PrintHeaders()
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
                            MessageBox.Show("Strange File Type: " & thisFile.FileName)
                        End If
                    End If
                End If
                Application.DoEvents()
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Generic Extraction Routine
    ''' </summary>
    ''' <param name="MPQFilename"></param>
    ''' <param name="FileFilter"></param>
    ''' <param name="DestinationFolder"></param>
    ''' <remarks></remarks>
    Private Sub ExtractFilesGeneric(ByVal MPQFilename As String, ByVal FileFilter As String, ByVal DestinationFolder As String)
        Me.Text = MPQFilename
        Dim Archive As MpqLib.Mpq.CArchive
        Dim FileList As System.Collections.Generic.IEnumerable(Of MpqLib.Mpq.CFileInfo)

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

                Application.DoEvents()
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class