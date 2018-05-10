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
    public partial class frmFaturaListesi : DevExpress.XtraEditors.XtraForm
    {
        Fonksiyonlar.DatabaseDataContext DB = new Fonksiyonlar.DatabaseDataContext();
        Fonksiyonlar.Formlar formlar = new Fonksiyonlar.Formlar();

        bool secim = false;

        public frmFaturaListesi(bool Secim)
        {
            InitializeComponent();
            secim = Secim;
        }

        private void frmFaturaListesi_Load(object sender, EventArgs e)
        {
            Listele();
        }

        void Listele()
        {
            var lst = from s in DB.Faturas
                      where s.FaturaTuru.Contains(txtFaturaTuru.Text) && s.FaturaNo.Contains(txtFaturaNo.Text)
                      select s;
            Liste.DataSource = lst;
        }

        private void txtFaturaTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            Listele();
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(gridView1.GetFocusedRowCellValue("FaturaID").ToString());
            if (ID > 0)
            {
                formlar.Fatura(true, ID, false);
            }
        }

        private void sagTikYenile_Click(object sender, EventArgs e)
        {
            Listele();
        }
    }
}