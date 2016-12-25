using System;
using System.Windows.Forms;
using Netil;
using System.ComponentModel;
using Netil.Helper;

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
