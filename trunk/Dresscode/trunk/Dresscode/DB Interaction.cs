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

namespace Dresscode
{
    class DB_Interaction
    {
        globals gl = new globals();
        public string selectioncommand(string sql, string firstname, string lastname, string studentid, string frmname, string dgn)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Clear();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, gl.oleconnection);
                dataAdapter.SelectCommand.Parameters.Add("firstname", OleDbType.VarChar, 255).Value = firstname;
                dataAdapter.SelectCommand.Parameters.Add("lastname", OleDbType.VarChar, 255).Value = lastname;
                dataAdapter.SelectCommand.Parameters.Add("studentid", OleDbType.VarChar, 255).Value = studentid;
                dataAdapter.SelectCommand.CommandType = CommandType.Text;
                
                DataGridView dgv = Application.OpenForms[frmname].Controls[dgn] as DataGridView;
                dataAdapter.Fill(ds);
                dgv.DataSource = (ds.Tables[0]);

                if (frmname == "Reports")
                {
                    for (int i = 0; i <= 10; i++)
                    {
                        if (i <= 2)
                            dgv.Columns[i].Visible = false;
                        dgv.Columns[i].ReadOnly = true;
                    }
                    dgv.AutoResizeColumns(
                        DataGridViewAutoSizeColumnsMode.AllCells);
                }
                if (frmname == "form_dresscode")
                {
                    dgv.Columns[0].Visible = false;
                    dgv.Columns[1].Visible = false;
                    dgv.AutoResizeColumns(
                        DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Error");
            }
            finally
            {
                gl.oleconnection.Close();
            }
            return null;
        }
    }
}