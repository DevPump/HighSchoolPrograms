﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Globalization;

namespace Dresscode
{
    public partial class Teacher_Editor : Form
    {
        public Teacher_Editor()
        {
            InitializeComponent();
        }
        globals gl = new globals();
        string SMTPHost;
        int Port;
        string host;
        string hostPass;
        string sendTo;
        DataSet ds = new DataSet();
        DB_Interaction dbi = new DB_Interaction();
        private void button_addteacher_Click(object sender, EventArgs e)
        {
            bool legit = false;
            if (textbox_teacherid.Text != "")
                legit = true;
            else
                legit = false;
            if (textbox_lastname.Text != "")
                legit = true;
            else
                legit = false;
            if (textbox_firstname.Text != "")
                legit = true;
            else
                legit = false;
            if (textbox_email.Text != "")
                legit = true;
            else
                legit = false;

            if (legit)
            {
                try
                {
                    string lastname = "", firstname = "", email = "", dean = "";
                    textbox_lastname.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textbox_lastname.Text);
                    textbox_firstname.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textbox_firstname.Text);
                    bool newuser = true;
                    if (gl.oleconnection.State == ConnectionState.Closed)
                        gl.oleconnection.Open();
                    OleDbCommand getteacherscommand = gl.oleconnection.CreateCommand();
                    getteacherscommand.CommandText = "SELECT * FROM `" + gl.tbl_teacherinfo + "` WHERE `" + gl.col_teacherid + "`=@tid";
                    getteacherscommand.Parameters.Add("tid", OleDbType.VarChar, 255).Value = textbox_teacherid.Text;
                    getteacherscommand.CommandType = CommandType.Text;
                    OleDbDataReader getteacher = getteacherscommand.ExecuteReader();

                    while (getteacher.Read())
                    {
                        if (getteacher[gl.col_teacherid].ToString() == textbox_teacherid.Text)
                            newuser = false;
                        lastname = getteacher[gl.col_lastname].ToString();
                        firstname = getteacher[gl.col_firstname].ToString();
                        email = getteacher[gl.col_email].ToString();
                        dean = getteacher[gl.col_dean].ToString();
                    }
                    if (gl.oleconnection.State == ConnectionState.Open)
                        gl.oleconnection.Close();
                    if (newuser)
                    {
                        //var initialization
                        sendTo = textbox_email.Text;
                        //pass gen
                        string alphabet = "abcdefghijklmnopqrstuvwxyz";
                        Random rand = new Random();
                        String newPass = "";
                        for (int i = 0; i < 8; i++)
                        {
                            if (rand.NextDouble() > 0.50)
                                newPass += rand.Next(10);
                            else
                                newPass += alphabet[rand.Next(26)];
                        }
                        MD5 md51 = new MD5CryptoServiceProvider();
                        md51.ComputeHash(ASCIIEncoding.ASCII.GetBytes(newPass));
                        byte[] result1 = md51.Hash;
                        StringBuilder strBuilder1 = new StringBuilder();
                        for (int i = 0; i < result1.Length; i++)
                            strBuilder1.Append(result1[i].ToString("x2"));
                        string admin = "";
                        if (checkbox_dean.Checked) admin = "Yes";
                        else
                            admin = "No";
                        string sql = "INSERT INTO `" + gl.tbl_teacherinfo + "` VALUES ('" + 0 + "', @teacherid,@password,@lastname,@firstname,@email,@admin)";
                        string[] pars = { "@teacherid", "@password", "@lastname", "@firstname", "@email", "@admin" };
                        string[] values = { CultureInfo.CurrentCulture.TextInfo.ToLower(textbox_teacherid.Text), strBuilder1.ToString(), CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textbox_lastname.Text), CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textbox_firstname.Text), textbox_email.Text, admin };
                        dbi.dbcommands(sql, pars, values);

