using System;

namespace RuletOyunu
{

    class Program
    {
        static void Main(string[] args)
        {
            Program.Start();
        }

        static public void Start()
        {
            int ownedChip = 100;
            Console.WriteLine("Rulet oyununa hoşgeldiniz!");

            while (ownedChip > 0)
            {
                //BAHİS
                Bahis bahis = Program.BahisYap(ownedChip);

                //RULET
                Chip result = Program.RuletOyna();

                //SONUÇ
                ownedChip = Program.Sonuc(ownedChip, bahis, result);


                if (ownedChip >= 300) // Chip sayısı 300 ve üzeri olduğunda oyun kazanılır.
                {
                    Console.WriteLine("KAZANDINIZ!");
                    break;
                }
                else if (ownedChip <= 0) // Chip sayısı 100 ve altına düştüğünde oyun kaybedilir.
                {
                    Console.WriteLine("Kalan çip sayınız: " + ownedChip + " Oyunu kaybettiniz.");
                    break;
                }
                else
                {
                    Console.WriteLine("Kalan çip sayınız: " + ownedChip + " Oyun devam ediyor.");

                }


            }

        }

        public class Chip
        {
            public int value { get; set; } // Gelen sayının değeri
            public String color { get; set; } // Gelen sayının rengi
            public String parity { get; set; } // Gelen sayının teklik-çiftliği

            public Chip()
            {
            }
        }

        public class Bahis
        {
            public String option { get; set; } // Bahis seçeneklerinden biri (sayı-nitelik)
            public String choice { get; set; } // Bahis seçimi
            public int chipAmount { get; set; } // Yatırılan çip miktarı

            public Bahis(string option, string choice, int chipAmount)
            {
                this.option = option;
                this.choice = choice;
                this.chipAmount = chipAmount;
            }
        }


        static public Bahis BahisYap(int ownedChip)
        {

            int chipAmount; // Yatırılacak chip miktarı
            String option; // Bahis seçeneklerinden biri (sayı-nitelik)

            Console.WriteLine("Bir sayı (0-99), Kırmızı ('K', 'k'), siyah ('S', 's'), tek ('T', 't') veya çift ('Ç', 'ç') seçin:");
            String choice = Console.ReadLine().ToLower(); //Alınan değer kontrol kısmında kolaylık sağlaması için küçük harfe dönüştürülüyor.

            bool isNumeric = int.TryParse(choice, out _); //Değerin sayısal veya string değer olup olmadığı kontrol ediliyor.
            if (isNumeric == true)
            {
                option = "number"; // Sayı seçildiğinde 'number' seçeneği atandı.

                while (Int32.Parse(choice) < 0 || Int32.Parse(choice) >= 100) //Sayının 0'dan 99'a kadar olma kontrolü.
                {
                    Console.WriteLine("Sadece 0 - 99 arasındaki sayılari seçebilirsiniz. Yeniden seçim yapınız:");
                    choice = Console.ReadLine();
                }
            }
            else
            {
                option = "qualification"; // Nitelik seçildiğinde 'qualification' seçeneği atandı.

                while (!(choice.Equals("k") || choice.Equals("s") || choice.Equals("t") || choice.Equals("ç")))
                {
                    // Seçim girdilerinin kontrolü yapıldı.

                    Console.WriteLine("Böyle bir seçim tanımlı değil. Yeniden seçim yapınız (('K', 'k'),('S', 's'),('T', 't'), ('Ç', 'ç')):");
                    choice = Console.ReadLine().ToLower();

                }
            }

            Console.WriteLine("Yatırmak istediğiniz chip miktarını giriniz:");
            chipAmount = Int32.Parse(Console.ReadLine());
            if (chipAmount > ownedChip)
            {
                Console.WriteLine("Sahip olduğunuzdan daha fazla çip ile oyuna katılamazsınız. Max bahis: " + ownedChip);
                Console.WriteLine("Yatırmak istediğiniz chip miktarını giriniz:");
                chipAmount = Int32.Parse(Console.ReadLine());
            }

            Bahis bahis = new Bahis(option, choice, chipAmount);

            return bahis;
        }

        static public Chip RuletOyna()
        {
            Chip chip = new Chip(); // Boş bir chip nesnesi yaratılır.
            Random random = new Random();
            int value = random.Next(100); // 0-99 arasından rastgele bir sayı seçilir
            chip.value = value;

            if (value % 2 == 0) // Sayı iki ile bölünebiliyorsa çift, değilse tektir.
            {
                chip.parity = "ç";
            }
            else
            {
                chip.parity = "t";
            }

            if (value <= 49) // 0-49 arası sayılar kırmızı, 50-99 arası sayılar siyahtır.
            {
                chip.color = "k";
            }
            else if (value > 49)
            {
                chip.color = "s";
            }

            Console.WriteLine("Kazanan sayı:" + chip.value);

            return chip;
        }

        static public int Sonuc(int ownedChip, Bahis bahis, Chip result)
        {
            int chipAmount = bahis.chipAmount; // Yatırılan çip miktarı Bahis nesnesinden alınır.

            switch (bahis.option)
            {
                case "number": // Sayı seçimi

                    if (Int32.Parse(bahis.choice) == result.value)
                    // Eğer seçtiğimiz sayı kazanan sayıyla eşleşirse 100 katı chip kazanılır.
                    {
                        ownedChip += ((chipAmount * 100) - chipAmount);
                    }
                    else
                    {
                        ownedChip -= chipAmount;
                    }
                    break;

                case "qualification": // Nitelik seçimi

                    if (bahis.choice == result.parity || bahis.choice == result.color)
                    // Eğer seçtiğimiz nitelik kazanan sayının herhangi bir niteliğiyle eşleşirse 2 katı chip kazanılır.
                    {
                        ownedChip += ((chipAmount * 2) - chipAmount);
                    }
                    else
                    {
                        ownedChip -= chipAmount;
                    }

                    break;
            }

            return ownedChip; // Sona kalan çip miktarı değerlendirilmek için döndürülür.
        }

    }
}
