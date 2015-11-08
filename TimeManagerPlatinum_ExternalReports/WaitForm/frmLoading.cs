// Developer Express Code Central Example:
// How to: display the loading panel for WinForm controls
// 
// This example demonstrates how to emulate the loading panel used in ASP.NET
// applications to indicate the loading process when a control sends a callback.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E2543


namespace TimeManagerPlatinum_ExternalReports.WaitForm {
    public partial class FrmLoading : DevExpress.XtraWaitForm.WaitForm {
        public FrmLoading() {
            InitializeComponent();
            progressPanel1.AutoSize = true;
        }

        public void ChangeDescription(string description)
        {
            progressPanel1.Description = description;
        }

        #region Overrides

        public override void SetCaption(string caption) {
            base.SetCaption(caption);
            progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description) {
            base.SetDescription(description);
            progressPanel1.Description = description;
        }

        #endregion

        public enum WaitFormCommand {
        }
    }
}