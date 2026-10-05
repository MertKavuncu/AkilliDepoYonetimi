using System;
using System.IO;
using System.Text.Json;

namespace AkilliDepoYonetimi
{
    public class Program
    {
        static void Main(string[] args)
        {
            DepoIslemleri depo = new DepoIslemleri();
            List<Musteri> musteriListesi = new List<Musteri>();
            List<Yonetici> yoneticiListesi = new List<Yonetici>();

            var jsonAyar = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (File.Exists("musteriler.json"))
            {
                musteriListesi = JsonSerializer.Deserialize<List<Musteri>>(File.ReadAllText("musteriler.json"), jsonAyar);
            }

            if (File.Exists("yoneticiler.json"))
            {
                yoneticiListesi = JsonSerializer.Deserialize<List<Yonetici>>(File.ReadAllText("yoneticiler.json"), jsonAyar);
            }

            int sonMusteriId = musteriListesi.Count > 0 ? musteriListesi.Max(x => x.MusteriId) : 0;
            int sonYoneticiId = yoneticiListesi.Count > 0 ? yoneticiListesi.Max(x => x.YoneticiId) : 0;
            int genelKullaniciId = Math.Max(sonMusteriId, sonYoneticiId);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n=== AKILLI DEPO SİSTEMİ GİRİŞ EKRANI ===");
                Console.WriteLine("1 - Giriş Yap");
                Console.WriteLine("2 - Kayıt Ol");
                Console.WriteLine("0 - Programı Kapat");
                Console.Write("Seçiminiz: \n");

                if (!int.TryParse(Console.ReadLine(), out int secim))
                {
                    Console.WriteLine($"Geçersiz bir seçenek girdiniz. lütfen tekrar deneyin.");
                    Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                    Console.ReadKey(true);
                    continue;
                }
                else if (secim == 0) { break; }

                switch (secim)
                {
                    case 1:
                    // Giriş Yap
                        bool girisDevam = true;

                        while (girisDevam)
                        {
                            Console.Clear();
                            Console.WriteLine("\nKullanıcı rolünü seçiniz:");
                            int kullaniciSecim = SayiAl("1 - Yönetici\n2 - Müşteri\n3 - Geri Dön\nSeçiminiz: \n");

                            switch (kullaniciSecim)
                            {
                                case 1:
                                    {
                                        //Yönetici
                                        int id = SayiAl("Yönetici ID: ");
                                        Yonetici bulunanYonetici = yoneticiListesi.FirstOrDefault(x => x.YoneticiId == id);

                                        if (bulunanYonetici == null)
                                        {
                                            Console.WriteLine("Hatalı ID girdiniz!");
                                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                                            Console.ReadKey(true);
                                            break;
                                        }
                                        YoneticiPaneli(depo, bulunanYonetici);
                                        girisDevam = false;

                                        break;
                                    }

                                case 2:
                                    {
                                        //Müşteri
                                        int id = SayiAl("Müşteri ID: ");
                                        Musteri bulunanMusteri = musteriListesi.FirstOrDefault(x => x.MusteriId == id);

                                        if (bulunanMusteri == null)
                                        {
                                            Console.WriteLine("Hatalı ID girdiniz!");
                                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                                            Console.ReadKey(true);
                                            break;
                                        }
                                        MusteriPaneli(depo, bulunanMusteri, musteriListesi);
                                        girisDevam = false;

                                        break;
                                    }

                                case 3:
                                    girisDevam = false; // Ana menüye dön
                                    break;

                                default:
                                    Console.WriteLine("Geçersiz bir menü numarası girdiniz!\n");
                                    Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                                    Console.ReadKey(true);
                                    break;
                            }
                        }
                        break; // Ana menünün case 1'ini bitirir

                    case 2:
                        {
                            // Kayıt Ol
                            bool kayitDevam = true;
                            while (kayitDevam)
                            {
                                Console.WriteLine("\nHangi rolde kayıt olmak istiyorsunuz?");
                                int kayitSecim = SayiAl("1 - Yönetici\n2 - Müşteri\n3 - Geri Dön\nSeçiminiz: ");

                                switch (kayitSecim)
                                {
                                    case 1:
                                        {
                                            string isim = MetinAl("İsminizi giriniz: ");
                                            genelKullaniciId++;

                                            yoneticiListesi.Add(new Yonetici(genelKullaniciId, isim));
                                            File.WriteAllText("yoneticiler.json", JsonSerializer.Serialize(yoneticiListesi, new JsonSerializerOptions { WriteIndented = true }));
                                            Console.WriteLine($"\nKayıt başarılı! Yönetici ID'niz: {genelKullaniciId}. Lütfen giriş yaparken bu ID'yi kullanmayı unutmayın.\n");
                                            kayitDevam = false;
                                            Console.WriteLine("\nAna menüye dönmek için bir tuşa basın...");
                                            Console.ReadKey(true);
                                            break;
                                        }
                                    case 2:
                                        {
                                            string isim = MetinAl("İsminizi giriniz: ");
                                            double bakiye = OndalikAl("Başlangıç bakiyenizi giriniz: ");
                                            genelKullaniciId++;

                                            try
                                            {
                                                musteriListesi.Add(new Musteri(genelKullaniciId, isim, bakiye));
                                                File.WriteAllText("musteriler.json", JsonSerializer.Serialize(musteriListesi, new JsonSerializerOptions { WriteIndented = true }));
                                                Console.WriteLine($"\nKayıt başarılı! Müşteri ID'niz: {genelKullaniciId}. Lütfen giriş yaparken bu ID'yi kullanmayı unutmayın.\n");
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine($"\nMüşteri Kaydı Başarısız: {ex.Message}");
                                                genelKullaniciId--;
                                            }

                                            kayitDevam = false;
                                            Console.WriteLine("\nAna menüye dönmek için bir tuşa basın...");
                                            Console.ReadKey(true);
                                            break;
                                        }
                                    case 3:
                                        {
                                            kayitDevam = false; // Ana menüye dön
                                            break;
                                        }
                                    default:
                                        {
                                            Console.WriteLine("Geçersiz bir menü numarası girdiniz!\n");
                                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                                            Console.ReadKey(true);
                                            break;
                                        }
                                }
                            }
                            break; // Ana menünün case 2'ini bitirir
                        }
                    default:
                        Console.WriteLine("Geçersiz bir menü numarası girdiniz!\n");
                        Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                        Console.ReadKey(true);
                        break;
                }
            }
        }
        static int SayiAl(string mesaj)
        {
            int sayi;
            while (true)
            {
                Console.Write(mesaj);
                if (int.TryParse(Console.ReadLine(), out sayi) && sayi >= 0)
                    return sayi;

                Console.WriteLine("Hatalı giriş! Lütfen 0 veya daha büyük geçerli bir sayı giriniz.");
            }
        }

