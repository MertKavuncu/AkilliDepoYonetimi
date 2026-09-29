using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoYonetimi
{
    internal class HassasKargoUrunleri : Urunler
    {
        public double HassasUrunMasraf { get; set; }

        public HassasKargoUrunleri(int urunId, string urunIsim, double urunFiyat, int urunAdet, double hassasUrunMasraf) : base( urunId,  urunIsim,  urunFiyat, urunAdet)
        {
            if (hassasUrunMasraf < 0)
            {
                throw new Exception("Hata: Hassas ürün masrafı negatif olamaz!");
            }

            HassasUrunMasraf = hassasUrunMasraf;
        }

        public override double SonFiyatHesapla()
        {
            return UrunFiyat + HassasUrunMasraf;
        }
    }
}
