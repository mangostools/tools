<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.tboxmysqlhost = New System.Windows.Forms.TextBox()
        Me.tboxmysqlport = New System.Windows.Forms.TextBox()
        Me.tboxmysqlname = New System.Windows.Forms.TextBox()
        Me.tboxmysqlpw = New System.Windows.Forms.TextBox()
        Me.tboxmysqlwordldb = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = Global.MaNGOSDatabaseTool.My.Resources.Resources.MaNGOS
        Me.PictureBox1.Location = New System.Drawing.Point(1, 5)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(100, 100)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Button1
        '
        Me.Button1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(12, 223)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(268, 40)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Connect"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'tboxmysqlhost
        '
        Me.tboxmysqlhost.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tboxmysqlhost.Location = New System.Drawing.Point(118, 28)
        Me.tboxmysqlhost.Multiline = True
        Me.tboxmysqlhost.Name = "tboxmysqlhost"
        Me.tboxmysqlhost.Size = New System.Drawing.Size(162, 21)
        Me.tboxmysqlhost.TabIndex = 2
        Me.tboxmysqlhost.Text = "127.0.0.1"
        Me.tboxmysqlhost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tboxmysqlport
        '
        Me.tboxmysqlport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tboxmysqlport.Location = New System.Drawing.Point(118, 68)
        Me.tboxmysqlport.Multiline = True
        Me.tboxmysqlport.Name = "tboxmysqlport"
        Me.tboxmysqlport.Size = New System.Drawing.Size(162, 21)
        Me.tboxmysqlport.TabIndex = 3
        Me.tboxmysqlport.Text = "3306"
        Me.tboxmysqlport.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tboxmysqlname
        '
        Me.tboxmysqlname.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tboxmysqlname.Location = New System.Drawing.Point(118, 108)
        Me.tboxmysqlname.Multiline = True
        Me.tboxmysqlname.Name = "tboxmysqlname"
        Me.tboxmysqlname.Size = New System.Drawing.Size(162, 21)
        Me.tboxmysqlname.TabIndex = 4
        Me.tboxmysqlname.Text = "mangos"
        Me.tboxmysqlname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tboxmysqlpw
        '
        Me.tboxmysqlpw.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tboxmysqlpw.Location = New System.Drawing.Point(118, 147)
        Me.tboxmysqlpw.Multiline = True
        Me.tboxmysqlpw.Name = "tboxmysqlpw"
        Me.tboxmysqlpw.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.tboxmysqlpw.Size = New System.Drawing.Size(162, 21)
        Me.tboxmysqlpw.TabIndex = 5
        Me.tboxmysqlpw.Text = "mangos"
        Me.tboxmysqlpw.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tboxmysqlwordldb
        '
        Me.tboxmysqlwordldb.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tboxmysqlwordldb.Location = New System.Drawing.Point(118, 187)
        Me.tboxmysqlwordldb.Multiline = True
        Me.tboxmysqlwordldb.Name = "tboxmysqlwordldb"
        Me.tboxmysqlwordldb.Size = New System.Drawing.Size(162, 21)
        Me.tboxmysqlwordldb.TabIndex = 6
        Me.tboxmysqlwordldb.Text = "mangos"
        Me.tboxmysqlwordldb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(118, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(27, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "host"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(118, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(25, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "port"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(118, 92)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "mysql user"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(118, 131)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(81, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "mysql password"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(118, 171)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(91, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "mangos database"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(9, 108)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(89, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "©2013 MaNGOS"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "MaNGOS Database Tool"
        Me.NotifyIcon1.Visible = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGray
        Me.ClientSize = New System.Drawing.Size(292, 275)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tboxmysqlwordldb)
        Me.Controls.Add(Me.tboxmysqlpw)
        Me.Controls.Add(Me.tboxmysqlname)
        Me.Controls.Add(Me.tboxmysqlport)
        Me.Controls.Add(Me.tboxmysqlhost)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "MaNGOS Database Tool"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents tboxmysqlhost As System.Windows.Forms.TextBox
    Friend WithEvents tboxmysqlport As System.Windows.Forms.TextBox
    Friend WithEvents tboxmysqlname As System.Windows.Forms.TextBox
    Friend WithEvents tboxmysqlpw As System.Windows.Forms.TextBox
    Friend WithEvents tboxmysqlwordldb As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon

End Class
