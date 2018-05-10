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
    public partial class frmParaTransfer : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();

        bool edit = false;
        int bankaID = -1;
        int cariID = -1;
        int islemID = -1;

        public frmParaTransfer()
        {
            InitializeComponent();
        }

        private void txtTransferTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtTransferTuru.SelectedIndex == 0)
            {
                rbtnGelen.Text = "Gelen Havale";
                rbtnGiden.Text = "Giden Havale";
            }
            else if (txtTransferTuru.SelectedIndex == 1)
            {
                rbtnGelen.Text = "Gelen EFT";
                rbtnGiden.Text = "Giden EFT";
            }
        }

        private void frmBankaListesi_Load(object sender, EventArgs e)
        {
            txtTarih.Text = DateTime.Now.ToShortDateString();
        }

        void Temizle()
        {
            txtAciklama.Text = "";
            txtBelgeNo.Text = "";
            txtCariAdı.Text = "";
            txtCariKodu.Text = "";
            txtHesapAdi.Text = "";
            txtHesapNo.Text = "";
            txtTarih.Text = DateTime.Now.ToShortDateString();
            txtTutar.Text = "0";
            edit = false;
            bankaID = -1;
            cariID = -1;
            islemID = -1;
            AnaForm.Aktarma = -1;
        }

        public void Ac(int ID)
        {
            try
            {
                edit = true;
                islemID = ID;
                Fonksiyonlar.BankaHareketleri banka = DB.BankaHareketleris.First(s => s.BankaHareketID == islemID);
                BankaAc(banka.BankaId.Value);
                CariAc(banka.CariId.Value);
                txtAciklama.Text = banka.Aciklama;
                txtBelgeNo.Text = banka.BelgeNo;
                txtTarih.Text = banka.Tarih.Value.ToShortDateString();
                txtTransferTuru.Text = banka.EvrakTuru;
                txtTutar.Text = banka.Tutar.ToString();
                if (banka.GCKodu == "G")
                    rbtnGelen.Checked = true;
                if (banka.GCKodu == "C")
                    rbtnGiden.Checked = true;

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

        void CariAc(int ID)
        {
            try
            {
                cariID = ID;
                txtCariKodu.Text = DB.Caris.First(s => s.CariID == cariID).CariKodu;
                txtCariAdı.Text = DB.Caris.First(s => s.CariID == cariID).CariAdi;
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
                Fonksiyonlar.BankaHareketleri banka = new Fonksiyonlar.BankaHareketleri();
                banka.Aciklama = txtAciklama.Text;
                banka.BankaId = bankaID;
                banka.BelgeNo = txtBelgeNo.Text;
                banka.CariId = cariID;
                banka.EvrakTuru = txtTransferTuru.SelectedItem.ToString();
                if (rbtnGelen.Checked)
                    banka.GCKodu = "G";
                if (rbtnGiden.Checked)
                    banka.GCKodu = "C";
                banka.Tarih = DateTime.Parse(txtTarih.Text);
                banka.Tutar = decimal.Parse(txtTutar.Text);
                banka.SaveDate = DateTime.Now;
                banka.SaveUser = AnaForm.UserID;
                DB.BankaHareketleris.InsertOnSubmit(banka);
                DB.SubmitChanges();

                Fonksiyonlar.CariHareketleri cari = new Fonksiyonlar.CariHareketleri();
                cari.Aciklama = txtAciklama.Text;
                if (rbtnGelen.Checked)
                    cari.Alacak = decimal.Parse(txtTutar.Text);
                if (rbtnGiden.Checked)
                    cari.Borc = decimal.Parse(txtTutar.Text);
                cari.CariId = cariID;
                cari.EvrakId = banka.BankaHareketID;
                cari.EvrakTuru = txtTransferTuru.SelectedItem.ToString();
                cari.Tarih = DateTime.Parse(txtTarih.Text);
                if (txtTransferTuru.SelectedIndex == 0)
                    cari.Tipi = "BH";
                if (txtTransferTuru.SelectedIndex == 1)
                    cari.Tipi = "EFT";
                cari.SaveDate = DateTime.Now;
                cari.SaveUser = AnaForm.UserID;
                DB.CariHareketleris.InsertOnSubmit(cari);
                DB.SubmitChanges();
                mesajlar.YeniKayit("Para Transfer Kaydı İşlendi");
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
                Fonksiyonlar.BankaHareketleri banka = DB.BankaHareketleris.First(s => s.BankaHareketID == islemID);
                banka.Aciklama = txtAciklama.Text;
                banka.BankaId = bankaID;
                banka.BelgeNo = txtBelgeNo.Text;
                banka.CariId = cariID;
                banka.EvrakTuru = txtTransferTuru.SelectedItem.ToString();
                if (rbtnGelen.Checked)
                    banka.GCKodu = "G";
                if (rbtnGiden.Checked)
                    banka.GCKodu = "C";
                banka.Tarih = DateTime.Parse(txtTarih.Text);
                banka.Tutar = decimal.Parse(txtTutar.Text);
                banka.EditDate = DateTime.Now;
                banka.EditUser = AnaForm.UserID;
                DB.SubmitChanges();

                Fonksiyonlar.CariHareketleri cari = DB.CariHareketleris.First(s => s.EvrakTuru == txtTransferTuru.SelectedItem.ToString() && s.EvrakId == islemID);
                cari.Aciklama = txtAciklama.Text;
                if (rbtnGelen.Checked)
                    cari.Alacak = decimal.Parse(txtTutar.Text);
                if (rbtnGiden.Checked)
                    cari.Borc = decimal.Parse(txtTutar.Text);
                cari.CariId = cariID;
                cari.EvrakId = banka.BankaHareketID;
                cari.EvrakTuru = txtTransferTuru.SelectedItem.ToString();
                cari.Tarih = DateTime.Parse(txtTarih.Text);
                if (txtTransferTuru.SelectedIndex == 0)
                    cari.Tipi = "BH";
                if (txtTransferTuru.SelectedIndex == 1)
                    cari.Tipi = "EFT";
                cari.EditDate = DateTime.Now;
                cari.EditUser = AnaForm.UserID;
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
                DB.CariHareketleris.DeleteOnSubmit(DB.CariHareketleris.First(s => s.EvrakTuru == txtTransferTuru.SelectedItem.ToString() && s.EvrakId == islemID));
                // sorun çıkması durumunda bu araya da DB.SubmitChanges(); ekle.
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
            int Id = formlar.BankaListesi(true);
            if (Id > 0)
                BankaAc(Id);
            AnaForm.Aktarma = -1;
        }

        private void txtCariKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = formlar.CariListesi(true);
            if (Id > 0)
                CariAc(Id);
            AnaForm.Aktarma = -1;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit && cariID > 0 && bankaID > 0 && islemID > 0 && mesajlar.Sil() == DialogResult.Yes)
                Guncelle();
            else
                Kaydet();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (edit && cariID > 0 && bankaID > 0 && islemID > 0 && mesajlar.Sil() == DialogResult.Yes)
                Sil();
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}