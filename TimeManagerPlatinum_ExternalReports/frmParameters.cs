using System;
using System.Data;
using System.Linq;
using DevExpress.XtraEditors;
using FirebirdSql.Data.FirebirdClient;

namespace TimeManagerPlatinum_ExternalReports
{
    public partial class FrmParameters : XtraForm
    {
        private Parameters _parameters;

        public FrmParameters()
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
            LoadCompanies();
            LoadDepartments();
            LoadCostCentres();
            LoadOccupations();
            LoadGrades();
            LoadWorkPatterns();
        }

        private void LoadDefaultDateTime()
        {
            dtFrom.DateTime = DateTime.Now;
            dtTo.DateTime = DateTime.Now;
            timeFrom.Time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            timeTo.Time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        }

        private void LoadEmployees()
        {
            var dbPath = Utilities.GetDbPath();
            if (dbPath == "")
            {
                return;
            }

            var connString = Utilities.BuildConnectionString(dbPath);
            var myConnection = new FbConnection(connString);
            myConnection.Open();

            var command = new FbCommand(
                "SELECT a.EMP_NO, a.NAME, a.SURNAME FROM EMP a ORDER BY a.EMP_NO ASC NULLS LAST", myConnection);
            var adapter = new FbDataAdapter(command);
            var dt = new DataTable();

            var i = adapter.Fill(dt);
            if (i > 0)
            {
                foreach (var row in dt.Rows)
                {
                    var employee = $"{((DataRow) row)[0]} ({((DataRow) row)[2]}, {((DataRow) row)[1]})";
                    var employeeItem = new DropDownItem
                    {
                        Id = ((DataRow) row)[0].ToString(),
                        Text = employee
                    };

                    cmbEmployeeFrom.Properties.Items.Add(employeeItem);
                    cmbEmployeeTo.Properties.Items.Add(employeeItem);
                }
                cmbEmployeeFrom.SelectedIndex = 0;
                cmbEmployeeTo.SelectedIndex = cmbEmployeeTo.Properties.Items.Count - 1;
            }
        }

        private void LoadCompanies()
        {
            var dbPath = Utilities.GetDbPath();
            if (dbPath == "")
            {
                return;
            }

            cmbCompany.Properties.Items.Clear();
            cmbCompany.Properties.Items.Add(new DropDownItem());

            var connString = Utilities.BuildConnectionString(dbPath);
            var myConnection = new FbConnection(connString);
            myConnection.Open();

            var command = new FbCommand(
                "SELECT a.CODE, a.DESCRIPTION FROM COMPANIES a ORDER BY a.CODE ASC NULLS LAST", myConnection);
            var adapter = new FbDataAdapter(command);
            var dt = new DataTable();

            var i = adapter.Fill(dt);
            if (i <= 0) return;
            foreach (var companyItem in from object row in dt.Rows select new DropDownItem
            {
                Id = ((DataRow) row)[0].ToString(),
                Text = ((DataRow)row)[1].ToString()
            })
            {
                cmbCompany.Properties.Items.Add(companyItem);
            }
            cmbCompany.SelectedIndex = 0;
        }

        private void LoadDepartments()
        {
            var dbPath = Utilities.GetDbPath();
            if (dbPath == "")
            {
                return;
            }

            cmbDepartment.Properties.Items.Clear();
            cmbDepartment.Properties.Items.Add(new DropDownItem());

            var connString = Utilities.BuildConnectionString(dbPath);
            var myConnection = new FbConnection(connString);
            myConnection.Open();

            var command = new FbCommand(
                "SELECT a.CODE, a.DESCRIPTION FROM DEPARTMENTS a ORDER BY a.CODE ASC NULLS LAST", myConnection);
            var adapter = new FbDataAdapter(command);
            var dt = new DataTable();

            var i = adapter.Fill(dt);
            if (i <= 0) return;
            foreach (var departmentItem in from object row in dt.Rows
                                        select new DropDownItem
                                        {
                                            Id = ((DataRow)row)[0].ToString(),
                                            Text = ((DataRow)row)[1].ToString()
                                        })
            {
                cmbDepartment.Properties.Items.Add(departmentItem);
            }            
                cmbDepartment.SelectedIndex = 0;
        }

        private void LoadCostCentres()
        {
            var dbPath = Utilities.GetDbPath();
            if (dbPath == "")
            {
                return;
            }

            cmbCostCentre.Properties.Items.Clear();
            cmbCostCentre.Properties.Items.Add(new DropDownItem());

            var connString = Utilities.BuildConnectionString(dbPath);
            var myConnection = new FbConnection(connString);
            myConnection.Open();

            var command = new FbCommand(
                "SELECT a.CODE, a.DESCRIPTION FROM CCENTRES a ORDER BY a.CODE ASC NULLS LAST", myConnection);
            var adapter = new FbDataAdapter(command);
            var dt = new DataTable();

            var i = adapter.Fill(dt);
            if (i <= 0) return;
            foreach (var costCentreItem in from object row in dt.Rows
                                           select new DropDownItem
                                           {
                                               Id = ((DataRow)row)[0].ToString(),
                                               Text = ((DataRow)row)[1].ToString()
                                           })
            {
                cmbCostCentre.Properties.Items.Add(costCentreItem);
            }
                cmbCostCentre.SelectedIndex = 0;
        }

