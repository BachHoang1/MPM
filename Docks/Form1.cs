using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Docks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Controls.Add(new DockingControl(this, DockStyle.Left, new MonthCalendar()));
            Controls.Add(new DockingControl(this, DockStyle.Top, new RichTextBox()));
        }
    }
}
