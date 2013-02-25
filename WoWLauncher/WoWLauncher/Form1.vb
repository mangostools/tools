Public Class Form1
    Dim IsDraggingForm As Boolean = False
    Private MousePos As New System.Drawing.Point(0, 0)

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown
        If e.Button = MouseButtons.Left Then
            IsDraggingForm = True
            MousePos = e.Location
        End If
    End Sub

    Private Sub Form1_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseUp
        If e.Button = MouseButtons.Left Then IsDraggingForm = False
    End Sub

    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseMove
        If IsDraggingForm Then
            Dim temp As Point = New Point(Me.Location + (e.Location - MousePos))
            Me.Location = temp
            temp = Nothing
        End If
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
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
End Class
