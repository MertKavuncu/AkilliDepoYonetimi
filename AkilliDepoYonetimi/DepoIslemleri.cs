using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace AkilliDepoYonetimi
{
    internal class DepoIslemleri
    {
        private List<Urunler> urunListesi = new List<Urunler>();
        private List<Siparis> siparisListesi = new List<Siparis>();
        private double toplamKasaGeliri = 0;
        private int sonSiparisId = 0;

        public DepoIslemleri()
        {
            if (File.Exists("urunler.json"))
            {
                string okunanJson = File.ReadAllText("urunler.json");
                urunListesi = JsonSerializer.Deserialize<List<Urunler>>(okunanJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
        public void VerileriKaydet()
        {
            string jsonMetin = JsonSerializer.Serialize(urunListesi, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("urunler.json", jsonMetin);
        }

        public bool SatisYap(Musteri musteri, int urunId, int adet)
        {
            Urunler urun = urunListesi.Find(x => x.UrunId == urunId);
            if (urun == null)
            {
                Console.WriteLine("Bu ID ile kayıtlı bir ürün bulunamadı!");
                return false;
            }

            if (adet <= 0)
            {
                Console.WriteLine($"Hata: Alınacak adet 0'dan büyük olmalıdır.");
                return false;
            }

            if (urun.UrunAdet < adet)
            {
                Console.WriteLine($"Yetersiz stok! Depoda sadece {urun.UrunAdet} adet var.");
                return false;
            }

            double birimFiyat = urun.SonFiyatHesapla();
            double ToplamFiyat = birimFiyat * adet;

            if (musteri.MusteriBakiye < ToplamFiyat)
            {
                Console.WriteLine("Müşterinin bakiyesi bu alışverişe yetmiyor!");
                return false;
            }

            urun.UrunAdet -= adet;
            musteri.MusteriBakiye -= ToplamFiyat;
            toplamKasaGeliri += ToplamFiyat;
            urun.ToplamKazanilanGelir += ToplamFiyat;

            siparisListesi.Add(new Siparis((++sonSiparisId), musteri, urun, adet, ToplamFiyat));

            Console.WriteLine($"Satış başarılı! {musteri.MusteriIsim} {adet} adet {urun.UrunIsim} satın aldı.");
            Console.WriteLine($"Kalan Müşteri Bakiyesi: {musteri.MusteriBakiye} TL | Kalan Stok: {urun.UrunAdet}");
            VerileriKaydet();
            return true;
        }

        public  bool YeniUrunEkle(Urunler yeniUrun)
        {
            if (yeniUrun == null)
            {
                Console.WriteLine($"Hata: Geçersiz ürün verisi.");
                return false;
            }

            if (urunListesi.Any(x => yeniUrun.UrunId == x.UrunId))
            {
                Console.WriteLine($"Bu ID zaten kullanımda!");
                return false;
            }

            if (urunListesi.Any(x => x.UrunIsim.Trim().ToLower() == yeniUrun.UrunIsim.Trim().ToLower()))
            {
                Console.WriteLine($"Hata: '{yeniUrun.UrunIsim}' zaten depoda mevcut! Yeni ürün açmak yerine lütfen 'Stok Artır' seçeneğini kullanın.");
                return false;
            }

            urunListesi.Add(yeniUrun);
            Console.WriteLine($"{yeniUrun.UrunIsim} başarıyla depoya eklendi.");
            VerileriKaydet();
            return true;
        }
        public bool UrunVarMi(int id)
        {
            return urunListesi.Any(x => x.UrunId == id);
        }
        public bool UrunIsimVarMi(string isim)
        {
            return urunListesi.Any(x => x.UrunIsim.Trim().ToLower() == isim.Trim().ToLower());
        }

        public bool StokArttir(int urunId, int adet)
        {
            if (adet <= 0)
            {
                Console.WriteLine($"Hata: Adet 0 veya negatif olamaz!");
                return false;
            }

            Urunler arttirilicakUrun = urunListesi.Find(x => x.UrunId == urunId);

            if (arttirilicakUrun == null)
            {
                Console.WriteLine($"Bu ID ile kayıtlı ürün bulunamadı");
                return false;
            }

            arttirilicakUrun.UrunAdet += adet;
            Console.WriteLine($"İşlem başarılı. Artık {arttirilicakUrun.UrunAdet} tane {arttirilicakUrun.UrunIsim} var stokta.");
            VerileriKaydet();
            return true;
        }
        public bool HasarliUrunDus(int urunId, int adet)
        {
            if (adet <= 0)
            {
                Console.WriteLine($"Hata: Adet 0 veya negatif olamaz!");
                return false;
            }

            Urunler dusulucekUrun = urunListesi.Find(x => x.UrunId == urunId);

            if (dusulucekUrun == null)
            {
                Console.WriteLine($"Bu ID ile kayıtlı ürün bulunamadı");
                return false;
            }

            if (dusulucekUrun.UrunAdet < adet)
            {
                Console.WriteLine($"Hata: Depoda sadece {dusulucekUrun.UrunAdet} adet {dusulucekUrun.UrunIsim} var. Var olandan fazla hasar kaydı girilemez!");
                return false;
            }

            dusulucekUrun.UrunAdet -= adet;
            Console.WriteLine($"İşlem başarılı. Artık {dusulucekUrun.UrunAdet} tane {dusulucekUrun.UrunIsim} var stokta.");
            VerileriKaydet();
            return true;
        }

        public bool TumUrunleriListele()
        {
            if (urunListesi.Count == 0)
            {
                Console.WriteLine($"Depoda hiç ürün yok!");
                return false;
            }

            foreach(Urunler x in urunListesi)
            {
                if (x.UrunAdet > 0)
                {
                    Console.WriteLine($"{x.UrunIsim} | ID: {x.UrunId} | Fiyat: {x.SonFiyatHesapla()} | Adet: {x.UrunAdet}");
                }
                else if(x.UrunAdet == 0)
                {
                    Console.WriteLine($"{x.UrunIsim} | ID: {x.UrunId} | Fiyat: {x.SonFiyatHesapla()} | Adet: STOKTA YOK");
                }
            }
            return true;
        }
        public bool TukenmekUzereOlanlariListele()
        {
            if (urunListesi.Count == 0)
            {
                Console.WriteLine($"Depoda hiç ürün yok!");
                return false;
            }

            List<Urunler> kritikUrunListesi = urunListesi.Where(x => x.UrunAdet <= 5).ToList();

            if (kritikUrunListesi.Count == 0)
            {
                Console.WriteLine("Harika! Depoda kritik seviyede (5 ve altı) tükenmek üzere olan ürün bulunmuyor.");
                return true;
            }

            Console.WriteLine($"Stok adedi 5 ve altında olan ürünler aşağıda listlenmiştir, lütfen yenilerini tedarik ediniz:\n");

            foreach(Urunler x in kritikUrunListesi)
            {
                if (x.UrunAdet > 0)
                {
                    Console.WriteLine($"{x.UrunIsim} | ID: {x.UrunId} | Fiyat: {x.SonFiyatHesapla()} | Adet: {x.UrunAdet}");
                }
                else if (x.UrunAdet == 0)
                {
                    Console.WriteLine($"{x.UrunIsim} | ID: {x.UrunId} | Fiyat: {x.SonFiyatHesapla()} | Adet: STOKTA YOK");
                }
            }
            return true;
        }

        public bool UrunleriSiralaVeListele(int kriter, bool azalanMi)
        {
            if (urunListesi.Count == 0)
            {
                Console.WriteLine("Depoda hiç ürün yok!");
                return false;
            }
            
            List<Urunler> siraliListe;

            switch (kriter)
            {
                case 1:
                    siraliListe = azalanMi
                        ? urunListesi.OrderByDescending(x => x.UrunIsim).ToList()
                        : urunListesi.OrderBy(x => x.UrunIsim).ToList();
                    break;

                case 2:
                    siraliListe = azalanMi
                        ? urunListesi.OrderByDescending(x => x.SonFiyatHesapla()).ToList()
                        : urunListesi.OrderBy(x => x.SonFiyatHesapla()).ToList();
                    break;

                case 3:
                    siraliListe = azalanMi
                        ? urunListesi.OrderByDescending(x => x.UrunAdet).ToList()
                        : urunListesi.OrderBy(x => x.UrunAdet).ToList();
                    break;

                default:
                    Console.WriteLine($"Geçersiz seçim! Lütfen 1, 2 veya 3 giriniz.\n");
                    return false;
            }

            foreach (Urunler x in siraliListe)
            {
                if (x.UrunAdet > 0)
                {
                    Console.WriteLine($"{x.UrunIsim} | ID: {x.UrunId} | Fiyat: {x.SonFiyatHesapla()} | Adet: {x.UrunAdet}");
                }
                else if (x.UrunAdet == 0)
                {
                    Console.WriteLine($"{x.UrunIsim} | ID: {x.UrunId} | Fiyat: {x.SonFiyatHesapla()} | Adet: STOKTA YOK");
                }
            }
            return true;
        }
        public bool UrunleriCesideGoreListele(int kriter)
        {
            if (urunListesi.Count == 0)
            {
                Console.WriteLine($"Depoda hiç ürün yok!");
                return false;
            }
            List<Urunler> siraliListeKategori;

            switch (kriter)
            {
                case 1:
                    siraliListeKategori = urunListesi.Where(x => x.GetType() == typeof(Urunler)).ToList();
                    break;

                case 2:
                    siraliListeKategori = urunListesi.Where(x => x is SogukZincirUrunleri).ToList();
                    break;

                case 3:
                    siraliListeKategori = urunListesi.Where(x => x is HassasKargoUrunleri).ToList();
                    break;

                default:
                    Console.WriteLine($"Geçersiz sıralama kriteri!");
                    return false;
            }
            if (siraliListeKategori.Count == 0) { Console.WriteLine($"Bu kategoride kayıtlı ürün bulunmuyor.\n"); return false; }

            foreach (Urunler x in siraliListeKategori)
            {
                if (x.UrunAdet > 0)
                {
                    Console.WriteLine($"{x.UrunIsim} | ID: {x.UrunId} | Fiyat: {x.SonFiyatHesapla()} | Adet: {x.UrunAdet}");
                }
                else if (x.UrunAdet == 0)
                {
                    Console.WriteLine($"{x.UrunIsim} | ID: {x.UrunId} | Fiyat: {x.SonFiyatHesapla()} | Adet: STOKTA YOK");
                }
            }
            return true;
        }
        public bool FiyatAraliginaGoreListele(double minFiyat, double maxFiyat)
        {
            if (urunListesi.Count == 0)
            {
                Console.WriteLine($"Depoda hiç ürün yok!");
                return false;
            }
            if(minFiyat<0 || maxFiyat < minFiyat)
            {
                Console.WriteLine($"Geçersiz fiyat aralığı!");
                return false;
            }
            List<Urunler> filtrelenmisListe = urunListesi.Where(x => x.SonFiyatHesapla() >= minFiyat && x.SonFiyatHesapla() <= maxFiyat).ToList();

            if (filtrelenmisListe.Count == 0) { Console.WriteLine($"Bu fiyat aralığında hiç ürün bulunamadı.\n"); return false; }

            foreach (Urunler x in filtrelenmisListe)
            {
                if (x.UrunAdet > 0)
                {
                    Console.WriteLine($"{x.UrunIsim} | ID: {x.UrunId} | Fiyat: {x.SonFiyatHesapla()} | Adet: {x.UrunAdet}");
                }
                else if (x.UrunAdet == 0)
                {
                    Console.WriteLine($"{x.UrunIsim} | ID: {x.UrunId} | Fiyat: {x.SonFiyatHesapla()} | Adet: STOKTA YOK");
                }
            }
            return true;
        }

        public bool KasaVeDepoOzet()
        {
            Console.WriteLine("--- DEPO VE ENVANTER DURUMU ---");
            if (urunListesi.Count == 0)
            {
                Console.WriteLine("Depoda kayıtlı hiçbir ürün bulunmuyor.\n");
            }
            else
            {
                double depodakiUrunlerinToplamDegeri = urunListesi.Sum(x => x.SonFiyatHesapla() * x.UrunAdet);
                Console.WriteLine($"Depoda toplam {urunListesi.Count} farklı ürün var.");
                Console.WriteLine($"Depodaki ürünlerin toplam değeri: {depodakiUrunlerinToplamDegeri:N2} TL\n");
            }

            Console.WriteLine("--- KASA VE SATIŞ DURUMU ---");
            if (toplamKasaGeliri == 0)
            {
                Console.WriteLine("Henüz hiç satış yapılmadı, kasa boş (0 TL).\n");
            }
            else
            {
                Console.WriteLine($"Kasada biriken toplam ciro: {toplamKasaGeliri:N2} TL");

                Urunler sampiyonUrun = urunListesi.OrderByDescending(x => x.ToplamKazanilanGelir).FirstOrDefault();
                if (sampiyonUrun != null && sampiyonUrun.ToplamKazanilanGelir > 0)
                {
                    Console.WriteLine($"En çok para kazandıran ürün: {sampiyonUrun.UrunIsim} (Getirdiği Toplam Ciro: {sampiyonUrun.ToplamKazanilanGelir:N2} TL)\n");
                }
            }

            return true;
        }
        public bool MusteriSiparisleriniListele(Musteri musteri)
        {
            List<Siparis> musteriSiparisleri = siparisListesi.Where(x => x.SiparisVerenMusteri.MusteriId == musteri.MusteriId).ToList();

            if (musteriSiparisleri.Count == 0)
            {
                Console.WriteLine("Henüz hiç siparişiniz bulunmuyor.\n");
                return false;
            }

            foreach (Siparis x in musteriSiparisleri)
            {
                Console.WriteLine($"Sipariş No: #{x.SiparisId} | Ürün: {x.SatilanUrun.UrunIsim} | Adet: {x.SatilanAdet} | Ödenen Tutar: {x.ToplamTutar} TL");
            }
            return true;
        }
        public bool SiparisIptalEt(int siparisId, Musteri musteri)
        {
            if (siparisId <= 0)
            {
                Console.WriteLine("Geçersiz sipariş numarası!\n");
                return false;
            }

            Siparis IadeEdilecekSiparis = siparisListesi.Find(x => x.SiparisId == siparisId);

            if (IadeEdilecekSiparis == null)
            {
                Console.WriteLine("Bu numaraya ait bir sipariş bulunamadı.\n");
                return false;
            }

            if (IadeEdilecekSiparis.SiparisVerenMusteri.MusteriId != musteri.MusteriId)
            {
                Console.WriteLine("Hata: Sadece kendi siparişlerinizi iptal edebilirsiniz!\n");
                return false;
            }

            IadeEdilecekSiparis.SatilanUrun.UrunAdet += IadeEdilecekSiparis.SatilanAdet;
            IadeEdilecekSiparis.SiparisVerenMusteri.MusteriBakiye += IadeEdilecekSiparis.ToplamTutar;
            toplamKasaGeliri -= IadeEdilecekSiparis.ToplamTutar;
            IadeEdilecekSiparis.SatilanUrun.ToplamKazanilanGelir -= IadeEdilecekSiparis.ToplamTutar;
            siparisListesi.Remove(IadeEdilecekSiparis);

            Console.WriteLine($"İade başarılı! #{IadeEdilecekSiparis.SiparisId} numaralı sipariş iptal edildi.");
            Console.WriteLine($"{IadeEdilecekSiparis.SatilanAdet} adet {IadeEdilecekSiparis.SatilanUrun.UrunIsim} stoğa geri eklendi. Hesabınıza {IadeEdilecekSiparis.ToplamTutar} TL iade edildi.\n");
            VerileriKaydet();
            return true;
        }
    }
}
