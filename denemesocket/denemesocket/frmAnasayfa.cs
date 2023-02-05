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
    public partial class frmAnasayfa : Form
    {
        public frmAnasayfa()
        {
            InitializeComponent();
        }

        private void formgetir(Form frm)
        {
            panel2.Controls.Clear();
            frm.MdiParent = this;
            frm.FormBorderStyle = FormBorderStyle.None;
            panel2.Controls.Add(frm);
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmSifrele sifre = new FrmSifrele();
            formgetir(sifre);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmDosyaHash hash = new FrmDosyaHash();
            formgetir(hash);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Frmsocketserver server = new Frmsocketserver();
            formgetir(server);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Frmsocketclient client = new Frmsocketclient();
            formgetir(client);
        }

      
    }
}
