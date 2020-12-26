﻿/*
 * This file is part of Soulworker Patcher.
 * Copyright (C) 2016-2017 Miyu, Dramiel Leayal
 *
 * Soulworker Patcher is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Soulworker Patcher is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Soulworker Patcher. If not, see <http://www.gnu.org/licenses/>.
 */

using SWPatcher.Helpers;
using SWPatcher.Helpers.GlobalVariables;
using System;
using System.Windows.Forms;

namespace SWPatcher.Forms
{
    internal partial class AboutBox : Form
    {
        private int ImagesCount = 72;

        internal AboutBox()
        {
            InitializeComponent();
            InitializeTextComponent();
        }

        private void InitializeTextComponent()
        {
            this.ButtonOk.Text = StringLoader.GetText("button_ok");
            this.Text = $"About {AssemblyAccessor.Title}";
            this.LabelProductName.Text = AssemblyAccessor.Product;
            this.LabelVersion.Text = $"Version {AssemblyAccessor.Version}";
            this.LabelCopyright.Text += "\nFork by Asaduji";
            this.TextBoxDescription.Text = StringLoader.GetText("patcher_description");
            this.LinkLabelWebsite.Links.Add(0, this.LinkLabelWebsite.Text.Length, Urls.SoulworkerWebsite);
            this.LogoPictureBox.ImageLocation = $"https://raw.githubusercontent.com/Miyuyami/SWPatcher/master/Images/{(new Random()).Next(ImagesCount) + 1}.png";
        }

        private void LinkLabelWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.LinkLabelWebsite.LinkVisited = true;
            System.Diagnostics.Process.Start(Urls.SoulworkerWebsite);
        }

        private void LabelCopyright_Click(object sender, EventArgs e)
        {
        }

        private void LabelProductName_Click(object sender, EventArgs e)
        {
        }

        private void TableLayoutPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start(Urls.ForkWebsite);
        }
    }
}