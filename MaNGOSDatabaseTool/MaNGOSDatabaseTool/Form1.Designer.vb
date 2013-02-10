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
<<<<<<< HEAD
        Me.Button1 = New System.Windows.Forms.Button()
        Me.tboxmysqlhost = New System.Windows.Forms.TextBox()
        Me.tboxmysqlport = New System.Windows.Forms.TextBox()
        Me.tboxmysqlname = New System.Windows.Forms.TextBox()
        Me.tboxmysqlpw = New System.Windows.Forms.TextBox()
        Me.tboxmysqlwordldb = New System.Windows.Forms.TextBox()
=======
        Me.MySQLConnect = New System.Windows.Forms.Button()
        Me.MySQLHost = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.MySQLPassword = New System.Windows.Forms.TextBox()
        Me.MySQLDatabase = New System.Windows.Forms.TextBox()
>>>>>>> c90b2ca99764c80a51ccf9713a0a89af6ed6a7d8
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MySqlPort = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = Global.MaNGOSDatabaseTool.My.Resources.Resources.MaNGOS
        Me.PictureBox1.Location = New System.Drawing.Point(3, 5)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(100, 100)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'MySQLConnect
        '
        Me.MySQLConnect.Cursor = System.Windows.Forms.Cursors.Hand
        Me.MySQLConnect.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MySQLConnect.Location = New System.Drawing.Point(12, 223)
        Me.MySQLConnect.Name = "MySQLConnect"
        Me.MySQLConnect.Size = New System.Drawing.Size(268, 40)
        Me.MySQLConnect.TabIndex = 1
        Me.MySQLConnect.Text = "Connect"
        Me.MySQLConnect.UseVisualStyleBackColor = True
        '
<<<<<<< HEAD
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
=======
        'MySQLHost
        '
        Me.MySQLHost.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MySQLHost.Location = New System.Drawing.Point(118, 28)
        Me.MySQLHost.Multiline = True
        Me.MySQLHost.Name = "MySQLHost"
        Me.MySQLHost.Size = New System.Drawing.Size(162, 21)
        Me.MySQLHost.TabIndex = 2
        Me.MySQLHost.Text = "127.0.0.1"
        Me.MySQLHost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(118, 68)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(162, 21)
        Me.TextBox2.TabIndex = 3
        Me.TextBox2.Text = "3306"
        Me.TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox3
        '
        Me.TextBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(118, 108)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(162, 21)
        Me.TextBox3.TabIndex = 4
        Me.TextBox3.Text = "mangos"
        Me.TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'MySQLPassword
        '
        Me.MySQLPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MySQLPassword.Location = New System.Drawing.Point(118, 147)
        Me.MySQLPassword.Multiline = True
        Me.MySQLPassword.Name = "MySQLPassword"
        Me.MySQLPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.MySQLPassword.Size = New System.Drawing.Size(162, 21)
        Me.MySQLPassword.TabIndex = 5
        Me.MySQLPassword.Text = "mangos"
        Me.MySQLPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'MySQLDatabase
        '
        Me.MySQLDatabase.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MySQLDatabase.Location = New System.Drawing.Point(118, 187)
        Me.MySQLDatabase.Multiline = True
        Me.MySQLDatabase.Name = "MySQLDatabase"
        Me.MySQLDatabase.Size = New System.Drawing.Size(162, 21)
        Me.MySQLDatabase.TabIndex = 6
        Me.MySQLDatabase.Text = "mangos"
        Me.MySQLDatabase.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
>>>>>>> c90b2ca99764c80a51ccf9713a0a89af6ed6a7d8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(118, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "MySQL Host"
        '
        'MySqlPort
        '
        Me.MySqlPort.AutoSize = True
        Me.MySqlPort.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MySqlPort.Location = New System.Drawing.Point(118, 52)
        Me.MySqlPort.Name = "MySqlPort"
        Me.MySqlPort.Size = New System.Drawing.Size(74, 13)
        Me.MySqlPort.TabIndex = 8
        Me.MySqlPort.Text = "MySQL Port"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(118, 92)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(77, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "MySQL User"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(118, 131)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(105, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "MySQL Password"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(118, 171)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(153, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "MaNGOS Database Name"
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
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.MySqlPort)
        Me.Controls.Add(Me.Label1)
<<<<<<< HEAD
        Me.Controls.Add(Me.tboxmysqlwordldb)
        Me.Controls.Add(Me.tboxmysqlpw)
        Me.Controls.Add(Me.tboxmysqlname)
        Me.Controls.Add(Me.tboxmysqlport)
        Me.Controls.Add(Me.tboxmysqlhost)
        Me.Controls.Add(Me.Button1)
=======
        Me.Controls.Add(Me.MySQLDatabase)
        Me.Controls.Add(Me.MySQLPassword)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.MySQLHost)
        Me.Controls.Add(Me.MySQLConnect)
>>>>>>> c90b2ca99764c80a51ccf9713a0a89af6ed6a7d8
        Me.Controls.Add(Me.PictureBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "©2013 MaNGOS"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
<<<<<<< HEAD
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents tboxmysqlhost As System.Windows.Forms.TextBox
    Friend WithEvents tboxmysqlport As System.Windows.Forms.TextBox
    Friend WithEvents tboxmysqlname As System.Windows.Forms.TextBox
    Friend WithEvents tboxmysqlpw As System.Windows.Forms.TextBox
    Friend WithEvents tboxmysqlwordldb As System.Windows.Forms.TextBox
=======
    Friend WithEvents MySQLConnect As System.Windows.Forms.Button
    Friend WithEvents MySQLHost As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents MySQLPassword As System.Windows.Forms.TextBox
    Friend WithEvents MySQLDatabase As System.Windows.Forms.TextBox
>>>>>>> c90b2ca99764c80a51ccf9713a0a89af6ed6a7d8
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MySqlPort As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon

End Class
