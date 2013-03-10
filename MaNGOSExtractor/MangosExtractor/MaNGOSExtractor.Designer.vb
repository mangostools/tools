<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MaNGOSExtractor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MaNGOSExtractor))
        Me.btnStartDBC = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtBaseFolder = New System.Windows.Forms.TextBox()
        Me.BtnQuit = New System.Windows.Forms.Button()
        Me.txtOutputFolder = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkDBC = New System.Windows.Forms.CheckBox()
        Me.chkSQL = New System.Windows.Forms.CheckBox()
        Me.chkCSV = New System.Windows.Forms.CheckBox()
        Me.lstMainLog = New System.Windows.Forms.ListBox()
        Me.brnWDB = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnSelectBaseFolder = New System.Windows.Forms.Button()
        Me.btnSelectOutputFolder = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnStartDBC
        '
        Me.btnStartDBC.Location = New System.Drawing.Point(720, 4)
        Me.btnStartDBC.Name = "btnStartDBC"
        Me.btnStartDBC.Size = New System.Drawing.Size(68, 23)
        Me.btnStartDBC.TabIndex = 8
        Me.btnStartDBC.Text = "&Start"
        Me.btnStartDBC.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Wow Root Folder"
        '
        'txtBaseFolder
        '
        Me.txtBaseFolder.BackColor = System.Drawing.Color.Silver
        Me.txtBaseFolder.Location = New System.Drawing.Point(102, 6)
        Me.txtBaseFolder.Name = "txtBaseFolder"
        Me.txtBaseFolder.Size = New System.Drawing.Size(566, 20)
        Me.txtBaseFolder.TabIndex = 1
        Me.txtBaseFolder.Text = "W:\World of Warcraft"
        '
        'BtnQuit
        '
        Me.BtnQuit.Location = New System.Drawing.Point(720, 30)
        Me.BtnQuit.Name = "BtnQuit"
        Me.BtnQuit.Size = New System.Drawing.Size(68, 23)
        Me.BtnQuit.TabIndex = 9
        Me.BtnQuit.Text = "E&xit"
        Me.BtnQuit.UseVisualStyleBackColor = True
        '
        'txtOutputFolder
        '
        Me.txtOutputFolder.BackColor = System.Drawing.Color.Silver
        Me.txtOutputFolder.Location = New System.Drawing.Point(102, 32)
        Me.txtOutputFolder.Name = "txtOutputFolder"
        Me.txtOutputFolder.Size = New System.Drawing.Size(566, 20)
        Me.txtOutputFolder.TabIndex = 3
        Me.txtOutputFolder.Text = "W:\World of Warcraft\Extracted"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(25, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Output Folder"
        '
        'chkDBC
        '
        Me.chkDBC.AutoSize = True
        Me.chkDBC.Location = New System.Drawing.Point(12, 87)
        Me.chkDBC.Name = "chkDBC"
        Me.chkDBC.Size = New System.Drawing.Size(91, 17)
        Me.chkDBC.TabIndex = 5
        Me.chkDBC.Text = "Extract DBC's"
        Me.chkDBC.UseVisualStyleBackColor = True
        '
        'chkSQL
        '
        Me.chkSQL.AutoSize = True
        Me.chkSQL.Location = New System.Drawing.Point(12, 110)
        Me.chkSQL.Name = "chkSQL"
        Me.chkSQL.Size = New System.Drawing.Size(80, 17)
        Me.chkSQL.TabIndex = 6
        Me.chkSQL.Text = "Export SQL"
        Me.chkSQL.UseVisualStyleBackColor = True
        '
        'chkCSV
        '
        Me.chkCSV.AutoSize = True
        Me.chkCSV.Location = New System.Drawing.Point(12, 133)
        Me.chkCSV.Name = "chkCSV"
        Me.chkCSV.Size = New System.Drawing.Size(80, 17)
        Me.chkCSV.TabIndex = 7
        Me.chkCSV.Text = "Export CSV"
        Me.chkCSV.UseVisualStyleBackColor = True
        '
        'lstMainLog
        '
        Me.lstMainLog.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.lstMainLog.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lstMainLog.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstMainLog.ForeColor = System.Drawing.Color.Black
        Me.lstMainLog.FormattingEnabled = True
        Me.lstMainLog.ItemHeight = 11
        Me.lstMainLog.Location = New System.Drawing.Point(102, 58)
        Me.lstMainLog.Name = "lstMainLog"
        Me.lstMainLog.Size = New System.Drawing.Size(686, 363)
        Me.lstMainLog.TabIndex = 13
        '
        'brnWDB
        '
        Me.brnWDB.Location = New System.Drawing.Point(102, 432)
        Me.brnWDB.Name = "brnWDB"
        Me.brnWDB.Size = New System.Drawing.Size(82, 23)
        Me.brnWDB.TabIndex = 14
        Me.brnWDB.Text = "&WDB to SQL"
        Me.brnWDB.UseVisualStyleBackColor = True
        Me.brnWDB.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.MangosExtractor.My.Resources.Resources.mangos_foundation
        Me.PictureBox1.Location = New System.Drawing.Point(5, 421)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(88, 31)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 15
        Me.PictureBox1.TabStop = False
        '
        'btnSelectBaseFolder
        '
        Me.btnSelectBaseFolder.Font = New System.Drawing.Font("Arial Narrow", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectBaseFolder.Location = New System.Drawing.Point(671, 4)
        Me.btnSelectBaseFolder.Margin = New System.Windows.Forms.Padding(0)
        Me.btnSelectBaseFolder.Name = "btnSelectBaseFolder"
        Me.btnSelectBaseFolder.Size = New System.Drawing.Size(23, 23)
        Me.btnSelectBaseFolder.TabIndex = 2
        Me.btnSelectBaseFolder.Text = "…"
        Me.btnSelectBaseFolder.UseVisualStyleBackColor = True
        '
        'btnSelectOutputFolder
        '
        Me.btnSelectOutputFolder.Font = New System.Drawing.Font("Arial Narrow", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectOutputFolder.Location = New System.Drawing.Point(671, 30)
        Me.btnSelectOutputFolder.Margin = New System.Windows.Forms.Padding(0)
        Me.btnSelectOutputFolder.Name = "btnSelectOutputFolder"
        Me.btnSelectOutputFolder.Size = New System.Drawing.Size(23, 23)
        Me.btnSelectOutputFolder.TabIndex = 4
        Me.btnSelectOutputFolder.Text = "…"
        Me.btnSelectOutputFolder.UseVisualStyleBackColor = True
        '
        'MaNGOSExtractor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ClientSize = New System.Drawing.Size(794, 458)
        Me.Controls.Add(Me.btnSelectOutputFolder)
        Me.Controls.Add(Me.btnSelectBaseFolder)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.brnWDB)
        Me.Controls.Add(Me.lstMainLog)
        Me.Controls.Add(Me.chkCSV)
        Me.Controls.Add(Me.chkSQL)
        Me.Controls.Add(Me.chkDBC)
        Me.Controls.Add(Me.txtOutputFolder)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.BtnQuit)
        Me.Controls.Add(Me.txtBaseFolder)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnStartDBC)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MaNGOSExtractor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "MaNGOSExtractor"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnStartDBC As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtBaseFolder As System.Windows.Forms.TextBox
    Friend WithEvents BtnQuit As System.Windows.Forms.Button
    Friend WithEvents txtOutputFolder As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkDBC As System.Windows.Forms.CheckBox
    Friend WithEvents chkSQL As System.Windows.Forms.CheckBox
    Friend WithEvents chkCSV As System.Windows.Forms.CheckBox
    Friend WithEvents lstMainLog As System.Windows.Forms.ListBox
    Friend WithEvents brnWDB As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents btnSelectBaseFolder As System.Windows.Forms.Button
    Friend WithEvents btnSelectOutputFolder As System.Windows.Forms.Button

End Class
