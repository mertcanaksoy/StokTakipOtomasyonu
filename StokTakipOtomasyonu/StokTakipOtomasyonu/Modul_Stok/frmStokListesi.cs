using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace StokTakipOtomasyonu.Modul_Stok
{

    public partial class frmStokListesi : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();

        public bool Secim = false;
        int secimID = -1;

        public frmStokListesi()
        {
            InitializeComponent();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void frmStokListesi_Load(object sender, EventArgs e)
        {
            Listele();
        }

        void Listele()
        {
            var lst = from s in DB.Stoks
                      where s.StokAdi.Contains(txtStokAdi.Text)&&s.StokBarkod.Contains(txtBarkod.Text)&&s.StokKodu.Contains(txtStokKodu.Text)
                      select s;
            Liste.DataSource = lst;
        }

        void Sec()
        {
            try
            {
                secimID = int.Parse(gridView1.GetFocusedRowCellValue("StokID").ToString());
            }
            catch (Exception)
            {
                secimID = -1;
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            txtStokKodu.Text = "";
            txtStokAdi.Text = "";
            txtBarkod.Text = "";
        }

        private void txtStokAdi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                Listele();
        }

        private void txtStokKodu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                Listele();
        }

        private void txtBarkod_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                Listele();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            Sec();
            if (Secim && secimID > 0)
            {
                AnaForm.Aktarma = secimID;
                this.Close();
            }
        }

       
    }
}