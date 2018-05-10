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
    public partial class frmCariyeCekCikisi : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();

        int cariId = -1;
        int cekId = -1;
        bool edit = false;
        
        public frmCariyeCekCikisi()
        {
            InitializeComponent();
        }

        private void frmCariyeCekCikisi_Load(object sender, EventArgs e)
        {
            txtTarih.Text = DateTime.Now.ToShortDateString();
            txtVadeTarihi.Text = DateTime.Now.ToShortDateString();
        }

        void Temizle()
        {
            txtBanka.Text = "";
            txtBelgeNo.Text = "";
            txtCariAdi.Text = "";
            txtCariKodu.Text = "";
            txtCekNo.Text = "";
            txtSube.Text = "";
            txtTarih.Text = DateTime.Now.ToShortDateString();
            txtTutar.Text = "";
            txtVadeTarihi.Text = DateTime.Now.ToShortDateString();
            cariId = -1;
            cekId = -1;
            edit = false;
            AnaForm.Aktarma = -1;
        }

        void CariAc(int ID)
        {
            try
            {
                cariId = ID;
                Fonksiyonlar.Cari cari = DB.Caris.First(s => s.CariID == cariId);
                txtCariAdi.Text = cari.CariAdi;
                txtCariKodu.Text = cari.CariKodu;
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        void CekGetir(int ID)
        {
            try
            {
                cekId = ID;
                Fonksiyonlar.Cek cek = DB.Ceks.First(s => s.CekID == cekId);
                txtBanka.Text = cek.Banka;
                txtSube.Text = cek.Sube;
                txtCekNo.Text = cek.CekNo;
                txtVadeTarihi.Text = cek.VadeTarihi.Value.ToShortDateString();
                txtTutar.Text = cek.Tutar.Value.ToString();
                if (cek.VerilenCariId != null)
                {
                    if (cek.VerilenCariId.Value > 0)
                    {
                        CariAc(cek.VerilenCariId.Value);
                        txtBelgeNo.Text = cek.VerilenCari_BelgeNo;
                        txtTarih.Text = cek.VerilenCari_Tarihi.Value.ToShortDateString();
                    } 
                }
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
                Fonksiyonlar.Cek cek = DB.Ceks.First(s => s.CekID == cekId);
                cek.VerilenCariId = cariId;
                cek.VerilenCari_Tarihi = DateTime.Parse(txtTarih.Text);
                cek.VerilenCari_BelgeNo = txtBelgeNo.Text;
                cek.Durumu = "Caride";
                cek.EditDate = DateTime.Now;
                cek.EditUser = AnaForm.UserID;
                DB.SubmitChanges();

                Fonksiyonlar.CariHareketleri cariHareket = new Fonksiyonlar.CariHareketleri();
                cariHareket.Aciklama = txtCekNo.Text + " Çek numaralı ve " + txtBelgeNo.Text + " Belge numaralı çek";
                cariHareket.Borc = decimal.Parse(txtTutar.Text);
                cariHareket.CariId = cariId;
                cariHareket.EvrakId = cekId;
                cariHareket.EvrakTuru = "Cariye Çek";
                cariHareket.Tarih = DateTime.Now;
                cariHareket.Tipi = "Çek İşlemi";
                cariHareket.SaveDate = DateTime.Now;
                cariHareket.SaveUser = AnaForm.UserID;
                DB.CariHareketleris.InsertOnSubmit(cariHareket);
                DB.SubmitChanges();
                mesajlar.YeniKayit("Cariye çek çıkışı işleminin hareket kaydı ve çek kaydı GÜNCELLEMESİ yapılmıştır");
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
                cek.VerilenCariId = cariId;
                cek.VerilenCari_Tarihi = DateTime.Parse(txtTarih.Text);
                cek.VerilenCari_BelgeNo = txtBelgeNo.Text;
                cek.Durumu = "Caride";
                cek.EditDate = DateTime.Now;
                cek.EditUser = AnaForm.UserID;
                DB.SubmitChanges();

                Fonksiyonlar.CariHareketleri cariHareket = DB.CariHareketleris.First(s => s.EvrakTuru == "Cariye Çek" && s.EvrakId == cekId);
                cariHareket.Aciklama = txtCekNo.Text + " Çek numaralı ve " + txtBelgeNo.Text + " Belge numaralı çek";
                cariHareket.Borc = decimal.Parse(txtTutar.Text);
                cariHareket.CariId = cariId;
                cariHareket.EvrakId = cekId;
                cariHareket.EvrakTuru = "Cariye Çek";
                cariHareket.Tarih = DateTime.Now;
                cariHareket.Tipi = "Çek İşlemi";
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

        public void Ac(int cekIDsi)
        {
            try
            {
                cekId = cekIDsi;
                CekGetir(cekId);
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
                CariAc(id);
            AnaForm.Aktarma = -1;
        }

        private void txtCekNo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int id = formlar.CekListesi(true);
            if (id > 0)
                CekGetir(id);
            AnaForm.Aktarma = -1;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit && mesajlar.Guncelle() == DialogResult.Yes && cariId > 0 && cekId > 0)
                Guncelle();
            else if (cariId > 0 && cekId > 0)
                Kaydet();
            else
                MessageBox.Show("Lütfen Çek veya Cari seçimi yapınız!");
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (edit && mesajlar.Sil() == DialogResult.Yes && cariId > 0 && cekId > 0)
                {
                    DB.CariHareketleris.DeleteOnSubmit(DB.CariHareketleris.First(s => s.EvrakTuru == "Cariye Çek" && s.EvrakId == cekId));
                    Fonksiyonlar.Cek cek = DB.Ceks.First(s => s.CekID == cekId);
                    cek.VerilenCari_BelgeNo = "";
                    cek.VerilenCariId = -1;
                    DB.SubmitChanges();
                }
            }
            catch (Exception ee)
            {
                mesajlar.Hata(ee);
            }      
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}