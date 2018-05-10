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
    public partial class frmBankaAcilisKarti : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();

        bool edit = false;
        int secimID = -1;

        public frmBankaAcilisKarti()
        {
            InitializeComponent();
        }

        private void frmBankaAcilisKarti_Load(object sender, EventArgs e)
        {
            Listele();
        }

        void Temizle()
        {
            txtAdres.Text = "";
            txtBankaAdi.Text = "";
            txtHesapNo.Text = "";
            txtHesapAdi.Text = "";
            txtIBAN.Text = "";
            txtSube.Text = "";
            txtTelefon.Text = "";
            txtTemsilci.Text = "";
            txtTemsilciEMail.Text = "";
            edit = false;
            secimID = -1;
            Listele();
        }

        void Listele()
        {
            var lst = from s in DB.Bankas
                      select s;
            Liste.DataSource = lst;
        }

        void Kaydet()
        {
            try
            {
                Fonksiyonlar.Banka banka = new Fonksiyonlar.Banka();
                banka.Adres = txtAdres.Text;
                banka.BankaAdi = txtBankaAdi.Text;
                banka.HesapAdi = txtHesapAdi.Text;
                banka.HesapNo = txtHesapNo.Text;
                banka.IBAN = txtIBAN.Text;
                banka.Sube = txtSube.Text;
                banka.Telefon = txtTelefon.Text;
                banka.Temsilci = txtTemsilci.Text;
                banka.TemsilciEMail = txtTemsilciEMail.Text;
                banka.SaveDate = DateTime.Now;
                banka.SaveUser = AnaForm.UserID;
                DB.Bankas.InsertOnSubmit(banka);
                DB.SubmitChanges();
                mesajlar.YeniKayit("Yeni Banka Kaydı Başarıyla Açıldı");
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
                Fonksiyonlar.Banka banka = DB.Bankas.First(s => s.BankaID == secimID);
                banka.Adres = txtAdres.Text;
                banka.BankaAdi = txtBankaAdi.Text;
                banka.HesapAdi = txtHesapAdi.Text;
                banka.HesapNo = txtHesapNo.Text;
                banka.IBAN = txtIBAN.Text;
                banka.Sube = txtSube.Text;
                banka.Telefon = txtTelefon.Text;
                banka.Temsilci = txtTemsilci.Text;
                banka.TemsilciEMail = txtTemsilciEMail.Text;
                banka.EditDate = DateTime.Now;
                banka.EditUser = AnaForm.UserID;
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
                DB.Bankas.DeleteOnSubmit(DB.Bankas.First(s => s.BankaID == secimID));
                DB.SubmitChanges();
                Temizle();
                mesajlar.Sil();
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
                edit = true;
                secimID = int.Parse(gridView1.GetFocusedRowCellValue("BankaID").ToString());
                if (secimID > 0)
                    Ac();
            }
            catch (Exception)
            {
                edit = false;
                secimID = -1;
            }
        }

        void Ac()
        {
            try
            {
                Fonksiyonlar.Banka banka = DB.Bankas.First(s => s.BankaID == secimID);
                txtAdres.Text = banka.Adres;
                txtBankaAdi.Text = banka.BankaAdi;
                txtHesapAdi.Text = banka.HesapAdi;
                txtHesapNo.Text = banka.HesapNo;
                txtIBAN.Text = banka.IBAN;
                txtSube.Text = banka.Sube;
                txtTelefon.Text = banka.Telefon;
                txtTemsilci.Text = banka.Temsilci;
                txtTemsilciEMail.Text = banka.TemsilciEMail;
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit && secimID > 0 && mesajlar.Guncelle() == DialogResult.Yes)
                Guncelle();
            else
                Kaydet();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (edit && secimID > 0 && mesajlar.Sil() == DialogResult.Yes)
                Sil();
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            Sec();
        }
    }
}