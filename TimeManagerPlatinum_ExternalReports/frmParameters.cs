using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FirebirdSql.Data.FirebirdClient;

namespace TimeManagerPlatinum_ExternalReports
{
    public partial class frmParameters : DevExpress.XtraEditors.XtraForm
    {
        private Parameters parameters = null;

        public frmParameters()
        {
            InitializeComponent();

            LoadParameterData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadParameterData()
        {
            LoadDefaultDateTime();
            LoadEmployees();
        }

        private void LoadDefaultDateTime()
        {
            dtFrom.DateTime = DateTime.Now;
            dtTo.DateTime = DateTime.Now;
            timeFrom.Time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            timeTo.Time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        }

        private class DropDownItem
        {
            public string text = "";
            public string employeeNumber;

            public override string ToString()
            {
                return text;
            }
        }

        private void LoadEmployees()
        {
            var dbPath = Utilities.GetDbPath();
            if (dbPath == "")
                Close();

            var connString = Utilities.BuildConnectionString(dbPath);
            FbConnection myConnection = new FbConnection(connString);
            myConnection.Open();

            FbCommand command = new FbCommand("SELECT a.EMP_NO, a.NAME, a.SURNAME FROM EMP a ORDER BY a.EMP_NO ASC NULLS LAST", myConnection);
            FbDataAdapter adapter = new FbDataAdapter(command);
            DataTable dt = new DataTable();

            var i = adapter.Fill(dt);
            if (i > 0)
            {
                foreach (var row in dt.Rows)
                {
                    var employee = String.Format("{0} ({1}, {2})", ((DataRow) row)[0], ((DataRow) row)[2],
                        ((DataRow) row)[1]);
                    var employeeItem = new DropDownItem {employeeNumber = ((DataRow) row)[0].ToString(), text = employee};

                    cmbEmployeeFrom.Properties.Items.Add(employeeItem);
                    cmbEmployeeTo.Properties.Items.Add(employeeItem);
                }
                cmbEmployeeFrom.SelectedIndex = 0;
                cmbEmployeeTo.SelectedIndex = cmbEmployeeTo.Properties.Items.Count - 1;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            parameters = new Parameters
            {
                FromDateTime =
                    new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, dtFrom.DateTime.Day, timeFrom.Time.Hour,
                        timeFrom.Time.Minute, timeFrom.Time.Second),
                ToDateTime =
                    new DateTime(dtTo.DateTime.Year, dtTo.DateTime.Month, dtTo.DateTime.Day, timeTo.Time.Hour,
                        timeTo.Time.Minute, timeTo.Time.Second),
                EmployeeFrom = ((DropDownItem) cmbEmployeeFrom.SelectedItem).employeeNumber,
                EmployeeTo = ((DropDownItem) cmbEmployeeTo.SelectedItem).employeeNumber,
                ZeroValues = chkZero.Checked,
                AccessVariance = (int)txtVariance.Value
            };
            
            Hide();
        }

        public Parameters GetParameters()
        {
            return parameters;
        }

        
    }
}