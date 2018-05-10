using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace StokTakipOtomasyonu.Modul_Cek
{
    public partial class frmCekListesi : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();

        int secilenId = -1;
        public bool secim = false;

        public frmCekListesi()
        {
            InitializeComponent();
        }

        void Listele()
        {
            var lst = from s in DB.Ceks
                      
                      select s;
            Liste.DataSource = lst;
        }

        void Sec()
        {
            try
            {
                secilenId = int.Parse(gridView1.GetFocusedRowCellValue("CekID").ToString());
            }
            catch (Exception)
            {
                
            }
        }

        void Temizle()
        {
            txtCekTuru.SelectedIndex = 0;
            txtCekNo.Text = "";
            txtBanka.Text = "";
        }

        private void frmCekListesi_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            Sec();
            if (secim && secilenId > 0)
            {
                AnaForm.Aktarma = secilenId;
                this.Close();
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            Temizle();
        }
    }
}