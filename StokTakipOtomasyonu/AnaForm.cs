using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace StokTakipOtomasyonu
{
    public partial class AnaForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();

        public static int UserID = -1;
        public static int Aktarma = -1;

        public AnaForm()
        {
            InitializeComponent();
        }

        private void AnaForm_Load(object sender, EventArgs e)
        {

        }

        public void Mesaj(string Baslik, string Mesaj)
        {
            ALC.Show(this, Baslik, Mesaj);
        }

        #region Stok Butonları
        private void barBtnStokKarti_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.StokKarti();
        }

        private void barBtnStokListesi_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.StokListesi();
        }

        private void barBtnStokGrup_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.StokGruplari();
        }

        private void barBtnStokHareketleri_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.StokHareketleri();
        }

        private void navBtnStokKarti_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.StokKarti();
        }

        private void navBtnStokListesi_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.StokListesi();
        }

        private void navBtnStokGruplari_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.StokGruplari();
        }

        private void navBtnStokHareketleri_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.StokHareketleri();
        }
        #endregion

        #region Cari Butonları
        private void barBtnCariAcilisKarti_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.CariAcilisKarti();
        }

        private void barBtnCariGruplari_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.CariGruplari();
        }

        private void barBtnCariListesi_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.CariListesi();
        }

        private void barBtnCariHareketleri_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void navBtnCariAcilisKarti_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.CariAcilisKarti();
        }

        private void navBtnCariGruplari_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.CariGruplari();
        }

        private void navBtnCariListesi_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.CariListesi();
        }

        private void navBtnCariHareketleri_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

        }
        #endregion

        #region Banka Butonları
        private void barBtnBankaAcilisKarti_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.BankaAcilisKarti();
        }

        private void barBtnBankaIslemi_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.BankaIslem();
        }

        private void barBtnBankaListesi_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.BankaListesi();
        }

        private void barBtnParaTransferi_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.BankaParaTransfer();
        }

        private void barBtnBankaHareketleri_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.BankaHareketleri();
        }

        private void navBtnBankaAcilisKarti_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.BankaAcilisKarti();
        }

        private void navBtnParaTransferi_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.BankaParaTransfer();
        }

        private void navBtnBankaListesi_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.BankaListesi();
        }

        private void navBtnBankaIslemi_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.BankaIslem();
        }

        private void navBtnBankaHareketleri_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.BankaHareketleri();
        }
        #endregion

        #region Kasa Butonları
        private void barBtnKasaAcilisKarti_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.KasaAcilisKarti();
        }

        private void barBtnKasaListesi_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.KasaListesi();
        }

        private void barBtnKasaDevirIslemKarti_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.KasaDevirIslemKarti();
        }

        private void barBtnKasaTahsilatOdeme_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.KasaTahsilatOdemeKarti();
        }

        private void barBtnKasaHareketleri_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.KasaHareketleri();
        }

        private void navBtnKasaAcilisKarti_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.KasaAcilisKarti();
        }

        private void navBtnKasaListesi_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.KasaListesi();
        }

        private void navBtnKasaDevirIslemKarti_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.KasaDevirIslemKarti();
        }

        private void navBtnKasaTahsilatOdeme_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.KasaTahsilatOdemeKarti();
        }

        private void navBtnKasaHareketleri_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            formlar.KasaHareketleri();
        }
        #endregion

        private void barBtnMusteriCeki_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.MusteriCeki();
        }

        private void barBtnKendiCeklerimiz_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.KendiCekimiz();
        }

        private void barBtnBankayaCekCikisi_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.BankayaCekCikisi();
        }

        private void barBtnCariyeCekCikisi_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.CariyeCekCikisi();
        }

        private void barBtnCekListesi_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.CekListesi();
        }

        private void AnaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void barBtnSatisFaturasi_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.Fatura();
        }

        private void barBtnFaturaListesi_ItemClick(object sender, ItemClickEventArgs e)
        {
            formlar.FaturaListesi();
        }
    }
}