using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakipOtomasyonu.Fonksiyonlar
{
    class ResimYukleme
    {
        public byte[] ResimYukle(Image Resim)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                Resim.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public Image ResimGetirme(byte[] GelenByteArray)
        {
            using(MemoryStream ms=new MemoryStream(GelenByteArray))
            {
                Image resim = Image.FromStream(ms);
                return resim;
            }
        }

    }
}
