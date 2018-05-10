using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Data.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace StokTakipOtomasyonu.Modul_Stok
{
    public partial class frmStokGruplari : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Mesajlar Mesajlar = new Fonksiyonlar.Mesajlar();

        public bool Secim = false;
        int secimID = -1;
        bool edit = false;


        public frmStokGruplari()
        {
            InitializeComponent();
        }

        private void frmStokGruplari_Load(object sender, EventArgs e)
        {
            Listele();
        }

        void Listele()
        {
            var lst = from s in DB.StokGrups
                      select s;
            Liste.DataSource = lst;

        }

        void Temizle()
        {
            txtGrupKodu.Text = "";
            txtGrupAdi.Text = "";
            edit = false;
            Listele();
        }

        void YeniKayit()
        {
            try
            {
                Fonksiyonlar.StokGrup Grup = new Fonksiyonlar.StokGrup();
                Grup.GrupAdi = txtGrupAdi.Text;
                Grup.GrupKodu = txtGrupKodu.Text;
                Grup.GrupSaveDate = DateTime.Now;
                Grup.GrupSaveUser = AnaForm.UserID;

                DB.StokGrups.InsertOnSubmit(Grup);
                DB.SubmitChanges();

                Mesajlar.YeniKayit("Yeni Grup Kaydı Oluşturuldu");
                Temizle();
            }
            catch (Exception e)
            {
                Mesajlar.Hata(e);
            }
        }

        void Guncelle()
        {
            try
            {
                Fonksiyonlar.StokGrup Grup = DB.StokGrups.First(x => x.GrupID == secimID);
                Grup.GrupKodu = txtGrupKodu.Text;
                Grup.GrupAdi = txtGrupAdi.Text;
                Grup.GrupEditUser = AnaForm.UserID;
                Grup.GrupEditDate = DateTime.Now;
                DB.SubmitChanges();
                Mesajlar.Guncelle(true);
                Temizle();
            }
            catch (Exception e)
            {
                Mesajlar.Hata(e);
            }
        }

        void Sil()
        {
            try
            {
                DB.StokGrups.DeleteOnSubmit(DB.StokGrups.First(x => x.GrupID == secimID));
                DB.SubmitChanges();
                Mesajlar.Sil("Grup Kaydı Silindi");
                Temizle();
            }
            catch (Exception e)
            {
                Mesajlar.Hata(e);
            }
        }

        void Sec()
        {
            try
            {
                edit = true;
                secimID=int.Parse(gridView1.GetFocusedRowCellValue("GrupID").ToString());
                txtGrupAdi.Text = gridView1.GetFocusedRowCellValue("GrupAdi").ToString();
                txtGrupKodu.Text = gridView1.GetFocusedRowCellValue("GrupKodu").ToString();
            }
            catch (Exception)
            {
                edit = false;
                secimID = -1;
            }
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (edit && secimID > 0 && Mesajlar.Sil() == DialogResult.Yes)
                Sil();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit && secimID > 0 && Mesajlar.Guncelle() == DialogResult.Yes)
                Guncelle();
            else
                YeniKayit();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            Sec();
            if (Secim && secimID > 0)
            {
                AnaForm.Aktarma = secimID;
                this.Close();
            }
        }
    }
}