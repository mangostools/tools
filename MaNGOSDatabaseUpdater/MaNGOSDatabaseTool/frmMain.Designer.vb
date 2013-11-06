<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnQuit = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lstOutput = New System.Windows.Forms.ListBox()
        Me.lstDBName = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'btnQuit
        '
        Me.btnQuit.Location = New System.Drawing.Point(411, 4)
        Me.btnQuit.Name = "btnQuit"
        Me.btnQuit.Size = New System.Drawing.Size(75, 23)
        Me.btnQuit.TabIndex = 1
        Me.btnQuit.Text = "Quit"
        Me.btnQuit.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'lstOutput
        '
        Me.lstOutput.FormattingEnabled = True
        Me.lstOutput.Location = New System.Drawing.Point(167, 33)
        Me.lstOutput.Name = "lstOutput"
        Me.lstOutput.Size = New System.Drawing.Size(319, 329)
        Me.lstOutput.TabIndex = 5
        '
        'lstDBName
        '
        Me.lstDBName.FormattingEnabled = True
        Me.lstDBName.Location = New System.Drawing.Point(12, 33)
        Me.lstDBName.Name = "lstDBName"
        Me.lstDBName.Size = New System.Drawing.Size(149, 329)
        Me.lstDBName.TabIndex = 6
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(500, 374)
        Me.Controls.Add(Me.lstDBName)
        Me.Controls.Add(Me.lstOutput)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnQuit)
        Me.Name = "frmMain"
        Me.Text = "frmMain"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnQuit As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lstOutput As System.Windows.Forms.ListBox
    Friend WithEvents lstDBName As System.Windows.Forms.ListBox
End Class
