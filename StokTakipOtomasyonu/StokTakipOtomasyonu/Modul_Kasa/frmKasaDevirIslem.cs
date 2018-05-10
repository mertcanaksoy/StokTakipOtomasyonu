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
    public partial class frmKasaDevirIslem : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();

        bool edit = false;
        int kasaID = -1;
        int islemID = -1;

        public frmKasaDevirIslem()
        {
            InitializeComponent();
        }

        private void frmKasaDevirIslem_Load(object sender, EventArgs e)
        {
            txtTarih.Text = DateTime.Now.ToShortDateString();
        }

        void Temizle()
        {
            txtTarih.Text = DateTime.Now.ToShortDateString();
            txtAciklama.Text = "";
            txtBelgeNo.Text = "";
            txtKasaAdi.Text = "";
            txtKasaKodu.Text = "";
            txtTutar.Text = "0";

            edit = false;
            kasaID = -1;
            islemID = -1;
            AnaForm.Aktarma = -1;
        }

        void Kaydet()
        {
            try
            {
                Fonksiyonlar.KasaHareketleri hareket = new Fonksiyonlar.KasaHareketleri();
                hareket.Aciklama = txtAciklama.Text;
                hareket.BelgeNo = txtBelgeNo.Text;
                hareket.EvrakTuru = "Kasa Devir Kartı";
                if (rbtnCikis.Checked)
                    hareket.GCKodu = "C";
                if (rbtnGiris.Checked)
                    hareket.GCKodu = "G";
                hareket.KasaId = kasaID;
                hareket.Tarih = DateTime.Parse(txtTarih.Text);
                hareket.Tutar = decimal.Parse(txtTutar.Text);
                hareket.SaveDate = DateTime.Now;
                hareket.SaveUser = AnaForm.UserID;

                DB.KasaHareketleris.InsertOnSubmit(hareket);
                DB.SubmitChanges();
                mesajlar.YeniKayit("Devir Kartı Hareket Kaydı Başarıyla İşlenmiştir");
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
                Fonksiyonlar.KasaHareketleri hareket = DB.KasaHareketleris.First(s => s.KasaHareketID == islemID);
                hareket.Aciklama = txtAciklama.Text;
                hareket.BelgeNo = txtBelgeNo.Text;
                hareket.EvrakTuru = "Kasa Devir Kartı";
                if (rbtnCikis.Checked)
                    hareket.GCKodu = "C";
                if (rbtnGiris.Checked)
                    hareket.GCKodu = "G";
                hareket.KasaId = kasaID;
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
                DB.KasaHareketleris.DeleteOnSubmit(DB.KasaHareketleris.First(s => s.KasaHareketID == islemID));
                DB.SubmitChanges();
                Temizle();
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        public void Ac(int ID)
        {
            try
            {
                islemID = ID;
                edit = true;
                Fonksiyonlar.KasaHareketleri hareket = DB.KasaHareketleris.First(s => s.KasaHareketID == islemID);
                txtAciklama.Text = hareket.Aciklama;
                txtBelgeNo.Text = hareket.BelgeNo;
                KasaAc(hareket.KasaId.Value);
                txtTarih.Text = hareket.Tarih.Value.ToShortDateString();
                txtTutar.Text = hareket.Tutar.Value.ToString();
                if (hareket.GCKodu == "C")
                    rbtnCikis.Checked = true;
                if (hareket.GCKodu == "G")
                    rbtnGiris.Checked = true;
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        void KasaAc(int ID)
        {
            try
            {
                kasaID = ID;
                txtKasaAdi.Text = DB.Kasas.First(s => s.KasaID == kasaID).KasaAdi;
                txtKasaKodu.Text = DB.Kasas.First(s => s.KasaID == kasaID).KasaKodu;
            }
            catch (Exception)
            {

                throw;
            }
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

        private void txtKasaKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int id = formlar.KasaListesi(true);
            if (id > 0)
            {
                KasaAc(id);
                AnaForm.Aktarma = -1;
            }
        }

        private void txtBelgeNo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

        }
    }
}