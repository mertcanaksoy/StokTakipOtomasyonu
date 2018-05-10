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
    public partial class frmKendiCekimiz : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();

        int cekId = -1;
        int bankaId = -1;
        bool edit = false;

        public frmKendiCekimiz()
        {
            InitializeComponent();
        }

        private void frmKendiCekimiz_Load(object sender, EventArgs e)
        {
            txtVadeTarihi.Text = DateTime.Now.ToShortDateString();
        }

        public void Ac(int CekIDsi)
        {
            try
            {
                cekId = CekIDsi;
                Fonksiyonlar.Cek cek = DB.Ceks.First(s => s.CekID == cekId);
                bankaId = DB.Bankas.First(s => s.BankaAdi == cek.Banka && s.HesapNo == cek.HesapNo).BankaID;
                BankaAc(bankaId);

                txtAciklama.Text = cek.Aciklama;
                txtBelgeNo.Text = cek.BelgeNo;
                txtCekNo.Text = cek.CekNo;
                txtTutar.Text = cek.Tutar.Value.ToString();
                txtVadeTarihi.Text = cek.VadeTarihi.Value.ToShortDateString();
                edit = true;
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
                Temizle();
            }
        }

        void BankaAc(int BankaIDsi)
        {
            try
            {
                bankaId = BankaIDsi;
                Fonksiyonlar.Banka banka = DB.Bankas.First(s => s.BankaID == bankaId);
                txtBanka.Text = banka.BankaAdi;
                txtHesapNo.Text = banka.HesapAdi;
                txtSube.Text = banka.Sube;
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        void Temizle()
        {
            txtAciklama.Text = "";
            txtBanka.Text = "";
            txtBelgeNo.Text = "";
            txtCekNo.Text = "";
            txtHesapNo.Text = "";
            txtSube.Text = "";
            txtTutar.Text = "";
            txtVadeTarihi.Text = DateTime.Now.ToShortDateString();
            cekId = -1;
            bankaId = -1;
            edit = false;
            AnaForm.Aktarma = -1;
        }

        void Kaydet()
        {
            try
            {
                Fonksiyonlar.Cek cek = new Fonksiyonlar.Cek();
                cek.Aciklama = txtAciklama.Text;
                cek.ACKodu = "A";
                cek.Banka = txtBanka.Text;
                cek.BelgeNo = txtBelgeNo.Text;
                cek.CekNo = txtCekNo.Text;
                cek.Durumu = "Portföy";
                cek.HesapNo = txtHesapNo.Text;
                cek.Sube = txtSube.Text;
                cek.Tahsil = "Hayır";
                cek.Tarih = DateTime.Now;
                cek.Tipi = "Kendi Çekimiz";
                cek.Tutar = decimal.Parse(txtTutar.Text);
                cek.VadeTarihi = DateTime.Parse(txtVadeTarihi.Text);
                cek.SaveDate = DateTime.Now;
                cek.SaveUser = AnaForm.UserID;
                DB.Ceks.InsertOnSubmit(cek);
                DB.SubmitChanges();
                mesajlar.YeniKayit(txtCekNo.Text + " Numaralı Kendi Çekinizin Çek Kaydı Yapılmıştır");

                Fonksiyonlar.BankaHareketleri bankahareketi = new Fonksiyonlar.BankaHareketleri();
                bankahareketi.Aciklama = txtCekNo.Text + " Numaralı ve " + txtVadeTarihi.Text + " Vadeli Kendi Çekimiz";
                bankahareketi.BankaId = bankaId;
                bankahareketi.BelgeNo = txtBelgeNo.Text;
                bankahareketi.EvrakId = cek.CekID;
                bankahareketi.EvrakTuru = "Kendi Çekimiz";
                bankahareketi.GCKodu = "C";
                bankahareketi.Tarih = DateTime.Now;
                bankahareketi.Tutar = 0;
                bankahareketi.SaveDate = DateTime.Now;
                bankahareketi.SaveUser = AnaForm.UserID;
                DB.BankaHareketleris.InsertOnSubmit(bankahareketi);
                DB.SubmitChanges();
                mesajlar.YeniKayit(txtCekNo.Text + " Numaralı Kendi Çekinizin Banka Hareket Kaydı Yapılmıştır");
                Temizle();
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        void Guncelle()
        {
            try
            {
                Fonksiyonlar.Cek cek = DB.Ceks.First(s => s.CekID == cekId);
                cek.Aciklama = txtAciklama.Text;
                cek.ACKodu = "A";
                cek.Banka = txtBanka.Text;
                cek.BelgeNo = txtBelgeNo.Text;
                cek.CekNo = txtCekNo.Text;
                cek.Durumu = "Portföy";
                cek.HesapNo = txtHesapNo.Text;
                cek.Sube = txtSube.Text;
                cek.Tahsil = "Hayır";
                cek.Tarih = DateTime.Now;
                cek.Tipi = "Kendi Çekimiz";
                cek.Tutar = decimal.Parse(txtTutar.Text);
                cek.VadeTarihi = DateTime.Parse(txtVadeTarihi.Text);
                cek.EditDate = DateTime.Now;
                cek.EditUser = AnaForm.UserID;
                DB.SubmitChanges();
                Fonksiyonlar.BankaHareketleri bankahareketi = DB.BankaHareketleris.First(s => s.EvrakId == cekId && s.EvrakTuru == "Kendi Çekimiz");
                bankahareketi.Aciklama = txtCekNo.Text + " Numaralı ve " + txtVadeTarihi.Text + " Vadeli Kendi Çekimiz";
                bankahareketi.BankaId = bankaId;
                bankahareketi.BelgeNo = txtBelgeNo.Text;
                bankahareketi.EvrakId = cek.CekID;
                bankahareketi.EvrakTuru = "Kendi Çekimiz";
                bankahareketi.GCKodu = "C";
                bankahareketi.Tarih = DateTime.Now;
                bankahareketi.Tutar = 0;
                bankahareketi.EditDate = DateTime.Now;
                bankahareketi.EditUser = AnaForm.UserID;
                DB.SubmitChanges();
                mesajlar.Guncelle(true);
                Temizle();
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        void Sil()
        {
            try
            {                
                DB.BankaHareketleris.DeleteOnSubmit(DB.BankaHareketleris.First(s => s.EvrakId == cekId && s.EvrakTuru == "Kendi Çekimiz"));
                DB.Ceks.DeleteOnSubmit(DB.Ceks.First(s => s.CekID == cekId));
                DB.SubmitChanges();
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
                BankaAc(id);
                AnaForm.Aktarma = -1;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit && cekId > 0 && mesajlar.Guncelle() == DialogResult.Yes)
                Guncelle();
            else
                Kaydet();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (edit && cekId > 0 && mesajlar.Sil() == DialogResult.Yes)
                Sil();
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}