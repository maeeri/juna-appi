using System;
namespace JunaProjekti
{
    class Program
    {
        static void Main(string[] args)
        {
            bool valikko = true;
            while (valikko)
            {
                valikko = MainMenu();
            }
        }
        private static bool MainMenu()
        {
            //Akin valikko
            Console.WriteLine("Vaihtoehtosi:\n1) Mistä-Mihin\n2) Ajoissa\n3) Seuraava Pysäkki\n4) Vaihtoraide\n5) Junan Palvelut\n6) Poistu");
            switch (Console.ReadLine())
            {
                case "1":
                    return true;
                case "2":
                    return true;
                case "3":
                    return true;
                case "4":
                    return true;
                case "5":
                    return true;
                case "6":
                    return false;
                default:
                    return true;
            }
        }
    }
}