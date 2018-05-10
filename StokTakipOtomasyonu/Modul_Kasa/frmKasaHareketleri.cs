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
    public partial class frmKasaHareketleri : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();

        int kasaID = -1;
        int evrakID = -1;
        int hareketID = -1;
        string evrakTuru;

        public frmKasaHareketleri()
        {
            InitializeComponent();
        }

        private void frmKasaHareketleri_Load(object sender, EventArgs e)
        {

        }

        void Listele()
        {
            var lst = from s in DB.View_KasaHareketleris
                      where s.KasaId == kasaID
                      select s;
            Liste.DataSource = lst;
        }

        void DurumGetir()
        {
            Fonksiyonlar.View_KasaDurum kasa = DB.View_KasaDurums.First(s => s.KasaId == kasaID);
            txtGiris.Text = kasa.Giris.Value.ToString();
            txtCikis.Text = kasa.Cikis.Value.ToString();
            txtBakiye.Text = kasa.Bakiye.Value.ToString();
        }

        public void Ac(int ID)
        {
            try
            {
                kasaID = ID;
                txtKasaKodu.Text = DB.Kasas.First(s => s.KasaID == kasaID).KasaKodu;
                txtKasaAdi.Text = DB.Kasas.First(s => s.KasaID == kasaID).KasaAdi;
                DurumGetir();
                Listele();
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
                hareketID = int.Parse(gridView1.GetFocusedRowCellValue("KasaHareketID").ToString());
                try
                {
                    evrakID = int.Parse(gridView1.GetFocusedRowCellValue("EvrakID").ToString());
                }
                catch (Exception)
                {
                    evrakID = -1;
                }
                evrakTuru = gridView1.GetFocusedRowCellValue("EvrakTuru").ToString();
            }
            catch (Exception)
            {
                hareketID = -1;
                evrakID = -1;
                evrakTuru = "";
            }
        }

        private void txtKasaKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int ID = formlar.KasaListesi(true);
            if (ID > 0)
            {
                Ac(ID);
                AnaForm.Aktarma = -1;
            }
        }

        private void sagTik_Opening(object sender, CancelEventArgs e)
        {
            Sec();
            if (evrakTuru == "Kasa Devir Kartı")
            {
                devirKartiDuzenle.Enabled = true;
                tahsilatOdemeDuzenle.Enabled = false;
            }
            else if (evrakTuru == "Kasa Tahsilat" || evrakTuru == "Kasa Ödeme") 
            {
                devirKartiDuzenle.Enabled = false;
                tahsilatOdemeDuzenle.Enabled = true;
            }
        }

        private void gridView1_Click(object sender, EventArgs e)
        {

        }

        private void devirKartiDuzenle_Click(object sender, EventArgs e)
        {
            formlar.KasaDevirIslemKarti(true, hareketID);
            Listele();
            DurumGetir();
        }

        private void tahsilatOdemeDuzenle_Click(object sender, EventArgs e)
        {
            formlar.KasaTahsilatOdemeKarti(true, hareketID);
            Listele();
            DurumGetir();
        }
    }
}