using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class HomeContentform : Form
    {
        public HomeContentform()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label9.Text = DateTime.Now.ToString("hh:mm:ss tt");

            // Determine greeting based on time
            var hour = DateTime.Now.Hour;
            string greeting;

            if (hour >= 5 && hour < 12)
                greeting = "Good Morning";
            else if (hour >= 12 && hour < 17)
                greeting = "Good Afternoon";
            else if (hour >= 17 && hour < 21)
                greeting = "Good Evening";
            else
                greeting = "Good Night";

            // Get user's name from session
            string name =  "Nadir Jamal";

            label10.Text = $"{greeting}, {name}";
        }

        private void HomeContentform_Load(object sender, EventArgs e)
        {
            timer1.Start();

        }
    }
}
