# 📦 Akıllı Depo ve Sipariş Yönetim Sistemi

C# ve .NET ortamında geliştirilmiş; Nesne Yönelimli Programlama (OOP) prensiplerini, LINQ sorgularını ve JSON tabanlı kalıcı veri saklama (data persistence) mimarisini kullanan kapsamlı bir konsol otomasyonudur.

---

## 🚀 Öne Çıkan Özellikler

### 1. Rol Bazlı Kullanıcı Yönetimi
- **Yönetici Paneli:** Stok kontrolü, yeni ürün ekleme, hasarlı ürün düşme, ciro takibi ve kritik stok raporlaması.
- **Müşteri Paneli:** Ürün kataloğu inceleme, bakiye yönetimi, sipariş oluşturma ve sipariş iptali (iade).
- Otomatik ID sayacı ve oturum doğrulama.

### 2. Nesne Yönelimli Mimari & Polimorfizm (OOP)
- **Kalıtım (Inheritance):** Temel `Urunler` sınıfından türetilen `SogukZincirUrunleri` (soğutma masrafı) ve `HassasKargoUrunleri` (özel taşıma güvencesi masrafı).
- **Polimorfizm:** Ürün türüne göre dinamik fiyat hesaplayan ezilmiş (`override`) `SonFiyatHesapla()` metotları.
- **Polimorfik JSON Serileştirme:** Farklı alt sınıfların tür kaybı yaşamadan diske yazılabilmesi için `[JsonDerivedType]` nitelikleri.

### 3. Gelişmiş Filtreleme ve Raporlama (LINQ)
- Tükenmek üzere olan ürünler için eşik değerli kritik stok analizi ($\le 5$).
- İsme, fiyata ve stok miktarına göre artan/azalan sıralama.
- Dinamik fiyat aralığına ve ürün türüne göre anlık filtreleme.
- Toplam kasa cirosu, depo envanter değeri ve en çok kazandıran ürün raporu.

### 4. Veri Kalıcılığı (JSON Persistence)
- Program kapatıldığında veriler kaybolmaz; ürünler, müşteriler ve yöneticiler yerel diskteki `.json` dosyalarına otomatik kaydedilir ve program açılışında geri yüklenir.
- Tükenen ürünler geçmiş sipariş kayıtlarını ve veri bütünlüğünü bozmamak adına veri tabanından silinmez, arayüzde dinamik olarak `[STOKTA YOK]` şeklinde listelenir.

### 5. Çökmeyen Giriş Mimarisi (Defensive Programming)
- Sayı, ondalık ve metin girişleri için özel yardımcı metotlar (`int.TryParse`, `double.TryParse`).
- Geçersiz karakter veya boş veri girişlerinde uygulamanın çökmesi engellenmiş, kullanıcı dostu hata mesajları kurgulanmıştır.

---

## 🛠️ Kullanılan Teknolojiler

- **Dil:** C#
- **Platform:** .NET (Console Application)
- **Veri Formatı:** JSON (`System.Text.Json`)
- **Sorgulama:** LINQ (Language Integrated Query)

---

## 💻 Kurulum ve Çalıştırma

1. Projeyi bilgisayarınıza klonlayın:
   ```bash
   git clone [https://github.com/MertKavuncu/AkilliDepoYonetimi.git](https://github.com/MertKavuncu/AkilliDepoYonetimi.git)
