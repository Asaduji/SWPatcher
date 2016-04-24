﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SWPatcher
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        string _SourceFolder = string.Empty;
        IniFile PatcherSetting = null;

        private void MainForm_Load(object sender, EventArgs e)
        {
            //because user may change windows user, so i think we shouldn't use %appdata% ....
            this._SourceFolder = System.IO.Directory.GetParent(Application.ExecutablePath).FullName;

            //i think we shouldn't keep OpenDialog in memory while user not using it much.
            PatcherSetting = new IniFile(_SourceFolder + "\\Settings.ini");
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(PatcherSetting.IniReadValue("patcher", "folder")))
            {
                using (OpenFileDialog Opener = new OpenFileDialog())
                {
                    Opener.Multiselect = false;
                    Opener.CheckFileExists = true;
                    Opener.CheckPathExists = true;
                    Opener.Title = "Select game executable file";
                    Opener.FileName = "soulworker100";
                    Opener.Filter = "soulworker100.exe|soulworker100.exe"; // hard coded or *.exe ?
                    Opener.DefaultExt = "exe";
                    if (Opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        PatcherSetting.IniWriteValue("patcher", "folder", System.IO.Directory.GetParent(Opener.FileName).FullName);
                    else
                    {
                        //How should we act when people click cancel this ............ ? Exit or warn them and re-open the OpenFileDialog ... ?
                        MessageBox.Show("Cannot found SoulWorker folder game", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Application.Exit();
                    }
                }
            }
            System.Net.WebClient theWebClient = new System.Net.WebClient();
            theWebClient.BaseAddress = "https://raw.githubusercontent.com/Miyuyami/SoulWorkerHQTranslations/master/";
            theWebClient.Proxy = null;
            string tmpResult = string.Empty;
            for (short i = 0; i < 2; i++)
            {
                try
                {
                    tmpResult = theWebClient.DownloadString("Languages");
                }
                catch (System.Net.WebException webEx)
                {
                    if (((System.Net.HttpWebResponse)webEx.Response).StatusCode == System.Net.HttpStatusCode.NotFound)
                        break;
                }
                if (string.IsNullOrEmpty(tmpResult) == false)
                    break;
            }

            if (string.IsNullOrEmpty(tmpResult) == false) // Double Check
            {
                string[] tbl_SupportLanguage = tmpResult.Split('\n');
                foreach (string supportLanguage in tbl_SupportLanguage)
                    comboBoxLanguages.Items.Add(supportLanguage);
                if (comboBoxLanguages.Items.Count > 0)
                {
                    string lastchosenLanguage = PatcherSetting.IniReadValue("patcher", "s_translationlanguage");
                    if (string.IsNullOrEmpty(lastchosenLanguage))
                        comboBoxLanguages.SelectedIndex = 0;
                    else
                        comboBoxLanguages.SelectedItem = lastchosenLanguage;
                    lastchosenLanguage = null;
                }
            }
            theWebClient.Dispose(); // just in case (i'll always leave this at the end of method)
        }

        private void buttonResetSWFolder_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog Opener = new OpenFileDialog())
            {
                Opener.Multiselect = false;
                Opener.CheckFileExists = true;
                Opener.CheckPathExists = true;
                Opener.Title = "Select game executable file";
                Opener.FileName = "soulworker100";
                Opener.Filter = "soulworker100.exe|soulworker100.exe";
                Opener.DefaultExt = "exe";
                if (Opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    PatcherSetting.IniWriteValue("patcher", "folder", System.IO.Directory.GetParent(Opener.FileName).FullName);
            }
        }

        private int isInProgress()
        {

            return 0;
        }

        private void comboBoxLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            PatcherSetting.IniWriteValue("patcher", "s_translationlanguage", (string)comboBoxLanguages.SelectedItem);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_Closing(object sender, FormClosingEventArgs e)
        {
            int progressID = isInProgress();
            if (progressID > 0)
            {
                // Promt user when doing progress
                if (MessageBox.Show("Are you sure you want to exit ?\n*Patcher is in progress*", "NOTICE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    // Stop progress and clean up according to progress's ID ... ?
                }
                else
                    e.Cancel = true;
            }
        }
    }
}