<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ImageBox1 = New Emgu.CV.UI.ImageBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnUpload = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.cboSource = New System.Windows.Forms.ComboBox()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.lblResolution = New System.Windows.Forms.Label()
        Me.numResolutionHeight = New System.Windows.Forms.NumericUpDown()
        Me.lblSensitivity = New System.Windows.Forms.Label()
        Me.trackBarSensitivity = New System.Windows.Forms.TrackBar()
        Me.btnCaptureImage = New System.Windows.Forms.Button()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.numResolutionWidth = New System.Windows.Forms.NumericUpDown()
        CType(Me.ImageBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numResolutionHeight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trackBarSensitivity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.numResolutionWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageBox1
        '
        Me.ImageBox1.Location = New System.Drawing.Point(12, 12)
        Me.ImageBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.ImageBox1.Name = "ImageBox1"
        Me.ImageBox1.Size = New System.Drawing.Size(640, 480)
        Me.ImageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ImageBox1.TabIndex = 2
        Me.ImageBox1.TabStop = False
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(12, 510)
        Me.txtName.Margin = New System.Windows.Forms.Padding(2)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(147, 20)
        Me.txtName.TabIndex = 3
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(12, 590)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(2)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(147, 20)
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "Save Face Data"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnUpload
        '
        Me.btnUpload.Location = New System.Drawing.Point(12, 534)
        Me.btnUpload.Margin = New System.Windows.Forms.Padding(2)
        Me.btnUpload.Name = "btnUpload"
        Me.btnUpload.Size = New System.Drawing.Size(147, 23)
        Me.btnUpload.TabIndex = 6
        Me.btnUpload.Text = "Upload Image"
        Me.btnUpload.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(11, 645)
        Me.Button1.Margin = New System.Windows.Forms.Padding(2)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(147, 23)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "Initialize Database"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cboSource
        '
        Me.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSource.FormattingEnabled = True
        Me.cboSource.Items.AddRange(New Object() {"Image", "Webcam"})
        Me.cboSource.Location = New System.Drawing.Point(6, 91)
        Me.cboSource.Name = "cboSource"
        Me.cboSource.Size = New System.Drawing.Size(119, 21)
        Me.cboSource.TabIndex = 9
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(16, 120)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(90, 23)
        Me.btnStart.TabIndex = 11
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'btnStop
        '
        Me.btnStop.Location = New System.Drawing.Point(16, 148)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(90, 23)
        Me.btnStop.TabIndex = 12
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'lblResolution
        '
        Me.lblResolution.AutoSize = True
        Me.lblResolution.Location = New System.Drawing.Point(164, 513)
        Me.lblResolution.Name = "lblResolution"
        Me.lblResolution.Size = New System.Drawing.Size(61, 13)
        Me.lblResolution.TabIndex = 13
        Me.lblResolution.Text = "Resolution:"
        '
        'numResolutionHeight
        '
        Me.numResolutionHeight.Location = New System.Drawing.Point(297, 511)
        Me.numResolutionHeight.Maximum = New Decimal(New Integer() {1080, 0, 0, 0})
        Me.numResolutionHeight.Minimum = New Decimal(New Integer() {240, 0, 0, 0})
        Me.numResolutionHeight.Name = "numResolutionHeight"
        Me.numResolutionHeight.Size = New System.Drawing.Size(60, 20)
        Me.numResolutionHeight.TabIndex = 15
        Me.numResolutionHeight.Value = New Decimal(New Integer() {480, 0, 0, 0})
        '
        'lblSensitivity
        '
        Me.lblSensitivity.AutoSize = True
        Me.lblSensitivity.Location = New System.Drawing.Point(164, 567)
        Me.lblSensitivity.Name = "lblSensitivity"
        Me.lblSensitivity.Size = New System.Drawing.Size(60, 13)
        Me.lblSensitivity.TabIndex = 16
        Me.lblSensitivity.Text = "Sensitivity:"
        '
        'trackBarSensitivity
        '
        Me.trackBarSensitivity.Location = New System.Drawing.Point(231, 555)
        Me.trackBarSensitivity.Maximum = 100
        Me.trackBarSensitivity.Minimum = 1
        Me.trackBarSensitivity.Name = "trackBarSensitivity"
        Me.trackBarSensitivity.Size = New System.Drawing.Size(225, 45)
        Me.trackBarSensitivity.TabIndex = 17
        Me.trackBarSensitivity.Value = 30
        '
        'btnCaptureImage
        '
        Me.btnCaptureImage.Location = New System.Drawing.Point(12, 562)
        Me.btnCaptureImage.Name = "btnCaptureImage"
        Me.btnCaptureImage.Size = New System.Drawing.Size(147, 23)
        Me.btnCaptureImage.TabIndex = 18
        Me.btnCaptureImage.Text = "Capture Image"
        Me.btnCaptureImage.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(25, 27)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(69, 17)
        Me.RadioButton1.TabIndex = 19
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "TRAINER"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Checked = True
        Me.RadioButton2.Location = New System.Drawing.Point(25, 58)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(62, 17)
        Me.RadioButton2.TabIndex = 20
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "ONLINE"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RadioButton1)
        Me.GroupBox1.Controls.Add(Me.RadioButton2)
        Me.GroupBox1.Controls.Add(Me.btnStart)
        Me.GroupBox1.Controls.Add(Me.btnStop)
        Me.GroupBox1.Controls.Add(Me.cboSource)
        Me.GroupBox1.Location = New System.Drawing.Point(521, 497)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(131, 192)
        Me.GroupBox1.TabIndex = 21
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "START MODE"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(167, 645)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(131, 23)
        Me.Button2.TabIndex = 22
        Me.Button2.Text = "Learning From Saved"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'numResolutionWidth
        '
        Me.numResolutionWidth.Location = New System.Drawing.Point(231, 511)
        Me.numResolutionWidth.Maximum = New Decimal(New Integer() {1920, 0, 0, 0})
        Me.numResolutionWidth.Minimum = New Decimal(New Integer() {320, 0, 0, 0})
        Me.numResolutionWidth.Name = "numResolutionWidth"
        Me.numResolutionWidth.Size = New System.Drawing.Size(60, 20)
        Me.numResolutionWidth.TabIndex = 14
        Me.numResolutionWidth.Value = New Decimal(New Integer() {640, 0, 0, 0})
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(664, 679)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnCaptureImage)
        Me.Controls.Add(Me.trackBarSensitivity)
        Me.Controls.Add(Me.lblSensitivity)
        Me.Controls.Add(Me.numResolutionHeight)
        Me.Controls.Add(Me.numResolutionWidth)
        Me.Controls.Add(Me.lblResolution)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnUpload)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.ImageBox1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Form1"
        Me.Text = "Face Recognition"
        CType(Me.ImageBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numResolutionHeight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trackBarSensitivity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.numResolutionWidth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ImageBox1 As Emgu.CV.UI.ImageBox
    Friend WithEvents txtName As TextBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnUpload As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents cboSource As ComboBox
    Friend WithEvents btnStart As Button
    Friend WithEvents btnStop As Button
    Friend WithEvents lblResolution As Label
    Friend WithEvents numResolutionHeight As NumericUpDown
    Friend WithEvents lblSensitivity As Label
    Friend WithEvents trackBarSensitivity As TrackBar
    Friend WithEvents btnCaptureImage As Button ' زر لالتقاط الصورة
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Button2 As Button
    Friend WithEvents numResolutionWidth As NumericUpDown
End Class
