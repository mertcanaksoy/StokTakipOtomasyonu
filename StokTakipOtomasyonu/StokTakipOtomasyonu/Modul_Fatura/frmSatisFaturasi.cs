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

namespace StokTakipOtomasyonu.Modul_Fatura
{
    public partial class frmSatisFaturasi : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();
        Fonksiyonlar.Mesajlar mesajlar = new Fonksiyonlar.Mesajlar();

        int cariId = -1;
        int odemeId = -1;
        int faturaId = -1;
        int irsaliyeId = -1;
        string odemeYeri = "";
        bool edit = false;
        bool irsaliyeAc = false;

        
        public frmSatisFaturasi(bool ac,int ID,bool irsaliye) 
        {
            InitializeComponent();
            edit = ac;
            if (irsaliye)
                irsaliyeId = ID;
            else
                faturaId = ID;
            irsaliyeAc = irsaliye;

            this.Shown += FrmSatisFaturasi_Shown;
        }

        private void FrmSatisFaturasi_Shown(object sender, EventArgs e) //Bu event sayesinde sayfa açıldıktan sonra 
        { //FaturaGetir() işlemini yapıyoruz. Önce yaparsak gridView1, instance alamayacağı için hata verir.
            if (edit)
                FaturaGetir();
        }

        void FaturaGetir()
        {
            try
            {
                Fonksiyonlar.Fatura fatura = DB.Faturas.First(s => s.FaturaID == faturaId);
                irsaliyeId = fatura.IrsaliyeId.Value;
                txtAciklama.Text = fatura.Aciklama;
                txtFaturaNo.Text = fatura.FaturaNo;
                if (fatura.OdemeYeriId > 0) //Fatura kapalı faturaysa
                {
                    txtFaturaTuru.SelectedIndex = 1; //Fatura türünü kapalı faturaya getirdik
                    if (fatura.OdemeYeri == "Kasa")
                    {                       
                        txtOdemeYeri.SelectedIndex = 0;
                        odemeYeri = fatura.OdemeYeri;

                        txtKasaAdi.Text = DB.Kasas.First(s => s.KasaID == fatura.OdemeYeriId.Value).KasaAdi;
                        txtKasaKodu.Text = DB.Kasas.First(s => s.KasaID == fatura.OdemeYeriId.Value).KasaKodu;
                    }
                    else if(fatura.OdemeYeri == "Banka")
                    {
                        txtOdemeYeri.SelectedIndex = 1;
                        odemeYeri = fatura.OdemeYeri;

                        txtHesapAdi.Text = DB.Bankas.First(s => s.BankaID == fatura.OdemeYeriId.Value).HesapAdi;
                        txtHesapNo.Text = DB.Bankas.First(s => s.BankaID == fatura.OdemeYeriId.Value).HesapNo;
                    }
                    odemeId = fatura.OdemeYeriId.Value;
                }
                else if (fatura.OdemeYeriId < 1)
                {
                    txtFaturaTuru.SelectedIndex = 0;
                }
                txtIrsaliyeNo.Text = DB.Irsaliyes.First(s => s.IrsaliyeID == fatura.IrsaliyeId).IrsaliyeNo;
                txtIrsaliyeTarihi.EditValue = DB.Irsaliyes.First(s => s.IrsaliyeID == fatura.IrsaliyeId).Tarih.Value.ToShortDateString();
                txtCariAdi.Text = DB.Caris.First(s => s.CariKodu == fatura.CariKodu).CariAdi;
                txtCariKodu.Text = fatura.CariKodu;
                txtFaturaTarihi.EditValue = fatura.Tarih.Value.ToShortDateString();
                var srg = from s in DB.View_Kalemlers
                          where s.FaturaId == faturaId
                          select s;
                foreach (Fonksiyonlar.View_Kalemler k in srg)
                {
                    gridView1.AddNewRow();
                    gridView1.SetFocusedRowCellValue("Miktar", k.Miktar);
                    gridView1.SetFocusedRowCellValue("BirimFiyat", k.BirimFiyat);
                    gridView1.SetFocusedRowCellValue("KDV", k.KDV);
                    gridView1.SetFocusedRowCellValue("Barkod", k.StokBarkod);
                    gridView1.SetFocusedRowCellValue("StokKodu", k.StokKodu);
                    gridView1.SetFocusedRowCellValue("StokAdi", k.StokAdi);
                    gridView1.SetFocusedRowCellValue("Birim", k.StokBirim);
                    gridView1.UpdateCurrentRow();
                }

            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        void StokGetir(int stokID)
        {
            try
            {
                Fonksiyonlar.Stok stok = DB.Stoks.First(s => s.StokID == stokID);
                gridView1.AddNewRow();
                gridView1.SetFocusedRowCellValue("Miktar", "0");
                gridView1.SetFocusedRowCellValue("Barkod", stok.StokBarkod);
                gridView1.SetFocusedRowCellValue("StokKodu", stok.StokKodu);
                gridView1.SetFocusedRowCellValue("StokAdi", stok.StokAdi);
                gridView1.SetFocusedRowCellValue("Birim", stok.StokBirim);
                gridView1.SetFocusedRowCellValue("BirimFiyat", stok.StokSatisFiyat);
                gridView1.SetFocusedRowCellValue("KDV", stok.StokSatisKDV);
            }
            catch (Exception e)
            {
                mesajlar.Hata(e);
            }
        }

        private void btnStokSec_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int stokID = formlar.StokListesi(true);
            if (stokID > 0)
            {
                StokGetir(stokID);
            }
            AnaForm.Aktarma = -1;
        }

        private void frmSatisFaturasi_Load(object sender, EventArgs e)
        {
            txtFaturaTarihi.Text = DateTime.Now.ToShortDateString();
            txtIrsaliyeTarihi.Text = DateTime.Now.ToShortDateString();
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                decimal miktar = 0, birimFiyat = 0, toplam = 0;

                if (e.Column.Name != "colToplam")
                {
                    miktar = decimal.Parse(gridView1.GetFocusedRowCellValue("Miktar").ToString());
                    if (gridView1.GetFocusedRowCellValue("BirimFiyat").ToString() != "")
                        birimFiyat = decimal.Parse(gridView1.GetFocusedRowCellValue("BirimFiyat").ToString());
                    toplam = miktar * birimFiyat;

                    gridView1.SetFocusedRowCellValue("Toplam", toplam.ToString());
                    Hesapla();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        void Hesapla()
        {
            try
            {
                decimal birimFiyat = 0, miktar = 0, genelToplam = 0, araToplam = 0, KDV = 0;
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    birimFiyat = decimal.Parse(gridView1.GetRowCellValue(i, "BirimFiyat").ToString());
                    miktar= decimal.Parse(gridView1.GetRowCellValue(i, "Miktar").ToString());
                    KDV = decimal.Parse(gridView1.GetRowCellValue(i, "KDV").ToString()) / 100 + 1;
                    araToplam += miktar * birimFiyat;
                    genelToplam += decimal.Parse(gridView1.GetRowCellValue(i, "Toplam").ToString()) * KDV;
                }

                txtAraToplam.Text = araToplam.ToString("0.00");
                txtKDV.Text = (genelToplam - araToplam).ToString("0.00");
                txtGenelToplam.Text = genelToplam.ToString("0.00");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void Temizle()
        {
            cariId = -1;
            faturaId = -1;
            irsaliyeId = -1;
            odemeId = -1;
            odemeYeri = "";
            edit = false;
            irsaliyeAc = false;
            txtAciklama.Text = "";
            txtAraToplam.Text = "0.00";
            txtCariAdi.Text = "";
            txtCariKodu.Text = "";
            txtFaturaNo.Text = "";
            txtFaturaTarihi.Text = DateTime.Now.ToShortDateString();
            txtFaturaTuru.SelectedIndex = 0;
            txtGenelToplam.Text = "0.00";
            txtHesapAdi.Text = "";
            txtHesapNo.Text = "";
            txtIrsaliyeNo.Text = "";
            txtIrsaliyeTarihi.Text = DateTime.Now.ToShortDateString();
            txtKasaAdi.Text = "";
            txtKasaKodu.Text = "";
            txtKDV.Text = "0.00";
            txtOdemeYeri.SelectedIndex = 0;
            AnaForm.Aktarma = -1;

            for (int i = gridView1.RowCount; i > -1; i--) 
            {
                gridView1.DeleteRow(i);
            }
        }

        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            Hesapla();
        }

        private void txtCariKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = formlar.CariListesi(true);
            if (Id > 0)
                CariSec(Id);
            AnaForm.Aktarma = -1;
        }

        void CariSec(int ID)
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

        private void txtFaturaTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtFaturaTuru.SelectedIndex == 0)
            {
                pnlHesapKasa.Enabled = false;
                txtOdemeYeri.Enabled = false;
            }
                
            if (txtFaturaTuru.SelectedIndex == 1)
            {
                pnlHesapKasa.Enabled = true;
                txtOdemeYeri.Enabled = true;
            }
                
        }

