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
        Me.MySQLConnect = New System.Windows.Forms.Button()
        Me.txtSQLHost = New System.Windows.Forms.TextBox()
        Me.txtSQLPort = New System.Windows.Forms.TextBox()
        Me.txtSQLUser = New System.Windows.Forms.TextBox()
        Me.txtSQLPassword = New System.Windows.Forms.TextBox()
        Me.txtSQLWorldDB = New System.Windows.Forms.TextBox()
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
        'txtSQLHost
        '
        Me.txtSQLHost.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSQLHost.Location = New System.Drawing.Point(118, 28)
        Me.txtSQLHost.Multiline = True
        Me.txtSQLHost.Name = "txtSQLHost"
        Me.txtSQLHost.Size = New System.Drawing.Size(162, 21)
        Me.txtSQLHost.TabIndex = 2
        Me.txtSQLHost.Text = "127.0.0.1"
        Me.txtSQLHost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtSQLPort
        '
        Me.txtSQLPort.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSQLPort.Location = New System.Drawing.Point(118, 68)
        Me.txtSQLPort.Multiline = True
        Me.txtSQLPort.Name = "txtSQLPort"
        Me.txtSQLPort.Size = New System.Drawing.Size(162, 21)
        Me.txtSQLPort.TabIndex = 3
        Me.txtSQLPort.Text = "3306"
        Me.txtSQLPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtSQLUser
        '
        Me.txtSQLUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSQLUser.Location = New System.Drawing.Point(118, 108)
        Me.txtSQLUser.Multiline = True
        Me.txtSQLUser.Name = "txtSQLUser"
        Me.txtSQLUser.Size = New System.Drawing.Size(162, 21)
        Me.txtSQLUser.TabIndex = 4
        Me.txtSQLUser.Text = "mangos"
        Me.txtSQLUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtSQLPassword
        '
        Me.txtSQLPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSQLPassword.Location = New System.Drawing.Point(118, 147)
        Me.txtSQLPassword.Multiline = True
        Me.txtSQLPassword.Name = "txtSQLPassword"
        Me.txtSQLPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtSQLPassword.Size = New System.Drawing.Size(162, 21)
        Me.txtSQLPassword.TabIndex = 5
        Me.txtSQLPassword.Text = "mangos"
        Me.txtSQLPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtSQLWorldDB
        '
        Me.txtSQLWorldDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSQLWorldDB.Location = New System.Drawing.Point(118, 187)
        Me.txtSQLWorldDB.Multiline = True
        Me.txtSQLWorldDB.Name = "txtSQLWorldDB"
        Me.txtSQLWorldDB.Size = New System.Drawing.Size(162, 21)
        Me.txtSQLWorldDB.TabIndex = 6
        Me.txtSQLWorldDB.Text = "mangos"
        Me.txtSQLWorldDB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
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
        Me.Controls.Add(Me.txtSQLWorldDB)
        Me.Controls.Add(Me.txtSQLPassword)
        Me.Controls.Add(Me.txtSQLUser)
        Me.Controls.Add(Me.txtSQLPort)
        Me.Controls.Add(Me.txtSQLHost)
        Me.Controls.Add(Me.MySQLConnect)
        Me.Controls.Add(Me.PictureBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "n bv"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents MySQLConnect As System.Windows.Forms.Button
    Friend WithEvents txtSQLHost As System.Windows.Forms.TextBox
    Friend WithEvents txtSQLPort As System.Windows.Forms.TextBox
    Friend WithEvents txtSQLUser As System.Windows.Forms.TextBox
    Friend WithEvents txtSQLPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtSQLWorldDB As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MySqlPort As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon

End Class
