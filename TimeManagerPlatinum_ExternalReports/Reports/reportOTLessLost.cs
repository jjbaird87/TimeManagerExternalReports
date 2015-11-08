using System;
using DevExpress.XtraReports.UI;

namespace TimeManagerPlatinum_ExternalReports.Reports
{
    public partial class ReportOtLessLost : XtraReport
    {
        public ReportOtLessLost()
        {
            InitializeComponent();
        }

        private static string FormatToHhmm(int input)
        {
            var hours = (input/60).ToString().PadLeft(2, '0');
            var minutes = (input%60).ToString().PadLeft(2, '0');
            return $"{hours}:{minutes}";
        }

        private void xrLabel19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel19.Text = FormatToHhmm(Convert.ToInt32(GetCurrentColumnValue("NormalTime")));
        }

        private void xrLabel20_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel20.Text = FormatToHhmm(Convert.ToInt32(GetCurrentColumnValue("OverTime")));
        }

        private void xrLabel21_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel21.Text = FormatToHhmm(Convert.ToInt32(GetCurrentColumnValue("DoubleTime")));
        }

        private void xrLabel22_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel22.Text = FormatToHhmm(Convert.ToInt32(GetCurrentColumnValue("Pphw")));
        }

        private void xrLabel23_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel23.Text = FormatToHhmm(Convert.ToInt32(GetCurrentColumnValue("NotApplicable")));
        }

        private void xrLabel24_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel24.Text = FormatToHhmm(Convert.ToInt32(GetCurrentColumnValue("LostTime")));
        }

        private void xrLabel25_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel25.Text = FormatToHhmm(Convert.ToInt32(GetCurrentColumnValue("TotalTime")));
        }

        private void xrLabel26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel26.Text = FormatToHhmm(Convert.ToInt32(GetCurrentColumnValue("NormalTime")));
        }

        private void xrLabel26_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private void xrLabel29_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            var overtime = Convert.ToInt32(lblOvertimeSummary.Summary.GetResult());
            var losttime = Convert.ToInt32(lblLostTimeSummary.Summary.GetResult());
            
            e.Text = FormatToHhmm(SubtractLostTimeFromOverTime(overtime, losttime));
        }

        private void xrLabel30_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private void xrLabel31_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private void xrLabel32_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private void xrLabel33_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private void xrLabel34_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private void xrLabel15_SummaryCalculated(object sender, TextFormatEventArgs e)
        {        
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private int SubtractLostTimeFromOverTime(int overTime, int lostTime)
        {
            var balancedOverTime = overTime;

            if (lostTime <= 0) return balancedOverTime;
            if (balancedOverTime <= 0) return balancedOverTime;

            balancedOverTime = overTime - lostTime;
            if (balancedOverTime < 0)
                balancedOverTime = 0;

            return balancedOverTime;
        }

        private int AddLostTimeToNormalTime(int normalTime, int overTime, int lostTime)
        {
            var balancedOverTime = SubtractLostTimeFromOverTime(overTime, lostTime);
            var overTimeUsed = overTime - balancedOverTime;
            var balancedNormalTime = normalTime + overTimeUsed;
            return balancedNormalTime;
        }

        //private int SubtractLostTimeFromOverTimeAndNormalTime(int normalTime, int overTime, int lostTime)
        //{
        //    var balancedOverTime = overTime;
        //    var balancedNormalTime = normalTime;
        //    var balancedLostTime = 0;

        //    if (lostTime <= 0) return balancedNormalTime;
        //    if (balancedNormalTime <= 0) return balancedNormalTime;

        //    balancedOverTime = overTime - lostTime;
        //    if (balancedOverTime < 0)
        //        balancedOverTime = 0;
        //    balancedLostTime = lostTime - (overTime - balancedOverTime);

        //    balancedNormalTime = normalTime - balancedLostTime;
        //    if (balancedNormalTime < 0)
        //        balancedNormalTime = 0;            

        //    return balancedNormalTime;
        //}

        private void lblBalancedNormalTime_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            var normalTime = Convert.ToInt32(lblBalancedNormalTime.Summary.GetResult());
            var overtime = Convert.ToInt32(lblOverTimeSummary1.Summary.GetResult());
            var losttime = Convert.ToInt32(lblLostTimeSummary.Summary.GetResult());

            e.Text = FormatToHhmm(AddLostTimeToNormalTime(normalTime, overtime, losttime));
        }
    }
}
