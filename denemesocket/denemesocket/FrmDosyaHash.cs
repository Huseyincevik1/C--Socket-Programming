using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace denemesocket
{
    public partial class FrmDosyaHash : Form
    {
        public FrmDosyaHash()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                DosyaHash yuklenenDosya = new DosyaHash(); //var
                fileDialog.Filter = "metin belgeleri (*.txt)|*.txt|all files (*.*)|*.*";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    button1.Text = "Ekli Dosya (" + fileDialog.SafeFileName + ")";
                    yuklenenDosya.fileName = fileDialog.SafeFileName;
                    yuklenenDosya.filePath = fileDialog.FileName;
                    Thread th_dosyaHash = new Thread(() =>dosyaHashle(File.ReadAllBytes(yuklenenDosya.filePath)));
                    th_dosyaHash.Start();
                }
            }
        }

        void dosyaHashle(byte[] fileBytes)
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate () {
                    textBox1.Text = DosyaHash.FileToHash(fileBytes);
                });
                return;
            }
        }
    }
}
