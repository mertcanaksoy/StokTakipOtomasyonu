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
    public partial class frmCariGruplari : DevExpress.XtraEditors.XtraForm
    {
        public bool secim = false;
        bool edit = false;
        int secimID = -1;

        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();

        public frmCariGruplari()
        {
            InitializeComponent();
        }

        void Sec()
        {
            try
            {
                edit = true;
                secimID = int.Parse(gridView1.GetFocusedRowCellValue("GrupID").ToString());
                txtGrupAdi.Text = gridView1.GetFocusedRowCellValue("GrupAdi").ToString();
                txtGrupKodu.Text = gridView1.GetFocusedRowCellValue("GrupKodu").ToString();
            }
            catch (Exception)
            {
                edit = false;
                secimID = -1;
            }
        }

        void Listele()
        {
            var lst = from s in DB.CariGrups
                      select s;
            Liste.DataSource = lst;
        }

        void Temizle()
        {
            txtGrupAdi.Text = "";
            txtGrupKodu.Text = "";
            edit = false;
            secimID = -1;
            Listele();
        }

        void Kaydet()
        {
            try
            {
                Fonksiyonlar.CariGrup Grup = new Fonksiyonlar.CariGrup();
                Grup.GrupAdi = txtGrupAdi.Text;
                Grup.GrupKodu = txtGrupKodu.Text;
                Grup.SaveDate = DateTime.Now;
                Grup.SaveUser = AnaForm.UserID;
                DB.CariGrups.InsertOnSubmit(Grup);
                DB.SubmitChanges();
                mesajlar.YeniKayit("Yeni Cari Grup Kaydı Başarıyla Oluşturuldu");
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
                Fonksiyonlar.CariGrup Grup = DB.CariGrups.First(s => s.GrupID == secimID);
                Grup.GrupAdi = txtGrupAdi.Text;
                Grup.GrupKodu = txtGrupKodu.Text;
                Grup.EditDate = DateTime.Now;
                Grup.EditUser = AnaForm.UserID;
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
                DB.CariGrups.DeleteOnSubmit(DB.CariGrups.First(s => s.GrupID == secimID));
                DB.SubmitChanges();
                Temizle();
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

        private void frmCariGruplari_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void frmCariGruplari_DoubleClick(object sender, EventArgs e)
        {
            //Bu alan yanlışlıkla açıldı.
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            Sec();
            if (secim && secimID > 0)
            {
                AnaForm.Aktarma = secimID;
                this.Close();
            }
        }
    }
}