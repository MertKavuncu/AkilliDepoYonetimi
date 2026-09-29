using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AkilliDepoYonetimi
{
    [JsonDerivedType(typeof(Urunler), "normal")]
    [JsonDerivedType(typeof(SogukZincirUrunleri), "soguk")]
    [JsonDerivedType(typeof(HassasKargoUrunleri), "hassas")]
    internal class Urunler
    {
        public int UrunId { get; set; }
        public string UrunIsim { get; set; }
        public double UrunFiyat { get; set; }
        public int UrunAdet { get; set; }
        public double ToplamKazanilanGelir { get; set; }

        public Urunler(int urunId, string urunIsim, double urunFiyat, int urunAdet)
        {
            if(urunId <= 0 || urunFiyat <= 0 || urunAdet < 0)
            {
                throw new Exception($"Hata: ID ve Fiyat 0'dan büyük, Adet ise 0 veya pozitif olmalıdır!");
            }

            UrunId = urunId;
            UrunIsim = urunIsim;
            UrunFiyat = urunFiyat;
            UrunAdet = urunAdet;
        }

        public virtual double SonFiyatHesapla()
        {
            return UrunFiyat;
        }
    }
}
