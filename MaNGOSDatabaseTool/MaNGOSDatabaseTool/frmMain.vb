Imports MaNGOSDatabaseTool.Framework.Database
Public Class frmMain

    Private Sub btnQuit_Click(sender As Object, e As EventArgs) Handles btnQuit.Click
        End
    End Sub

    'Private Sub btnReload_Click(sender As Object, e As EventArgs)
    '    Dim thisResult As New Framework.Database.SQLResult
    '    Dim npcflag As New Dictionary(Of String, Int32)
    '    npcflag.Add("npcflag", 0)
    '    thisResult = DB.World.Select("SELECT * FROM creature_template WHERE npcflag<> @npcflag", npcflag)

    '    If thisResult.Count <> 0 Then
    '        Me.DataGridView1.DataSource = thisResult
    '    End If
    'End Sub

    'Private Sub btnReloadItems_Click(sender As Object, e As EventArgs)
    '    Dim thisResult As New Framework.Database.SQLResult
    '    Dim npcflag As New Dictionary(Of String, Int32)
    '    npcflag.Add("npcflag", 0)
    '    thisResult = DB.World.Select("SELECT Name, entry, class, subclass, displayid,quality FROM item_template ORDER BY Class, SubClass, Name")

    '    If thisResult.Count <> 0 Then
    '        Me.DataGridView2.DataSource = thisResult
    '    End If

    'End Sub

    'Private Sub btnREloadGameObjects_Click(sender As Object, e As EventArgs)
    '    Dim thisResult As New Framework.Database.SQLResult
    '    Dim npcflag As New Dictionary(Of String, Int32)
    '    npcflag.Add("npcflag", 0)
    '    thisResult = DB.World.Select("SELECT NAME, faction, TYPE, entry, displayid,scriptname FROM Gameobject_template")

    '    If thisResult.Count <> 0 Then
    '        Me.DataGridView3.DataSource = thisResult
    '    End If

    'End Sub

    'Private Sub btnReloadCreatureAi_Click(sender As Object, e As EventArgs)
    '    Dim thisResult As New Framework.Database.SQLResult
    '    Dim npcflag As New Dictionary(Of String, Int32)
    '    npcflag.Add("npcflag", 0)
    '    thisResult = DB.World.Select("SELECT * FROM creature_ai_scripts")

    '    If thisResult.Count <> 0 Then
    '        Me.DataGridView4.DataSource = thisResult
    '    End If
    'End Sub

    Private Sub btnTabPage_Click(sender As Object, e As EventArgs)
        Dim TableName As String = sender.name 'btnReloadaccount_access
        TableName = TableName.Substring(9, TableName.Length - 9)
        Dim thisResult As New Framework.Database.SQLResult
        thisResult = DB.World.Select("SELECT * FROM " & TableName)
        If Not IsNothing(thisResult) Then
            If thisResult.Count <> 0 Then

                Dim NewTab As TabPage
                Dim Temp As DataGridView
                NewTab = Me.tabMain.Controls.Item(TableName)
                TableName = "DataGridView" & TableName
                Temp = NewTab.Controls.Item(TableName)
                Temp.DataSource = thisResult
            End If
        End If
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim thisResult As New Framework.Database.SQLResult
        Dim npcflag As New Dictionary(Of String, Int32)
        '        npcflag.Add("npcflag", 0)
        thisResult = DB.World.Select("SELECT table_name FROM information_schema.tables WHERE table_type = 'base table' AND table_schema='test'")

        If thisResult.Count <> 0 Then
            Dim tabName As String
            For dbResults = 0 To thisResult.Count - 1
                tabName = thisResult.Rows(dbResults).Item(0).ToString()
                Me.tabMain.TabPages.Add(tabName, tabName)

                '
                'btnReload
                '
                Dim btnTemp As New System.Windows.Forms.Button()
                btnTemp.Location = New System.Drawing.Point(6, 6)
                btnTemp.Name = "btnReload" & tabName
                btnTemp.Size = New System.Drawing.Size(75, 23)
                btnTemp.TabIndex = 4
                btnTemp.Text = "Reload"
                btnTemp.UseVisualStyleBackColor = True
                AddHandler btnTemp.Click, AddressOf Me.btnTabPage_Click
                '
                'DataGridView1
                '
                Dim DataGridViewTemp = New System.Windows.Forms.DataGridView()
                DataGridViewTemp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
                DataGridViewTemp.Location = New System.Drawing.Point(6, 35)
                DataGridViewTemp.Name = "DataGridView" & tabName
                DataGridViewTemp.Size = New System.Drawing.Size(886, 364)
                DataGridViewTemp.TabIndex = 3

                Me.tabMain.TabPages(tabName).Controls.Add(btnTemp)
                Me.tabMain.TabPages(tabName).Controls.Add(DataGridViewTemp)
            Next
        End If

        '        Me.tabMain.TabPages.Add("dddd" & Me.tabMain.TabPages.Count)
    End Sub
End Class