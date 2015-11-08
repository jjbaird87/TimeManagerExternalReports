using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FirebirdSql.Data.FirebirdClient;
using DataTable = System.Data.DataTable;

namespace TimeManagerPlatinum_ExternalReports.ReportClasses
{
    public static class ReportCalcs
    {
        public static DataTable GetVarianceData(Parameters reportParams)
        {
            var dbPath = Utilities.GetDbPath();
            if (dbPath == "")
                return null;

            var connString = Utilities.BuildConnectionString(dbPath);
            var myConnection = new FbConnection(connString);
            myConnection.Open();

            var command =
                new FbCommand(
                    "SELECT a.SHIFT, a.CODE, b.NAME, b.SURNAME, a.CLKTIME, a.IO, a.CLOCKADDRESS, a.CALCDATE " +
                    "FROM CLOCKING a " + "INNER JOIN EMP b " + "ON a.CODE = b.EMP_NO " +
                    $"WHERE a.CLKTIME >= '{reportParams.FromDateTime.ToString("dd.MM.yyyy, HH:mm:ss.000")}' " +
                    $"AND a.CLKTIME <= '{reportParams.ToDateTime.ToString("dd.MM.yyyy, HH:mm:ss.000")}' " +
                    $"AND a.CODE >= '{reportParams.EmployeeFrom}' AND a.CODE <= '{reportParams.EmployeeTo}' " +
                    "ORDER BY a.CODE, a.CLKTIME", myConnection);
            var adapter = new FbDataAdapter(command);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public static DataTable GetClockdData(Parameters reportParams)
        {
            var dbPath = Utilities.GetDbPath();
            if (dbPath == "")
                return null;

            var connString = Utilities.BuildConnectionString(dbPath);
            var myConnection = new FbConnection(connString);
            myConnection.Open();

            //Build WHERE statement
            var where =    $"WHERE a.DATETIME >= '{reportParams.FromDateTime.ToString("dd.MM.yyyy, HH: mm: ss.000")}'" +
                           $"AND a.DATETIME <= '{reportParams.ToDateTime.ToString("dd.MM.yyyy, HH:mm:ss.000")}' " +
                           $"AND a.EMPNO >= '{reportParams.EmployeeFrom}' AND a.EMPNO <= '{reportParams.EmployeeTo}' ";

            if (reportParams.CompanyId != null)
                where += $"AND b.COMPANY = {reportParams.CompanyId} ";
            if (reportParams.DepartmentId != null)
                where += $"AND b.DEPARTMENT = {reportParams.DepartmentId} ";
            if (reportParams.CostCentreId != null)
                where += $"AND b.COST_CENTRE = {reportParams.CostCentreId} ";
            if (reportParams.GradeId != null)
                where += $"AND b.GRADE = {reportParams.GradeId} ";
            if (reportParams.OccupationId != null)
                where += $"AND b.OCCUPATION = {reportParams.OccupationId} ";
            if (reportParams.WorkPatternId != null)
                where += $"AND b.WORKPATTERN = {reportParams.WorkPatternId} ";

            var command =
                new FbCommand(
                    "SELECT a.EMPNO, a.DATETIME, a.WORKPAT, a.STATUS, a.ABSENTCODE, " +
                    "a.SHIFTS, a.CALC0, a.CALC1, a.CALC2, a.CALC3, a.CALC4, a.CALC, " +
                    "a.TOTALHOURS, a.TARGET0, a.TARGET1, a.TARGET2, a.TARGET3, " +
                    "a.TARGET4, a.RATE, a.DOW, a.FREE, a.LOST, a.CALC5, a.SHIFT, b.NAME, b.SURNAME " +
                    "FROM CLOCKD a " +
                    "INNER JOIN EMP b ON a.EMPNO = b.EMP_NO " +
                    where +                    
                    "ORDER BY a.EMPNO, a.DATETIME", myConnection);
            var adapter = new FbDataAdapter(command);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public static DataRow GetFirstAndLastClocking(string empNo, DateTime dateTime)
        {
            var dbPath = Utilities.GetDbPath();
            if (dbPath == "")
                return null;

            var connString = Utilities.BuildConnectionString(dbPath);
            using (var myConnection = new FbConnection(connString))
            {
                myConnection.Open();

                var command =
                    new FbCommand(
                        "SELECT MAX(a.CLKTIME) AS MINTIME, MIN(a.CLKTIME) AS MAXTIME " +
                        "FROM CLOCKING a " +
                        $"WHERE a.CALCDATE = '{dateTime.ToString("dd.MM.yyyy, HH: mm: ss.000")}' " +
                        $"AND a.CODE = '{empNo}' ", myConnection);
                var adapter = new FbDataAdapter(command);
                var dt = new DataTable();
                adapter.Fill(dt);

                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }            
        }

        public static List<OverTimeLessLostTime> GetOverTimeLessLostTime(Parameters reportParams)
        {            
            var dt = GetClockdData(reportParams);
            var report = new List<OverTimeLessLostTime>();

            foreach (var calcRow in from DataRow r in dt.Rows select new OverTimeLessLostTime
            {
                EmpNo = r["EMPNO"].ToString(),
                EmpName = r["NAME"] + " " + r["SURNAME"],
                WorkPattern = r["WORKPAT"].ToString(),
                Date = Convert.ToDateTime(r["DATETIME"]),
                NormalTime =  Convert.ToInt32(r["CALC0"]),
                OverTime = Convert.ToInt32(r["CALC1"]),
                DoubleTime = Convert.ToInt32(r["CALC2"]),                    
                Pphw = Convert.ToInt32(r["CALC3"]),
                NotApplicable = Convert.ToInt32(r["CALC4"]),
                LostTime = Convert.ToInt32(r["LOST"]),
                //TotalTime = Convert.ToInt32(r["TOTALHOURS"])
            })
            {
                var dr = GetFirstAndLastClocking(calcRow.EmpNo, calcRow.Date);
                if (dr != null)
                {
                    calcRow.InTime = dr[1] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr[1]);
                    calcRow.OutTime = dr[0] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr[0]);                    
                }
                report.Add(calcRow);
            }

            return report;
        }
    }
}
