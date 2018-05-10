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

namespace StokTakipOtomasyonu.Modul_Banka
{
    public partial class frmBankaListesi : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();

        public bool secim = false;
        int secimID = -1;

        public frmBankaListesi()
        {
            InitializeComponent();
        }

        private void frmBankaListesi_Load(object sender, EventArgs e)
        {
            Listele();
        }

        void Listele()
        {
            var lst = from s in DB.View_BankaListesis
                      where s.HesapAdi.Contains(txtHesapAdi.Text) && s.HesapNo.Contains(txtHesapNo.Text) && s.IBAN.Contains(txtIBAN.Text)
                      select s;
            Liste.DataSource = lst;
        }

        void Sec()
        {
            try
            {
                secimID = int.Parse(gridView1.GetFocusedRowCellValue("BankaID").ToString());
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
    }
}