Public Class WoWLauncherMain

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        WoWLauncherOptions.Visible = False
        PictureBox1.Parent = Me
        PictureBox2.Parent = Me
        PictureBox3.Parent = Me
        PictureBox4.Parent = Me
        PictureBox1.BackColor = Color.Transparent
        PictureBox2.BackColor = Color.Transparent
        PictureBox3.BackColor = Color.Transparent
        PictureBox4.BackColor = Color.Transparent
    End Sub

    Private Sub PictureBox1_MouseEnter(sender As System.Object, e As System.EventArgs) Handles PictureBox1.MouseEnter
        PictureBox1.BackgroundImage = My.Resources.play2
    End Sub

    Private Sub PictureBox1_MouseLeave(sender As System.Object, e As System.EventArgs) Handles PictureBox1.MouseLeave
        PictureBox1.Parent = Me
        PictureBox1.BackgroundImage = Nothing
    End Sub

    Private Sub PictureBox2_MouseEnter(sender As System.Object, e As System.EventArgs) Handles PictureBox2.MouseEnter
        PictureBox2.BackgroundImage = My.Resources.ptr2
    End Sub

    Private Sub PictureBox2_MouseLeave(sender As System.Object, e As System.EventArgs) Handles PictureBox2.MouseLeave
        PictureBox2.Parent = Me
        PictureBox2.BackgroundImage = Nothing
    End Sub

    Private Sub PictureBox3_MouseEnter(sender As System.Object, e As System.EventArgs) Handles PictureBox3.MouseEnter
        PictureBox3.BackgroundImage = My.Resources.support2
    End Sub

    Private Sub PictureBox3_MouseLeave(sender As System.Object, e As System.EventArgs) Handles PictureBox3.MouseLeave
        PictureBox3.Parent = Me
        PictureBox3.BackgroundImage = Nothing
    End Sub

    Private Sub PictureBox4_MouseEnter(sender As System.Object, e As System.EventArgs) Handles PictureBox4.MouseEnter
        PictureBox4.BackgroundImage = My.Resources.options2
    End Sub

    Private Sub PictureBox4_MouseLeave(sender As System.Object, e As System.EventArgs) Handles PictureBox4.MouseLeave
        PictureBox4.Parent = Me
        PictureBox4.BackgroundImage = Nothing
    End Sub

    Private Sub PictureBox1_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseClick
        PictureBox1.BackgroundImage = My.Resources.play3
    End Sub

    Private Sub PictureBox2_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles PictureBox2.MouseClick
        PictureBox2.BackgroundImage = My.Resources.ptr3
    End Sub

    Private Sub PictureBox3_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles PictureBox3.MouseClick
        PictureBox3.BackgroundImage = My.Resources.support3
    End Sub

    Private Sub PictureBox4_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles PictureBox4.MouseClick
        PictureBox4.BackgroundImage = My.Resources.options3
        WoWLauncherOptions.Visible = True
    End Sub
End Class
