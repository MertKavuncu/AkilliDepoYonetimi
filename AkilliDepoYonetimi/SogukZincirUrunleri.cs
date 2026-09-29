using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoYonetimi
{
    internal class SogukZincirUrunleri : Urunler
    {
        public double UrunSogutmaMasraf { get; set; }

        public SogukZincirUrunleri(int urunId, string urunIsim, double urunFiyat, int urunAdet, double urunSogutmaMasraf) : base(urunId, urunIsim, urunFiyat, urunAdet)
        {
            if (urunSogutmaMasraf < 0)
            {
                throw new Exception("Hata: Soğutma masrafı negatif olamaz!");
            }

            UrunSogutmaMasraf = urunSogutmaMasraf;
        }

        public override double SonFiyatHesapla()
        {
            return UrunFiyat + UrunSogutmaMasraf;
        }
    }
}
