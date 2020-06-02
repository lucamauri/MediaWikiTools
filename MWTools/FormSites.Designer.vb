<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormSites
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormSites))
        Me.GridSource = New System.Windows.Forms.DataGridView()
        Me.BoxSites = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BoxQuery = New System.Windows.Forms.TextBox()
        Me.BoxLUA = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BTNTabler = New System.Windows.Forms.Button()
        CType(Me.GridSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GridSource
        '
        Me.GridSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridSource.Location = New System.Drawing.Point(12, 12)
        Me.GridSource.Name = "GridSource"
        Me.GridSource.Size = New System.Drawing.Size(960, 262)
        Me.GridSource.TabIndex = 0
        '
        'BoxSites
        '
        Me.BoxSites.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BoxSites.Location = New System.Drawing.Point(12, 305)
        Me.BoxSites.Multiline = True
        Me.BoxSites.Name = "BoxSites"
        Me.BoxSites.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.BoxSites.Size = New System.Drawing.Size(960, 120)
        Me.BoxSites.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(897, 686)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 283)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 19)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Sites.xml"
        '
        'BoxQuery
        '
        Me.BoxQuery.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BoxQuery.Location = New System.Drawing.Point(12, 450)
        Me.BoxQuery.Multiline = True
        Me.BoxQuery.Name = "BoxQuery"
        Me.BoxQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.BoxQuery.Size = New System.Drawing.Size(960, 102)
        Me.BoxQuery.TabIndex = 5
        '
        'BoxLUA
        '
        Me.BoxLUA.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BoxLUA.Location = New System.Drawing.Point(12, 577)
        Me.BoxLUA.Multiline = True
        Me.BoxLUA.Name = "BoxLUA"
        Me.BoxLUA.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.BoxLUA.Size = New System.Drawing.Size(960, 92)
        Me.BoxLUA.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 428)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 19)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "IW Query"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 555)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(83, 19)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "LUA Array"
        '
        'BTNTabler
        '
        Me.BTNTabler.Location = New System.Drawing.Point(10, 686)
        Me.BTNTabler.Name = "BTNTabler"
        Me.BTNTabler.Size = New System.Drawing.Size(75, 23)
        Me.BTNTabler.TabIndex = 9
        Me.BTNTabler.Text = "Tabler"
        Me.BTNTabler.UseVisualStyleBackColor = True
        '
        'FormSites
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 721)
        Me.Controls.Add(Me.BTNTabler)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BoxLUA)
        Me.Controls.Add(Me.BoxQuery)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.BoxSites)
        Me.Controls.Add(Me.GridSource)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormSites"
        Me.Text = "FormSites"
        CType(Me.GridSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GridSource As DataGridView
    Friend WithEvents BoxSites As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents BoxQuery As TextBox
    Friend WithEvents BoxLUA As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents BTNTabler As Button
End Class