        private void LoadOccupations()
        {
            var dbPath = Utilities.GetDbPath();
            if (dbPath == "")
            {
                return;
            }

            cmbOccupation.Properties.Items.Clear();
            cmbOccupation.Properties.Items.Add(new DropDownItem());

            var connString = Utilities.BuildConnectionString(dbPath);
            var myConnection = new FbConnection(connString);
            myConnection.Open();

            var command = new FbCommand(
                "SELECT a.CODE, a.DESCRIPTION FROM OCCUPATIONS a ORDER BY a.CODE ASC NULLS LAST", myConnection);
            var adapter = new FbDataAdapter(command);
            var dt = new DataTable();

            var i = adapter.Fill(dt);
            if (i <= 0) return;
            foreach (var occupationItem in from object row in dt.Rows
                                           select new DropDownItem
                                           {
                                               Id = ((DataRow)row)[0].ToString(),
                                               Text = ((DataRow)row)[1].ToString()
                                           })
            {
                cmbOccupation.Properties.Items.Add(occupationItem);
            }
                cmbOccupation.SelectedIndex = 0;
        }

        private void LoadGrades()
        {
            var dbPath = Utilities.GetDbPath();
            if (dbPath == "")
            {
                return;
            }

            cmbGrade.Properties.Items.Clear();
            cmbGrade.Properties.Items.Add(new DropDownItem());

            var connString = Utilities.BuildConnectionString(dbPath);
            var myConnection = new FbConnection(connString);
            myConnection.Open();

            var command = new FbCommand(
                "SELECT a.CODE, a.DESCRIPTION FROM GRADES a ORDER BY a.CODE ASC NULLS LAST", myConnection);
            var adapter = new FbDataAdapter(command);
            var dt = new DataTable();

            var i = adapter.Fill(dt);
            if (i <= 0) return;
            foreach (var gradeItem in from object row in dt.Rows
                                           select new DropDownItem
                                           {
                                               Id = ((DataRow)row)[0].ToString(),
                                               Text = ((DataRow)row)[1].ToString()
                                           })
            {
                cmbGrade.Properties.Items.Add(gradeItem);
            }
                cmbGrade.SelectedIndex = 0;
        }

        private void LoadWorkPatterns()
        {
            var dbPath = Utilities.GetDbPath();
            if (dbPath == "")
            {
                return;
            }

            cmbWorkPattern.Properties.Items.Clear();
            cmbWorkPattern.Properties.Items.Add(new DropDownItem());

            var connString = Utilities.BuildConnectionString(dbPath);
            var myConnection = new FbConnection(connString);
            myConnection.Open();

            var command = new FbCommand(
                "SELECT a.CODE, a.DESCRIPTION FROM WORKPATTERNS a ORDER BY a.CODE ASC NULLS LAST", myConnection);
            var adapter = new FbDataAdapter(command);
            var dt = new DataTable();

            var i = adapter.Fill(dt);
            if (i <= 0) return;
            foreach (var workPatternItem in from object row in dt.Rows
                                      select new DropDownItem
                                      {
                                          Id = ((DataRow)row)[0].ToString(),
                                          Text = ((DataRow)row)[0] + " - " + ((DataRow)row)[1]
                                      })
            {
                cmbWorkPattern.Properties.Items.Add(workPatternItem);
            }
            cmbWorkPattern.SelectedIndex = 0;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            _parameters = new Parameters
            {
                FromDateTime =
                    new DateTime(dtFrom.DateTime.Year, dtFrom.DateTime.Month, dtFrom.DateTime.Day, timeFrom.Time.Hour,
                        timeFrom.Time.Minute, timeFrom.Time.Second),
                ToDateTime =
                    new DateTime(dtTo.DateTime.Year, dtTo.DateTime.Month, dtTo.DateTime.Day, timeTo.Time.Hour,
                        timeTo.Time.Minute, timeTo.Time.Second),
                EmployeeFrom = ((DropDownItem) cmbEmployeeFrom.SelectedItem).Id,
                EmployeeTo = ((DropDownItem) cmbEmployeeTo.SelectedItem).Id,
                CompanyId = ((DropDownItem) cmbCompany.SelectedItem).Id,
                DepartmentId = ((DropDownItem)cmbDepartment.SelectedItem).Id,
                CostCentreId = ((DropDownItem)cmbCostCentre.SelectedItem).Id,                
                GradeId = ((DropDownItem)cmbGrade.SelectedItem).Id,
                OccupationId = ((DropDownItem)cmbOccupation.SelectedItem).Id,
                WorkPatternId = ((DropDownItem)cmbWorkPattern.SelectedItem).Id,
                ZeroValues = chkZero.Checked,
                AccessVariance = (int) txtVariance.Value
            };

            Hide();
        }

        public Parameters GetParameters()
        {
            return _parameters;
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void frmParameters_Load(object sender, EventArgs e)
        {
        }

        private class DropDownItem
        {
            public string Id;
            public string Text = "";

            public override string ToString()
            {
                return Text;
            }
        }
    }
}