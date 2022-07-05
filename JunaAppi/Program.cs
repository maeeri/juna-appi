using System;

using System.Threading.Tasks;


namespace JunaProjekti
{
    class Program
    {
        //Johanna miettiin metodia, joka hakisi seuraavan pysäkin
        private static void GetNextStation()
        {
            //junan numeron perusteella, 
            Console.WriteLine("Annan junan numero");
        }
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

        //Mari-Annen metodi junalaiturien löytämiseen
        private static async Task FindTrack()
        {
            Console.WriteLine("Minkä junan (numero) lähtöraiteen haluat hakea?");
            int.TryParse(Console.ReadLine(), out int junaNumero);
            Console.WriteLine("Minkä aseman tiedot haluat?");
            string asemaRaide = Console.ReadLine().ToLower();
        }
    }
}