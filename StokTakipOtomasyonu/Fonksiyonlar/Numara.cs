using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakipOtomasyonu.Fonksiyonlar
{
    class Numara
    {
        DatabaseDataContext DB = new DatabaseDataContext();
        Mesajlar mesajlar = new Mesajlar();

        public string StokKodKodNumarasi()
        {
            try
            {
                int numara = int.Parse((from s in DB.Stoks
                                        orderby s.StokID descending
                                        select s).First().StokKodu);
                numara++;
                string num = numara.ToString().PadLeft(7, '0');
                return num;
            }
            catch (Exception)
            {
                return "0000001";
            }
        }

        public string CariKodKodNumarasi()
        {
            try
            {
                int numara = int.Parse((from s in DB.Caris
                                        orderby s.CariID descending
                                        select s).First().CariKodu);
                numara++;
                string num = numara.ToString().PadLeft(7, '0');
                return num;
            }
            catch (Exception)
            {
                return "0000001";
            }
        }

        public string KasaKodKodNumarasi()
        {
            try
            {
                int numara = int.Parse((from s in DB.Kasas
                                        orderby s.KasaID descending
                                        select s).First().KasaKodu);
                numara++;
                string num = numara.ToString().PadLeft(7, '0');
                return num;
            }
            catch (Exception)
            {
                return "0000001";
            }
        }
    }
}
