using PRP;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRP
{
    public partial class report_viwer : Form
    {
        db_ppl DBClass = new db_ppl();
        public report_viwer()
        {
            InitializeComponent();
        }
        private ReportDocument _RptDoc;
        public ReportDocument RptDoc
        {
            get { return _RptDoc; }
            set { _RptDoc = value; }
        }


        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
        private string _RptFormula;
        public string RptFormula
        {
            get { return _RptFormula; }
            set { _RptFormula = value; }
        }
        private void RptLoad()
        {
            try
            {
                if (RptDoc != null)
                {
                    //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["SQL_CONN"].ToString());

                    ConnectionInfo ci = new ConnectionInfo();
                    ci.ServerName = "172.16.2.247";
                    ci.DatabaseName = "CMS";
                    ci.UserID = "sa";
                    ci.Password = "sa*1209";
                    foreach (CrystalDecisions.CrystalReports.Engine.Table tbl in RptDoc.Database.Tables)
                    {
                        TableLogOnInfo logon = tbl.LogOnInfo;
                        logon.ConnectionInfo = ci;
                        tbl.ApplyLogOnInfo(logon);
                        tbl.Location = tbl.Location;
                    }
                    //crystalReportViewer1.
                    crystalReportViewer1.EnableDrillDown = false;
                    crystalReportViewer1.ReportSource = RptDoc;


                    if (RptFormula != string.Empty && RptFormula != null)
                    {
                        crystalReportViewer1.SelectionFormula = RptFormula;
                    }
                    try
                    {
                        crystalReportViewer1.RefreshReport();
                        //crystalReportViewer1.PrintReport();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }

            }
            catch (ExportException ex)
            {
                throw ex;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                this.Location = Screen.PrimaryScreen.WorkingArea.Location;
                this.Size = Screen.PrimaryScreen.WorkingArea.Size;
                RptLoad();
                Cursor.Current = Cursors.Default;
            }
            catch (ExportException ex)
            {
                MessageBox.Show(ex.ToString(), "Alart", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
