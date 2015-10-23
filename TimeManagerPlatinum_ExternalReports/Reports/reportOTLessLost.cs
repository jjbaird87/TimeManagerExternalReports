using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using DevExpress.XtraReports.UI;
using TimeManagerPlatinum_ExternalReports.ReportClasses;

namespace TimeManagerPlatinum_ExternalReports.Reports
{
    public partial class reportOTLessLost : DevExpress.XtraReports.UI.XtraReport
    {
        public reportOTLessLost()
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
            xrLabel20.Text = FormatToHhmm(Convert.ToInt32(GetCurrentColumnValue("BalancedOverTime")));
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
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
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

        private void xrLabel45_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private void xrLabel46_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private void xrLabel47_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private void xrLabel48_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private void xrLabel49_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private void xrLabel50_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private void xrLabel3_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }

        private void xrLabel15_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = FormatToHhmm(Convert.ToInt32(e.Value));
        }
    }
}
