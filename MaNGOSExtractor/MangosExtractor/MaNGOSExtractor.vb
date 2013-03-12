Imports System
Imports System.IO
Imports MangosExtractor.Core
Imports System.Data

Public Class MaNGOSExtractor
    ''' <summary>
    ''' Starts the DBC Extraction process
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnStartDBC_Click(sender As Object, e As EventArgs) Handles btnStartDBC.Click
        lstMainLog.Items.Clear()
        Core.alertlist = lstMainLog
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        btnStartDBC.Enabled = False

        Dim colBaseFiles As New SortedList()    'Collection containing all the base files
        Dim colMainFiles As New SortedList()    'Collection containing all the main files
        Dim colUpdateFiles As New SortedList()  'Collection containing any update or patch files

        Dim colFolders As New Collection                'Collection to hold for the folders to be processed
        Dim myFolders As System.IO.DirectoryInfo

        If System.IO.Directory.Exists(txtBaseFolder.Text) = False Then
            Alert("Warcraft folder '" & txtBaseFolder.Text & "' can not be located", Core.AlertNewLine.AddCRLF)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            btnStartDBC.Enabled = True
            Exit Sub
        End If

        ReadWarcraftExe(txtBaseFolder.Text & "\Wow.exe")
        If Core.FullVersion <> "" Then
            Alert("Warcraft Version v" & Core.FullVersion & " Build " & Core.BuildNo, Core.AlertNewLine.AddCRLF)
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
                        colUpdateFiles.Add(file.FullName, file.FullName)
                    ElseIf file.FullName.ToLower.Contains("base") = True Then
                        colBaseFiles.Add(file.FullName, file.FullName)
                    Else
                        colMainFiles.Add(file.FullName, file.FullName)
                    End If
                Next
            Next

            If txtOutputFolder.Text.EndsWith("\") = False Then txtOutputFolder.Text = txtOutputFolder.Text & "\"
            If My.Computer.FileSystem.DirectoryExists(txtOutputFolder.Text) = False Then
                Directory.CreateDirectory(txtOutputFolder.Text)
            End If


            For Each strItem As DictionaryEntry In colBaseFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)
                Try
                    Core.ExtractDBCFiles(strItem.Value, "*.db?", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
            Next

            For Each strItem As DictionaryEntry In colMainFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)
                Try
                    Core.ExtractDBCFiles(strItem.Value, "*.db?", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
            Next

            For Each strItem As DictionaryEntry In colUpdateFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)

                Try
                    Core.ExtractDBCFiles(strItem.Value, "*.db?", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
                Threading.Thread.Sleep(0)
            Next
            Alert("Extraction Finished", Core.AlertNewLine.AddCRLF)
        End If

        If chkCSV.Checked = True Or chkSQL.Checked = True Or chkExportXML.Checked = True Then
            'Now that we have all the DBC's extracted and patched, we need to check the export options and export data
            ExportFiles(txtBaseFolder.Text, txtOutputFolder.Text, chkCSV.Checked, chkSQL.Checked, chkExportXML.Checked)
            Alert("Finished Exporting", Core.AlertNewLine.AddCRLF)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default
        btnStartDBC.Enabled = True
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
    ''' Set runningAsGui = true and set the alertlist to main screen listbox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MaNGOSExtractor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Text = "MaNGOSExtractor" & Core.MaNGOSExtractorCore.Version()
        Core.runAsGui = True
        Core.alertlist = lstMainLog

        'Dim Test As Object = 65536
        'MessageBox.Show(Core.getObjectType(Test, "Int32"))

    End Sub

    Private Sub brnWDB_Click(sender As Object, e As EventArgs) Handles brnWDB.Click
        lstMainLog.Items.Clear()
        Core.alertlist = lstMainLog
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.brnWDB.Enabled = False
        Dim colBaseFiles As New SortedList()    'Collection containing all the base files
        Dim colMainFiles As New SortedList()    'Collection containing all the main files
        Dim colUpdateFiles As New SortedList()  'Collection containing any update or patch files

        Dim colFolders As New Collection                'Collection to hold for the folders to be processed
        Dim myFolders As System.IO.DirectoryInfo

        If System.IO.Directory.Exists(txtBaseFolder.Text) = False Then
            Alert("Warcraft folder '" & txtBaseFolder.Text & "' can not be located", Core.AlertNewLine.AddCRLF)
            Exit Sub
        End If

        'Now that we have all the DBC's extracted and patched, we need to check the export options and export data
        txtOutputFolder.Text = txtOutputFolder.Text & "\WDBFiles"
        If txtOutputFolder.Text.EndsWith("\") = False Then txtOutputFolder.Text = txtOutputFolder.Text & "\"
        If My.Computer.FileSystem.DirectoryExists(txtOutputFolder.Text) = False Then
            Directory.CreateDirectory(txtOutputFolder.Text)
        End If


        'If chkCSV.Checked = True Or chkSQL.Checked = True Then
        'Now that we have all the DBC's extracted and patched, we need to check the export options and export data
        myFolders = New System.IO.DirectoryInfo(txtBaseFolder.Text & "\CACHE\WDB\engb")
        For Each file As System.IO.FileInfo In myFolders.GetFiles("*.WDB")
            Dim dbcDataTable As New DataTable

            'Load the entire DBC into a DataTable to be processed by both exports
            '                If chkCSV.Checked = True Or chkSQL.Checked = True Then
            Alert("Loading WBC " & file.Name & " into memory", Core.AlertNewLine.AddCRLF)
            loadDBCtoDataTable(txtBaseFolder.Text & "\CACHE\WDB\engb" & "\" & file.Name, dbcDataTable)
            Application.DoEvents()
            'End If

            ' If chkSQL.Checked = True Then
            Alert("Creating SQL for " & file.Name, Core.AlertNewLine.AddCRLF)
            exportSQL(txtOutputFolder.Text & "\" & file.Name, dbcDataTable, txtBaseFolder.Text)
            Application.DoEvents()
            ' End If


        Next
        Alert("Finished Exporting", Core.AlertNewLine.AddCRLF)
        'End If
        Me.Cursor = System.Windows.Forms.Cursors.Default
        brnWDB.Enabled = True
    End Sub

    Private Sub btnSelectBaseFolder_Click(sender As Object, e As EventArgs) Handles btnSelectBaseFolder.Click
        FolderBrowserDialog1.SelectedPath = txtBaseFolder.Text
        If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtBaseFolder.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub btnSelectOutputFolder_Click(sender As Object, e As EventArgs) Handles btnSelectOutputFolder.Click
        FolderBrowserDialog1.SelectedPath = txtOutputFolder.Text
        If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtOutputFolder.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
End Class