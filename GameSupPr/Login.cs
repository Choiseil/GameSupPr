using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameSupPr
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btn_signOn_Click(object sender, EventArgs e)
        {
            SignOn sign = new SignOn();
            sign.ShowDialog();
        }
    }
}
