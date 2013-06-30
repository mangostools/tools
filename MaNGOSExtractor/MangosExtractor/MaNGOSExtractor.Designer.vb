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
        Me.lstMainLog = New System.Windows.Forms.ListBox()
        Me.brnWDB = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnSelectBaseFolder = New System.Windows.Forms.Button()
        Me.btnSelectOutputFolder = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkExtractVmap = New System.Windows.Forms.CheckBox()
        Me.chkExtractMaps = New System.Windows.Forms.CheckBox()
        Me.chkExportMD = New System.Windows.Forms.CheckBox()
        Me.chkExportXML = New System.Windows.Forms.CheckBox()
        Me.chkCSV = New System.Windows.Forms.CheckBox()
        Me.chkSQL = New System.Windows.Forms.CheckBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.chkExtractWDT = New System.Windows.Forms.CheckBox()
        Me.chkExtractWMO = New System.Windows.Forms.CheckBox()
        Me.chkExtractADT = New System.Windows.Forms.CheckBox()
        Me.chkDBC = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkExportH = New System.Windows.Forms.CheckBox()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnStartDBC
        '
        Me.btnStartDBC.BackColor = System.Drawing.Color.LightGray
        Me.btnStartDBC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnStartDBC.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnStartDBC.Location = New System.Drawing.Point(709, 4)
        Me.btnStartDBC.Name = "btnStartDBC"
        Me.btnStartDBC.Size = New System.Drawing.Size(79, 23)
        Me.btnStartDBC.TabIndex = 8
        Me.btnStartDBC.Text = "&Start"
        Me.btnStartDBC.UseVisualStyleBackColor = False
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
        Me.txtBaseFolder.BackColor = System.Drawing.Color.LightGray
        Me.txtBaseFolder.Location = New System.Drawing.Point(102, 6)
        Me.txtBaseFolder.Name = "txtBaseFolder"
        Me.txtBaseFolder.Size = New System.Drawing.Size(566, 20)
        Me.txtBaseFolder.TabIndex = 1
        Me.txtBaseFolder.Text = "W:\World of Warcraft"
        '
        'BtnQuit
        '
        Me.BtnQuit.BackColor = System.Drawing.Color.LightGray
        Me.BtnQuit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnQuit.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BtnQuit.Location = New System.Drawing.Point(709, 30)
        Me.BtnQuit.Name = "BtnQuit"
        Me.BtnQuit.Size = New System.Drawing.Size(79, 23)
        Me.BtnQuit.TabIndex = 9
        Me.BtnQuit.Text = "E&xit"
        Me.BtnQuit.UseVisualStyleBackColor = False
        '
        'txtOutputFolder
        '
        Me.txtOutputFolder.BackColor = System.Drawing.Color.LightGray
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
        'lstMainLog
        '
        Me.lstMainLog.BackColor = System.Drawing.Color.LightBlue
        Me.lstMainLog.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lstMainLog.Font = New System.Drawing.Font("Lucida Console", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstMainLog.ForeColor = System.Drawing.Color.Black
        Me.lstMainLog.FormattingEnabled = True
        Me.lstMainLog.ItemHeight = 12
        Me.lstMainLog.Location = New System.Drawing.Point(5, 58)
        Me.lstMainLog.Name = "lstMainLog"
        Me.lstMainLog.Size = New System.Drawing.Size(783, 324)
        Me.lstMainLog.TabIndex = 13
        '
        'brnWDB
        '
        Me.brnWDB.BackColor = System.Drawing.Color.LightGray
        Me.brnWDB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.brnWDB.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.brnWDB.Location = New System.Drawing.Point(121, 378)
        Me.brnWDB.Name = "brnWDB"
        Me.brnWDB.Size = New System.Drawing.Size(82, 23)
        Me.brnWDB.TabIndex = 14
        Me.brnWDB.Text = "&WDB to SQL"
        Me.brnWDB.UseVisualStyleBackColor = False
        Me.brnWDB.Visible = False
        '
        'btnSelectBaseFolder
        '
        Me.btnSelectBaseFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.btnSelectOutputFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectOutputFolder.Location = New System.Drawing.Point(671, 30)
        Me.btnSelectOutputFolder.Margin = New System.Windows.Forms.Padding(0)
        Me.btnSelectOutputFolder.Name = "btnSelectOutputFolder"
        Me.btnSelectOutputFolder.Size = New System.Drawing.Size(23, 23)
        Me.btnSelectOutputFolder.TabIndex = 4
        Me.btnSelectOutputFolder.Text = "…"
        Me.btnSelectOutputFolder.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.chkExportH)
        Me.Panel1.Controls.Add(Me.chkExtractVmap)
        Me.Panel1.Controls.Add(Me.chkExtractMaps)
        Me.Panel1.Controls.Add(Me.chkExportMD)
        Me.Panel1.Controls.Add(Me.chkExportXML)
        Me.Panel1.Controls.Add(Me.chkCSV)
        Me.Panel1.Controls.Add(Me.chkSQL)
        Me.Panel1.Location = New System.Drawing.Point(208, 402)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(580, 50)
        Me.Panel1.TabIndex = 18
        '
        'chkExtractVmap
        '
        Me.chkExtractVmap.AutoSize = True
        Me.chkExtractVmap.BackColor = System.Drawing.Color.Transparent
        Me.chkExtractVmap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExtractVmap.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExtractVmap.Location = New System.Drawing.Point(242, 3)
        Me.chkExtractVmap.Name = "chkExtractVmap"
        Me.chkExtractVmap.Size = New System.Drawing.Size(242, 18)
        Me.chkExtractVmap.TabIndex = 24
        Me.chkExtractVmap.Text = "Create VMaps from DBC, WMO, MDL, WDT"
        Me.chkExtractVmap.UseVisualStyleBackColor = False
        '
        'chkExtractMaps
        '
        Me.chkExtractMaps.AutoSize = True
        Me.chkExtractMaps.BackColor = System.Drawing.Color.Transparent
        Me.chkExtractMaps.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExtractMaps.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExtractMaps.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkExtractMaps.Location = New System.Drawing.Point(242, 24)
        Me.chkExtractMaps.Margin = New System.Windows.Forms.Padding(1)
        Me.chkExtractMaps.Name = "chkExtractMaps"
        Me.chkExtractMaps.Size = New System.Drawing.Size(172, 18)
        Me.chkExtractMaps.TabIndex = 21
        Me.chkExtractMaps.Text = "Create Maps using DBC/ADT"
        Me.chkExtractMaps.UseVisualStyleBackColor = False
        '
        'chkExportMD
        '
        Me.chkExportMD.AutoSize = True
        Me.chkExportMD.BackColor = System.Drawing.Color.Transparent
        Me.chkExportMD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExportMD.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExportMD.Location = New System.Drawing.Point(120, 3)
        Me.chkExportMD.Name = "chkExportMD"
        Me.chkExportMD.Size = New System.Drawing.Size(97, 18)
        Me.chkExportMD.TabIndex = 20
        Me.chkExportMD.Text = "MD from DBC"
        Me.chkExportMD.UseVisualStyleBackColor = False
        '
        'chkExportXML
        '
        Me.chkExportXML.AutoSize = True
        Me.chkExportXML.BackColor = System.Drawing.Color.Transparent
        Me.chkExportXML.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExportXML.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExportXML.Location = New System.Drawing.Point(120, 24)
        Me.chkExportXML.Name = "chkExportXML"
        Me.chkExportXML.Size = New System.Drawing.Size(102, 18)
        Me.chkExportXML.TabIndex = 19
        Me.chkExportXML.Text = "XML from DBC"
        Me.chkExportXML.UseVisualStyleBackColor = False
        '
        'chkCSV
        '
        Me.chkCSV.AutoSize = True
        Me.chkCSV.BackColor = System.Drawing.Color.Transparent
        Me.chkCSV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkCSV.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkCSV.Location = New System.Drawing.Point(4, 24)
        Me.chkCSV.Name = "chkCSV"
        Me.chkCSV.Size = New System.Drawing.Size(101, 18)
        Me.chkCSV.TabIndex = 18
        Me.chkCSV.Text = "CSV from DBC"
        Me.chkCSV.UseVisualStyleBackColor = False
        '
        'chkSQL
        '
        Me.chkSQL.AutoSize = True
        Me.chkSQL.BackColor = System.Drawing.Color.Transparent
        Me.chkSQL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkSQL.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkSQL.Location = New System.Drawing.Point(4, 3)
        Me.chkSQL.Name = "chkSQL"
        Me.chkSQL.Size = New System.Drawing.Size(101, 18)
        Me.chkSQL.TabIndex = 17
        Me.chkSQL.Text = "SQL from DBC"
        Me.chkSQL.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.chkExtractWDT)
        Me.Panel2.Controls.Add(Me.chkExtractWMO)
        Me.Panel2.Controls.Add(Me.chkExtractADT)
        Me.Panel2.Controls.Add(Me.chkDBC)
        Me.Panel2.Location = New System.Drawing.Point(5, 402)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(198, 50)
        Me.Panel2.TabIndex = 20
        '
        'chkExtractWDT
        '
        Me.chkExtractWDT.AutoSize = True
        Me.chkExtractWDT.BackColor = System.Drawing.Color.Transparent
        Me.chkExtractWDT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExtractWDT.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExtractWDT.Location = New System.Drawing.Point(84, 26)
        Me.chkExtractWDT.Name = "chkExtractWDT"
        Me.chkExtractWDT.Size = New System.Drawing.Size(88, 18)
        Me.chkExtractWDT.TabIndex = 22
        Me.chkExtractWDT.Text = "Vmap WDT"
        Me.chkExtractWDT.UseVisualStyleBackColor = False
        '
        'chkExtractWMO
        '
        Me.chkExtractWMO.AutoSize = True
        Me.chkExtractWMO.BackColor = System.Drawing.Color.Transparent
        Me.chkExtractWMO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExtractWMO.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExtractWMO.Location = New System.Drawing.Point(84, 3)
        Me.chkExtractWMO.Name = "chkExtractWMO"
        Me.chkExtractWMO.Size = New System.Drawing.Size(118, 18)
        Me.chkExtractWMO.TabIndex = 21
        Me.chkExtractWMO.Text = "Vmap WMO/MDL"
        Me.chkExtractWMO.UseVisualStyleBackColor = False
        '
        'chkExtractADT
        '
        Me.chkExtractADT.AutoSize = True
        Me.chkExtractADT.BackColor = System.Drawing.Color.Transparent
        Me.chkExtractADT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExtractADT.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExtractADT.Location = New System.Drawing.Point(4, 26)
        Me.chkExtractADT.Name = "chkExtractADT"
        Me.chkExtractADT.Size = New System.Drawing.Size(78, 18)
        Me.chkExtractADT.TabIndex = 20
        Me.chkExtractADT.Text = "Map ADT"
        Me.chkExtractADT.UseVisualStyleBackColor = False
        '
        'chkDBC
        '
        Me.chkDBC.AutoSize = True
        Me.chkDBC.BackColor = System.Drawing.Color.Transparent
        Me.chkDBC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkDBC.Checked = True
        Me.chkDBC.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDBC.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkDBC.Location = New System.Drawing.Point(3, 3)
        Me.chkDBC.Name = "chkDBC"
        Me.chkDBC.Size = New System.Drawing.Size(61, 18)
        Me.chkDBC.TabIndex = 18
        Me.chkDBC.Text = "DBC's"
        Me.chkDBC.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 388)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 13)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Extraction Options"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(209, 388)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 13)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "Export Options"
        '
        'chkExportH
        '
        Me.chkExportH.AutoSize = True
        Me.chkExportH.BackColor = System.Drawing.Color.Transparent
        Me.chkExportH.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExportH.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExportH.Location = New System.Drawing.Point(478, 3)
        Me.chkExportH.Name = "chkExportH"
        Me.chkExportH.Size = New System.Drawing.Size(88, 18)
        Me.chkExportH.TabIndex = 25
        Me.chkExportH.Text = "H from DBC"
        Me.chkExportH.UseVisualStyleBackColor = False
        '
        'MaNGOSExtractor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightBlue
        Me.ClientSize = New System.Drawing.Size(794, 458)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnSelectOutputFolder)
        Me.Controls.Add(Me.btnSelectBaseFolder)
        Me.Controls.Add(Me.brnWDB)
        Me.Controls.Add(Me.lstMainLog)
        Me.Controls.Add(Me.txtOutputFolder)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.BtnQuit)
        Me.Controls.Add(Me.txtBaseFolder)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnStartDBC)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MaNGOSExtractor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "MaNGOSExtractor"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnStartDBC As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtBaseFolder As System.Windows.Forms.TextBox
    Friend WithEvents BtnQuit As System.Windows.Forms.Button
    Friend WithEvents txtOutputFolder As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lstMainLog As System.Windows.Forms.ListBox
    Friend WithEvents brnWDB As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents btnSelectBaseFolder As System.Windows.Forms.Button
    Friend WithEvents btnSelectOutputFolder As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkExportXML As System.Windows.Forms.CheckBox
    Friend WithEvents chkCSV As System.Windows.Forms.CheckBox
    Friend WithEvents chkSQL As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents chkDBC As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkExportMD As System.Windows.Forms.CheckBox
    Friend WithEvents chkExtractADT As System.Windows.Forms.CheckBox
    Friend WithEvents chkExtractWMO As System.Windows.Forms.CheckBox
    Friend WithEvents chkExtractVmap As System.Windows.Forms.CheckBox
    Friend WithEvents chkExtractMaps As System.Windows.Forms.CheckBox
    Friend WithEvents chkExtractWDT As System.Windows.Forms.CheckBox
    Friend WithEvents chkExportH As System.Windows.Forms.CheckBox

End Class
