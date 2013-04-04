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

        If chkDBC.Checked = True Or chkExtractMaps.Checked = True Or chkExtractWMO.Checked = True Or chkExtractWDT.Checked = True Or chkExtractADT.Checked = True Then
            'Set the Top level as {Wow Folder}\data then load all the MPQ files
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
#If _MyType <> "Console" Then
                    Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
                Next
            Next

            If txtOutputFolder.Text.EndsWith("\") = False Then txtOutputFolder.Text = txtOutputFolder.Text & "\"
            If My.Computer.FileSystem.DirectoryExists(txtOutputFolder.Text) = False Then
                Directory.CreateDirectory(txtOutputFolder.Text)
            End If
        End If

        If chkDBC.Checked = True Then
            For Each strItem As DictionaryEntry In colBaseFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)
                Try
                    Core.ExtractDBCFiles(strItem.Value, "*.db?", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colMainFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)
                Try
                    Core.ExtractDBCFiles(strItem.Value, "*.db?", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colUpdateFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)

                Try
                    Core.ExtractDBCFiles(strItem.Value, "*.db?", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next
            Alert("Extraction Finished", Core.AlertNewLine.AddCRLF)
        End If

        If chkExtractADT.Checked = True Then
            For Each strItem As DictionaryEntry In colBaseFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)
                Try
                    Core.ExtractADTFiles(strItem.Value, "*.adt", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
                'Try
                '    Core.ExtractFilesGeneric(strItem.Value, "*.wdt", txtOutputFolder.Text)
                'Catch ex As Exception
                '    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                'End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colMainFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)
                Try
                    Core.ExtractADTFiles(strItem.Value, "*.adt", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
                'Try
                '    Core.ExtractFilesGeneric(strItem.Value, "*.wdt", txtOutputFolder.Text)
                'Catch ex As Exception
                '    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                'End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colUpdateFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)

                Try
                    Core.ExtractADTFiles(strItem.Value, "*.adt", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
                'Try
                '    Core.ExtractFilesGeneric(strItem.Value, "*.wdt", txtOutputFolder.Text)
                'Catch ex As Exception
                '    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                'End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next
        End If

        If chkExtractMaps.Checked = True Then
            'Load the DBC Data into the Dictionary dictMaps
            Dim dtMaps As New DataTable
            Dim dictMaps As New Dictionary(Of Integer, String)

            Alert("Loading Maps: ", Core.AlertNewLine.AddCRLF)

            dtMaps = loadDBCtoDataTable(txtOutputFolder.Text & "\DBFilesClient" & "\map.dbc")
            'Alert(dtMaps.Rows.Count() - 2 & " Maps Loaded", Core.AlertNewLine.AddCRLF)

            For counter As Integer = 0 To dtMaps.Rows.Count() - 2
                'Debug.WriteLine("MapID: {0} Name: {1}", dtMaps.Rows(counter)(0), dtMaps.Rows(counter)(1))
                dictMaps.Add(dtMaps.Rows(counter)(0), dtMaps.Rows(counter)(1))
            Next

            'Load the DBC Data into the Dictionary dictAreaTable
            Dim dtAreaTable As New DataTable
            Dim dictAreaTable As New Dictionary(Of Integer, String)

            Alert("Loading Areas: ", Core.AlertNewLine.AddCRLF)

            dtAreaTable = loadDBCtoDataTable(txtOutputFolder.Text & "\DBFilesClient" & "\AreaTable.dbc")
            'Alert(dtAreaTable.Rows.Count() - 2 & " Areas Loaded", Core.AlertNewLine.AddCRLF)

            For counter As Integer = 0 To dtAreaTable.Rows.Count() - 2
                dictAreaTable.Add(dtAreaTable.Rows(counter)(0), dtAreaTable.Rows(counter)(3))
                'dictAreaTable.Add(dtAreaTable.Rows(counter)(0), dtAreaTable.Rows(counter)(11))
            Next

            'Load the DBC Data into the Dictionary dictLiquidType
            Dim dtLiquidType As New DataTable
            Dim dictLiquidType As New Dictionary(Of Integer, Integer)

            Alert("Loading Liquid Types: ", Core.AlertNewLine.AddCRLF)
            dtLiquidType = loadDBCtoDataTable(txtOutputFolder.Text & "\DBFilesClient" & "\LiquidType.dbc")
            'Alert(dtLiquidType.Rows.Count() - 2 & " Liquids Loaded", Core.AlertNewLine.AddCRLF)


            For counter As Integer = 0 To dtLiquidType.Rows.Count() - 2
                dictLiquidType.Add(dtLiquidType.Rows(counter)(0), dtLiquidType.Rows(counter)(3))
            Next

            Alert("Extraction Finished", Core.AlertNewLine.AddCRLF)


            Alert(".... Converting Maps", Core.AlertNewLine.AddCRLF)
            Dim ADT_RES = 64
            Dim MapKey As String = "000"
            Dim MapX As String = "00"
            Dim MapY As String = "00"
            Dim ADTfilename As String = ""
            Dim MapFilename As String = ""

            If My.Computer.FileSystem.DirectoryExists(txtOutputFolder.Text & "maps\") = False Then
                Directory.CreateDirectory(txtOutputFolder.Text & "maps\")
            End If
            For Each thisMap As KeyValuePair(Of Integer, String) In dictMaps
                Alert(" Extracting..." & thisMap.Value, AlertNewLine.AddCRLF)
                For x As Integer = 0 To ADT_RES
                    For y As Integer = 0 To ADT_RES
                        ADTfilename = txtOutputFolder.Text & "World\Maps\" & thisMap.Value & "\" & thisMap.Value & "_" & x & "_" & y & ".adt"
                        If My.Computer.FileSystem.FileExists(ADTfilename) = True Then
                            Alert("Reading from: " & ADTfilename, Core.AlertNewLine.AddCRLF)
                            MapKey = thisMap.Key.ToString() '"000"
                            If MapKey.Length() = 1 Then MapKey = "00" & MapKey
                            If MapKey.Length() = 2 Then MapKey = "0" & MapKey
                            MapX = x.ToString() '"00"
                            If MapX.Length() = 1 Then MapX = "0" & MapX
                            MapY = y.ToString() '"00"
                            If MapY.Length() = 1 Then MapY = "0" & MapY


                            'ADTReader.Program.Dump(ADTfilename)
                            MapFilename = txtOutputFolder.Text & "maps\" & MapKey.Substring(0, 3) & MapY.Substring(0, 2) & MapX.Substring(0, 2) & ".map"
                            ADTReader.Program.ConvertADT(ADTfilename, MapFilename, x, y, dictMaps, dictAreaTable, dictLiquidType)
                            'Alert(" Writing to: " & MapFilename, Core.AlertNewLine.AddCRLF)
                            'ConvertADT(ADTfilename, MapFilename, dictMaps, dictAreaTable, dictLiquidType)
                        End If
#If _MyType <> "Console" Then
                            Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
                    Next
                Next
            Next
        End If


        If chkExtractWMO.Checked = True Then
            For Each strItem As DictionaryEntry In colBaseFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)
                Try
                    Core.ExtractFilesGeneric(strItem.Value, "*.wmo", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
                'Try
                '    Core.ExtractFilesGeneric(strItem.Value, "*.wdt", txtOutputFolder.Text)
                'Catch ex As Exception
                '    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                'End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colMainFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)
                Try
                    Core.ExtractFilesGeneric(strItem.Value, "*.wmo", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
                'Try
                '    Core.ExtractFilesGeneric(strItem.Value, "*.wdt", txtOutputFolder.Text)
                'Catch ex As Exception
                '    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                'End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colUpdateFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)

                Try
                    Core.ExtractFilesGeneric(strItem.Value, "*.wmo", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
                'Try
                '    Core.ExtractFilesGeneric(strItem.Value, "*.wdt", txtOutputFolder.Text)
                'Catch ex As Exception
                '    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                'End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next
        End If

        If chkExtractWDT.Checked = True Then
            For Each strItem As DictionaryEntry In colBaseFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)
                Try
                    Core.ExtractWDTFiles(strItem.Value, "*.wdt", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
                'Try
                '    Core.ExtractFilesGeneric(strItem.Value, "*.wdt", txtOutputFolder.Text)
                'Catch ex As Exception
                '    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                'End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colMainFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)
                Try
                    Core.ExtractWDTFiles(strItem.Value, "*.wdt", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
                'Try
                '    Core.ExtractFilesGeneric(strItem.Value, "*.wdt", txtOutputFolder.Text)
                'Catch ex As Exception
                '    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                'End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colUpdateFiles
                Alert("Reading: " & strItem.Value, Core.AlertNewLine.AddCRLF)

                Try
                    Core.ExtractWDTFiles(strItem.Value, "*.wdt", txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                End Try
                'Try
                '    Core.ExtractFilesGeneric(strItem.Value, "*.wdt", txtOutputFolder.Text)
                'Catch ex As Exception
                '    Alert(ex.Message, Core.AlertNewLine.AddCRLF)
                'End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next
        End If




        If chkCSV.Checked = True Or chkSQL.Checked = True Or chkExportXML.Checked = True Or chkExportMD.Checked = True Then
            'Now that we have all the DBC's extracted and patched, we need to check the export options and export data
            ExportDBCFiles(txtBaseFolder.Text, txtOutputFolder.Text, chkCSV.Checked, chkSQL.Checked, chkExportXML.Checked, chkExportMD.Checked)
            Alert("Finished Exporting", Core.AlertNewLine.AddCRLF)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default
        btnStartDBC.Enabled = True

        Alert("Finished", AlertNewLine.AddCRLF)
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

        Alert("Welcome to MaNGOS Extractor" & Core.MaNGOSExtractorCore.Version() & " for all versions of MaNGOS", AlertNewLine.AddCRLF)
        Alert("=============================================================", AlertNewLine.AddCRLF)
        Alert("", AlertNewLine.AddCRLF)
        Alert("What is working:-", AlertNewLine.AddCRLF)
        Alert("----------------------------------", AlertNewLine.AddCRLF)
        Alert("Extract and patch DBC", AlertNewLine.AddCRLF)
        Alert("Export to SQL/CSV/XML/MD", AlertNewLine.AddCRLF)
        Alert("Extract and patch ADT *", AlertNewLine.AddCRLF)
        Alert("Extract WMO/MDL *", AlertNewLine.AddCRLF)
        Alert("Extract and patch WDT *", AlertNewLine.AddCRLF)
 
        Alert("", AlertNewLine.AddCRLF)
        Alert("What is not working:-", AlertNewLine.AddCRLF)
        Alert("----------------------------------", AlertNewLine.AddCRLF)
        Alert("Extract Maps is WIP", AlertNewLine.AddCRLF)
        Alert("Extract VMaps", AlertNewLine.AddCRLF)
        Alert("", AlertNewLine.AddCRLF)
        Alert("* Equivilent to VMAP Extractor Functionality", AlertNewLine.AddCRLF)

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
#If _MyType <> "Console" Then
            Application.DoEvents()
#Else
                    Threading.Thread.Sleep(0)
#End If

            'End If

            ' If chkSQL.Checked = True Then
            Alert("Creating SQL for " & file.Name, Core.AlertNewLine.AddCRLF)
            exportSQL(txtOutputFolder.Text & "\" & file.Name, dbcDataTable, txtBaseFolder.Text)
#If _MyType <> "Console" Then
            Application.DoEvents()
#Else
                    Threading.Thread.Sleep(0)
#End If
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