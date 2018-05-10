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

namespace StokTakipOtomasyonu.Modul_Kasa
{
    public partial class frmKasaListesi : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();

        public bool secim = false;
        int secimID = -1;

        public frmKasaListesi()
        {
            InitializeComponent();
        }

        private void frmKasaListesi_Load(object sender, EventArgs e)
        {
            Listele();
        }

        void Listele()
        {
            var lst = from s in DB.View_KasaListesis
                      where s.KasaKodu.Contains(txtKasaKodu.Text)&&s.KasaAdi.Contains(txtKasaAdi.Text)
                      select s;
            Liste.DataSource = lst;
        }

        void Temizle()
        {
            txtKasaKodu.Text = "";
            txtKasaAdi.Text = "";
            secim = false;
            AnaForm.Aktarma = -1;
        }

        void Sec()
        {
            try
            {
                secimID = int.Parse(gridView1.GetFocusedRowCellValue("KasaID").ToString());
            }
            catch (Exception)
            {
                secimID = -1;
            }
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            Sec();
            if (secim && secimID > 0)
            {
                AnaForm.Aktarma = secimID;
                this.Close();
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }
    }
}