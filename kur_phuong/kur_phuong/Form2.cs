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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            if (textBox1.Text.Length != 16 || textBox2.Text.Length != 16)
            {
                MessageBox.Show("Wrong input!");
                return;
            }
            uint K1 = (uint)Convert.ToInt32(textBox1.Text.Substring(0, 8), 16);  
            uint K2 = (uint)Convert.ToInt32(textBox1.Text.Substring(8, 8), 16);
            uint M1 = (uint)Convert.ToInt32(textBox2.Text.Substring(0, 8), 16);
            uint M2 = (uint)Convert.ToInt32(textBox2.Text.Substring(8, 8), 16);
            uint C1 = 0;
            uint C2 = 0;
           
            StreamWriter sw = new StreamWriter("results.txt");
            DES des = new DES();
            sw.Write("\nKey: ");
            des.ShowByte(sw,K1); des.ShowByte(sw,K2);
            sw.Write("\nInput String: ");
            des.ShowByte(sw, M1); des.ShowByte(sw, M2);
            des.MahoaDES(sw,M1, M2, K1, K2, ref C1, ref C2);
            sw.Write("\nOutput String: ");
            des.ShowByte(sw, C1); des.ShowByte(sw, C2);
            sw.Close();                    
            MessageBox.Show(string.Format("Output String: {0:X}{1:X}", C1, C2));
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Script script = new Script();
            script.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            button2.Enabled = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
            button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 16 || textBox2.Text.Length != 16)
            {
                MessageBox.Show("Dữ liệu đầu vào sai");
                return;
            }
            uint K1 = (uint)Convert.ToInt32(textBox1.Text.Substring(0, 8), 16);
            uint K2 = (uint)Convert.ToInt32(textBox1.Text.Substring(8, 8), 16);
            uint C1 = (uint)Convert.ToInt32(textBox2.Text.Substring(0, 8), 16);
            uint C2 = (uint)Convert.ToInt32(textBox2.Text.Substring(8, 8), 16);
           
            uint MC1 = 0;
            uint MC2 = 0;
            StreamWriter sw = new StreamWriter("results.txt");
            DES des = new DES();
            sw.Write("\nKey: ");
            des.ShowByte(sw, K1); des.ShowByte(sw, K2);
            sw.Write("\nInput String: ");
            des.ShowByte(sw, C1); des.ShowByte(sw, C2);
            des.GiaiMaDES(sw, C1, C2, K1, K2, ref MC1, ref MC2);
            sw.Write("\nOutput String: ");
            des.ShowByte(sw, MC1); des.ShowByte(sw, MC2);
            sw.Close();
            MessageBox.Show(string.Format("Output String: {0:X}{1:X}", MC1, MC2));
            button2.Enabled = true;
        }
    }
}
