using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoYonetimi
{
    internal class Yonetici
    {
        public int YoneticiId { get; set; }
        public string YoneticiIsim { get; set; }

        public Yonetici(int yoneticiId, string yoneticiIsim)
        {
            YoneticiId = yoneticiId;
            YoneticiIsim = yoneticiIsim;
        }

        public void BilgiGoster()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine($"  YÖNETİCİ PANELİ: {YoneticiIsim} (ID: {YoneticiId})");
            Console.WriteLine("==================================================");
        }
    }
}
