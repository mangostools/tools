Imports MaNGOSDatabaseTool.Framework.Database

Public Class Form1

    Private Sub MySQLConnect_Click(sender As Object, e As EventArgs) Handles MySQLConnect.Click
        'If SQLConnection.Connect(tboxmysqlhost.Text, tboxmysqlname.Text, tboxmysqlpw.Text, tboxmysqlwordldb.Text) Then
        '    Datastores.dbused = True
        '    Me.Close()
        'Else
        '    MessageBox.Show(SQLConnection.[error].Message)
        'End If
        Dim blnCharactersDBOpen As Boolean = False
        blnCharactersDBOpen = DB.Characters.Init(txtSQLHost.Text, txtSQLUser.Text, txtSQLPassword.Text, txtSQLCharacterDB.Text, txtSQLPort.Text)
        Dim blnRealmsDBOpen As Boolean = False
        blnRealmsDBOpen = DB.Realms.Init(txtSQLHost.Text, txtSQLUser.Text, txtSQLPassword.Text, txtSQLRealmDB.Text, txtSQLPort.Text)
        Dim blnWorldDBOpen As Boolean = False
        blnWorldDBOpen = DB.World.Init(txtSQLHost.Text, txtSQLUser.Text, txtSQLPassword.Text, txtSQLWorldDB.Text, txtSQLPort.Text)

        If blnCharactersDBOpen = True And blnRealmsDBOpen = True And blnWorldDBOpen = True Then
            Dim tform As New frmMain
            tform.Show()
            Me.Hide()
        End If

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