        static double OndalikAl(string mesaj)
        {
            double sayi;
            while (true)
            {
                Console.Write(mesaj);
                if (double.TryParse(Console.ReadLine(), out sayi) && sayi >= 0)
                    return sayi;

                Console.WriteLine("Hatalı giriş! Lütfen 0 veya daha büyük geçerli bir sayı giriniz.");
            }
        }

        static string MetinAl(string mesaj)
        {
            while (true)
            {
                Console.Write(mesaj);
                string girdi = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(girdi))
                {
                    Console.WriteLine("Bu alan boş bırakılamaz, lütfen bir metin girin!");
                }
                else { return girdi.Trim(); }
            }
        }

        static void YoneticiPaneli(DepoIslemleri depo, Yonetici yonetici)
        {
            while (true)
            {
                Console.Clear();
                yonetici.BilgiGoster();

                Console.WriteLine("\n1 - Depoya Yeni Ürün Ekle");
                Console.WriteLine("2 - Stok Ekle");
                Console.WriteLine("3 - Hasarlı Ürün Düş");
                Console.WriteLine("4 - Kasa ve Ciro Durumu");
                Console.WriteLine("5 - Kritik Stok Raporu");
                Console.WriteLine("6 - Ürünleri Listele ve Filtrele");
                Console.WriteLine("0 - Çıkış Yap (Ana Menüye Dön)\n");

                int secim = SayiAl("Seçiminiz: ");
                if (secim == 0) return;                

                switch (secim)
                {
                    case 1:
                        {
                            Console.Clear();
                            int yeniUrunId;

                            while (true)
                            {
                                yeniUrunId = SayiAl("Ürün ID: ");
                                if (!depo.UrunVarMi(yeniUrunId)) break;

                                Console.WriteLine("Hata: Bu ID zaten kullanımda! Lütfen farklı bir ID giriniz.\n");
                            }
                            string yeniUrunIsim;
                            while (true)
                            {
                                yeniUrunIsim = MetinAl("\nÜrün İsim: ");
                                if (!depo.UrunIsimVarMi(yeniUrunIsim)) break;

                                Console.WriteLine($"Hata: '{yeniUrunIsim}' zaten depoda mevcut! Yeni ürün açmak yerine lütfen 'Stok Artır' seçeneğini kullanın.");
                            }

                            double yeniUrunFiyat = OndalikAl("\nBirim Fiyat: ");
                            int yeniUrunAdet = SayiAl("\nÜrün Adet: ");

                            int turSecim;
                            while (true)
                            {
                                turSecim = SayiAl("\nÜrün hangi türde?\n1 - Normal Ürün\n2 - Soğuk Zincir Ürünü\n3 - Hassas Kargo Ürünü\nSeçiminiz: ");

                                if (turSecim >= 1 && turSecim <= 3)
                                {
                                    break;
                                }

                                Console.WriteLine("Geçersiz tür seçimi! Lütfen 1, 2 veya 3 giriniz.");
                            }
                            try
                            {
                                Urunler eklenecekUrun = null;

                                switch (turSecim)
                                {
                                    case 1:
                                        eklenecekUrun = new Urunler(yeniUrunId, yeniUrunIsim, yeniUrunFiyat, yeniUrunAdet);
                                        break;
                                    case 2:
                                        double yeniUrunSogutmaMasraf = OndalikAl("Soğuk zincir ürünlerinde ekstra soğutma masrafı bulunur. Ekstra masraf tutarını giriniz: ");
                                        eklenecekUrun = new SogukZincirUrunleri(yeniUrunId, yeniUrunIsim, yeniUrunFiyat, yeniUrunAdet, yeniUrunSogutmaMasraf);
                                        break;
                                    case 3:
                                        double yeniUrunKargoMasraf = OndalikAl("Hassas kargo ürünlerinde ekstra taşıma masrafı bulunur. Ekstra masraf tutarını giriniz: ");
                                        eklenecekUrun = new HassasKargoUrunleri(yeniUrunId, yeniUrunIsim, yeniUrunFiyat, yeniUrunAdet, yeniUrunKargoMasraf);
                                        break;
                                }

                                depo.YeniUrunEkle(eklenecekUrun);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"\nÜrün Eklenemedi: {ex.Message}");
                            }
                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            int urunId = SayiAl("\nÜrün ID: ");
                            int urunAdet = SayiAl("\nÜrün Adet: ");
                            depo.StokArttir(urunId, urunAdet);
                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            int urunId = SayiAl("\nÜrün ID: ");
                            int urunAdet = SayiAl("\nÜrün Adet: ");
                            depo.HasarliUrunDus(urunId, urunAdet);
                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            depo.KasaVeDepoOzet();
                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break; 
                        }
                    case 5:
                        {
                            Console.Clear();
                            depo.TukenmekUzereOlanlariListele();
                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break;
                        }
                    case 6:
                        {
                            UrunListelemeMenusu(depo);
                            break;
                        }

                    default:
                        Console.WriteLine("\nGeçersiz bir menü numarası girdiniz! Devam etmek için bir tuşa basın...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void MusteriPaneli(DepoIslemleri depo, Musteri musteri, List<Musteri> musteriListesi)
        {
            while (true)
            {
                Console.Clear();
                musteri.BilgiGoster();

                Console.WriteLine("\n1 - Ürünleri Listele ve Filtrele");
                Console.WriteLine("2 - Ürün Satın Al");
                Console.WriteLine("3 - Siparişleri Görüntüle");
                Console.WriteLine("4 - Sipariş İptal Et (İade)");
                Console.WriteLine("5 - Bakiye Yükle");
                Console.WriteLine("0 - Çıkış Yap (Ana Menüye Dön)\n");

                int secim = SayiAl("Seçiminiz: ");
                if (secim == 0) return;

                switch (secim)
                {
                    case 1:
                        {
                            UrunListelemeMenusu(depo);
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            int urunId = SayiAl("\nÜrün ID: ");
                            int urunAdet = SayiAl("\nÜrün Adet: ");
                            depo.SatisYap(musteri, urunId, urunAdet);
                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            depo.MusteriSiparisleriniListele(musteri);
                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            int siparisId = SayiAl("İptal etmak istediğiniz siparişin ID: ");
                            depo.SiparisIptalEt(siparisId, musteri);
                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break;
                        }
                    case 5:
                        {
                            Console.Clear();
                            double bakiye = OndalikAl("Hesabınıza yatırmak istediğiniz para miktarını giriniz: ");
                            musteri.BakiyeYukle(bakiye);
                            File.WriteAllText("musteriler.json", JsonSerializer.Serialize(musteriListesi, new JsonSerializerOptions { WriteIndented = true }));
                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("\nGeçersiz bir menü numarası girdiniz! Devam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break;
                        }
                }
            }
        }

        static void UrunListelemeMenusu(DepoIslemleri depo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Listeleme/Filtreleme Menüsü.\nYapmak istediğiniz işlemi seçiniz:\n\n1 - Tüm Ürünleri Liste\n2 - Ürünleri Çeşitlerine Göre Listele\n3 - Ürünleri Sırala Ve Listele\n4 - Ürünleri Fiyat Aralığına Göre Listele\n0 - Geri Dön");
                int secim = SayiAl("Seçiminiz: ");
                if (secim == 0) return;

                switch (secim)
                {
                    case 1:
                        {
                            Console.Clear();
                            depo.TumUrunleriListele();
                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            int kriter = SayiAl("Hangi Kategoriye Göre Sıralamak İstediğinizi Seçiniz:\n1 - Normal Ürünler\n2 - Soğuk Zincir Ürünleri\n3 - Hassas Kargo Ürünleri\nSeçiminiz: ");

                            depo.UrunleriCesideGoreListele(kriter);
                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            int kriter = SayiAl("Neye Göre Listelemek İstediğinizi Seçiniz:\n1 - İsme Göre\n2 - Fiyata Göre\n3 - Stoğa Göre\nSeçiminiz: ");

                            int kriter2;
                            while (true)
                            {
                                kriter2 = SayiAl("\nNasıl Sıralanmasını İstediğinizi Seçiniz:\n1 - Artan\n2 - Azalan\nSeçiminiz: ");
                                if (kriter2 == 1 || kriter2 == 2) break;
                                Console.WriteLine("Geçersiz seçim! Lütfen 1 (Artan) veya 2 (Azalan) giriniz.");
                            }

                            bool azalanMi = (kriter2 == 2);
                            depo.UrunleriSiralaVeListele(kriter, azalanMi);

                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            double minFiyat = OndalikAl("Minimum Fiyatı Giriniz: ");
                            double maxFiyat = OndalikAl("Maksimum Fiyatı Giriniz: ");

                            depo.FiyatAraliginaGoreListele(minFiyat, maxFiyat);
                            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
                            Console.ReadKey(true);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("\nGeçersiz menü seçimi! Lütfen 0-4 arasında bir sayı giriniz.");
                            Console.ReadKey(true);
                            break;
                        }
                }
            }
        }
    }
}