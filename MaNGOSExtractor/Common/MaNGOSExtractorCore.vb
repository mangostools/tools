Imports System.IO
Imports System.Text
Imports MpqLib

Namespace Core
    Module MaNGOSExtractorCore
        Private m_Version As String = " v1.1"
        ReadOnly Property Version As String
            Get
                Return m_Version
            End Get
            'Private Set(value As Integer)
            '    m_Version = value
            'End Set
        End Property

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

                            Console.WriteLine("Destination Folder: {0}", DestinationFolder)
                            Console.WriteLine("Sub Folder: {0}", strSubFolder)

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

        Function getObjectType(ByRef invalue As Object, ByRef OrigType As String) As String

            Dim thisType As String = DirectCast(invalue.ToString, String)

            Dim retType As String = "Int32"
            If IsNumeric(thisType) = False Then
                retType = "String"
            Else
                Dim blnInt32 As Boolean = False
                Dim blnInt64 As Boolean = False
                Dim blnDouble As Boolean = False
                Try
                    Dim MyInt32 As Int32 = DirectCast(invalue, Int32)
                    blnInt32 = True
                Catch
                    blnInt32 = False
                End Try

                Try
                    Dim MyInt64 As Int64 = DirectCast(invalue, Int64)
                    blnInt64 = True
                Catch
                    blnInt64 = False
                End Try

                Try
                    Dim MyDouble As Double = DirectCast(invalue, Double)
                    blnDouble = True
                Catch
                    blnDouble = False
                End Try

                If blnInt64 = False And blnDouble = False Then
                    retType = "Int32"
                ElseIf blnInt64 = True Then
                    retType = "Int64"
                ElseIf blnDouble = True Then
                    retType = "Double"
                End If

            End If

            'retType = thisType

            Select Case OrigType
                Case "String"
                    'If we've been set at String, nothing should change it again
                    'retType = OrigType
                Case "Int32"
                    'Default Type
                Case "Int64"
                    If retType = "Int32" Then retType = OrigType
                Case "Double"
                    'If we've been set at Double, nothing should lower it again
                    retType = OrigType
                Case ""

                Case Else
                    retType = "Int32"
            End Select

            Return retType
        End Function

        Function getObjectTypeOld(ByRef invalue As Object, ByRef OrigType As String) As String
            Dim type = invalue.GetType()
            Dim thisType As String = type.ToString().Replace("System.", "")
            Dim retType As String = thisType

            Select Case OrigType
                Case "String"
                    'If we've been set at String, nothing should change it again
                    ' retType = OrigType
                Case "Int32"
                    'Default Type
                Case "Int64"
                    If retType = "Int32" Then retType = OrigType
                Case "Double"
                    'If we've been set at Double, nothing should lower it again
                    retType = OrigType
                Case Else
                    retType = "Int32"
            End Select
            Return retType
        End Function

    End Module
End Namespace