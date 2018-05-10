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
    public partial class frmKasaTahsilatOdeme : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();

        bool edit = false;
        int islemID = -1;
        int kasaID = -1;
        int cariID = -1;
        int cariHareketID = -1;

        public frmKasaTahsilatOdeme()
        {
            InitializeComponent();
        }

        private void frmKasaTahsilatOdeme_Load(object sender, EventArgs e)
        {
            txtTarih.Text = DateTime.Now.ToShortDateString();
        }

        private void txtIslemTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        void Temizle()
        {
            txtAciklama.Text = "";
            txtBelgeNo.Text = "";
            txtCariAdi.Text = "";
            txtCariKodu.Text = "";
            txtIslemTuru.SelectedIndex = 0;
            txtKasaAdi.Text = "";
            txtKasaKodu.Text = "";
            txtTarih.Text = DateTime.Now.ToShortDateString();
            txtTutar.Text = "0";
            edit = false;
            islemID = -1;
            kasaID = -1;
            cariID = -1;
            cariHareketID = -1;
            AnaForm.Aktarma = -1;
        }

        public void Ac(int hareketID)
        {
            try
            {
                edit = true;
                islemID = hareketID;
                Fonksiyonlar.KasaHareketleri kasaHareketi = DB.KasaHareketleris.First(s => s.KasaHareketID == islemID);
                cariHareketID = DB.CariHareketleris.First(s => s.EvrakTuru == kasaHareketi.EvrakTuru && s.EvrakId == islemID).CariHareketID;
                MessageBox.Show("Cari Hareket ID: " + cariHareketID.ToString());
                txtAciklama.Text = kasaHareketi.Aciklama;
                txtBelgeNo.Text = kasaHareketi.BelgeNo;
                if (kasaHareketi.EvrakTuru == "Kasa Tahsilat")
                    txtIslemTuru.SelectedIndex = 0;
                if (kasaHareketi.EvrakTuru == "Kasa Ödeme")
                    txtIslemTuru.SelectedIndex = 1;
                txtTarih.Text = kasaHareketi.Tarih.Value.ToShortDateString();
                txtTutar.Text = kasaHareketi.Tutar.Value.ToString();
                KasaAc(kasaHareketi.KasaId.Value);
                CariAc(kasaHareketi.CariId.Value);
            }
            catch (Exception e)
            {
                Temizle();
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
                kasaID = -1;
            }
        }
        //Linq sorgusuna dikkat et hata olabilir.
        void CariAc(int ID)
        {
            try
            {
                cariID = ID;
                txtCariAdi.Text = DB.Caris.First(s => s.CariID == kasaID).CariAdi;
                txtCariKodu.Text = DB.Caris.First(s => s.CariID == kasaID).CariKodu;
            }
            catch (Exception)
            {
                cariID = -1;
            }
        }

        void Kaydet()
        {
            try
            {
                Fonksiyonlar.KasaHareketleri kasaHareketi = new Fonksiyonlar.KasaHareketleri();
                kasaHareketi.Aciklama = txtAciklama.Text;
                kasaHareketi.BelgeNo = txtBelgeNo.Text;
                kasaHareketi.CariId = cariID;
                kasaHareketi.EvrakTuru = txtIslemTuru.SelectedItem.ToString();
                if (txtIslemTuru.SelectedIndex == 0)
                    kasaHareketi.GCKodu = "G";
                if (txtIslemTuru.SelectedIndex == 1)
                    kasaHareketi.GCKodu = "C";
                kasaHareketi.KasaId = kasaID;
                kasaHareketi.SaveDate = DateTime.Now;
                kasaHareketi.SaveUser = AnaForm.UserID;
                kasaHareketi.Tarih = DateTime.Parse(txtTarih.Text);
                kasaHareketi.Tutar = decimal.Parse(txtTutar.Text);

                DB.KasaHareketleris.InsertOnSubmit(kasaHareketi);
                DB.SubmitChanges();
                mesajlar.YeniKayit(txtIslemTuru.SelectedItem.ToString() + " Kasa Hareketi Başarıyla Kaydedildi");

                Fonksiyonlar.CariHareketleri cariHareket = new Fonksiyonlar.CariHareketleri();
                cariHareket.Aciklama = txtBelgeNo.Text + " Belge Numaralı " + txtIslemTuru.SelectedIndex.ToString() + " İşlemi";
                if (txtIslemTuru.SelectedIndex == 0)
                    cariHareket.Alacak = decimal.Parse(txtTutar.Text);
                if (txtIslemTuru.SelectedIndex == 1)
                    cariHareket.Borc = decimal.Parse(txtTutar.Text);
                cariHareket.CariId = cariID;
                cariHareket.EvrakId = kasaHareketi.KasaHareketID;
                cariHareket.EvrakTuru = txtIslemTuru.SelectedItem.ToString();
                cariHareket.Tarih = DateTime.Parse(txtTarih.Text);
                if (txtIslemTuru.SelectedIndex == 0)
                    cariHareket.Tipi = "KT";
                if (txtIslemTuru.SelectedIndex == 1)
                    cariHareket.Tipi = "KÖ";
                cariHareket.SaveDate = DateTime.Now;
                cariHareket.SaveUser = AnaForm.UserID;

                DB.CariHareketleris.InsertOnSubmit(cariHareket);
                DB.SubmitChanges();
                mesajlar.YeniKayit(txtIslemTuru.SelectedItem.ToString() + " Cari Hareketi Başarıyla Kaydedildi");
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
                Fonksiyonlar.KasaHareketleri kasaHareketi = DB.KasaHareketleris.First(s => s.KasaHareketID == islemID);
                kasaHareketi.Aciklama = txtAciklama.Text;
                kasaHareketi.BelgeNo = txtBelgeNo.Text;
                kasaHareketi.CariId = cariID;
                kasaHareketi.EvrakTuru = txtIslemTuru.SelectedItem.ToString();
                if (txtIslemTuru.SelectedIndex == 0)
                    kasaHareketi.GCKodu = "G";
                if (txtIslemTuru.SelectedIndex == 1)
                    kasaHareketi.GCKodu = "C";
                kasaHareketi.KasaId = kasaID;
                kasaHareketi.EditDate = DateTime.Now;
                kasaHareketi.EditUser = AnaForm.UserID;
                kasaHareketi.Tarih = DateTime.Parse(txtTarih.Text);
                kasaHareketi.Tutar = decimal.Parse(txtTutar.Text);

                DB.SubmitChanges();
                mesajlar.Guncelle(true);

                Fonksiyonlar.CariHareketleri cariHareket = DB.CariHareketleris.First(s=>s.CariHareketID==cariHareketID);
                cariHareket.Aciklama = txtBelgeNo.Text + " Belge Numaralı " + txtIslemTuru.SelectedIndex.ToString() + " İşlemi";
                if (txtIslemTuru.SelectedIndex == 0)
                    cariHareket.Alacak = decimal.Parse(txtTutar.Text);
                if (txtIslemTuru.SelectedIndex == 1)
                    cariHareket.Borc = decimal.Parse(txtTutar.Text);
                cariHareket.CariId = cariID;
                cariHareket.EvrakId = kasaHareketi.KasaHareketID;
                cariHareket.EvrakTuru = txtIslemTuru.SelectedItem.ToString();
                cariHareket.Tarih = DateTime.Parse(txtTarih.Text);
                if (txtIslemTuru.SelectedIndex == 0)
                    cariHareket.Tipi = "KT";
                if (txtIslemTuru.SelectedIndex == 1)
                    cariHareket.Tipi = "KÖ";
                cariHareket.EditDate = DateTime.Now;
                cariHareket.EditUser = AnaForm.UserID;

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
                DB.CariHareketleris.DeleteOnSubmit(DB.CariHareketleris.First(s => s.CariHareketID == cariHareketID));
                Temizle();
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        private void txtKasaKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = formlar.KasaListesi(true);
            if (Id > 0)
            {
                KasaAc(Id);
                AnaForm.Aktarma = -1;
            }
        }

        private void txtCariKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = formlar.CariListesi(true);
            if (Id > 0)
            {
                CariAc(Id);
                AnaForm.Aktarma = -1;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit && islemID > 0 && cariHareketID > 0 && mesajlar.Guncelle() == DialogResult.Yes)
                Guncelle();
            else
                Kaydet();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (edit && islemID > 0 && cariHareketID > 0 && mesajlar.Sil() == DialogResult.Yes)
                Sil();
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}