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

namespace StokTakipOtomasyonu.Modul_Stok
{
    public partial class frmStokKarti : DevExpress.XtraEditors.XtraForm
    {

        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();
        Fonksiyonlar.Numara numaralar = new Fonksiyonlar.Numara();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();
        Fonksiyonlar.ResimYukleme resimYukle = new Fonksiyonlar.ResimYukleme();

        bool edit = false;
        bool resimSec = false;
        OpenFileDialog Dosya = new OpenFileDialog();
        int grupID = -1;
        int stokID = -1;

        public frmStokKarti()
        {
            InitializeComponent();
        }

        private void frmStokKarti_Load(object sender, EventArgs e)
        {
            txtStokKodu.Text = numaralar.StokKodKodNumarasi();
        }

        void Temizle()
        {
            txtStokKodu.Text = numaralar.StokKodKodNumarasi();
            txtStokAdi.Text = "";
            txtSatisKDV.Text = "0";
            txtSatisFiyat.Text = "0";
            txtGrupKodu.Text = "";
            txtGrupAdi.Text = "";
            txtBirim.SelectedIndex = 0;
            txtBarkod.Text = "";
            txtAlisKDV.Text = "0";
            txtAlisFiyat.Text = "0";
            pictureBox1.Image = null;
            edit = false;
            resimSec = false;
            grupID = -1;
            stokID = -1;
            AnaForm.Aktarma = -1;

        }

        void ResimSec()
        {
            Dosya.Filter = "Jpg(*.jpg)|*.jpg|Jpeg(*.jpeg)|*.jpeg";
            if (Dosya.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = Dosya.FileName;
                resimSec = true;
            }
        }

        void Kaydet()
        {
            try
            {
                Fonksiyonlar.Stok stok = new Fonksiyonlar.Stok();
                stok.StokAdi = txtStokAdi.Text;
                stok.StokAlisFiyat = decimal.Parse(txtAlisFiyat.Text);
                stok.StokAlisKDV = decimal.Parse(txtAlisKDV.Text);
                stok.StokBarkod = txtBarkod.Text;
                stok.StokBirim = txtBirim.Text;
                stok.StokGrupId = grupID;
                stok.StokKodu = txtStokKodu.Text;
                if(resimSec) stok.StokResim = new System.Data.Linq.Binary(resimYukle.ResimYukle(pictureBox1.Image));
                stok.StokSatisFiyat = decimal.Parse(txtSatisFiyat.Text);
                stok.StokSatisKDV = decimal.Parse(txtSatisKDV.Text);
                stok.StokSaveDate = DateTime.Now;
                stok.StokSaveUser = AnaForm.UserID;
                DB.Stoks.InsertOnSubmit(stok);
                DB.SubmitChanges();
                mesajlar.YeniKayit("Yeni Stok Kaydı Oluşturuldu!");
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
                Fonksiyonlar.Stok stok = DB.Stoks.First(x => x.StokID == stokID);
                stok.StokAdi = txtStokAdi.Text;
                stok.StokAlisFiyat = decimal.Parse(txtAlisFiyat.Text);
                stok.StokAlisKDV = decimal.Parse(txtAlisKDV.Text);
                stok.StokBarkod = txtBarkod.Text;
                stok.StokBirim = txtBirim.Text;
                stok.StokGrupId = grupID;
                stok.StokKodu = txtStokKodu.Text;
                if(resimSec)    stok.StokResim = new System.Data.Linq.Binary(resimYukle.ResimYukle(pictureBox1.Image));
                stok.StokSatisFiyat = decimal.Parse(txtSatisFiyat.Text);
                stok.StokSatisKDV = decimal.Parse(txtSatisKDV.Text);
                stok.StokEditDate = DateTime.Now;
                stok.StokEditUser = AnaForm.UserID;
                DB.SubmitChanges();
                mesajlar.Guncelle(true);
                Temizle();
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        public void Sil()
        {
            try
            {
                DB.Stoks.DeleteOnSubmit(DB.Stoks.First(x => x.StokID == stokID));
                DB.SubmitChanges();
                Temizle();
            }
            catch (Exception e)
            {
                
            }
        }

        public void Ac(int ID)
        {
            edit = true;
            stokID = ID;
            Fonksiyonlar.Stok stok = DB.Stoks.First(x => x.StokID == stokID);
            GrupAc(stok.StokGrupId.Value);
            if(resimSec) //Burayı sonradan ekledim
            pictureBox1.Image = resimYukle.ResimGetirme(stok.StokResim.ToArray());
            txtAlisFiyat.Text = stok.StokAlisFiyat.ToString();
            txtAlisKDV.Text = stok.StokAlisKDV.ToString();
            txtBarkod.Text = stok.StokBarkod;
            txtBirim.Text = stok.StokBirim;
            txtSatisFiyat.Text = stok.StokSatisFiyat.ToString();
            txtSatisKDV.Text = stok.StokSatisKDV.ToString();
            txtStokAdi.Text = stok.StokAdi;
            txtStokKodu.Text = stok.StokKodu;
        }

        void GrupAc(int ID)
        {
            grupID = ID;
            txtGrupAdi.Text = DB.StokGrups.First(x => x.GrupID == grupID).GrupAdi;
            txtGrupKodu.Text= DB.StokGrups.First(x => x.GrupID == grupID).GrupKodu;
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            ResimSec();
        }

        private void txtStokKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        { //StokKodu'na, Events'den ButtonClick eventi eklendi
            int ID = formlar.StokListesi(true);
            if (ID > 0)
            {
                Ac(ID);
            }
            AnaForm.Aktarma = -1;
        }

        private void txtGrupKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {//GrupKodu'na, Events'den ButtonClick eventi eklendi
            int ID = formlar.StokGruplari(true);
            if (ID > 0)
            {
                GrupAc(ID);
            }
            AnaForm.Aktarma = -1;
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (edit && stokID > 0 && mesajlar.Sil() == DialogResult.Yes)
                Sil();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit && stokID > 0 && mesajlar.Guncelle() == DialogResult.Yes)
            {
                Guncelle();
            }
            else
            {
                Kaydet();
            }
        }
    }
}