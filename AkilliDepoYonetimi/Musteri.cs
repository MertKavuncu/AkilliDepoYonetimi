using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoYonetimi
{
    internal class Musteri
    {
        public int MusteriId { get; private set; }
        public string MusteriIsim { get; set; }
        public double MusteriBakiye { get; set; }

        public Musteri(int musteriId, string musteriIsim, double musteriBakiye)
        {
            if(musteriId<=0 || musteriBakiye < 0)
            {
                throw new Exception($"Hata: ID 0'dan büyük, bakiye ise 0 veya pozitif olmalıdır!");
            }

            MusteriId = musteriId;
            MusteriIsim = musteriIsim;
            MusteriBakiye = musteriBakiye;
        }
        public bool BakiyeYukle(double miktar)
        {
            if (miktar <= 0)
            {
                Console.WriteLine("Yüklenecek miktar 0'dan büyük olmalıdır!\n");
                return false;
            }
            MusteriBakiye += miktar;
            Console.WriteLine($"{miktar} TL yüklendi. Güncel bakiye: {MusteriBakiye} TL\n");
            return true;
        }
        public void BilgiGoster()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine($"  HESAP SAHİBİ: {MusteriIsim} (ID: {MusteriId}) | GÜNCEL BAKİYE: {MusteriBakiye} TL");
            Console.WriteLine("==================================================");
        }
    }
}
