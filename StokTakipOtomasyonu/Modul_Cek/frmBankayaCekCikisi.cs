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
    public partial class frmBankayaCekCikisi : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();

        Fonksiyonlar.Cek cek;

        int cekId = -1;
        int bankaId = -1;

        public frmBankayaCekCikisi()
        {
            InitializeComponent();
        }

        private void frmBankayaCekCikisi_Load(object sender, EventArgs e)
        {
            txtVadeTarihi.Text = DateTime.Now.ToShortDateString();
            txtTarih.Text = DateTime.Now.ToShortDateString();
        }

        void Temizle()
        {
            txtBanka.Text = "";
            txtBankaAdi.Text = "";
            txtBelgeNo.Text = "";
            txtCekNo.Text = "";
            txtHesapNo.Text = "";
            txtSube.Text = "";
            txtTarih.Text = DateTime.Now.ToShortDateString();
            txtTutar.Text = "";
            cek = null;
            cekId = -1;
            bankaId = -1;
            AnaForm.Aktarma = -1;
        }

        void CekGetir(int ID)
        {
            try
            {
                cekId = ID;
                cek = DB.Ceks.First(s => s.CekID == cekId);
                txtBanka.Text = cek.Banka;
                txtCekNo.Text = cek.CekNo;
                txtSube.Text = cek.Sube;
                txtVadeTarihi.Text = cek.VadeTarihi.Value.ToShortDateString();
                txtTutar.Text = cek.Tutar.Value.ToString();
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        void BankaGetir(int ID)
        {
            try
            {
                bankaId = ID;
                txtHesapNo.Text = DB.Bankas.First(s => s.BankaID == bankaId).HesapNo;
                txtBankaAdi.Text = DB.Bankas.First(s => s.BankaID == bankaId).BankaAdi;
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        void Kaydet()
        {
            try
            {
                Fonksiyonlar.BankaHareketleri hareket = new Fonksiyonlar.BankaHareketleri();
                hareket.Aciklama = txtBelgeNo.Text + " Belge numaralı ve " + txtCekNo.Text + " Numaralı çekin bankaya çıkışı";
                hareket.BankaId = bankaId;
                hareket.BelgeNo = txtBelgeNo.Text;
                hareket.EvrakId = cekId;
                hareket.EvrakTuru = "Bankaya Çek";
                hareket.GCKodu = "G";
                hareket.Tarih = DateTime.Parse(txtTarih.Text);
                hareket.Tutar = decimal.Parse(txtTutar.Text);
                hareket.SaveDate = DateTime.Now;
                hareket.SaveUser = AnaForm.UserID;
                DB.BankaHareketleris.InsertOnSubmit(hareket);
                DB.SubmitChanges();
                mesajlar.YeniKayit(txtCekNo.Text + " Numaralı çekin banka hareket kaydı işlenmiştir.");
                cek.VerilenBanka_BelgeNo = txtBelgeNo.Text;
                cek.VerilenBanka_Tarihi = DateTime.Parse(txtTarih.Text);
                cek.VerilenBankaId = bankaId;
                cek.Durumu = "Bankada";
                DB.SubmitChanges();
                mesajlar.YeniKayit(txtCekNo.Text + " Numaralı çekin durum güncellemesi yapılmıştır.");
                Temizle();
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        private void txtHesapNo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int id = formlar.BankaListesi(true);
            if (id > 0)
            {
                BankaGetir(id);
            }
            AnaForm.Aktarma = -1;   
        }

        private void txtCekNo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int id = formlar.CekListesi(true);
            if (id > 0)
            {
                CekGetir(id);
            }
            AnaForm.Aktarma = -1;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if(cek!=null&& cekId > 0 && bankaId > 0)
            {
                Kaydet();
            }
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}