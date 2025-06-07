using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace BillsManagmentSystem.Reports
{
    public partial class PrintSalesBillReport : DevExpress.XtraReports.UI.XtraReport
    {
        public PrintSalesBillReport()
        {
            InitializeComponent();
        }

        private void PageHeader_BeforePrint(object sender, CancelEventArgs e)
        {

        }
    }
}
