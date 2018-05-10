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

namespace StokTakipOtomasyonu.Modul_Cari
{
    public partial class frmCariAcilisKarti : DevExpress.XtraEditors.XtraForm
    {

        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();
        Fonksiyonlar.Numara numara = new Fonksiyonlar.Numara();

        bool edit = false;
        int cariID = -1;
        int grupID = -1;

        public frmCariAcilisKarti()
        {
            InitializeComponent();
        }

        private void frmCariAcilisKarti_Load(object sender, EventArgs e)
        {
            txtCariKodu.Text = numara.CariKodKodNumarasi();
        }

        void Temizle()
        {
            foreach (Control CT in groupControl1.Controls)
                if (CT is TextEdit || CT is ButtonEdit)
                    CT.Text = "";

            foreach (Control CE in groupControl2.Controls)
                if (CE is TextEdit || CE is ButtonEdit || CE is MemoEdit)
                    CE.Text = "";

            txtCariKodu.Text = numara.CariKodKodNumarasi();
            edit = false;
            cariID = -1;
            grupID = -1;
            AnaForm.Aktarma = -1;
        }

        void Kaydet()
        {
            try
            {
                Fonksiyonlar.Cari cari = new Fonksiyonlar.Cari();
                cari.Adres = txtAdres.Text;
                cari.CariAdi = txtCariAdi.Text;
                cari.CariKodu = txtCariKodu.Text;
                cari.VergiDairesi = txtVergiDairesi.Text;
                cari.VergiNo = txtVergiNo.Text;
                cari.Telefon1 = txtTel1.Text;
                cari.Telefon2 = txtTel2.Text;
                cari.WebAdresi = txtWeb.Text;
                cari.Mail = txtMail.Text;
                cari.Fax1 = txtFax1.Text;
                cari.Fax2 = txtFax2.Text;
                cari.GrupId = grupID;
                cari.Ulke = txtUlke.Text;
                cari.Sehir = txtSehir.Text;
                cari.Ilce = txtIlce.Text;
                cari.Yetkili1 = txtYetkili1.Text;
                cari.Yetkili2 = txtYetkili2.Text;
                cari.YetkiliEMail1 = txtYetkiliEMail1.Text;
                cari.YetkiliEMail2 = txtYetkiliEMail2.Text;
                cari.SaveDate = DateTime.Now;
                cari.SaveUser = AnaForm.UserID;

                DB.Caris.InsertOnSubmit(cari);
                DB.SubmitChanges();
                mesajlar.YeniKayit("Yeni Cari Kaydı Başarıyla Oluşturuldu");
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
                Fonksiyonlar.Cari cari = DB.Caris.First(s => s.CariID == cariID);
                cari.Adres = txtAdres.Text;
                cari.CariAdi = txtCariAdi.Text;
                cari.CariKodu = txtCariKodu.Text;
                cari.VergiDairesi = txtVergiDairesi.Text;
                cari.VergiNo = txtVergiNo.Text;
                cari.Telefon1 = txtTel1.Text;
                cari.Telefon2 = txtTel2.Text;
                cari.WebAdresi = txtWeb.Text;
                cari.Mail = txtMail.Text;
                cari.Fax1 = txtFax1.Text;
                cari.Fax2 = txtFax2.Text;
                cari.GrupId = grupID;
                cari.Ulke = txtUlke.Text;
                cari.Sehir = txtSehir.Text;
                cari.Ilce = txtIlce.Text;
                cari.Yetkili1 = txtYetkili1.Text;
                cari.Yetkili2 = txtYetkili2.Text;
                cari.YetkiliEMail1 = txtYetkiliEMail1.Text;
                cari.YetkiliEMail2 = txtYetkiliEMail2.Text;
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
                DB.Caris.DeleteOnSubmit(DB.Caris.First(s => s.CariID == cariID));
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
                edit = true;
                cariID = ID;
                Fonksiyonlar.Cari cari = DB.Caris.First(s => s.CariID == cariID);
                txtAdres.Text = cari.Adres;
                txtCariAdi.Text = cari.CariAdi;
                txtCariKodu.Text = cari.CariKodu;
                txtVergiDairesi.Text = cari.VergiDairesi;
                txtVergiNo.Text = cari.VergiNo;
                txtTel1.Text = cari.Telefon1;
                txtTel2.Text = cari.Telefon2;
                txtWeb.Text = cari.WebAdresi;
                txtMail.Text = cari.Mail;
                txtFax1.Text = cari.Fax1;
                txtFax2.Text = cari.Fax2;
                txtUlke.Text = cari.Ulke;
                txtSehir.Text = cari.Sehir;
                txtIlce.Text = cari.Ilce;
                txtYetkili1.Text = cari.Yetkili1;
                txtYetkili2.Text = cari.Yetkili2;
                txtYetkiliEMail1.Text = cari.YetkiliEMail1;
                txtYetkiliEMail2.Text = cari.YetkiliEMail2;
                GrupAc(cari.GrupId.Value);

            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        void GrupAc(int ID)
        {
            try
            {
                grupID = ID;
                Fonksiyonlar.CariGrup grup = DB.CariGrups.First(s => s.GrupID == grupID);
                txtGrupAdi.Text = grup.GrupAdi;
                txtGrupKodu.Text = grup.GrupKodu;
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit && cariID > 0 && mesajlar.Guncelle() == DialogResult.Yes)
            {
                Guncelle();
            }
            else
            {
                Kaydet();
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (edit && cariID > 0 && mesajlar.Sil() == DialogResult.Yes)
                Sil();
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCariKodu_Click(object sender, EventArgs e)
        {
            int ID = formlar.CariListesi(true); //Formlar class'ından
            if (ID > 0)
            {
                Ac(ID);
            }
            AnaForm.Aktarma = -1;
        }

        private void txtGrupKodu_Click(object sender, EventArgs e)
        {
            int ID = formlar.CariGruplari(true); //Formlar class'ından
            if (ID > 0)
            {
                GrupAc(ID);
            }
            AnaForm.Aktarma = -1;
        }
    }
}