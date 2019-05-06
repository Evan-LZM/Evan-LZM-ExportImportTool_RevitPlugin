using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Export_Import_Tool
{
    public partial class WaitingForm : System.Windows.Forms.Form
    {
        Form parentForm;
        public WaitingForm(DateTime plannedTime, Form parentForm)
        {
            InitializeComponent();
            PlannedTimeLabel.Text = plannedTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.parentForm = parentForm;
            CurrentTimeLabel.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        //When the cancel button is pressed the form will dispose.
        private void WaitCancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //When the form is loaded, the timer will start working
        private void WaitingForm_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        //The CurrentTimeLabel will dynamically display current time.
        //When the current time equals the planned time, this form will automatically dispose and continue the export process.
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            CurrentTimeLabel.Text = dt.ToString("yyyy-MM-dd HH:mm:ss");
            if(CurrentTimeLabel.Text == PlannedTimeLabel.Text)
            {
                timer1.Enabled = false;
                this.Dispose();
            }
        }
    }
}
