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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtnQuit = New System.Windows.Forms.Button()
        Me.txtOutputFolder = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkDBC = New System.Windows.Forms.CheckBox()
        Me.chkSQL = New System.Windows.Forms.CheckBox()
        Me.chkCSV = New System.Windows.Forms.CheckBox()
        Me.lstMainLog = New System.Windows.Forms.ListBox()
        Me.brnWDB = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnStartDBC
        '
        Me.btnStartDBC.Location = New System.Drawing.Point(410, 429)
        Me.btnStartDBC.Name = "btnStartDBC"
        Me.btnStartDBC.Size = New System.Drawing.Size(68, 23)
        Me.btnStartDBC.TabIndex = 0
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
        Me.txtBaseFolder.Location = New System.Drawing.Point(102, 6)
        Me.txtBaseFolder.Name = "txtBaseFolder"
        Me.txtBaseFolder.Size = New System.Drawing.Size(450, 20)
        Me.txtBaseFolder.TabIndex = 2
        Me.txtBaseFolder.Text = "W:\World of Warcraft"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 58)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Processed Files"
        '
        'BtnQuit
        '
        Me.BtnQuit.Location = New System.Drawing.Point(484, 429)
        Me.BtnQuit.Name = "BtnQuit"
        Me.BtnQuit.Size = New System.Drawing.Size(68, 23)
        Me.BtnQuit.TabIndex = 6
        Me.BtnQuit.Text = "E&xit"
        Me.BtnQuit.UseVisualStyleBackColor = True
        '
        'txtOutputFolder
        '
        Me.txtOutputFolder.Location = New System.Drawing.Point(102, 32)
        Me.txtOutputFolder.Name = "txtOutputFolder"
        Me.txtOutputFolder.Size = New System.Drawing.Size(450, 20)
        Me.txtOutputFolder.TabIndex = 8
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
        Me.chkDBC.TabIndex = 9
        Me.chkDBC.Text = "Extract DBC's"
        Me.chkDBC.UseVisualStyleBackColor = True
        '
        'chkSQL
        '
        Me.chkSQL.AutoSize = True
        Me.chkSQL.Location = New System.Drawing.Point(12, 110)
        Me.chkSQL.Name = "chkSQL"
        Me.chkSQL.Size = New System.Drawing.Size(80, 17)
        Me.chkSQL.TabIndex = 10
        Me.chkSQL.Text = "Export SQL"
        Me.chkSQL.UseVisualStyleBackColor = True
        '
        'chkCSV
        '
        Me.chkCSV.AutoSize = True
        Me.chkCSV.Location = New System.Drawing.Point(12, 133)
        Me.chkCSV.Name = "chkCSV"
        Me.chkCSV.Size = New System.Drawing.Size(80, 17)
        Me.chkCSV.TabIndex = 11
        Me.chkCSV.Text = "Export CSV"
        Me.chkCSV.UseVisualStyleBackColor = True
        '
        'lstMainLog
        '
        Me.lstMainLog.FormattingEnabled = True
        Me.lstMainLog.Location = New System.Drawing.Point(102, 58)
        Me.lstMainLog.Name = "lstMainLog"
        Me.lstMainLog.Size = New System.Drawing.Size(450, 368)
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
        'MaNGOSExtractor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(564, 458)
        Me.Controls.Add(Me.brnWDB)
        Me.Controls.Add(Me.lstMainLog)
        Me.Controls.Add(Me.chkCSV)
        Me.Controls.Add(Me.chkSQL)
        Me.Controls.Add(Me.chkDBC)
        Me.Controls.Add(Me.txtOutputFolder)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.BtnQuit)
        Me.Controls.Add(Me.Label2)
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
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnStartDBC As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtBaseFolder As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents BtnQuit As System.Windows.Forms.Button
    Friend WithEvents txtOutputFolder As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkDBC As System.Windows.Forms.CheckBox
    Friend WithEvents chkSQL As System.Windows.Forms.CheckBox
    Friend WithEvents chkCSV As System.Windows.Forms.CheckBox
    Friend WithEvents lstMainLog As System.Windows.Forms.ListBox
    Friend WithEvents brnWDB As System.Windows.Forms.Button

End Class
