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
    public partial class frmBankaIslem : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();

        bool edit = false;
        int islemID = -1;
        int bankaID = -1;

        public frmBankaIslem()
        {
            InitializeComponent();
        }

        private void frmBankaIslem_Load(object sender, EventArgs e)
        {
            txtTarih.Text = DateTime.Now.ToShortDateString();
        }

        public void Ac(int ID)
        {
            try
            {
                edit = true;
                islemID = ID;
                Fonksiyonlar.BankaHareketleri hareket = DB.BankaHareketleris.First(s => s.BankaHareketID == islemID);
                BankaAc(hareket.BankaId.Value);
                txtAciklama.Text = hareket.Aciklama;
                txtBelgeNo.Text = hareket.BelgeNo;
                txtTarih.Text = hareket.Tarih.Value.ToShortDateString();
                txtTutar.Text = hareket.Tutar.ToString();
                if (hareket.GCKodu == "G")
                    rbtnGiris.Checked = true;
                if (hareket.GCKodu == "C")
                    rbtnCikis.Checked = true;
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        void BankaAc(int ID)
        {
            try
            {
                bankaID = ID;
                txtHesapAdi.Text = DB.Bankas.First(s => s.BankaID == bankaID).HesapAdi;
                txtHesapNo.Text = DB.Bankas.First(s => s.BankaID == bankaID).HesapNo;
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        void Temizle()
        {
            txtAciklama.Text = "";
            txtBelgeNo.Text = "";
            txtHesapAdi.Text = "";
            txtHesapNo.Text = "";
            txtTarih.Text = DateTime.Now.ToShortDateString();
            txtTutar.Text = "0";
            edit = false;
            islemID = -1;
            bankaID = -1;
            AnaForm.Aktarma = -1;
        }

        void Kaydet()
        {
            try
            {
                Fonksiyonlar.BankaHareketleri hareket = new Fonksiyonlar.BankaHareketleri();
                hareket.Aciklama = txtAciklama.Text;
                hareket.BankaId = bankaID;
                hareket.BelgeNo = txtBelgeNo.Text;
                hareket.EvrakTuru = "Banka İşlem";
                if (rbtnGiris.Checked)
                    hareket.GCKodu = "G";
                if (rbtnCikis.Checked)
                    hareket.GCKodu = "C";
                hareket.Tarih = DateTime.Parse(txtTarih.Text);
                hareket.Tutar = decimal.Parse(txtTutar.Text);
                hareket.SaveDate = DateTime.Now;
                hareket.SaveUser = AnaForm.UserID;
                DB.BankaHareketleris.InsertOnSubmit(hareket);
                DB.SubmitChanges();
                mesajlar.YeniKayit("Yeni Banka İşlem Kaydı Yapılmıştır");
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
                Fonksiyonlar.BankaHareketleri hareket = DB.BankaHareketleris.First(s => s.BankaHareketID == islemID);
                hareket.Aciklama = txtAciklama.Text;
                hareket.BankaId = bankaID;
                hareket.BelgeNo = txtBelgeNo.Text;
                hareket.EvrakTuru = "Banka İşlem";
                if (rbtnGiris.Checked)
                    hareket.GCKodu = "G";
                if (rbtnCikis.Checked)
                    hareket.GCKodu = "C";
                hareket.Tarih = DateTime.Parse(txtTarih.Text);
                hareket.Tutar = decimal.Parse(txtTutar.Text);
                hareket.EditDate = DateTime.Now;
                hareket.EditUser = AnaForm.UserID;
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
                DB.BankaHareketleris.DeleteOnSubmit(DB.BankaHareketleris.First(s => s.BankaHareketID == islemID));
                DB.SubmitChanges();
                Temizle();
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        private void txtHesapAdi_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int ID = formlar.BankaListesi(true);
            if (ID > 0)
                BankaAc(ID);
            AnaForm.Aktarma = -1;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit && islemID > 0 && mesajlar.Guncelle() == DialogResult.Yes)
                Guncelle();
            else
                Kaydet();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (edit && islemID > 0 && mesajlar.Sil() == DialogResult.Yes)
                Sil();
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}