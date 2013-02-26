Public Class WoWLauncherOptions

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If RadioButton1.Checked Then
            Dim lines() As String = IO.File.ReadAllLines("realmlist.wtf")
            lines(0) = "set realmlist" & TextBox1.Text
            IO.File.WriteAllLines("realmlist.wtf", lines)
            Label1.Text = "All Options Saved"
        Else
            Label1.Text = "Please select an option"
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            TextBox1.Enabled = True
            Label1.Text = Nothing
        End If
    End Sub
End Class