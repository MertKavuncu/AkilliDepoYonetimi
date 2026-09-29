using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoYonetimi
{
    internal class Siparis
    {
        public int SiparisId { get; set; }
        public Musteri SiparisVerenMusteri { get; set; }
        public Urunler SatilanUrun { get; set; }
        public int SatilanAdet { get; set; }
        public double ToplamTutar { get; set; }

        public Siparis(int siparisId, Musteri siparisVerenMusteri, Urunler satilanUrun, int satilanAdet, double toplamTutar)
        {
            if (siparisId <= 0 || satilanAdet <= 0 || toplamTutar <= 0)
            {
                throw new Exception("Hata: Sipariş bilgileri geçersiz (ID, adet veya tutar 0 ve altı olamaz)!");
            }

            SiparisId = siparisId;
            SiparisVerenMusteri = siparisVerenMusteri;
            SatilanUrun = satilanUrun;
            SatilanAdet = satilanAdet;
            ToplamTutar = toplamTutar;
        }
    }
}
