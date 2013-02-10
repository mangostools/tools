Imports MaNGOSDatabaseTool.Framework.Database

Public Class Form1

    Private Sub MySQLConnect_Click(sender As Object, e As EventArgs) Handles MySQLConnect.Click
        'If SQLConnection.Connect(tboxmysqlhost.Text, tboxmysqlname.Text, tboxmysqlpw.Text, tboxmysqlwordldb.Text) Then
        '    Datastores.dbused = True
        '    Me.Close()
        'Else
        '    MessageBox.Show(SQLConnection.[error].Message)
        'End If
        'DB.Characters.Init(WorldConfig.CharDBHost, WorldConfig.CharDBUser, WorldConfig.CharDBPassword, WorldConfig.CharDBDataBase, WorldConfig.CharDBPort);
        'DB.Realms.Init(RealmConfig.RealmDBHost, RealmConfig.RealmDBUser, RealmConfig.RealmDBPassword, RealmConfig.RealmDBDataBase, RealmConfig.RealmDBPort);
        Dim blnDBOpen As Boolean = False
        blnDBOpen = DB.World.Init(tboxmysqlhost.Text, tboxmysqlname.Text, tboxmysqlpw.Text, tboxmysqlwordldb.Text, tboxmysqlport.Text)

        If blnDBOpen = True Then
            Dim tform As New frmMain
            tform.Show()
            Me.Hide()
        End If

    End Sub

End Class
