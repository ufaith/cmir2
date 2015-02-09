﻿namespace Server
{
    partial class SystemInfoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.MonsterSpawnChanceTextBox = new System.Windows.Forms.TextBox();
            this.FishingMobIndexComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.FishingSuccessRateMultiplierTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.FishingDelayTextBox = new System.Windows.Forms.TextBox();
            this.FishingSuccessRateStartTextBox = new System.Windows.Forms.TextBox();
            this.FishingAttemptsTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.FishingSuccessRateMultiplierTextBox);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.FishingDelayTextBox);
            this.tabPage1.Controls.Add(this.FishingSuccessRateStartTextBox);
            this.tabPage1.Controls.Add(this.FishingAttemptsTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(365, 229);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Fishing";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.MonsterSpawnChanceTextBox);
            this.groupBox1.Controls.Add(this.FishingMobIndexComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(6, 151);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(353, 72);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Monster";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Mob Spawn : ";
            // 
            // MonsterSpawnChanceTextBox
            // 
            this.MonsterSpawnChanceTextBox.Location = new System.Drawing.Point(137, 41);
            this.MonsterSpawnChanceTextBox.Name = "MonsterSpawnChanceTextBox";
            this.MonsterSpawnChanceTextBox.Size = new System.Drawing.Size(100, 20);
            this.MonsterSpawnChanceTextBox.TabIndex = 3;
            this.MonsterSpawnChanceTextBox.TextChanged += new System.EventHandler(this.MonsterSpawnChanceTextBox_TextChanged);
            // 
            // FishingMobIndexComboBox
            // 
            this.FishingMobIndexComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FishingMobIndexComboBox.FormattingEnabled = true;
            this.FishingMobIndexComboBox.Location = new System.Drawing.Point(137, 16);
            this.FishingMobIndexComboBox.Name = "FishingMobIndexComboBox";
            this.FishingMobIndexComboBox.Size = new System.Drawing.Size(100, 21);
            this.FishingMobIndexComboBox.TabIndex = 10;
            this.FishingMobIndexComboBox.SelectedIndexChanged += new System.EventHandler(this.FishingMobIndexComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Mob Spawn Chance % : ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Success Rate Multiplier : ";
            // 
            // FishingSuccessRateMultiplierTextBox
            // 
            this.FishingSuccessRateMultiplierTextBox.Location = new System.Drawing.Point(143, 61);
            this.FishingSuccessRateMultiplierTextBox.Name = "FishingSuccessRateMultiplierTextBox";
            this.FishingSuccessRateMultiplierTextBox.Size = new System.Drawing.Size(100, 20);
            this.FishingSuccessRateMultiplierTextBox.TabIndex = 8;
            this.FishingSuccessRateMultiplierTextBox.TextChanged += new System.EventHandler(this.FishingSuccessRateMultiplierTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Delay / ms : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Success Rate Start % : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Attempts / round : ";
            // 
            // FishingDelayTextBox
            // 
            this.FishingDelayTextBox.Location = new System.Drawing.Point(143, 87);
            this.FishingDelayTextBox.Name = "FishingDelayTextBox";
            this.FishingDelayTextBox.Size = new System.Drawing.Size(100, 20);
            this.FishingDelayTextBox.TabIndex = 2;
            this.FishingDelayTextBox.TextChanged += new System.EventHandler(this.FishingDelayTextBox_TextChanged);
            // 
            // FishingSuccessRateStartTextBox
            // 
            this.FishingSuccessRateStartTextBox.Location = new System.Drawing.Point(143, 35);
            this.FishingSuccessRateStartTextBox.Name = "FishingSuccessRateStartTextBox";
            this.FishingSuccessRateStartTextBox.Size = new System.Drawing.Size(100, 20);
            this.FishingSuccessRateStartTextBox.TabIndex = 1;
            this.FishingSuccessRateStartTextBox.TextChanged += new System.EventHandler(this.FishingSuccessRateStartTextBox_TextChanged);
            // 
            // FishingAttemptsTextBox
            // 
            this.FishingAttemptsTextBox.Location = new System.Drawing.Point(143, 9);
            this.FishingAttemptsTextBox.Name = "FishingAttemptsTextBox";
            this.FishingAttemptsTextBox.Size = new System.Drawing.Size(100, 20);
            this.FishingAttemptsTextBox.TabIndex = 0;
            this.FishingAttemptsTextBox.TextChanged += new System.EventHandler(this.FishingAttemptsTextBox_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(373, 255);
            this.tabControl1.TabIndex = 0;
            // 
            // SystemInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 279);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemInfoForm";
            this.Text = "SystemInfoForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SystemInfoForm_FormClosed);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MonsterSpawnChanceTextBox;
        private System.Windows.Forms.TextBox FishingDelayTextBox;
        private System.Windows.Forms.TextBox FishingSuccessRateStartTextBox;
        private System.Windows.Forms.TextBox FishingAttemptsTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox FishingSuccessRateMultiplierTextBox;
        private System.Windows.Forms.ComboBox FishingMobIndexComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;

    }
}