                        //email
                        SmtpClient sm = new SmtpClient(SMTPHost, Port);
                        sm.EnableSsl = false;
                        sm.Credentials = new NetworkCredential(host, hostPass);
                        MailAddress from = new MailAddress(host);
                        MailAddress to = new MailAddress(sendTo);
                        MailMessage mMsg = new MailMessage(from, to);
                        mMsg.Subject = "Dresscode User Creation";
                        mMsg.Body = "Hello " + textbox_firstname.Text + " " + textbox_lastname.Text + "\nYour Login information is, ID: \"" + textbox_teacherid.Text + "\" Password:\"" + newPass + "\".";
                        sm.Send(mMsg);
                        datagridupdate();
                        MessageBox.Show(textbox_firstname.Text + " " + textbox_lastname.Text + " was successfully added");
                        textbox_email.Clear();
                        textbox_firstname.Clear();
                        textbox_teacherid.Clear();
                        textbox_lastname.Clear();
                        checkbox_dean.Checked = false;
                    }
                    else
                        MessageBox.Show("The teacher ID: " + textbox_teacherid.Text + " is already in use.\nName: " + lastname + ", " + firstname + "\nEmail: " + email + "\nIs Dean: " + dean,"Teacher ID already in use.");
                }
                catch (Exception x) { MessageBox.Show(x.Message); }
            }
            else
                MessageBox.Show("Something appears to be empty");
        }

        private void Teacher_Editor_Load(object sender, EventArgs e)
        {
            pictureBox3.Location = new System.Drawing.Point(12, 220);
            try
            {
                if (gl.oleconnection.State == ConnectionState.Closed)
                    gl.oleconnection.Open();
                OleDbCommand emailcommand = gl.oleconnection.CreateCommand();
                emailcommand.CommandText = "SELECT * FROM `" + gl.tbl_emailsettings + "`";
                OleDbDataReader emailreader = emailcommand.ExecuteReader();
                while (emailreader.Read())
                {
                    SMTPHost = emailreader[gl.col_smtpserver].ToString();
                    host = emailreader[gl.col_hostemail].ToString();
                    hostPass = emailreader[gl.col_hostpassword].ToString();
                    Port = int.Parse(emailreader[gl.col_portnumber].ToString());
                }
                datagridupdate();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            finally
            {
                if (gl.oleconnection.State == ConnectionState.Open)
                    gl.oleconnection.Close();
            }
            datagridview_teachers.Visible = false;
            timer1.Enabled = true;
            timer1.Interval = 50;
        }

        private void datagridview_teachers_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = false;
            DialogResult dr = MessageBox.Show("Are you sure you want to delete:\n" + datagridview_teachers[3, datagridview_teachers.CurrentCell.RowIndex].Value.ToString() + " " + datagridview_teachers[2, datagridview_teachers.CurrentCell.RowIndex].Value.ToString(), "Confirmation", MessageBoxButtons.YesNo);
            if (dr == DialogResult.No)
                e.Cancel = true;
            else
            {
                try
                {
                    string[] pars = { "@idnum" };
                    string[] values = { datagridview_teachers[0, datagridview_teachers.CurrentCell.RowIndex].Value.ToString() };
                    dbi.dbcommands("DELETE * FROM `" + gl.tbl_teacherinfo + "` WHERE `" + gl.col_teacherid + "`=@idnum", pars, values);
                }
                catch (Exception) { }
            }
        }
        public void datagridupdate()
        {
            dbi.sql = "SELECT * FROM `" + gl.tbl_teacherinfo + "`";
            dbi.frmname = this.Name;
            dbi.dgn = datagridview_teachers.Name;
            dbi.teste();
            //dbi.dgvselectioncommand("SELECT * FROM `" + gl.tbl_teacherinfo + "`", "", "", "", "", "", this.Name, datagridview_teachers.Name);
        }

        private void datagridview_teachers_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
                OleDbDataAdapter adpt = new OleDbDataAdapter();
                string sql = "UPDATE `" + gl.tbl_teacherinfo + "` SET [" + gl.col_teacherid + "]=@tid,[" + gl.col_lastname + "]=@lastname, [" + gl.col_firstname + "]=@firstname, [" + gl.col_email + "]=@email, [" + gl.col_dean + "]=@dean WHERE [" + gl.col_id + "]=@idnum";
                adpt.UpdateCommand = new OleDbCommand(sql, gl.oleconnection);
                adpt.UpdateCommand.Parameters.Add("tid", OleDbType.VarChar, 255).Value = datagridview_teachers[1, datagridview_teachers.CurrentCell.RowIndex].Value.ToString();
                adpt.UpdateCommand.Parameters.Add("lastname", OleDbType.VarChar, 255).Value = datagridview_teachers[3, datagridview_teachers.CurrentCell.RowIndex].Value.ToString();
                adpt.UpdateCommand.Parameters.Add("firstname", OleDbType.VarChar, 255).Value = datagridview_teachers[4, datagridview_teachers.CurrentCell.RowIndex].Value.ToString();
                adpt.UpdateCommand.Parameters.Add("email", OleDbType.VarChar, 255).Value = datagridview_teachers[5, datagridview_teachers.CurrentCell.RowIndex].Value.ToString();
                adpt.UpdateCommand.Parameters.Add("dean", OleDbType.VarChar, 255).Value = datagridview_teachers[6, datagridview_teachers.CurrentCell.RowIndex].Value.ToString();
                adpt.UpdateCommand.Parameters.Add("idnum", OleDbType.Guid, 255).Value = Guid.Parse(datagridview_teachers[0, datagridview_teachers.CurrentCell.RowIndex].Value.ToString());
                adpt.UpdateCommand.CommandType = CommandType.Text;
                adpt.UpdateCommand.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            finally
            {
                if (gl.oleconnection.State == ConnectionState.Open) gl.oleconnection.Close();
                datagridview_teachers.EndEdit();
                datagridupdate();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (dbi.finished)
            {
                //550 S 225
                if (this.Size.Height != this.MaximumSize.Height)
                    this.Size = new System.Drawing.Size(0, this.Size.Height + 10);
                else
                {
                    //Place Loading markhor in spot of datagridview until fully loaded then make dgv visible.
                    timer1.Enabled = false;
                    this.MinimumSize = new System.Drawing.Size(this.MaximumSize.Width, this.MaximumSize.Height);
                    pictureBox3.Visible = false;
                    pictureBox3.Enabled = false;
                    datagridview_teachers.Visible = true;
                }
            }
        }
    }
}
