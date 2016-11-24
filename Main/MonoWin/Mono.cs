using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Netil;

namespace Main.MonoWin
{
    public partial class Mono : Form
    {
        private core Core=new core();
        public Mono()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Core.ToString());
        }
    }
}
