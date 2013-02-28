Imports System
Imports System.IO
'Imports ICSharpCode.SharpZipLib.BZip2
'Imports ICSharpCode.SharpZipLib.Zip.Compression.Streams
'Imports ICSharpCode.SharpZipLib
'Imports StormLibSharp

Public Class Form1
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
        For Each thisFolder As System.IO.DirectoryInfo In StartFolder.GetDirectories()
            If thisFolder.FullName.ToLower.Contains("cache") = False And thisFolder.FullName.ToLower.Contains("updates") = False Then
                FolderList.Add(thisFolder, thisFolder.FullName)
                ReadFolders(thisFolder, FolderList)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Extracts DBC Files including checks not Patch files (Stormlib version)
    ''' </summary>
    ''' <param name="MPQFilename"></param>
    ''' <param name="FileFilter"></param>
    ''' <param name="DestinationFolder"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub ExtractDBCFilesStormLib(ByVal MPQFilename As String, ByVal FileFilter As String, ByVal DestinationFolder As String)
        Me.Text = MPQFilename
        Dim Archive As StormLib.MpqArchive

        Archive = New StormLib.MpqArchive(MPQFilename, 0, StormLib.OpenArchiveFlags.READ_ONLY)

        'For Each thisArchive As StormLib.MpqArchive In ArchiveSet
        'Next


        Try
            Archive = New StormLib.MpqArchive(MPQFilename, 0, StormLib.OpenArchiveFlags.READ_ONLY)

            Archive.ExtractFile(MPQFilename, "w", FileFilter)

            'For Each thisFile As StormLib.MpqArchiveSet In FileList
            '    Dim inbyteData(thisFile.Size - 1) As Byte


            '    If inbyteData.Length > 20 Then
            '        Archive.ExportFile(thisFile.FileName, inbyteData)


            '        If (inbyteData(0) = 87 And inbyteData(1) = 68 And inbyteData(2) = 66 And inbyteData(3) = 67) Or (inbyteData(0) = 87 And inbyteData(1) = 68 And inbyteData(2) = 66 And inbyteData(3) = 50) Then 'Is a WDBC/WDB2 File

            '            If thisFile.FileName.Contains("\") = True Then
            '                If My.Computer.FileSystem.DirectoryExists(DestinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\")))) = False Then
            '                    Directory.CreateDirectory(DestinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\"))))
            '                End If
            '            Else
            '                If My.Computer.FileSystem.DirectoryExists(DestinationFolder) = False Then
            '                    Directory.CreateDirectory(DestinationFolder)
            '                End If
            '            End If

            '            If My.Computer.FileSystem.FileExists(DestinationFolder & "\" & thisFile.FileName) = True Then
            '                My.Computer.FileSystem.DeleteFile(DestinationFolder & "\" & thisFile.FileName)
            '            End If
            '            Archive.ExportFile(thisFile.FileName, DestinationFolder & "\" & thisFile.FileName)

            '            ' Create a file and write the byte data to a file.
            '            '                Dim oFileStream As System.IO.FileStream
            '            'oFileStream = New System.IO.FileStream(DestinationFolder & "\" & thisFile.FileName, System.IO.FileMode.Create)
            '            'oFileStream.Write(inbyteData, 0, inbyteData.Length)
            '            'oFileStream.Close()
            '        ElseIf (inbyteData(0) = 80 And inbyteData(1) = 84 And inbyteData(2) = 67 And inbyteData(3) = 72) Then   'PTCH File
            '            If thisFile.FileName.Contains("\") = True Then
            '                If My.Computer.FileSystem.DirectoryExists(DestinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\")))) = False Then
            '                    Directory.CreateDirectory(DestinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\"))))
            '                End If
            '            Else
            '                If My.Computer.FileSystem.DirectoryExists(DestinationFolder) = False Then
            '                    Directory.CreateDirectory(DestinationFolder)
            '                End If
            '            End If

            '            If inbyteData.Length > 20 Then
            '                Archive.ExportFile(thisFile.FileName, inbyteData)
            '            End If
            '            If My.Computer.FileSystem.FileExists(DestinationFolder & "\" & thisFile.FileName & ".patch") = False Then
            '                Archive.ExportFile(thisFile.FileName, DestinationFolder & "\" & thisFile.FileName & ".patch")
            '            Else
            '                Archive.ExportFile(thisFile.FileName, DestinationFolder & "\" & thisFile.FileName & "_" & MPQFilename.Substring(MPQFilename.LastIndexOf("\") + 1, MPQFilename.Length - (MPQFilename.LastIndexOf("\") + 1) - 4) & ".patch")
            '            End If

            '            ' Create a file and write the byte data to a file.
            '            '                Dim oFileStream As System.IO.FileStream
            '            'oFileStream = New System.IO.FileStream(DestinationFolder & "\" & thisFile.FileName, System.IO.FileMode.Create)
            '            'oFileStream.Write(inbyteData, 0, inbyteData.Length)
            '            'oFileStream.Close()
            '        Else    'File is something else
            '            If thisFile.FileName.EndsWith(".db") = False Then
            '                MessageBox.Show("Strange File Type: " & thisFile.FileName)
            '            End If
            '        End If
            '    End If
            '    Application.DoEvents()


            'Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Extracts DBC Files including checks not Patch files (MPQLib Version)
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
                    If (inbyteData(0) = 87 And inbyteData(1) = 68 And inbyteData(2) = 66 And inbyteData(3) = 67) Then intFileType = 1
                    If (inbyteData(0) = 87 And inbyteData(1) = 68 And inbyteData(2) = 66 And inbyteData(3) = 50) Then intFileType = 2
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

                        'If My.Computer.FileSystem.FileExists(DestinationFolder & "\" & thisFile.FileName & "_" & MPQFilename.Substring(MPQFilename.LastIndexOf("\") + 1, MPQFilename.Length - (MPQFilename.LastIndexOf("\") + 1) - 4)) = True Then
                        '    My.Computer.FileSystem.DeleteFile(DestinationFolder & "\" & thisFile.FileName & "_" & MPQFilename.Substring(MPQFilename.LastIndexOf("\") + 1, MPQFilename.Length - (MPQFilename.LastIndexOf("\") + 1) - 4))
                        'End If
                        'Archive.ExportFile(thisFile.FileName, DestinationFolder & "\" & thisFile.FileName & "_" & MPQFilename.Substring(MPQFilename.LastIndexOf("\") + 1, MPQFilename.Length - (MPQFilename.LastIndexOf("\") + 1) - 4))

                        ' Create a file and write the byte data to a file.
                        '                Dim oFileStream As System.IO.FileStream
                        'oFileStream = New System.IO.FileStream(DestinationFolder & "\" & thisFile.FileName, System.IO.FileMode.Create)
                        'oFileStream.Write(inbyteData, 0, inbyteData.Length)
                        'oFileStream.Close()
                    ElseIf intFileType = 3 Then   'PTCH File
                        'TODO: The Patch files need to be handled correctly, currently we are just saving them as .patch

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

                        'If inbyteData.Length > 20 Then
                        '    Archive.ExportFile(thisFile.FileName, inbyteData)
                        'End If

                        'If the file already exists, delete it and recreate it
                        If My.Computer.FileSystem.FileExists(DestinationFolder & "\" & thisFile.FileName & ".patch") = False Then
                            Archive.ExportFile(thisFile.FileName, DestinationFolder & "\" & thisFile.FileName & ".patch")
                        Else
                            Archive.ExportFile(thisFile.FileName, DestinationFolder & "\" & thisFile.FileName & "_" & MPQFilename.Substring(MPQFilename.LastIndexOf("\") + 1, MPQFilename.Length - (MPQFilename.LastIndexOf("\") + 1) - 4) & ".patch")
                        End If

                        ' Create a file and write the byte data to a file.
                        '                Dim oFileStream As System.IO.FileStream
                        'oFileStream = New System.IO.FileStream(DestinationFolder & "\" & thisFile.FileName, System.IO.FileMode.Create)
                        'oFileStream.Write(inbyteData, 0, inbyteData.Length)
                        'oFileStream.Close()
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

    'Private Shared Function BZip2Decompress(ByVal Data As Stream, ByVal ExpectedLength As Integer) As Byte()
    '    Dim output As New MemoryStream
    '    BZip2.Decompress(Data, output)
    '    Return output.ToArray
    'End Function
    'Private Shared Function DecompressMulti(ByVal Input As Byte(), ByVal OutputLength As Integer) As Byte()
    '    Dim sinput As Stream = New MemoryStream(Input)
    '    Dim comptype As Byte = sinput.ReadByte

    '    'BZip2
    '    If ((comptype And 16) <> 0) Then
    '        Dim result As Byte() = BZip2Decompress(sinput, OutputLength)
    '        comptype = CByte((comptype And 239))
    '        If (comptype = 0) Then
    '            Return result
    '        End If
    '        sinput = New MemoryStream(result)
    '    End If

    '    ''PKLib
    '    'If ((comptype And 8) <> 0) Then
    '    '    Dim result As Byte() = PKDecompress(sinput, OutputLength)
    '    '    comptype = CByte((comptype And 247))
    '    '    If (comptype = 0) Then
    '    '        Return result
    '    '    End If
    '    '    sinput = New MemoryStream(result)
    '    'End If

    '    ''ZLib
    '    'If ((comptype And 2) <> 0) Then
    '    '    Dim result As Byte() = ZlibDecompress(sinput, OutputLength)
    '    '    comptype = CByte((comptype And 253))
    '    '    If (comptype = 0) Then
    '    '        Return result
    '    '    End If
    '    '    sinput = New MemoryStream(result)
    '    'End If

    '    If ((comptype And 1) <> 0) Then
    '        'Dim result As Byte() = MpqHuffman.Decompress(sinput)
    '        'comptype = CByte((comptype And 254))
    '        'If (comptype = 0) Then
    '        '    Return result
    '        'End If
    '        'sinput = New MemoryStream(result)
    '    End If
    '    If ((comptype And 128) <> 0) Then
    '        'Dim result As Byte() = MpqWavCompression.Decompress(sinput, 2)
    '        'comptype = CByte((comptype And 127))
    '        'If (comptype = 0) Then
    '        '    Return result
    '        'End If
    '        'sinput = New MemoryStream(result)
    '    End If
    '    If ((comptype And 64) <> 0) Then
    '        'Dim result As Byte() = MpqWavCompression.Decompress(sinput, 1)
    '        'comptype = CByte((comptype And 191))
    '        'If (comptype = 0) Then
    '        '    Return result
    '        'End If
    '        'sinput = New MemoryStream(result)
    '    End If

    '    Throw New Exception(String.Format("Unhandled compression flags: 0x{0:X}", comptype))
    'End Function
End Class