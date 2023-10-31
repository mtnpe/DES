using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kur_phuong
{
    public partial class Script : Form
    {
        public Script()
        {
            InitializeComponent();
        }

        private void Script_Load(object sender, EventArgs e)
        {
            string content = File.ReadAllText("results.txt");
            richTextBox1.Text = content;
        }
    }
}
