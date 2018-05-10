using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace StokTakipOtomasyonu.Fonksiyonlar
{
    class Mesajlar
    {
        AnaForm MesajForm = new AnaForm();

        public void YeniKayit(string Mesaj)
        {
            MesajForm.Mesaj("Yeni Kayıt Girişi", Mesaj);
        }

        public DialogResult Guncelle()
        {
            return MessageBox.Show("Seçili olan kayıt kalıcı olarak güncellenecektir\n Güncelleme işlemini onaylıyor musunuz?", "Güncelleme işlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public DialogResult Sil()
        {
            return MessageBox.Show("Seçili olan kayıt kalıcı olarak silinecektir.\n Silme işlemini onaylıyor musunuz?","Silme İşlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public void Guncelle(bool Guncelleme)
        {
            MesajForm.Mesaj("Kayıt Güncelleme", "Kayıt güncellenmiştir");          
        }

        public void Sil(string Mesaj)
        {
            MessageBox.Show(Mesaj, "Kayıt Silindi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void Hata(Exception Hata)
        {
            MesajForm.Mesaj("Hata Oluştu", Hata.Message);
        }

        public void FormAcilis(string FormAdi)
        {
            MesajForm.Mesaj("", FormAdi + " Formu Açıldı");
        }

        public void FormKpanis(string FormAdi)
        {
            MesajForm.Mesaj("", FormAdi + " Formu Kapatıldı");
        }
    }
}
