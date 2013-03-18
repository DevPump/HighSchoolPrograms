﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Data.OleDb;

namespace Dresscode
{
    public partial class Password : Form
    {
        public Password()
        {
            InitializeComponent();
        }
        //
        string oldPass;
        string SMTPHost;
        int Port;
        string host;
        string hostPass;
        string sendTo;
        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        globals global = new globals();
        //

        private void Password_Load(object sender, EventArgs e)
        {
            //pull old info here.
            try
            {
                global.oleconnection.Open();
                OleDbCommand command = global.oleconnection.CreateCommand();
                command.CommandText = "SELECT * FROM `Email Settings`";
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    SMTPHost = reader["smtpserver"].ToString();
                    host = reader["hostemail"].ToString();
                    hostPass = reader["hostpassword"].ToString();
                    Port = int.Parse(reader["portnumber"].ToString());
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            finally
            {
                global.oleconnection.Close();
            }
        }

        private void linkLabel_forgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (textBox_teacherID.Text != "")
            {
                string message = "By clicking OK you confirm that you wish to have a new password emailed to you.";
                DialogResult dr = MessageBox.Show(message, "Password Reset", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        //database pull
                        global.oleconnection.Open();
                        OleDbCommand command = global.oleconnection.CreateCommand();
                        command.CommandText = "SELECT * FROM `Teacher Info` WHERE teacherid ='" + textBox_teacherID.Text + "'";
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            sendTo = reader["email"].ToString();
                        }
                        if (sendTo == "" || sendTo == " ")
                        {
                            AddEmail adp = new AddEmail();
                            adp.teacherid = textBox_teacherID.Text;
                            adp.ShowDialog();
                            sendTo = adp.email;
                        }
                        global.oleconnection.Close();
                        // password generator
                        Random rand = new Random();
                        String newPass = "";
                        for (int i = 0; i < 8; i++)
                        {
                            if (rand.NextDouble() > 0.50)
                            {
                                newPass += rand.Next(10);
                            }
                            else
                            {
                                newPass += alphabet[rand.Next(26)];
                            }
                        }
                        //database
                        global.oleconnection.Open();
                        OleDbDataAdapter adpt = new OleDbDataAdapter();
                        adpt.UpdateCommand = new OleDbCommand("UPDATE `Teacher Info` SET [password]='" + newPass + "' WHERE [teacherid]='" + textBox_teacherID.Text + "'", global.oleconnection);
                        adpt.UpdateCommand.ExecuteNonQuery();
                        global.oleconnection.Close();
                        // email
                        SmtpClient sm = new SmtpClient(SMTPHost, Port);
                        sm.EnableSsl = false;
                        sm.Credentials = new NetworkCredential(host, hostPass);
                        MailAddress from = new MailAddress(host);
                        MailAddress to = new MailAddress(sendTo);
                        MailMessage mMsg = new MailMessage(from, to);
                        mMsg.Subject = "Dresscode Password Reset";
                        mMsg.Body = "Your password has been changed to \"" + newPass + "\".";
                        sm.Send(mMsg);

                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }
                    finally
                    {
                        global.oleconnection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("You must enter your teacher ID first.");
            }
        }

        private void button_change_pass_Click(object sender, EventArgs e)
        {
            if (textBox_teacherID.Text != "")
            {
                global.oleconnection.Open();

                OleDbCommand com = global.oleconnection.CreateCommand();
                com.CommandText = "SELECT * FROM `Teacher Info` WHERE teacherid=@tid";
                com.Parameters.Add("tid", OleDbType.VarChar, 255).Value = textBox_teacherID.Text;
                com.CommandType = CommandType.Text;
                OleDbDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    oldPass = read["password"].ToString();
                }
                global.oleconnection.Close();
                if (oldPass == textBox_old_pass.Text)
                {
                    if (textBox_new_pass_first.Text != textBox_old_pass.Text)
                    {
                        if (textBox_new_pass_first.Text == textBox_new_pass_second.Text)
                        {
                            global.oleconnection.Open();
                            OleDbDataAdapter adpt = new OleDbDataAdapter();
                            adpt.UpdateCommand = new OleDbCommand("UPDATE `Teacher Info` SET [password]=@pass WHERE [teacherid]=@tid", global.oleconnection);
                            adpt.UpdateCommand.Parameters.Add("pass", OleDbType.VarChar, 255).Value = textBox_new_pass_first.Text;
                            adpt.UpdateCommand.Parameters.Add("tid", OleDbType.VarChar, 255).Value = textBox_teacherID.Text;
                            adpt.UpdateCommand.CommandType = CommandType.Text;
                            adpt.UpdateCommand.ExecuteNonQuery();
                            global.oleconnection.Close();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Your new password must be the same in both boxes.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Your new password can not match your old one.");
                    }
                }
                else
                {
                    MessageBox.Show("You must enter your old password.");
                }
            }
            else
            {
                MessageBox.Show("You must enter your teacher ID first.");
            }
        }
    }
}