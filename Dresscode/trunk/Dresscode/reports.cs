﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Globalization;


namespace Dresscode
{
    public partial class Reports : Form
    {
        BindingSource bSource = new BindingSource();
        string sql = "", firstname, lastname, studentid, teacher, infraction, learningcenter;
        globals gl = new globals();
        OleDbDataAdapter dAdapter;
        DB_Interaction dbi = new DB_Interaction();
        bool wtfbrah = true;
        //
        int county = 0,
nineWeeksDatabase = 0,
currentNineWeeks = 0,
totalinfractions = 1;
        /**/
        DateTime
firstnineweeksstart,
firstnineweeksend,

secondtnineweeksstart,
secondnineweeksend,

thirdnineweeksstart,
thirdnineweeksend,

forthnineweeksstart,
forthnineweeksend;
        //
        public Reports()
        {
            InitializeComponent();
        }

        private void reports_Load(object sender, EventArgs e)
        {
            try
            {
                if(gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
                OleDbCommand getdatescommand = gl.oleconnection.CreateCommand();
                getdatescommand.CommandText = "SELECT * FROM `" + gl.tbl_nineweeksdates + "`";
                OleDbDataReader getdateinfo = getdatescommand.ExecuteReader();
                while (getdateinfo.Read())
                {
                    firstnineweeksstart = DateTime.Parse(getdateinfo[gl.col_firstnineweeksstart].ToString());
                    firstnineweeksend = DateTime.Parse(getdateinfo[gl.col_firstnineweeksend].ToString());

                    secondtnineweeksstart = DateTime.Parse(getdateinfo[gl.col_secondnineweeksstart].ToString());
                    secondnineweeksend = DateTime.Parse(getdateinfo[gl.col_secondnineweeksend].ToString());

                    thirdnineweeksstart = DateTime.Parse(getdateinfo[gl.col_thirdnineweeksstart].ToString());
                    thirdnineweeksend = DateTime.Parse(getdateinfo[gl.col_thirdnineweeksend].ToString());

                    forthnineweeksstart = DateTime.Parse(getdateinfo[gl.col_forthnineweeksstart].ToString());
                    forthnineweeksend = DateTime.Parse(getdateinfo[gl.col_forthnineweeksend].ToString());
                }
            }
            catch (Exception x) { MessageBox.Show(x.Message, "Error"); }
            finally { if(gl.oleconnection.State == ConnectionState.Open) gl.oleconnection.Close(); }

            try
            {
                if(gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
                OleDbCommand getinfractioncommand = gl.oleconnection.CreateCommand();
                getinfractioncommand.CommandText = "SELECT * FROM `"+gl.tbl_reports+"`";
                OleDbDataReader getinfraction = getinfractioncommand.ExecuteReader();
                while (getinfraction.Read())
                {
                    if (!combobox_teacher.Items.Contains(getinfraction[gl.col_teacher].ToString()))
                    {
                        combobox_teacher.Items.Add(getinfraction[gl.col_teacher].ToString());
                    }

                    if (!comboBox_infraction_select.Items.Contains(getinfraction[gl.col_infractions].ToString()))
                    {
                        comboBox_infraction_select.Items.Add(getinfraction[gl.col_infractions].ToString());
                    }
                    if (!comboBox_learningcenter.Items.Contains(getinfraction[gl.col_learningcenter].ToString()))
                        comboBox_learningcenter.Items.Add(getinfraction[gl.col_learningcenter].ToString());
                }
            }
            catch (Exception x) { MessageBox.Show(x.Message, "Error"); }
            finally { if(gl.oleconnection.State == ConnectionState.Open) gl.oleconnection.Close(); }
        }
        public void getinfractions()
        {
            try
            {
                //dbi.dgvselectioncommand(sql, firstname, lastname, studentid,teacher,infraction, this.Name, dataGridView_reports.Name);
                dbi.sql = sql;
                dbi.firstname = firstname;
                dbi.lastname = lastname;
                dbi.studentid = studentid;
                dbi.teacher = teacher;
                dbi.infraction = infraction;
                dbi.frmname = this.Name;
                dbi.dgn = dataGridView_reports.Name;
                dbi.learningcenter = learningcenter;
                dbi.teste();
                if(gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
                OleDbCommand getinfractioncommand = gl.oleconnection.CreateCommand();
                getinfractioncommand.CommandText = sql;
                if(teacher != "")
                    getinfractioncommand.Parameters.Add("teacher", OleDbType.VarChar, 255).Value = teacher;
                if(infraction != "")
                    getinfractioncommand.Parameters.Add("infraction", OleDbType.VarChar, 255).Value = infraction;
                if (firstname != "")
                {
                    getinfractioncommand.Parameters.Add("firstname", OleDbType.VarChar, 255).Value = firstname;
                    getinfractioncommand.Parameters.Add("lastname", OleDbType.VarChar, 255).Value = lastname;
                    getinfractioncommand.Parameters.Add("studentid", OleDbType.VarChar, 255).Value = studentid;
                }
                if (learningcenter != "")
                    getinfractioncommand.Parameters.Add("learningcenter", OleDbType.VarChar, 255).Value = learningcenter;
                getinfractioncommand.CommandType = CommandType.Text;
                OleDbDataReader getinfraction = getinfractioncommand.ExecuteReader();
                label_nineweeks.Text = "Infractions this 9 weeks: 0";
                label_total_reports.Text = "Total Infractions: 0";
                while (getinfraction.Read())
                {
                    DateTime databasedate = DateTime.Parse(getinfraction[gl.col_reportdate].ToString());
                    if (databasedate >= firstnineweeksstart && databasedate <= firstnineweeksend)
                    {
                        nineWeeksDatabase = 1;
                    }
                    if (databasedate >= secondtnineweeksstart && databasedate <= secondnineweeksend)
                    {
                        nineWeeksDatabase = 2;
                    }
                    if (databasedate >= thirdnineweeksstart && databasedate <= thirdnineweeksend)
                    {
                        nineWeeksDatabase = 3;
                    }
                    if (databasedate >= forthnineweeksstart && databasedate <= forthnineweeksend)
                    {
                        nineWeeksDatabase = 4;
                    }
                    if (DateTime.Today >= secondtnineweeksstart && DateTime.Today <= secondnineweeksend)
                    {
                        currentNineWeeks = 2;
                    }
                    if (DateTime.Today >= thirdnineweeksstart && DateTime.Today <= thirdnineweeksend)
                    {
                        currentNineWeeks = 3;
                    }
                    if (DateTime.Today >= forthnineweeksstart && DateTime.Today <= forthnineweeksend)
                    {
                        currentNineWeeks = 4;
                    }
                    if (currentNineWeeks == nineWeeksDatabase)
                    {
                        label_nineweeks.Text = "Infractions this 9 weeks: " + (county++).ToString();
                    }
                    label_total_reports.Text = "Total Infractions: " + (totalinfractions++).ToString();
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            finally
            {
                if(gl.oleconnection.State == ConnectionState.Open) gl.oleconnection.Close();
            }
        }

        private void checkBox_date_single_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_date_single.Checked == true)
            {
                datetimepicker_date_start.Enabled = true;
                checkBox_date_range.Enabled = true;
            }
            else
            {
                datetimepicker_date_start.Enabled = false;
                checkBox_date_range.Enabled = false;
                checkBox_date_range.Checked = false;
            }
        }

        private void checkBox_date_range_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_date_range.Checked == true)
            {
                datetimepicker_date_end.Enabled = true;
            }
            else
            {
                datetimepicker_date_end.Enabled = false;
            }
        }

        private void checkBox_teacher_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_teacher.Checked == true)
            {
                combobox_teacher.Enabled = true;
            }
            else
            {
                combobox_teacher.Enabled = false;
                combobox_teacher.Text = "";
            }
        }

