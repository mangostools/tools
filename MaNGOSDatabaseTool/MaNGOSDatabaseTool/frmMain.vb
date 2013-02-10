Imports MaNGOSDatabaseTool.Framework.Database
Public Class frmMain

    Private Sub btnQuit_Click(sender As Object, e As EventArgs) Handles btnQuit.Click
        End
    End Sub

    Private Sub btnReload_Click(sender As Object, e As EventArgs) Handles btnReload.Click
        Dim thisResult As New Framework.Database.SQLResult
        Dim npcflag As New Dictionary(Of String, Int32)
        npcflag.Add("npcflag", 0)
        thisResult = DB.World.Select("SELECT * FROM creature_template WHERE npcflag<> @npcflag", npcflag)

        If thisResult.Count <> 0 Then
            Me.DataGridView1.DataSource = thisResult
        End If
    End Sub
End Class