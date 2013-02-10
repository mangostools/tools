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

    Private Sub btnReloadItems_Click(sender As Object, e As EventArgs) Handles btnReloadItems.Click
        Dim thisResult As New Framework.Database.SQLResult
        Dim npcflag As New Dictionary(Of String, Int32)
        npcflag.Add("npcflag", 0)
        thisResult = DB.World.Select("SELECT Name, entry, class, subclass, displayid,quality FROM item_template ORDER BY Class, SubClass, Name")

        If thisResult.Count <> 0 Then
            Me.DataGridView2.DataSource = thisResult
        End If

    End Sub

    Private Sub btnREloadGameObjects_Click(sender As Object, e As EventArgs) Handles btnReloadGameObjects.Click
        Dim thisResult As New Framework.Database.SQLResult
        Dim npcflag As New Dictionary(Of String, Int32)
        npcflag.Add("npcflag", 0)
        thisResult = DB.World.Select("SELECT NAME, faction, TYPE, entry, displayid,scriptname FROM Gameobject_template")

        If thisResult.Count <> 0 Then
            Me.DataGridView3.DataSource = thisResult
        End If

    End Sub

    Private Sub btnReloadCreatureAi_Click(sender As Object, e As EventArgs) Handles btnReloadCreatureAi.Click
        Dim thisResult As New Framework.Database.SQLResult
        Dim npcflag As New Dictionary(Of String, Int32)
        npcflag.Add("npcflag", 0)
        thisResult = DB.World.Select("SELECT * FROM creature_ai_scripts")

        If thisResult.Count <> 0 Then
            Me.DataGridView4.DataSource = thisResult
        End If
    End Sub
End Class