        private void txtOdemeYeri_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtOdemeYeri.SelectedIndex == 0) //Kasa seçiliyse
            {
                txtHesapAdi.Enabled = false;
                txtHesapNo.Enabled = false;
                txtKasaAdi.Enabled = true;
                txtKasaKodu.Enabled = true;
                odemeYeri = txtOdemeYeri.Text;
            }
            if (txtOdemeYeri.SelectedIndex == 1) //Banka seçiliyse
            {
                txtHesapAdi.Enabled = true;
                txtHesapNo.Enabled = true;
                txtKasaAdi.Enabled = false;
                txtKasaKodu.Enabled = false;
                odemeYeri = txtOdemeYeri.Text;
            }
        }

        void FaturaKaydet()
        {
            try
            {
                Fonksiyonlar.Fatura fatura = new Fonksiyonlar.Fatura();
                fatura.Aciklama = txtAciklama.Text;
                fatura.CariKodu = txtCariKodu.Text;
                fatura.FaturaNo = txtFaturaNo.Text;
                fatura.FaturaTuru = "Satış Faturası";
                fatura.GenelToplam = decimal.Parse(txtGenelToplam.Text);
                fatura.IrsaliyeId = irsaliyeId;
                fatura.OdemeYeri = txtOdemeYeri.SelectedItem.ToString();
                fatura.OdemeYeriId = odemeId;
                fatura.Tarih = DateTime.Parse(txtFaturaTarihi.Text);
                fatura.SaveDate = DateTime.Now;
                fatura.SaveUser = AnaForm.UserID;
                DB.Faturas.InsertOnSubmit(fatura);
                DB.SubmitChanges();
                faturaId = fatura.FaturaID;

                if (irsaliyeId < 0)
                {
                    Fonksiyonlar.Irsaliye irsaliye = new Fonksiyonlar.Irsaliye();
                    irsaliye.Aciklama = txtAciklama.Text;
                    irsaliye.CariKodu = txtCariKodu.Text;
                    irsaliye.FaturaId = fatura.FaturaID;
                    irsaliye.IrsaliyeNo = txtIrsaliyeNo.Text;
                    irsaliye.Tarih = DateTime.Parse(txtIrsaliyeTarihi.Text);
                    irsaliye.SaveDate = DateTime.Now;
                    irsaliye.SaveUser = AnaForm.UserID;
                    DB.Irsaliyes.InsertOnSubmit(irsaliye);
                    DB.SubmitChanges();
                    irsaliyeId = irsaliye.IrsaliyeID;
                    fatura.IrsaliyeId = irsaliyeId;
                }

                Fonksiyonlar.StokHareketleri[] stokHareketleri = new Fonksiyonlar.StokHareketleri[gridView1.RowCount];
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    stokHareketleri[i] = new Fonksiyonlar.StokHareketleri();
                    stokHareketleri[i].BirimFiyat = decimal.Parse(gridView1.GetRowCellValue(i, "BirimFiyat").ToString());
                    stokHareketleri[i].FaturaId = faturaId;
                    stokHareketleri[i].GCKodu = "C";
                    stokHareketleri[i].IrsaliyeId = irsaliyeId;
                    stokHareketleri[i].KDV = decimal.Parse(gridView1.GetRowCellValue(i, "KDV").ToString());
                    stokHareketleri[i].Miktar = int.Parse(gridView1.GetRowCellValue(i, "Miktar").ToString());
                    stokHareketleri[i].StokKodu = gridView1.GetRowCellValue(i, "StokKodu").ToString();
                    stokHareketleri[i].Tipi = "Satış Faturası";
                    stokHareketleri[i].SaveDate = DateTime.Now;
                    stokHareketleri[i].SaveUser = AnaForm.UserID;
                    DB.StokHareketleris.InsertOnSubmit(stokHareketleri[i]);
                }
                DB.SubmitChanges();
                Fonksiyonlar.CariHareketleri cariHareket = new Fonksiyonlar.CariHareketleri();
                cariHareket.Aciklama = txtFaturaNo.Text + " Numaralı satış faturası tutarı";

                if (txtFaturaTuru.SelectedIndex == 0)
                {
                    cariHareket.Alacak = 0;
                    cariHareket.Borc = decimal.Parse(txtGenelToplam.Text);
                }
                else if(txtFaturaTuru.SelectedIndex == 1)
                {
                    cariHareket.Alacak = decimal.Parse(txtGenelToplam.Text);
                    cariHareket.Borc = decimal.Parse(txtGenelToplam.Text);
                }
                cariHareket.CariId = cariId;
                cariHareket.Tarih = DateTime.Now;
                cariHareket.Tipi = "SF";
                cariHareket.EvrakTuru = "Satış Faturası";
                cariHareket.EvrakId = fatura.FaturaID;
                cariHareket.SaveDate = DateTime.Now;
                cariHareket.SaveUser = AnaForm.UserID;
                DB.CariHareketleris.InsertOnSubmit(cariHareket);
                DB.SubmitChanges();
                mesajlar.YeniKayit("Yeni Fatura Kaydı Başarıyla Yapıldı");
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
                Fonksiyonlar.Fatura fatura = DB.Faturas.First(s => s.FaturaID == faturaId);
                fatura.FaturaNo = txtFaturaNo.Text;
                fatura.Aciklama = txtAciklama.Text;
                fatura.CariKodu = txtCariKodu.Text;
                fatura.GenelToplam = decimal.Parse(txtGenelToplam.Text);
                fatura.OdemeYeri = txtOdemeYeri.Text;
                fatura.OdemeYeriId = odemeId;
                fatura.EditDate = DateTime.Now;
                fatura.EditUser = AnaForm.UserID;
                DB.SubmitChanges();

                Fonksiyonlar.Irsaliye irsaliye = DB.Irsaliyes.First(s => s.IrsaliyeID == irsaliyeId);
                irsaliye.IrsaliyeNo = txtIrsaliyeNo.Text;
                irsaliye.Tarih = DateTime.Parse(txtIrsaliyeTarihi.SelectedText);
                irsaliye.EditDate = DateTime.Now;
                irsaliye.EditUser = AnaForm.UserID;
                DB.StokHareketleris.DeleteAllOnSubmit(DB.StokHareketleris.Where(s => s.FaturaId == faturaId));
                DB.SubmitChanges();

                Fonksiyonlar.StokHareketleri[] stokHareketi = new Fonksiyonlar.StokHareketleri[gridView1.RowCount];
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    stokHareketi[i] = new Fonksiyonlar.StokHareketleri();
                    stokHareketi[i].FaturaId = faturaId;
                    stokHareketi[i].BirimFiyat = decimal.Parse(gridView1.GetRowCellValue(i, "BirimFiyat").ToString());
                    stokHareketi[i].GCKodu = "C";
                    stokHareketi[i].IrsaliyeId = irsaliyeId;
                    stokHareketi[i].KDV = decimal.Parse(gridView1.GetRowCellValue(i, "KDV").ToString());
                    stokHareketi[i].Miktar = int.Parse(gridView1.GetRowCellValue(i, "Miktar").ToString());
                    stokHareketi[i].StokKodu = gridView1.GetRowCellValue(i, "StokKodu").ToString();
                    stokHareketi[i].Tipi = "Satış Faturası";
                    stokHareketi[i].SaveDate = DateTime.Now;
                    stokHareketi[i].SaveUser = AnaForm.UserID;
                    DB.StokHareketleris.InsertOnSubmit(stokHareketi[i]);
                }
                DB.SubmitChanges();

                Fonksiyonlar.CariHareketleri cariHareket = DB.CariHareketleris.First(s => s.EvrakTuru == "Satış Faturası" && s.EvrakId == faturaId);
                if (txtFaturaTuru.SelectedIndex==0)
                {
                    cariHareket.Alacak = 0;
                    cariHareket.Borc = decimal.Parse(txtGenelToplam.Text);
                }
                else if(txtFaturaTuru.SelectedIndex == 1)
                {
                    cariHareket.Alacak = decimal.Parse(txtGenelToplam.Text);
                    cariHareket.Borc = decimal.Parse(txtGenelToplam.Text);
                }
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

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit && faturaId > 0)
                Guncelle();
            else
                FaturaKaydet();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {

        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtOdemeYeri_EnabledChanged(object sender, EventArgs e)
        {
            if (txtOdemeYeri.Enabled)
            {
                odemeYeri = txtOdemeYeri.Text;
            }
            else if (!txtOdemeYeri.Enabled)
            {
                odemeYeri = "";
            }
        }
    }
}