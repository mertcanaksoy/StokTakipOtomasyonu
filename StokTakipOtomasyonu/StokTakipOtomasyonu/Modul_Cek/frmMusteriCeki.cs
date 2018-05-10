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
    public partial class frmMusteriCeki : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();

        int cariId = -1;
        int cekId = -1;
        bool edit = false;

        public frmMusteriCeki()
        {
            InitializeComponent();
        }

        private void frmMusteriCeki_Load(object sender, EventArgs e)
        {
            txtVadeTarihi.Text = DateTime.Now.ToShortDateString();
        }

        void Temizle()
        {
            txtAciklama.Text = "";
            txtAsilBorclu.Text = "";
            txtBanka.Text = "";
            txtBelgeNo.Text = "";
            txtCariAdi.Text = "";
            txtCariKodu.Text = "";
            txtCekNo.Text = "";
            txtCekTuru.SelectedIndex = 0;
            txtHesapNo.Text = "";
            txtSube.Text = "";
            txtTutar.Text = "";
            txtVadeTarihi.Text = DateTime.Now.ToShortDateString();

            cariId = -1;
            cekId = -1;
            AnaForm.Aktarma = -1;
            edit = false;
        }

        void Kaydet()
        {
            try
            {
                Fonksiyonlar.Cek cek = new Fonksiyonlar.Cek();
                cek.Aciklama = txtAciklama.Text;
                if (txtCekTuru.SelectedIndex == 0)
                    cek.ACKodu = "A"; //Asıl çek
                if (txtCekTuru.SelectedIndex == 1)
                    cek.ACKodu = "C"; //Cari çek
                cek.AlinanCariId = cariId;
                cek.Banka = txtBanka.Text;
                cek.BelgeNo = txtBelgeNo.Text;
                cek.CekNo = txtCekNo.Text;
                cek.Durumu = "Portföy";
                cek.HesapNo = txtHesapNo.Text;
                cek.Sube = txtSube.Text;
                cek.Tahsil = "Hayır"; //Tahsil edildi mi? sorusuna yanıt
                cek.Tarih = DateTime.Now;
                cek.VadeTarihi = DateTime.Parse(txtVadeTarihi.Text);
                cek.Tutar = decimal.Parse(txtTutar.Text);
                cek.Tipi = "Müşteri Çeki";
                cek.AsilBorclu = txtAsilBorclu.Text;
                cek.SaveDate = DateTime.Now;
                cek.SaveUser = AnaForm.UserID;

                DB.Ceks.InsertOnSubmit(cek);
                DB.SubmitChanges();
                mesajlar.YeniKayit(txtCekNo.Text + " Numaralı Müşteri Çeki Kaydı Gerçekleşmiştir");
                //Cek işlendikten sonra bunun cari hareketlere de işlenmesi lazım. O yüzden yeni bir cari hareket açıyoruz
                Fonksiyonlar.CariHareketleri cariHareket = new Fonksiyonlar.CariHareketleri();
                cariHareket.Aciklama = txtBelgeNo.Text + " Belge Numaralı ve " + txtCekNo.Text + " Çek Numaralı Müşteri Çeki";
                cariHareket.CariId = cariId;
                cariHareket.EvrakId = cek.CekID;
                cariHareket.EvrakTuru = "Müşteri Çeki";
                cariHareket.Tarih = DateTime.Now;
                cariHareket.Tipi = "MÇ";
                cariHareket.SaveDate = DateTime.Now;
                cariHareket.SaveUser = AnaForm.UserID;
                DB.CariHareketleris.InsertOnSubmit(cariHareket);
                DB.SubmitChanges();
                mesajlar.YeniKayit(txtCekNo.Text + " Numaralı Müşteri Çeki Cari Kaydı Gerçekleşmiştir");
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
                if (txtCekTuru.SelectedIndex == 0)
                    cek.ACKodu = "A"; //Asıl çek
                if (txtCekTuru.SelectedIndex == 1)
                    cek.ACKodu = "C"; //Cari çek
                cek.AlinanCariId = cariId;
                cek.Banka = txtBanka.Text;
                cek.BelgeNo = txtBelgeNo.Text;
                cek.CekNo = txtCekNo.Text;
                cek.Durumu = "Portföy";
                cek.HesapNo = txtHesapNo.Text;
                cek.Sube = txtSube.Text;
                cek.Tahsil = "Hayır"; //Tahsil edildi mi? sorusuna yanıt
                cek.VadeTarihi = DateTime.Parse(txtVadeTarihi.Text);
                cek.Tutar = decimal.Parse(txtTutar.Text);
                cek.Tipi = "Müşteri Çeki";
                cek.AsilBorclu = txtAsilBorclu.Text;
                cek.EditDate = DateTime.Now;
                cek.EditUser = AnaForm.UserID;

                DB.SubmitChanges();
                //Cek işlendikten sonra bunun cari hareketlere de işlenmesi lazım. O yüzden yeni bir cari hareket açıyoruz
                Fonksiyonlar.CariHareketleri cariHareket = DB.CariHareketleris.First(s => s.EvrakId == cekId && s.EvrakTuru == "Müşteri Çeki"&&s.Tipi=="MÇ");
                cariHareket.Aciklama = txtBelgeNo.Text + " Belge Numaralı ve " + txtCekNo.Text + " Çek Numaralı Müşteri Çeki";
                cariHareket.CariId = cariId;
                cariHareket.EvrakId = cek.CekID;
                cariHareket.EvrakTuru = "Müşteri Çeki";
                cariHareket.Tarih = DateTime.Now;
                cariHareket.Tipi = "MÇ";
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

        void CariAc(int ID)
        {
            cariId = ID;
            Fonksiyonlar.Cari cari = DB.Caris.First(s => s.CariID == cariId);
            txtCariAdi.Text = cari.CariAdi;
            txtCariKodu.Text = cari.CariKodu;
        }

        public void Ac(int ID)
        {
            try
            {
                cekId = ID;
                Fonksiyonlar.Cek cek = DB.Ceks.First(s => s.CekID == cekId);
                txtAciklama.Text = cek.Aciklama;
                if (cek.ACKodu == "A")
                    txtCekTuru.SelectedIndex = 0;
                if (cek.ACKodu == "C")
                    txtCekTuru.SelectedIndex = 1;
                txtAsilBorclu.Text = cek.AsilBorclu;
                txtBanka.Text = cek.Banka;
                txtBelgeNo.Text = cek.BelgeNo;
                txtCekNo.Text = cek.CekNo;
                txtHesapNo.Text = cek.HesapNo;
                txtSube.Text = cek.Sube;
                txtTutar.Text = cek.Tutar.Value.ToString();
                txtVadeTarihi.Text = cek.VadeTarihi.Value.ToShortDateString();
                CariAc(cek.AlinanCariId.Value);
                edit = true;

            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
                Temizle();
            }
        }

        private void txtCariKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int id = formlar.CariListesi(true);
            if (id > 0)
            {
                CariAc(id);
                AnaForm.Aktarma = -1;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit)
                Guncelle();
            else
                Kaydet();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if(edit&&cekId>0&& mesajlar.Sil() == DialogResult.Yes)
            {
                DB.Ceks.DeleteOnSubmit(DB.Ceks.First(s => s.CekID == cekId));
                DB.CariHareketleris.DeleteOnSubmit(DB.CariHareketleris.First(s => s.EvrakId == cekId && s.EvrakTuru == "Müşteri Çeki"));
                DB.SubmitChanges();
                Temizle();
            }
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}