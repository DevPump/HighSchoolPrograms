﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Globalization;

namespace Dresscode
{
    public partial class Student_Editor : Form
    {
        public Student_Editor()
        {
            InitializeComponent();
        }
        globals gl = new globals();
        DB_Interaction dbi = new DB_Interaction();

        private void textBox_studentID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            bool goodop = false;
            DialogResult dr = MessageBox.Show("Warning, this will erase all current students and repopulate with the selected Excel spreadsheet\nMake sure the Excel document is formated in this order and format \"Student ID\" \"LastName\" \"FirstName\" \"Grade\"\nAll student Info must be on sheet1\nExcel document type: 97-2003 *.xls\nDo you want to continue?", "Verification", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    DialogResult dr2 = MessageBox.Show("Warning, this will erase all current students and repopulate with the selected Excel spreadsheet\nMake sure the Excel document is formated in this order\n\"Student ID\" \"LastName\" \"FirstName\" \"Grade\"\nThis will reappear and notify when the operation is completed\n5-10 minutes is required to complete this operation.\nDo you want to continue?", "Verification", MessageBoxButtons.YesNo);
                    if (dr2 == DialogResult.Yes)
                    {
                        try
                        {
                            this.Hide();
                            if (gl.oleconnection.State == ConnectionState.Open)
                                gl.oleconnection.Close();
                            System.Data.OleDb.OleDbConnection excelconnection = new System.Data.OleDb.OleDbConnection(@"provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + ofd.FileName + "';Extended Properties=Excel 8.0;");
                            System.Data.DataSet DtSet = new System.Data.DataSet();
                            System.Data.OleDb.OleDbDataAdapter oledbd_readexcel = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", excelconnection);
                            oledbd_readexcel.Fill(DtSet);
                            excelconnection.Close();
                            if (gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
                            DataTableReader dtr_excel = DtSet.CreateDataReader(DtSet.Tables[0]);
                            string dsql = "DELETE * FROM `" + gl.tbl_studentinfo + "`";
                            string[] dpar = { };
                            string[] dvalues = { };
                            dbi.dbcommands(dsql, dpar, dvalues);
                            while (dtr_excel.Read())
                            {
                                if (gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
                                OleDbDataAdapter oledba_massadd = new OleDbDataAdapter();
                                string studentid = dtr_excel.GetValue(0).ToString();
                                string lastname = dtr_excel.GetValue(1).ToString();
                                string firstname = dtr_excel.GetValue(2).ToString();
                                string grade = dtr_excel.GetValue(3).ToString();
                                string sql = "INSERT INTO `" + gl.tbl_studentinfo + "` VALUES ('" + 0 + "',@studentid,@lastname,@firstname,@grade)";
                                string[] pars = { "@studentid", "@lastname", "@firstname", "@grade" };
                                string[] values = { studentid, lastname, firstname, grade };
                                dbi.dbcommands(sql, pars, values);
                            }
                            this.Show();
                            dbupdater();
                            //datagridupdate();
                            MessageBox.Show("All students have been added successfully");
                            goodop = true;
                        }
                        catch (Exception ex)
                        {
                            this.Show();
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }
            if (!goodop)
                MessageBox.Show("Canceled", "Operation Status");
        }

        private void button_addstudent_Click(object sender, EventArgs e)
        {
            textBox_lastname.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textBox_lastname.Text);
            textBox_firstname.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textBox_firstname.Text);
            string lastname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textBox_lastname.Text);
            string firstname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textBox_firstname.Text);
            bool properinfo = false;
            if (lastname != "")
                if (firstname != "")
                    if (textBox_studentID.Text != "")
                        properinfo = true;
            if (properinfo)
            {
                try
                {
                    bool newentry = true;
                    if (gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
                    OleDbCommand checkforstudent = gl.oleconnection.CreateCommand();
                    checkforstudent.CommandText = "SELECT * FROM `" + gl.tbl_studentinfo + "`";
                    OleDbDataReader checkexistingstudent = checkforstudent.ExecuteReader();
                    while (checkexistingstudent.Read())
                    {
                        if (newentry == true)
                        {
                            if (textBox_studentID.Text.Contains(checkexistingstudent[gl.col_studentid].ToString()))
                            {
                                newentry = false;
                                break;
                            }
                        }
                    }
                    if (gl.oleconnection.State == ConnectionState.Open) gl.oleconnection.Close();
                    if (newentry)
                    {
                        DialogResult dr = MessageBox.Show("Is this correct?\nStudent ID: " + textBox_studentID.Text + "\nStudent Name: " + textBox_firstname.Text + " " + textBox_lastname.Text + "\nGrade: " + numericUpDown1.Value.ToString() + "", "Verification", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            string sql = "INSERT INTO `" + gl.tbl_studentinfo + "` VALUES ('" + 0 + "',@studentid,@lastname,@firstname,@grade)";
                            string[] pars = { "@studentid", "@lastname", "@firstname", "@grade" };
                            string[] values = { textBox_studentID.Text, lastname, firstname, int.Parse(numericUpDown1.Value.ToString()).ToString() };
                            dbi.dbcommands(sql, pars, values);
                            dbupdater();
                            //datagridupdate();
                            MessageBox.Show("Student ID: " + textBox_studentID.Text + "\nStudent Name: " + textBox_firstname.Text + " " + textBox_lastname.Text + "\nGrade: " + numericUpDown1.Value.ToString() + "\nHas been successfully added to the student list.", "Success");
                            textBox_studentID.Text = "";
                            textBox_firstname.Text = "";
                            textBox_lastname.Text = "";
                            numericUpDown1.Value = 9;
                        }
                    }
                    else
                    {
                        if (gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
                        OleDbCommand getexisting = gl.oleconnection.CreateCommand();
                        getexisting.CommandText = "SELECT * FROM `" + gl.tbl_studentinfo + "` WHERE `" + gl.col_studentid + "`=@studentid";
                        getexisting.Parameters.AddWithValue("studentid", textBox_studentID.Text);
                        OleDbDataReader getexistingstudent = checkforstudent.ExecuteReader();
                        string efirst = "";
                        string elast = "";
                        string egrade = "";
                        while (getexistingstudent.Read())
                        {
                            if (textBox_studentID.Text.Contains(getexistingstudent[gl.col_studentid].ToString()))
                            {
                                efirst = getexistingstudent[gl.col_firstname].ToString();
                                elast = getexistingstudent[gl.col_lastname].ToString();
                                egrade = getexistingstudent[gl.col_grade].ToString();
                                break;
                            }
                        }
                        MessageBox.Show("The Student ID: " + textBox_studentID.Text + " already exists.\nExisting Student: " + efirst + " " + elast + "\nGrade: " + egrade);
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message, "Error");
                }
                finally
                {
                    if (gl.oleconnection.State == ConnectionState.Open)
                        gl.oleconnection.Close();
                }
            }
            else
                MessageBox.Show("Something appears to be missing");
        }

        private void textBox_firstname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
                e.Handled = true;
        }

        private void textBox_lastname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
                e.Handled = true;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
                OleDbDataAdapter adpt = new OleDbDataAdapter();
                string sql = "UPDATE `" + gl.tbl_studentinfo + "` SET [" + gl.col_studentid + "]=@sid,[" + gl.col_lastname + "]=@lastname, [" + gl.col_firstname + "]=@firstname, [" + gl.col_grade + "]=@grade WHERE [" + gl.col_id + "]=@idnum";
                adpt.UpdateCommand = new OleDbCommand(sql, gl.oleconnection);
                adpt.UpdateCommand.Parameters.Add("sid", OleDbType.VarChar, 255).Value = dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                adpt.UpdateCommand.Parameters.Add("lastname", OleDbType.VarChar, 255).Value = dataGridView1[2, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                adpt.UpdateCommand.Parameters.Add("firstname", OleDbType.VarChar, 255).Value = dataGridView1[3, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                adpt.UpdateCommand.Parameters.Add("grade", OleDbType.VarChar, 255).Value = dataGridView1[4, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                adpt.UpdateCommand.Parameters.Add("idnum", OleDbType.Guid, 255).Value = Guid.Parse(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());
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
                dataGridView1.EndEdit();
                dbupdater();
                //datagridupdate();
            }
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            //When DB is modified, change to new form.
            e.Cancel = false;
            DialogResult dr = MessageBox.Show("Are you sure you want to delete:\n" + dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString() + " " + dataGridView1[2, dataGridView1.CurrentCell.RowIndex].Value.ToString(), "Confirmation", MessageBoxButtons.YesNo);
            if (dr == DialogResult.No)
                e.Cancel = true;
            else
            {
                try
                {
                    string[] pars = { "@idnum" };
                    string[] values = { dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString() };
                    dbi.dbcommands("DELETE * FROM `" + gl.tbl_studentinfo + "` WHERE `" + gl.col_id + "`=@idnum", pars, values);
                }
                catch (Exception) { }
            }
        }
        public void dbupdater()
        {

        }
        public class dbretrival
        {

        }

        private void Student_Editor_Load(object sender, EventArgs e)
        {
            pictureBox1.Location = new System.Drawing.Point(220, 12);
            dataGridView1.Visible = true;
            dbi.sql = "SELECT * FROM `" + gl.tbl_studentinfo + "`";
            dbi.frmname = this.Name;
            dbi.dgn = dataGridView1.Name;
            dbi.teste();
            pictureBox1.Visible = true;
            timer1.Enabled = true;
            timer1.Interval = 50;
            textBox_firstname.Text = "Please Wait...";
            textBox_lastname.Text = "Please Wait...";
            textBox_studentID.Text = "Please Wait...";
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Org 230, 210
            if (dbi.finished)
            {
                Random rand = new Random();
                if (this.Size.Width != this.MaximumSize.Width)
                    this.Size = new System.Drawing.Size(this.Size.Width + 5, 0);
                else
                {
                    timer1.Enabled = false;
                    this.MinimumSize = new System.Drawing.Size(this.MaximumSize.Width, this.MaximumSize.Height);
                    dataGridView1.Visible = true;
                    if (textBox_studentID.Text == "Please Wait...")
                    {
                        textBox_studentID.Clear();
                        textBox_lastname.Clear();
                        textBox_firstname.Clear();
                        textBox_firstname.Enabled = true;
                        textBox_firstname.Enabled = true;
                        textBox_firstname.Enabled = true;
                        pictureBox1.Visible = false;
                        button_addstudent.Enabled = true;
                        button1.Enabled = true;
                        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbupdater();
            textBox_firstname.Focus();
        }

    }
}
