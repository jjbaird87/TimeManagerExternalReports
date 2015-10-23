using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TimeManagerPlatinum_ExternalReports.ReportClasses
{
    public class OverTimeLessLostTime
    {
        [DisplayName(@"Number")]
        public string EmpNo { get; set; }

        [DisplayName(@"Name")]
        public string EmpName { get; set; }

        public string WorkPattern { get; set; }

        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        [DisplayName(@"In")]
        public DateTime InTime { get; set; }

        [DisplayName(@"Out")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime OutTime { get; set; }

        [DisplayName(@"NT")]
        public int NormalTime { get; set; }

        [DisplayName(@"1.5")]
        public int OverTime { get; set; }

        [DisplayName(@"Balanced 1.5")]
        public int BalancedOverTime { get; set; }

        [DisplayName(@"1.5 Difference")]
        public int OverTimeDifference => OverTime - BalancedOverTime;

        [DisplayName(@"2.0")]
        public int DoubleTime { get; set; }

        [DisplayName(@"PPHW")]
        public int Pphw { get; set; }

        [DisplayName(@"N/A")]
        public int NotApplicable { get; set; }

        [DisplayName(@"Lost")]
        public int LostTime { get; set; }

        [DisplayName(@"Total")]
        public int TotalTime => NormalTime + BalancedOverTime + DoubleTime + NotApplicable + Pphw;

        public void SubtractLostTimeFromOverTime()
        {
            BalancedOverTime = OverTime;

            if (LostTime <= 0) return;
            if (BalancedOverTime <= 0) return;

            BalancedOverTime = OverTime - LostTime;
            if (BalancedOverTime < 0)
                BalancedOverTime = 0;
        }
    }
}
