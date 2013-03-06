Imports System
Imports System.IO
Imports MangosExtractor.Core

Public Class MaNGOSExtractor
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

        If chkDBC.Checked = True Then
            'Set the Top level as {Wow Folder}\data
            myFolders = New System.IO.DirectoryInfo(txtBaseFolder.Text & "\data")

            'Add the Data folder to the collection before we start walking down the tree
            colFolders.Add(myFolders, myFolders.FullName)

            'Build a list of all the subfolders under data
            Core.ReadFolders(myFolders, colFolders)

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

            If txtOutputFolder.Text.EndsWith("\") = False Then txtOutputFolder.Text = txtOutputFolder.Text & "\"
            If My.Computer.FileSystem.DirectoryExists(txtOutputFolder.Text) = False Then
                Directory.CreateDirectory(txtOutputFolder.Text)
            End If


            For Each strItem As String In colBaseFiles
                ListBox1.Items.Add("  BASE: " & strItem)
                Try
                    Me.Text = strItem
                    Core.ExtractDBCFiles(strItem, "*.db*", txtOutputFolder.Text)
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
            Next

            For Each strItem As String In colMainFiles
                ListBox1.Items.Add("  FILE: " & strItem)
                Try
                    Me.Text = strItem
                    Core.ExtractDBCFiles(strItem, "*.db*", txtOutputFolder.Text)
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
            Next

            For Each strItem As String In colUpdateFiles
                ListBox1.Items.Add("UPDATE: " & strItem)

                Try
                    Me.Text = strItem
                    Core.ExtractDBCFiles(strItem, "*.db*", txtOutputFolder.Text)
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
                Application.DoEvents()
            Next
            Me.Text = ("Extraction Finished")
        End If

        'Now that we have all the DBC's extracted and patched, we need to check the export options and export data
        If chkSQL.Checked = True Then
            myFolders = New System.IO.DirectoryInfo(txtOutputFolder.Text & "\DBFilesClient")
            For Each file As System.IO.FileInfo In myFolders.GetFiles("*.DBC")
                Me.Text = "Extracting: " & file.Name
                Core.exportSQL(txtOutputFolder.Text & "\DBFilesClient" & "\" & file.Name)
                Application.DoEvents()
            Next
        End If
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

    Private Sub MaNGOSExtractor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Text = "MaNGOSExtractor" & Core.MaNGOSExtractorCore.Version()

        'Dim Test = "1"
        'MessageBox.Show(CheckValue(Test) & " Data:" & Test)

        'Test = "1.1"
        'MessageBox.Show(CheckValue(Test) & " Data:" & Test)

        'Test = "-14325.1455"
        'MessageBox.Show(Core.CheckValue(Test) & " Data:" & Test)
    End Sub

End Class