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
    public partial class frmBankaHareketleri : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();

        int bankaID = -1;
        int islemID = -1;
        string evrakTuru;

        public frmBankaHareketleri()
        {
            InitializeComponent();
        }

        private void frmBankaHareketleri_Load(object sender, EventArgs e)
        {

        }

        void Listele()
        {
            var lst = from s in DB.View_BankaHareketleris
                      where s.BankaId==bankaID
                      select s;
            Liste.DataSource = lst;
        }

        public void BankaAc(int ID)
        {
            try
            {
                bankaID = ID;
                Fonksiyonlar.View_BankaListesi banka = DB.View_BankaListesis.First(s => s.BankaID == bankaID);
                txtHesapAdi.Text = banka.HesapAdi;
                txtHesapNo.Text = banka.HesapNo;
                txtGiris.Text = banka.Giris.Value.ToString();
                txtCikis.Text = banka.Cikis.Value.ToString();
                txtBakiye.Text = banka.Bakiye.Value.ToString();
                Listele();
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        void Sec()
        {
            try
            {
                islemID = int.Parse(gridView1.GetFocusedRowCellValue("BankaHareketID").ToString());
                evrakTuru = gridView1.GetFocusedRowCellValue("EvrakTuru").ToString();
            }
            catch (Exception)
            {
                islemID = -1;
                evrakTuru = "";
            }
        }

        private void txtHesapAdi_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = formlar.BankaListesi(true);
            if (Id > 0)
                BankaAc(Id);
            AnaForm.Aktarma = -1;
        }

        private void sagTik_Opening(object sender, CancelEventArgs e)
        {
            Sec();
            if (islemID > 0)
            {
                if (evrakTuru == "Banka İşlem")
                {
                    BankaIslemiDuzenle.Enabled = true;
                    ParaTransferiDuzenle.Enabled = false;
                }
                else if (evrakTuru == "Havale" || evrakTuru == "EFT")
                {
                    BankaIslemiDuzenle.Enabled = false;
                    ParaTransferiDuzenle.Enabled = true;
                }
            }
        }

        private void BankaIslemiDuzenle_Click(object sender, EventArgs e)
        {
            formlar.BankaIslem(true, islemID);
            Listele();
        }

        private void ParaTransferiDuzenle_Click(object sender, EventArgs e)
        {
            formlar.BankaParaTransfer(true, islemID);
            Listele();
        }
    }
}