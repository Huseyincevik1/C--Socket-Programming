using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace denemesocket
{
    public partial class FrmSifrele : Form
    {
        public FrmSifrele()
        {
            InitializeComponent();
        }
        
        private void FrmSifrele_Load(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked){
                textBox2.Enabled = false;
                button1.Enabled = false;
            }
            else {
                textBox2.Enabled = true;
                button1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox3.Text = Sifreleme.SHA256(textBox1.Text);
            }
            else if(radioButton2.Checked)
            {
                if(textBox2.Text == "asdasd")
                {
                    textBox3.Text = Sifreleme.sifrelemd5(textBox1.Text,"asdasd");
                }
                else
                {
                    MessageBox.Show("Anahtar Doğru Değil!!");
                }
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "asdasd")
            {
                textBox3.Text = Sifreleme.cozmd5(textBox1.Text, "asdasd");
            }
            else
            {
                MessageBox.Show("Anahtar Doğru Değil!!");
            }
        }
    }
}
