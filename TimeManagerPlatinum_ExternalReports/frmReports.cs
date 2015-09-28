using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using FirebirdSql.Data.FirebirdClient;
using TimeManagerPlatinum_ExternalReports.Properties;

namespace TimeManagerPlatinum_ExternalReports
{
    public partial class FrmReports : XtraForm
    {
        public FrmReports()
        {
            InitializeComponent();
        }

        private void dbConnection_ItemClick(object sender, ItemClickEventArgs e)
        {
            var openDlg = new OpenFileDialog
            {
                FileName = "TIMEMANAGERPLATINUM.GDB",
                Filter = @"Firebird Database (*.gdb)|*.gdb|Firebird Database (*.fdb)|*.fdb|All files (*.*)|*.*",
                AddExtension = true
            };
            if (openDlg.ShowDialog() != DialogResult.OK)
                return;

            if (!ConnectToDb(openDlg.FileName))
            {
                XtraMessageBox.Show("Selected database is invalid");
                return;
            }
               
            Settings.Default.DB_Path = openDlg.FileName;
            Settings.Default.Save();

            XtraMessageBox.Show("Database path saved successfully");
        }

        private bool ConnectToDb(string dbPath)
        {
            string conn = Utilities.BuildConnectionString(dbPath);

            FbConnection myConnection = new FbConnection(conn);
            try
            {
                myConnection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void wastedTimeItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            var parameters = new frmParameters();
            parameters.ShowDialog();
                           
            var reportParams = parameters.GetParameters();
            if (reportParams == null)
                return; 

            //GetDataForWastedTimeReport(reportParams);
            GetAccessVariance(reportParams);
        }

        private void GetAccessVariance(Parameters reportParams)
        {
            var dt = GetData(reportParams);
            if (dt.Rows.Count == 0)
                return;

            var info = new CurrentInfo {CurrentDate = new DateTime(1980, 1, 1), CurrentEmployee = "0000"};

            var wastedLine = new WastedTime();
            var lstWastedTime = new List<WastedTime>();
            foreach (var row in dt.Rows)
            {
                var r = (DataRow)row;

                info.ReadDate = (DateTime) r["CLKTIME"];
                info.CurrentShift = r["SHIFT"].ToString();

                //CHECK IF DAY HAS CHANGED
                if (info.CurrentDate.Day != ((DateTime) r["CALCDATE"]).Day)
                {
                    if (info.CurrentDate.Year != 1980 && wastedLine.Processed)
                    {
                        lstWastedTime.Add(wastedLine);
                        wastedLine = new WastedTime();
                    }
                    info.CurrentDate = (DateTime) r["CALCDATE"];
                }

                //CHECK IF EMPLOYEE HAS CHANGED
                if (info.CurrentEmployee != r["CODE"].ToString() )
                {
                    if (info.CurrentEmployee != "0000" && wastedLine.Processed)
                    {
                        lstWastedTime.Add(wastedLine);
                        wastedLine = new WastedTime();
                    }

                    info.CurrentEmployee = r["CODE"].ToString();
                    info.CurrentName = r["NAME"] + "  " + r["SURNAME"];
                }

                if (r["IO"].ToString().TrimEnd() == "AI")
                {
                    ProcessAccessIn(ref wastedLine, ref lstWastedTime, info);
                }
                if (r["IO"].ToString().TrimEnd() == "I")
                {
                    ProcessClockIn(ref wastedLine, ref lstWastedTime, info);
                }
                if (r["IO"].ToString().TrimEnd() == "O")
                {
                    ProcessClockOut(ref wastedLine, ref lstWastedTime, info);
                }
                if (r["IO"].ToString().TrimEnd() == "AO")
                {
                    ProcessAccessOut(ref wastedLine, ref lstWastedTime, info);
                }                
            }

            PostProcessing(ref lstWastedTime, reportParams);
            grdReport.DataSource = lstWastedTime;
        }

        private DataTable GetData(Parameters reportParams)
        {
            var dbPath = Utilities.GetDbPath();
            if (dbPath == "")
                Close();

            var connString = Utilities.BuildConnectionString(dbPath);
            var myConnection = new FbConnection(connString);
            myConnection.Open();

            var command =
                new FbCommand(
                    String.Format(
                        "SELECT a.SHIFT, a.CODE, b.NAME, b.SURNAME, a.CLKTIME, a.IO, a.CLOCKADDRESS, a.CALCDATE " +
                        "FROM CLOCKING a " +
                        "INNER JOIN EMP b " +
                        "ON a.CODE = b.EMP_NO " +
                        "WHERE a.CLKTIME >= '{0}' AND a.CLKTIME <= '{1}' " +
                        "AND a.CODE >= '{2}' AND a.CODE <= '{3}' " +
                        "ORDER BY a.CODE, a.CLKTIME", reportParams.FromDateTime.ToString("dd.MM.yyyy, HH:mm:ss.000"),
                        reportParams.ToDateTime.ToString("dd.MM.yyyy, HH:mm:ss.000"), reportParams.EmployeeFrom,
                        reportParams.EmployeeTo), myConnection);
            var adapter = new FbDataAdapter(command);
            var dt = new DataTable();
            var i = adapter.Fill(dt);
            if (i <= 0) return dt;
            return dt;
        }

        private void ProcessAccessIn(ref WastedTime line, ref List<WastedTime> lstWastedTime, CurrentInfo info)
        {
            if (line.In != null || line.Out != null)
            {
                lstWastedTime.Add(line);
                line = new WastedTime();
            }

            line.In = "*" + (info.ReadDate).ToString("HH:mm:ss");
            line.InDt = info.ReadDate;
            line.AccessClocking = true;
            line.DateDisplay = info.CurrentDate;            
            line.EmployeeNumber = info.CurrentEmployee;
            line.Name = info.CurrentName;
            line.Shift = info.CurrentShift;
            line.Processed = true;
        }

        private void ProcessClockIn(ref WastedTime line, ref List<WastedTime> lstWastedTime, CurrentInfo info)
        {
            if (line.In != null || line.Out != null)
            {
                lstWastedTime.Add(line);
                line = new WastedTime();
            }

            line.In = (info.ReadDate).ToString("HH:mm:ss");
            line.InDt = info.ReadDate;
            line.AccessClocking = false;
            line.DateDisplay = info.CurrentDate;
            line.EmployeeNumber = info.CurrentEmployee;
            line.Name = info.CurrentName;
            line.Shift = info.CurrentShift;
            line.Processed = true;
        }

        private void ProcessClockOut(ref WastedTime line, ref List<WastedTime> lstWastedTime, CurrentInfo info)
        {
            if (line.In != null && line.Out == null && !line.AccessClocking)
            {
                line.Out = (info.ReadDate).ToString("HH:mm:ss");
                line.OutDt = info.ReadDate;
                lstWastedTime.Add(line);
                line = new WastedTime();
            }
            else
            {
                if (line.Processed)
                {
                    lstWastedTime.Add(line);
                }
                line = new WastedTime
                {
                    DateDisplay = info.CurrentDate,
                    EmployeeNumber = info.CurrentEmployee,
                    Name = info.CurrentName,
                    Out = (info.ReadDate).ToString("HH:mm:ss"),
                    OutDt = info.ReadDate,
                    Shift = info.CurrentShift,
                    AccessClocking = false
                };

                lstWastedTime.Add(line);
                line = new WastedTime();
            }
        }

        private void ProcessAccessOut(ref WastedTime line, ref List<WastedTime> lstWastedTime, CurrentInfo info)
        {
            if (line.In != null && line.Out == null && line.AccessClocking && line.Processed)
            {
                line.Out = "*" + (info.ReadDate).ToString("HH:mm:ss");
                line.OutDt = info.ReadDate;
                lstWastedTime.Add(line);
                line = new WastedTime();
            }
            else
            {
                if (line.Processed)
                {
                    lstWastedTime.Add(line);
                }
                line = new WastedTime
                {
                    DateDisplay = info.CurrentDate,
                    EmployeeNumber = info.CurrentEmployee,
                    Name = info.CurrentName,
                    Out = "*" + (info.ReadDate).ToString("HH:mm:ss"),
                    OutDt = info.ReadDate,
                    Shift = info.CurrentShift,
                    AccessClocking = true
                };

                lstWastedTime.Add(line);
                line = new WastedTime();
            }
        }

        private void PostProcessing(ref List<WastedTime> lstWastedTime, Parameters reportParams)
        {
            var info = new CurrentInfo
            {
                CurrentEmployee = "",
                CurrentName = "",
                CurrentDate = new DateTime(1980, 1, 1)
            };

            DateTime? aiDateTime = null;
            DateTime? aoDateTime = null;
            DateTime? iDateTime = null;
            DateTime? oDateTime = null;

            var counter = 0;
            var dayCheck = false;

            var display = lstWastedTime;

            foreach (var row in lstWastedTime.ToArray())
            {
                //CHECK IF DAY HAS CHANGED
                if (info.CurrentDate.Day != row.DateDisplay.Day)
                {
                    //Remove rows if no calculation made for the day
                    if (!reportParams.ZeroValues)
                    {
                        if (!dayCheck)
                        {
                            var x =display.RemoveAll(
                                i => i.EmployeeNumber == info.CurrentEmployee && i.DateDisplay == info.CurrentDate);
                            counter -= x;
                        }
                    }

                    info.CurrentDate = row.DateDisplay;
                                        
                    aiDateTime = null;
                    iDateTime = null;
                    aoDateTime = null;
                    oDateTime = null;
                    dayCheck = false;
                }

                //CHECK IF EMPLOYEE HAS CHANGED
                if (info.CurrentEmployee != row.EmployeeNumber)
                {
                    info.CurrentEmployee = row.EmployeeNumber;
                    
                    aiDateTime = null;
                    iDateTime = null;
                    aoDateTime = null;
                    oDateTime = null;
                    dayCheck = false;
                }

                //GET FIRST Access In for Employee for Day
                if (row.InDt != null && row.AccessClocking)
                {
                    if (aiDateTime == null)
                    {
                        aiDateTime = row.InDt;
                        aoDateTime = null;
                        oDateTime = null;
                    }
                }

                //GET FIRST Clock In for Employee for Day
                if (row.InDt != null && !row.AccessClocking)
                {
                    if (aiDateTime != null)
                    {
                        if (iDateTime == null)
                        {
                            iDateTime = row.InDt;
                            aoDateTime = null;
                            oDateTime = null;
                        }
                            
                    }
                }

                


                //Calculate In
                if (aiDateTime != null && iDateTime != null)
                {
                    var wastedTimeIn = ((DateTime)iDateTime - (DateTime)aiDateTime);
                    if (reportParams.AccessVariance != 0)
                    {
                        if (wastedTimeIn.Minutes >= reportParams.AccessVariance)
                        {
                            display[counter].AccessVarianceIn = String.Format("{0}:{1}:{2}",
                                wastedTimeIn.Hours.ToString().PadLeft(2, '0'),
                                wastedTimeIn.Minutes.ToString().PadLeft(2, '0'),
                                wastedTimeIn.Seconds.ToString().PadLeft(2, '0'));

                            dayCheck = true;
                        }
                        
                     
                    }
                    else
                    {
                        display[counter].AccessVarianceIn = String.Format("{0}:{1}:{2}", wastedTimeIn.Hours.ToString().PadLeft(2, '0'),
                        wastedTimeIn.Minutes.ToString().PadLeft(2, '0'), wastedTimeIn.Seconds.ToString().PadLeft(2, '0'));

                        dayCheck = true;
                    }

                    aiDateTime = null;
                    iDateTime = null;
                    aoDateTime = null;
                    oDateTime = null;
                }

                //Calculate Out
                if (aoDateTime != null && oDateTime != null &&
                    (row.InDt != null))
                {
                    var wastedTimeOut = ((DateTime)aoDateTime - (DateTime)oDateTime);
                    if (wastedTimeOut.TotalSeconds > 0)
                    {
                        display[counter - 1].AccessVarianceOut = String.Format("{0}:{1}:{2}",
                            wastedTimeOut.Hours.ToString().PadLeft(2, '0'),
                            wastedTimeOut.Minutes.ToString().PadLeft(2, '0'),
                            wastedTimeOut.Seconds.ToString().PadLeft(2, '0'));

                        aoDateTime = null;
                        oDateTime = null;

                        dayCheck = true;
                    }
                }

                //GET LAST Access Out for Employee
                if (row.OutDt != null && row.AccessClocking)
                {
                    aoDateTime = row.OutDt;
                }

                //GET LAST Clock Out for Employee
                if (row.OutDt != null && !row.AccessClocking)
                {
                    oDateTime = row.OutDt;
                }

                counter++;
            }
        }

        private class CurrentInfo
        {
            public string CurrentEmployee { get; set; }
            public string CurrentName { get; set; }
            public string CurrentShift { get; set; }
            public DateTime CurrentDate { get; set; }

            public DateTime ReadDate { get; set; }
        }

        private class WastedTime
        {
            [DisplayName(@"Employee Number")]
            public string EmployeeNumber { get; set; }
            
            [DisplayName(@"Employee Name")]
            public string Name { get; set; }

            [DisplayName(@"Date")]
            public DateTime DateDisplay { get; set; }

            [DisplayName(@"Shift")]
            public string Shift { get; set; }

            [DisplayName(@"In")]
            public string In { get; set; }

            [DisplayName(@"Out")]
            public string Out { get; set; }

            [DisplayName(@"Access Variance In")]
            public string AccessVarianceIn { get; set; }

            [DisplayName(@"Access Variance Out")]
            public string AccessVarianceOut { get; set; }

            [Browsable(false)]
            public bool AccessClocking { get; set; }

            [Browsable(false)]
            public bool Processed { get; set; }

            [Browsable(false)]
            public DateTime? InDt { get; set; }

            [Browsable(false)]
            public DateTime? OutDt { get; set; }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            var saveDlg = new SaveFileDialog {DefaultExt = "xls", AddExtension = true, FileName = "TMP_Variance_Report"};
            if (saveDlg.ShowDialog() != DialogResult.OK) return;
            grdvReport.ExportToXlsx(saveDlg.FileName);
            XtraMessageBox.Show("File saved successfully");
        }

    }
}