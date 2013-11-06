Imports MaNGOSDatabaseTool.Framework.Database
Public Class frmMain

    Private Sub btnQuit_Click(sender As Object, e As EventArgs) Handles btnQuit.Click
        End
    End Sub

    'Private Sub btnTabPage_Click(sender As Object, e As EventArgs)
    '    sender.enabled = False
    '    Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
    '    Dim TableName As String = sender.name 'btnReloadaccount_access
    '    TableName = TableName.Substring(9, TableName.Length - 9)
    '    Dim thisResult As New Framework.Database.SQLResult
    '    thisResult = DB.World.Select("SELECT * FROM " & TableName)
    '    If Not IsNothing(thisResult) Then
    '        If thisResult.Count <> 0 Then

    '            Dim NewTab As TabPage
    '            Dim Temp As DataGridView
    '            NewTab = Me.tabMain.Controls.Item(TableName)
    '            TableName = "DataGridView" & TableName
    '            Temp = NewTab.Controls.Item(TableName)
    '            '                Temp.Visible = False
    '            Temp.DataSource = thisResult
    '            '              Application.DoEvents()
    '            '               Temp.Visible = True
    '            For TempCol As Integer = 0 To Temp.ColumnCount - 1
    '                Temp.Columns(TempCol).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
    '            Next
    '            Temp.Columns(Temp.ColumnCount - 1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

    '        End If
    '    End If
    '    Me.Cursor = System.Windows.Forms.Cursors.Default
    '    sender.enabled = True
    'End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim thisResult As New Framework.Database.SQLResult
        Dim npcflag As New Dictionary(Of String, Int32)
        Dim columnName As String
        '        npcflag.Add("npcflag", 0)
        '        thisResult = DB.World.Select("SELECT table_name FROM information_schema.tables WHERE table_type = 'base table' AND table_schema='" & Form1.txtSQLWorldDB.Text & "'")
        thisResult = DB.World.Select("SELECT `COLUMN_NAME` FROM `INFORMATION_SCHEMA`.`COLUMNS` WHERE `TABLE_SCHEMA`='" & Form1.txtSQLWorldDB.Text & "' AND `TABLE_NAME`='db_version' LIMIT 2,1;")
        If thisResult.Count <> 0 Then
            columnName = thisResult.Rows(0).Item(0).ToString()
            lstDBName.Items.Add("World DB Version:")
            lstOutput.Items.Add(columnName.Replace("required,", ""))
        Else
            MsgBox(thisResult.ErrorMsg)
        End If

        thisResult = DB.Characters.Select("SELECT `COLUMN_NAME` FROM `INFORMATION_SCHEMA`.`COLUMNS` WHERE `TABLE_SCHEMA`='" & Form1.txtSQLCharacterDB.Text & "' AND `TABLE_NAME`='character_db_version';")
        If thisResult.Count <> 0 Then
            columnName = thisResult.Rows(0).Item(0).ToString()
            lstDBName.Items.Add("Characters DB Version:")
            lstOutput.Items.Add(columnName.Replace("required,", ""))
        Else
            MsgBox(thisResult.ErrorMsg)
        End If

        thisResult = DB.Realms.Select("SELECT `COLUMN_NAME` FROM `INFORMATION_SCHEMA`.`COLUMNS` WHERE `TABLE_SCHEMA`='" & Form1.txtSQLRealmDB.Text & "' AND `TABLE_NAME`='realmd_db_version';")
        If thisResult.Count <> 0 Then
            columnName = thisResult.Rows(0).Item(0).ToString()
            lstDBName.Items.Add("Realm DB Version:")
            lstOutput.Items.Add(columnName.Replace("required,", ""))
        Else
            MsgBox(thisResult.ErrorMsg)
        End If
    End Sub
End Class