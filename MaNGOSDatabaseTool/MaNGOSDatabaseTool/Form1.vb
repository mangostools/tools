Imports MaNGOSDatabaseTool.Framework.Database

Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'If SQLConnection.Connect(tboxmysqlhost.Text, tboxmysqlname.Text, tboxmysqlpw.Text, tboxmysqlwordldb.Text) Then
        '    Datastores.dbused = True
        '    Me.Close()
        'Else
        '    MessageBox.Show(SQLConnection.[error].Message)
        'End If
        'DB.Characters.Init(WorldConfig.CharDBHost, WorldConfig.CharDBUser, WorldConfig.CharDBPassword, WorldConfig.CharDBDataBase, WorldConfig.CharDBPort);
        'DB.Realms.Init(RealmConfig.RealmDBHost, RealmConfig.RealmDBUser, RealmConfig.RealmDBPassword, RealmConfig.RealmDBDataBase, RealmConfig.RealmDBPort);
        DB.World.Init(tboxmysqlhost.Text, tboxmysqlname.Text, tboxmysqlpw.Text, tboxmysqlwordldb.Text, tboxmysqlport.Text)
    End Sub
End Class
