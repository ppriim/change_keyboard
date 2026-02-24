using System;
using System.Windows.Forms;

namespace KeyboardLayoutSwitcher;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        // Load existing settings into UI
        ConfigManager.LoadSettings();
        chkEnableAI.Checked = ConfigManager.Settings.EnableAI;
        txtApiKey.Text = ConfigManager.Settings.OpenAIApiKey;
        
        // Find and select the model in dropdown, or default to first item
        if (!string.IsNullOrEmpty(ConfigManager.Settings.AIModel))
        {
            int index = cboModel.FindStringExact(ConfigManager.Settings.AIModel);
            if (index != -1) cboModel.SelectedIndex = index;
            else cboModel.SelectedIndex = 0;
        }
        else cboModel.SelectedIndex = 0;
        
        UpdateUIState();
    }

    private void chkEnableAI_CheckedChanged(object sender, EventArgs e)
    {
        UpdateUIState();
    }

    private void UpdateUIState()
    {
        txtApiKey.Enabled = chkEnableAI.Checked;
        cboModel.Enabled = chkEnableAI.Checked;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (chkEnableAI.Checked && string.IsNullOrWhiteSpace(txtApiKey.Text))
        {
            MessageBox.Show("Please enter an OpenAI API Key.", "Missing Key", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ConfigManager.Settings.EnableAI = chkEnableAI.Checked;
        ConfigManager.Settings.OpenAIApiKey = txtApiKey.Text.Trim();
        ConfigManager.Settings.AIModel = cboModel.SelectedItem?.ToString() ?? "gpt-4o-mini";
        ConfigManager.SaveSettings();

        MessageBox.Show("Settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void lnkGetApiKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://platform.openai.com/api-keys",
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show("Could not open link: " + ex.Message);
        }
    }
}
