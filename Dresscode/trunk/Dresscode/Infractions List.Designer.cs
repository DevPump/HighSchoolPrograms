﻿namespace Dresscode
{
    partial class Infractions_List
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button_removeinfraction = new System.Windows.Forms.Button();
            this.listBox_infractions = new System.Windows.Forms.ListBox();
            this.textBox_infraction = new System.Windows.Forms.TextBox();
            this.button_addinfraction = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button_removeinfraction);
            this.groupBox4.Controls.Add(this.listBox_infractions);
            this.groupBox4.Controls.Add(this.textBox_infraction);
            this.groupBox4.Controls.Add(this.button_addinfraction);
            this.groupBox4.Location = new System.Drawing.Point(12, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(325, 200);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Infractions";
            // 
            // button_removeinfraction
            // 
            this.button_removeinfraction.Location = new System.Drawing.Point(244, 87);
            this.button_removeinfraction.Name = "button_removeinfraction";
            this.button_removeinfraction.Size = new System.Drawing.Size(75, 59);
            this.button_removeinfraction.TabIndex = 3;
            this.button_removeinfraction.Text = "Remove";
            this.button_removeinfraction.UseVisualStyleBackColor = true;
            this.button_removeinfraction.Click += new System.EventHandler(this.button_removeinfraction_Click);
            // 
            // listBox_infractions
            // 
            this.listBox_infractions.FormattingEnabled = true;
            this.listBox_infractions.Location = new System.Drawing.Point(6, 50);
            this.listBox_infractions.Name = "listBox_infractions";
            this.listBox_infractions.Size = new System.Drawing.Size(232, 134);
            this.listBox_infractions.TabIndex = 2;
            // 
            // textBox_infraction
            // 
            this.textBox_infraction.Location = new System.Drawing.Point(6, 24);
            this.textBox_infraction.Name = "textBox_infraction";
            this.textBox_infraction.Size = new System.Drawing.Size(232, 20);
            this.textBox_infraction.TabIndex = 1;
            this.textBox_infraction.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_infraction_KeyDown);
            // 
            // button_addinfraction
            // 
            this.button_addinfraction.Location = new System.Drawing.Point(244, 22);
            this.button_addinfraction.Name = "button_addinfraction";
            this.button_addinfraction.Size = new System.Drawing.Size(75, 59);
            this.button_addinfraction.TabIndex = 0;
            this.button_addinfraction.Text = "Add";
            this.button_addinfraction.UseVisualStyleBackColor = true;
            this.button_addinfraction.Click += new System.EventHandler(this.button_addinfraction_Click);
            // 
            // Infractions_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 222);
            this.Controls.Add(this.groupBox4);
            this.Name = "Infractions_List";
            this.Text = "Infractions_List";
            this.Load += new System.EventHandler(this.Infractions_List_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button_removeinfraction;
        private System.Windows.Forms.ListBox listBox_infractions;
        private System.Windows.Forms.TextBox textBox_infraction;
        private System.Windows.Forms.Button button_addinfraction;
    }
}