        private void checkBox_infraction_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_infraction.Checked == true)
            {
                comboBox_infraction_select.Enabled = true;
            }
            else
            {
                comboBox_infraction_select.Enabled = false;
            }
        }

        private void checkBox_period_range_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_period_range.Checked == true)
            {
                numericUpDown_period_end.Enabled = true;
            }
            else
            {
                numericUpDown_period_end.Enabled = false;
            }
        }

        private void checkBox_period_single_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_period_single.Checked == true)
            {
                numericUpDown_period_start.Enabled = true;
                checkBox_period_range.Enabled = true;
            }
            else
            {
                numericUpDown_period_start.Enabled = false;
                checkBox_period_range.Enabled = false;
                checkBox_period_range.Checked = false;
            }
        }

        private void checkBox_grade_single_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_grade_single.Checked == true)
            {
                numericUpDown_grade_start.Enabled = true;
                checkBox_grade_range.Enabled = true;
            }
            else
            {
                numericUpDown_grade_start.Enabled = false;
                checkBox_grade_range.Enabled = false;
                checkBox_grade_range.Checked = false;
            }
        }

        private void checkBox_grade_range_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_grade_range.Checked == true)
            {
                numericUpDown_grade_end.Enabled = true;
            }
            else
            {
                numericUpDown_grade_end.Enabled = false;
            }
        }

        private void checkBox_student_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_student.Checked == true)
            {
                comboBox_student_firstname.Enabled = true;
                comboBox_student_last.Enabled = true;
            }
            else
            {
                comboBox_student_firstname.Enabled = false;
                comboBox_student_last.Enabled = false;
            }
        }
        private void comboBox_student_last_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                getstudentinfo();
        }

        private void comboBox_student_firstname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                getstudentinfo();
        }

        public void getstudentinfo()
        {
            OleDbCommand getstudentinfocommand = gl.oleconnection.CreateCommand();
            int retrievalcode = 0;
            comboBox_student_firstname.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(comboBox_student_firstname.Text);
            comboBox_student_last.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(comboBox_student_last.Text);
            firstname = comboBox_student_firstname.Text;
            lastname = comboBox_student_last.Text;
            comboBox_student_firstname.Items.Clear();
            comboBox_student_last.Items.Clear();
            getstudentinfocommand.CommandText = "SELECT * FROM `"+gl.tbl_studentinfo+"`";
            if (comboBox_student_firstname.Text == "" && comboBox_student_last.Text == "")
                retrievalcode = -1;
            if (comboBox_student_firstname.Text == "" && comboBox_student_last.Text != "")
            {
                retrievalcode = 0;
                getstudentinfocommand.CommandText += " WHERE `" + gl.col_lastname + "`=@lastname";
                getstudentinfocommand.Parameters.Add("lastname", OleDbType.VarChar, 255).Value = lastname;
            }
            if (comboBox_student_firstname.Text != "" && comboBox_student_last.Text == "")
            {
                retrievalcode = 1;
                getstudentinfocommand.CommandText += " WHERE `"+gl.col_firstname+"`=@firstname";
                getstudentinfocommand.Parameters.Add("firstname", OleDbType.VarChar, 255).Value = firstname;
            }
            if (comboBox_student_last.Text != "" && comboBox_student_firstname.Text != "")
            {
                for (int i = 0; i < comboBox_student_firstname.Text.Length; i++)
                {
                    if (comboBox_student_firstname.Text[i] == ' ')
                    {
                        firstname = comboBox_student_firstname.Text.Substring(0, i);
                        studentid = comboBox_student_firstname.Text.Substring(i + 1, (comboBox_student_firstname.Text.Length - (i + 1)));
                    }
                }
                for (int i = 0; i < comboBox_student_last.Text.Length; i++)
                {
                    if (comboBox_student_last.Text[i] == ' ')
                    {
                        lastname = comboBox_student_last.Text.Substring(0, i);
                        studentid = comboBox_student_last.Text.Substring(i + 1, (comboBox_student_last.Text.Length - (i + 1)));
                    }
                }
                retrievalcode = 2;
                getstudentinfocommand.CommandText = "SELECT * FROM `"+gl.tbl_studentinfo+"` WHERE `"+gl.col_firstname+"`=@firstname AND `"+gl.col_lastname+"`=@lastname";
                getstudentinfocommand.Parameters.Add("firstname", OleDbType.VarChar, 255).Value = firstname;
                getstudentinfocommand.Parameters.Add("lastname", OleDbType.VarChar, 255).Value = lastname;
            }
            try
            {
                if(gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
                if (retrievalcode != -1)
                {
                    getstudentinfocommand.CommandType = CommandType.Text;
                    OleDbDataReader getstudentinfo = getstudentinfocommand.ExecuteReader();
                    while (getstudentinfo.Read())
                    {
                        if (retrievalcode == 0)
                        {
                            comboBox_student_firstname.Text = getstudentinfo[gl.col_firstname].ToString() + " " + getstudentinfo[gl.col_studentid].ToString();
                            comboBox_student_firstname.Items.Add(getstudentinfo[gl.col_firstname].ToString() + " " + getstudentinfo[gl.col_studentid ].ToString());
                        }
                        if (retrievalcode == 1)
                        {
                            comboBox_student_last.Text = getstudentinfo[gl.col_lastname].ToString() + " " + getstudentinfo[gl.col_studentid ].ToString();
                            comboBox_student_last.Items.Add(getstudentinfo[gl.col_lastname].ToString() + " " + getstudentinfo[gl.col_studentid ].ToString());
                        }
                        if (retrievalcode == 2)
                        {
                            comboBox_student_last.Text = lastname + " " + getstudentinfo[gl.col_studentid].ToString();
                            comboBox_student_last.Items.Add(lastname + " " + getstudentinfo[gl.col_studentid].ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter a first or last name", "Missing Name");
                }
            }
            catch (Exception x) { MessageBox.Show(x.Message, "Error"); }
            finally { if(gl.oleconnection.State == ConnectionState.Open) gl.oleconnection.Close(); }
        }

        private void button_export_excel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Document | *.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                    ExcelApp.Application.Workbooks.Add(Type.Missing);
                    ExcelApp.Columns.ColumnWidth = 15;
                    for (int i = 4; i < dataGridView_reports.Columns.Count + 1; i++)
                    {
                        ExcelApp.Cells[1, i - 3] = dataGridView_reports.Columns[i - 1].HeaderText;
                    }
                    for (int i = 0; i < dataGridView_reports.Rows.Count - 1; i++)
                    {
                        for (int j = 3; j < dataGridView_reports.Columns.Count; j++)
                        {
                            ExcelApp.Cells[i + 2, j - 2] = dataGridView_reports.Rows[i].Cells[j].Value.ToString();
                        }
                    }
                    ExcelApp.ActiveWorkbook.SaveCopyAs(sfd.FileName); //or .xlsx file, depending of the excel version of your system
                    ExcelApp.ActiveWorkbook.Saved = true;
                    ExcelApp.Quit();
                }
                else
                    MessageBox.Show("Your document was not exported", "Not exported");
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void dataGridView_reports_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = false;
            DialogResult dr = MessageBox.Show("Are you sure you want to delete?", "Confirmation", MessageBoxButtons.YesNo);
            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                if(gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
                OleDbDataAdapter adpt = new OleDbDataAdapter();
                adpt.UpdateCommand = new OleDbCommand("DELETE * FROM "+gl.tbl_reports+" WHERE "+gl.col_id+"=@idnum", gl.oleconnection);
                adpt.UpdateCommand.Parameters.Add("@idnum", OleDbType.VarChar, 255).Value = dataGridView_reports[0, dataGridView_reports.CurrentCell.RowIndex].Value.ToString();
                adpt.UpdateCommand.CommandType = CommandType.Text;
                adpt.UpdateCommand.ExecuteNonQuery();
                if(gl.oleconnection.State == ConnectionState.Open) gl.oleconnection.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_9weeksstart.Checked == true)
            {
                comboBox_9weeksstart.Enabled = true;
                checkBox_9weeksend.Enabled = true;
            }
            else
            {
                comboBox_9weeksstart.Enabled = false;
                checkBox_9weeksend.Checked = false;
                checkBox_9weeksend.Enabled = false;
                comboBox_9weeksend.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_9weeksend.Checked == true)
            {
                comboBox_9weeksend.Enabled = true;
                comboBox_9weeksend.Enabled = true;
            }
            else
            {
                comboBox_9weeksend.Enabled = false;
                comboBox_9weeksend.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_semester.Checked)
                comboBox_semster.Enabled = true;
            else
                comboBox_semster.Enabled = false;
        }

        private void dataGridView_reports_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(dataGridView_reports[12, dataGridView_reports.CurrentCell.RowIndex].Value.ToString() != "" || dataGridView_reports[12, dataGridView_reports.CurrentCell.RowIndex].Value.ToString() != null)
                {
                    if(gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
                OleDbDataAdapter adpt = new OleDbDataAdapter();
                adpt.UpdateCommand = new OleDbCommand("UPDATE `"+gl.tbl_reports+"` SET ["+gl.col_deanaction+"]=@dean,["+ gl.col_dateofdeanaction +"]=@dateofaction WHERE ["+gl.col_id+"]=@idnum", gl.oleconnection);
                adpt.UpdateCommand.Parameters.Add("dean", OleDbType.VarChar, 255).Value = dataGridView_reports[12, dataGridView_reports.CurrentCell.RowIndex].Value.ToString();
                adpt.UpdateCommand.Parameters.Add("dateofaction", OleDbType.VarChar, 255).Value = DateTime.Now.ToShortDateString();//dataGridView_reports[11, dataGridView_reports.CurrentCell.RowIndex].Value.ToString();
                adpt.UpdateCommand.Parameters.Add("idnum", OleDbType.Guid, 255).Value = Guid.Parse(dataGridView_reports[0, dataGridView_reports.CurrentCell.RowIndex].Value.ToString());
                adpt.UpdateCommand.CommandType = CommandType.Text;
                adpt.UpdateCommand.ExecuteNonQuery();
            }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            finally
            {
                if(gl.oleconnection.State == ConnectionState.Open) gl.oleconnection.Close();
                dataGridView_reports.EndEdit();
                button_retrieve.PerformClick();
            }
        }
        private void weeksDatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var nwd = new Nine_Weeks_Dates();
            nwd.ShowDialog();
            this.Show();
        }

        private void teacherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var te = new Teacher_Editor();
            te.ShowDialog();
            this.Show();
        }

        private void studentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var se = new Student_Editor();
            se.ShowDialog();
            this.Show();
        }

        private void infractionsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var il = new Infractions_List();
            il.ShowDialog();
            this.Show();
        }
        public void getquery()
        {
            wtfbrah = true;
                sql = "SELECT * FROM `"+gl.tbl_reports+"` WHERE";
                Boolean hasStarted = false;

                if (checkBox_date_single.Checked)
                {
                    wtfbrah = false;
                    if (checkBox_date_range.Checked)
                    {
                        if (hasStarted)
                            sql += " AND";
                        sql += " `"+ gl.col_reportdate + "` BETWEEN #" + datetimepicker_date_start.Value.ToShortDateString() + "# AND #" + datetimepicker_date_end.Value.ToShortDateString() + "#";
                        hasStarted = true;
                    }
                    else
                    {
                        if (hasStarted)
                            sql += " AND";
                        sql += " `"+ gl.col_reportdate + "` = #" + datetimepicker_date_start.Value.ToShortDateString() + "#";
                        hasStarted = true;
                    }
                }

                if (checkBox_teacher.Checked)
                {
                    if (combobox_teacher.Text != "")
                    {
                        wtfbrah = false;
                        if (hasStarted)
                            sql += " AND";
                        teacher = combobox_teacher.Text;
                        sql += " `"+ gl.col_teacher + "`=@teacher";
                        hasStarted = true;
                    }
                    else
                    {
                        MessageBox.Show("Please select a teacher.\nBlank Teacher is another way to say any.\nAll reports will be loaded with the specified critera","Missing Teacher");
                    }
                }
                if (checkBox_infraction.Checked)
                {
                    if (comboBox_infraction_select.Text != "")
                    {
                        wtfbrah = false;
                        if (hasStarted)
                            sql += " AND";
                        infraction = comboBox_infraction_select.Text;
                        sql += " `"+ gl.col_infractions +"`=@infraction";
                        hasStarted = true;
                    }
                    else
                    {

                        MessageBox.Show("Please select an Infraction.\nBlank Infraction is another way to say any.\nAll reports will be loaded with the specified critera", "Missing Infraction");
                    }
                }
                if (checkBox_period_single.Checked)
                {
                    wtfbrah = false;
                    if (checkBox_period_range.Checked)
                    {
                        if (hasStarted)
                            sql += " AND";
                        sql += " "+gl.col_period+" BETWEEN " + numericUpDown_period_start.Value.ToString() + " AND " + numericUpDown_period_end.Value.ToString() + "";
                        hasStarted = true;
                    }
                    else
                    {
                        if (hasStarted)
                            sql += " AND";
                        sql += " " + gl.col_period + " = " + numericUpDown_period_start.Value.ToString() + "";
                        hasStarted = true;
                    }
                }
                if (checkBox_grade_single.Checked)
                {
                    wtfbrah = false;
                    if (checkBox_grade_range.Checked)
                    {
                        if (hasStarted)
                            sql += " AND";
                        sql += " " + gl.col_grade + " BETWEEN " + numericUpDown_grade_start.Value.ToString() + " AND " + numericUpDown_grade_end.Value.ToString() + "";
                        hasStarted = true;
                    }
                    else
                    {
                        if (hasStarted)
                            sql += " AND";
                        sql += " " + gl.col_grade + " = " + numericUpDown_grade_start.Value.ToString() + "";
                        hasStarted = true;
                    }
                }
                if (checkBox_student.Checked)
                {
                    getstudentinfo();
                    wtfbrah = false;
                    if (hasStarted)
                        sql += " AND";
                    for (int i = 0; i < comboBox_student_firstname.Text.Length; i++)
                    {
                        if (comboBox_student_firstname.Text[i] == ' ')
                        {
                            firstname = comboBox_student_firstname.Text.Substring(0, i);
                            studentid = comboBox_student_firstname.Text.Substring(i + 1, (comboBox_student_firstname.Text.Length - (i + 1)));
                        }
                    }
                    for (int i = 0; i < comboBox_student_last.Text.Length; i++)
                    {
                        if (comboBox_student_last.Text[i] == ' ')
                        {
                            lastname = comboBox_student_last.Text.Substring(0, i);
                            studentid = comboBox_student_last.Text.Substring(i + 1, (comboBox_student_last.Text.Length - (i + 1)));
                        }
                    }
                    sql += " `"+gl.col_firstname+"`=@firstname AND `"+gl.col_lastname+"`=@lastname AND `"+gl.col_studentid+"`=@studentid";
                    hasStarted = true;
                }
                if (checkBox_9weeksstart.Checked)
                {
                    wtfbrah = false;
                    if (checkBox_9weeksend.Checked)
                    {
                        if (comboBox_9weeksstart.Text != "" && comboBox_9weeksend.Text != "")
                        {
                            if (hasStarted)
                                sql += " AND";
                            DateTime startdate = new DateTime();
                            DateTime enddate = new DateTime();
                            if (comboBox_9weeksstart.Text == "1st 9 weeks")
                            {
                                startdate = firstnineweeksstart;
                            }
                            if (comboBox_9weeksstart.Text == "2nd 9 weeks")
                            {
                                startdate = secondtnineweeksstart;
                            }
                            if (comboBox_9weeksstart.Text == "3rd 9 weeks")
                            {
                                startdate = thirdnineweeksstart;
                            }
                            if (comboBox_9weeksstart.Text == "4th 9 weeks")
                            {
                                enddate = forthnineweeksstart;
                            }
                            if (comboBox_9weeksend.Text == "1st 9 weeks")
                            {
                                enddate = firstnineweeksend;
                            }
                            if (comboBox_9weeksend.Text == "2nd 9 weeks")
                            {
                                enddate = secondnineweeksend;
                            }
                            if (comboBox_9weeksend.Text == "3rd 9 weeks")
                            {
                                enddate = thirdnineweeksend;
                            }
                            if (comboBox_9weeksend.Text == "4th 9 weeks")
                            {
                                enddate = forthnineweeksend;
                            }
                            sql += " `"+ gl.col_reportdate + "` BETWEEN #" + startdate + "# AND #" + enddate + "#";
                            hasStarted = true;
                        }
                        else
                            MessageBox.Show("Please select the 9 weeks for both.");
                    }
                    else
                    {
                        if (comboBox_9weeksstart.Text != "")
                        {
                            if (hasStarted)
                                sql += " AND";
                            if (comboBox_9weeksstart.Text == "1st 9 weeks")
                            {
                                sql += " `"+ gl.col_reportdate + "` BETWEEN #" + firstnineweeksstart + "# AND #" + firstnineweeksend + "#";
                            }
                            if (comboBox_9weeksstart.Text == "2nd 9 weeks")
                            {
                                sql += " `"+ gl.col_reportdate + "` BETWEEN #" + secondtnineweeksstart + "# AND #" + secondnineweeksend + "#";
                            }
                            if (comboBox_9weeksstart.Text == "3rd 9 weeks")
                            {
                                sql += " `"+ gl.col_reportdate + "` BETWEEN #" + thirdnineweeksstart + "# AND #" + thirdnineweeksend + "#";
                            }
                            if (comboBox_9weeksstart.Text == "4th 9 weeks")
                            {
                                sql += " `"+ gl.col_reportdate + "` BETWEEN #" + forthnineweeksstart + "# AND #" + forthnineweeksend + "#";
                            }
                            hasStarted = true;
                        }
                        else
                            MessageBox.Show("Please select the 9 weeks.");
                    }
                }
                if (checkBox_semester.Checked)
                {
                    if (comboBox_semster.Text != "")
                    {
                        wtfbrah = false;
                        if (hasStarted)
                            sql += " AND";
                        if (comboBox_semster.Text == "1st semester")
                            sql += " `"+ gl.col_reportdate + "` BETWEEN #" + firstnineweeksstart + "# AND #" + secondnineweeksend + "#";
                        if (comboBox_semster.Text == "2nd semester")
                            sql += " `"+ gl.col_reportdate + "` BETWEEN #" + thirdnineweeksstart + "# AND #" + forthnineweeksend + "#";
                        hasStarted = true;
                    }
                    else
                        MessageBox.Show("Please select the semester.");
                }
                if (checkbox_deanaction.Checked)
                {
                    wtfbrah = false;
                    if (hasStarted)
                        sql += " AND";
                    sql += " `"+gl.col_deanaction+"`='None'";
                    hasStarted = true;
                }

                if (checkbox_dateofdeanaction_single.Checked)
                {
                    wtfbrah = false;
                    if (checkbox_dateofdeanaction_range.Checked)
                    {
                        if (hasStarted)
                            sql += " AND";
                        sql += " `" + gl.col_dateofdeanaction + "` BETWEEN #" + datetimepicker_dateofdean_start.Value.ToShortDateString() + "# AND #" + datetimepicker_dateofdean_end.Value.ToShortDateString() + "#";
                        hasStarted = true;
                    }
                    else
                    {
                        if (hasStarted)
                            sql += " AND";
                        sql += " `" + gl.col_dateofdeanaction + "` = #" + datetimepicker_dateofdean_start.Value.ToShortDateString() + "#";
                        hasStarted = true;
                    }
                }

                if (checkBox_learningcenter.Checked)
                {

                    if (comboBox_learningcenter.Text != "")
                    {
                        wtfbrah = false;
                        if (hasStarted)
                            sql += " AND";
                        learningcenter = comboBox_learningcenter.Text;
                        sql += " `" + gl.col_learningcenter + "`=@learningcenter";
                        hasStarted = true;
                    }
                    else
                    {

                        MessageBox.Show("Please select an Status.\nBlank Status is another way to say any.\nAll reports will be loaded with the specified critera", "Missing Infraction");
                    }
                }
                if (wtfbrah)
                {
                    sql = "SELECT * FROM `"+gl.tbl_reports+"`";
                }
                dAdapter = new OleDbDataAdapter(sql, gl.oleconnection);
                county = 1;
                totalinfractions = 1;
        }

        private void button_retrieve_Click(object sender, EventArgs e)
        {
            firstname = "";
            lastname = "";
            studentid = "";
            teacher = "";
            infraction = "";
            getquery();
            getinfractions();
        }

        private void checkbox_dateofdeanaction_CheckedChanged(object sender, EventArgs e)
        {

            if (checkbox_dateofdeanaction_single.Checked == true)
            {
                datetimepicker_dateofdean_start.Enabled = true;
                checkbox_dateofdeanaction_range.Enabled = true;
            }
            else
            {
                datetimepicker_dateofdean_start.Enabled = false;
                checkbox_dateofdeanaction_range.Enabled = false;
                checkbox_dateofdeanaction_range.Checked = false;
            }
        }

        private void checkbox_dateofdean_range_CheckedChanged(object sender, EventArgs e)
        {

            if (checkbox_dateofdeanaction_range.Checked == true)
                datetimepicker_dateofdean_end.Enabled = true;
            else
                datetimepicker_dateofdean_end.Enabled = false;
        }

        private void checkBox_learningcenter_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_learningcenter.Checked)
                comboBox_learningcenter.Enabled = true;
                else
                comboBox_learningcenter.Enabled = false;
        }
    }
}