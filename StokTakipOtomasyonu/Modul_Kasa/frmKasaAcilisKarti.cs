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
    public partial class frmKasaAcilisKarti : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();
        Fonksiyonlar.Numara numaralar = new Fonksiyonlar.Numara();

        bool edit = false;
        int secimID = -1;

        public frmKasaAcilisKarti()
        {
            InitializeComponent();
        }

        private void frmKasaAcilisKarti_Load(object sender, EventArgs e)
        {
            txtKasaKodu.Text = numaralar.KasaKodKodNumarasi();
            Listele();
        }

        void Temizle()
        {
            txtKasaKodu.Text = numaralar.KasaKodKodNumarasi();
            txtKasaAdi.Text = "";
            txtAciklama.Text = "";
            edit = false;
            secimID = -1;
            Listele();
        }

        void Kaydet()
        {
            try
            {
                Fonksiyonlar.Kasa kasa = new Fonksiyonlar.Kasa();
                kasa.Aciklama = txtAciklama.Text;
                kasa.KasaAdi = txtKasaAdi.Text;
                kasa.KasaKodu = txtKasaKodu.Text;
                kasa.SaveDate = DateTime.Now;
                kasa.SaveUser = AnaForm.UserID;

                DB.Kasas.InsertOnSubmit(kasa);
                DB.SubmitChanges();
                mesajlar.YeniKayit("Yeni Kasa Kaydı Başarıyla Oluşturulmuştur.");
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
                Fonksiyonlar.Kasa kasa = DB.Kasas.First(s => s.KasaID == secimID);
                kasa.Aciklama = txtAciklama.Text;
                kasa.KasaAdi = txtKasaAdi.Text;
                kasa.KasaKodu = txtKasaKodu.Text;
                kasa.EditDate = DateTime.Now;
                kasa.EditUser = AnaForm.UserID;
               
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
                DB.Kasas.DeleteOnSubmit(DB.Kasas.First(s => s.KasaID == secimID));
                DB.SubmitChanges();
                Temizle();
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
                edit = true; /* güncelleme sorunu için buraya bak*/
                secimID = int.Parse(gridView1.GetFocusedRowCellValue("KasaID").ToString());
                txtKasaKodu.Text = gridView1.GetFocusedRowCellValue("KasaKodu").ToString();
                txtKasaAdi.Text = gridView1.GetFocusedRowCellValue("KasaAdi").ToString();
                txtAciklama.Text = gridView1.GetFocusedRowCellValue("Aciklama").ToString();
            }
            catch (Exception)
            {
                edit = false;
                secimID = -1;
            }
        }

        void Listele()
        {
            var lst = from s in DB.Kasas
                      select s;
            Liste.DataSource = lst; // Liste, formdaki gridview'ın adı
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (txtKasaAdi.Text!="" &&txtAciklama.Text!="")
            {
                if (edit && secimID > 0 && mesajlar.Guncelle() == DialogResult.Yes)
                    Guncelle();
                else
                    Kaydet();
            }
            else
            {
                MessageBox.Show("Kasa Adı ve Açıklama Boş Bırakılamaz.", "İşlem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Temizle();
        }
    }
}