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
                    MisMih();
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

        /* private static async Task<string> FindTrack()
        {

        } */

        private static async Task MisMih()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Mistä olet Lähdössä");
                    string lähtö = Console.ReadLine();

                    Console.WriteLine("Minne olet Menossa");
                    string saapuminen = Console.ReadLine();

                    /*Console.WriteLine("Minä päivänä olet menossa?");
                    DateTime paiva = Convert.ToDateTime(Console.ReadLine());*/

                    //Juna juna = await JunaApi.GetJuna(lähtö, saapuminen);

                    //if (juna == null)
                        Console.WriteLine("\nJunaa ei löydy");
                    //else
                       // PrintJunaData(juna);
                }
                catch (FormatException)
                {

                    Console.WriteLine("Please write the your answer in the format that is instructed (inside parantheses). Press enter and start again.");
                    continue;
                }
                break;
            }

        }
    }
}