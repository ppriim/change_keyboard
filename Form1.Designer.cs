namespace KeyboardLayoutSwitcher;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.chkEnableAI = new System.Windows.Forms.CheckBox();
        this.lblApiKey = new System.Windows.Forms.Label();
        this.txtApiKey = new System.Windows.Forms.TextBox();
        this.btnSave = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.lblModel = new System.Windows.Forms.Label();
        this.cboModel = new System.Windows.Forms.ComboBox();
        this.lblInfo = new System.Windows.Forms.Label();
        this.lnkGetApiKey = new System.Windows.Forms.LinkLabel();
        this.SuspendLayout();
        // 
        // chkEnableAI
        // 
        this.chkEnableAI.AutoSize = true;
        this.chkEnableAI.Location = new System.Drawing.Point(20, 20);
        this.chkEnableAI.Name = "chkEnableAI";
        this.chkEnableAI.Size = new System.Drawing.Size(163, 19);
        this.chkEnableAI.TabIndex = 0;
        this.chkEnableAI.Text = "Enable AI Smart Correction";
        this.chkEnableAI.UseVisualStyleBackColor = true;
        this.chkEnableAI.CheckedChanged += new System.EventHandler(this.chkEnableAI_CheckedChanged);
        // 
        // lblApiKey
        // 
        this.lblApiKey.Location = new System.Drawing.Point(17, 65);
        this.lblApiKey.Name = "lblApiKey";
        this.lblApiKey.Size = new System.Drawing.Size(94, 15);
        this.lblApiKey.TabIndex = 1;
        this.lblApiKey.Text = "OpenAI API Key:";
        // 
        // txtApiKey
        // 
        this.txtApiKey.Location = new System.Drawing.Point(120, 62);
        this.txtApiKey.Name = "txtApiKey";
        this.txtApiKey.PasswordChar = '*';
        this.txtApiKey.Size = new System.Drawing.Size(262, 23);
        this.txtApiKey.TabIndex = 2;
        // 
        // lnkGetApiKey
        // 
        this.lnkGetApiKey.AutoSize = true;
        this.lnkGetApiKey.Location = new System.Drawing.Point(120, 44);
        this.lnkGetApiKey.Name = "lnkGetApiKey";
        this.lnkGetApiKey.Size = new System.Drawing.Size(126, 15);
        this.lnkGetApiKey.TabIndex = 8;
        this.lnkGetApiKey.TabStop = true;
        this.lnkGetApiKey.Text = "What is this? / Get Key";
        this.lnkGetApiKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGetApiKey_LinkClicked);
        // 
        // lblModel
        // 
        this.lblModel.AutoSize = true;
        this.lblModel.Location = new System.Drawing.Point(17, 98);
        this.lblModel.Name = "lblModel";
        this.lblModel.Size = new System.Drawing.Size(63, 15);
        this.lblModel.TabIndex = 6;
        this.lblModel.Text = "AI Model:";
        // 
        // cboModel
        // 
        this.cboModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cboModel.FormattingEnabled = true;
        this.cboModel.Items.AddRange(new object[] {
        "gpt-4o-mini",
        "gpt-4o",
        "gpt-3.5-turbo"});
        this.cboModel.Location = new System.Drawing.Point(120, 95);
        this.cboModel.Name = "cboModel";
        this.cboModel.Size = new System.Drawing.Size(262, 23);
        this.cboModel.TabIndex = 7;
        // 
        // btnSave
        // 
        this.btnSave.Location = new System.Drawing.Point(226, 160);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(75, 23);
        this.btnSave.TabIndex = 3;
        this.btnSave.Text = "Save";
        this.btnSave.UseVisualStyleBackColor = true;
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Location = new System.Drawing.Point(307, 160);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(75, 23);
        this.btnCancel.TabIndex = 4;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
        // 
        // lblInfo
        // 
        this.lblInfo.AutoSize = true;
        this.lblInfo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
        this.lblInfo.Location = new System.Drawing.Point(17, 128);
        this.lblInfo.Name = "lblInfo";
        this.lblInfo.Size = new System.Drawing.Size(325, 15);
        this.lblInfo.TabIndex = 5;
        this.lblInfo.Text = "AI handles ambiguous keys like '-' using the selected model.";
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(400, 200);
        this.Controls.Add(this.cboModel);
        this.Controls.Add(this.lblModel);
        this.Controls.Add(this.lblInfo);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.txtApiKey);
        this.Controls.Add(this.lblApiKey);
        this.Controls.Add(this.lnkGetApiKey);
        this.Controls.Add(this.chkEnableAI);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "Form1";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Keyboard Layout Switcher - Settings";
        this.Load += new System.EventHandler(this.Form1_Load);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.CheckBox chkEnableAI;
    private System.Windows.Forms.Label lblApiKey;
    private System.Windows.Forms.TextBox txtApiKey;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblModel;
    private System.Windows.Forms.ComboBox cboModel;
    private System.Windows.Forms.LinkLabel lnkGetApiKey;
    private System.Windows.Forms.Label lblInfo;
}
