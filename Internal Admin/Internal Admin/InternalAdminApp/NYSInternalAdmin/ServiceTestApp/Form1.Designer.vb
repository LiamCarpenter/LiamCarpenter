<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.btnTestInteceptor = New System.Windows.Forms.Button()
        Me.tbGuardianInput = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbGuardianOutput = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tbInteceptorXML = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tbBossOutput = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tbSQLOutput = New System.Windows.Forms.TextBox()
        Me.btnSSOSync = New System.Windows.Forms.Button()
        Me.btnTestMailing = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnTestInteceptor
        '
        Me.btnTestInteceptor.Location = New System.Drawing.Point(14, 448)
        Me.btnTestInteceptor.Name = "btnTestInteceptor"
        Me.btnTestInteceptor.Size = New System.Drawing.Size(188, 124)
        Me.btnTestInteceptor.TabIndex = 0
        Me.btnTestInteceptor.Text = "Test Inteceptor"
        Me.btnTestInteceptor.UseVisualStyleBackColor = True
        '
        'tbGuardianInput
        '
        Me.tbGuardianInput.Location = New System.Drawing.Point(421, 445)
        Me.tbGuardianInput.Name = "tbGuardianInput"
        Me.tbGuardianInput.Size = New System.Drawing.Size(275, 20)
        Me.tbGuardianInput.TabIndex = 1
        Me.tbGuardianInput.Text = "E:\Development Testing\Evolvi\GuardianInput\"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(219, 448)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(197, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Location of XML Files for Guardian Input"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(211, 474)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(205, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Location of XML Files for Guardian Output"
        '
        'tbGuardianOutput
        '
        Me.tbGuardianOutput.Location = New System.Drawing.Point(421, 471)
        Me.tbGuardianOutput.Name = "tbGuardianOutput"
        Me.tbGuardianOutput.Size = New System.Drawing.Size(275, 20)
        Me.tbGuardianOutput.TabIndex = 3
        Me.tbGuardianOutput.Text = "E:\Development Testing\Evolvi\GuardianOutput\"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(235, 500)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(181, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Location of XML Files for Interceptor:"
        '
        'tbInteceptorXML
        '
        Me.tbInteceptorXML.Location = New System.Drawing.Point(421, 497)
        Me.tbInteceptorXML.Name = "tbInteceptorXML"
        Me.tbInteceptorXML.Size = New System.Drawing.Size(275, 20)
        Me.tbInteceptorXML.TabIndex = 5
        Me.tbInteceptorXML.Text = "E:\Development Testing\Evolvi\InterceptorXML\"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(269, 529)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(147, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Folder to send BOSS Output: "
        '
        'tbBossOutput
        '
        Me.tbBossOutput.Location = New System.Drawing.Point(421, 526)
        Me.tbBossOutput.Name = "tbBossOutput"
        Me.tbBossOutput.Size = New System.Drawing.Size(275, 20)
        Me.tbBossOutput.TabIndex = 7
        Me.tbBossOutput.Text = "E:\Development Testing\Evolvi\BossOutput\"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(277, 555)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(139, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Folder to send SQL Output: "
        '
        'tbSQLOutput
        '
        Me.tbSQLOutput.Location = New System.Drawing.Point(421, 552)
        Me.tbSQLOutput.Name = "tbSQLOutput"
        Me.tbSQLOutput.Size = New System.Drawing.Size(275, 20)
        Me.tbSQLOutput.TabIndex = 9
        Me.tbSQLOutput.Text = "E:\Development Testing\Evolvi\SQLOutput\"
        '
        'btnSSOSync
        '
        Me.btnSSOSync.Location = New System.Drawing.Point(508, 12)
        Me.btnSSOSync.Name = "btnSSOSync"
        Me.btnSSOSync.Size = New System.Drawing.Size(188, 124)
        Me.btnSSOSync.TabIndex = 11
        Me.btnSSOSync.Text = "Run SSO Sync"
        Me.btnSSOSync.UseVisualStyleBackColor = True
        '
        'btnTestMailing
        '
        Me.btnTestMailing.Location = New System.Drawing.Point(14, 260)
        Me.btnTestMailing.Name = "btnTestMailing"
        Me.btnTestMailing.Size = New System.Drawing.Size(188, 115)
        Me.btnTestMailing.TabIndex = 12
        Me.btnTestMailing.Text = "Test Mailing (Be Aware you will need to step thourgh this it will still send the " & _
    "email)"
        Me.btnTestMailing.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(708, 597)
        Me.Controls.Add(Me.btnTestMailing)
        Me.Controls.Add(Me.btnSSOSync)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.tbSQLOutput)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.tbBossOutput)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbInteceptorXML)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbGuardianOutput)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbGuardianInput)
        Me.Controls.Add(Me.btnTestInteceptor)
        Me.Name = "Form1"
        Me.Text = "Service Test Form"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnTestInteceptor As System.Windows.Forms.Button
    Friend WithEvents tbGuardianInput As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbGuardianOutput As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbInteceptorXML As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbBossOutput As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tbSQLOutput As System.Windows.Forms.TextBox
    Friend WithEvents btnSSOSync As System.Windows.Forms.Button
    Friend WithEvents btnTestMailing As System.Windows.Forms.Button

End Class
