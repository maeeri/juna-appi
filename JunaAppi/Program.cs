using System;
using System.Threading.Tasks;


namespace JunaAppi
{
    class Program
    {
<<<<<<< HEAD
        //johanna taiteili tähän taas asciiartia
        private static string AsciiArt = @"
                                                                        
 _________________________________________________________________________
        __   _     _   _     _   __         __     ____     ____       __
        /    /    /    /|   /    / |        / |    /    )   /    )     / 
-------/----/----/----/-| -/----/__|-------/__|---/____/---/____/-----/--
      /    /    /    /  | /    /   | ===  /   |  /        /          /   
_(___/____(____/____/___|/____/____|_____/____|_/________/________ _/_ __

  ___________   _______________________________________^__
 ___   ___ |||  ___   ___   ___    ___ ___  |   __  ,----\
|   | |   |||| |   | |   | |   |  |   |   | |  |  | |_____\
|___| |___|||| |___| |___| |___|  | O | O | |  |  |        \
           |||                    |___|___| |  |__|         )
___________|||______________________________|______________/
           ||| TIIMI KUTONEN                             /--------
-----------'''---------------------------------------' ";

        //Johanna miettii metodia, joka hakisi seuraavan pysäkin
        private static async Task GetNextStation()
=======
        //Johanna miettiin metodia, joka hakisi seuraavan pysäkin
        private static void GetNextStation()
>>>>>>> 143ec8f424946422e24e50f026a989d2c88f5ee3
        {
            //junan numeron perusteella, 
            Console.WriteLine("Annan junan numero");
<<<<<<< HEAD
            string junanNumero = Console.ReadLine(); 
            TrainTrackingLatest trackedTrain = await TrainsApi.GetLocation(junanNumero);
            
=======
>>>>>>> 143ec8f424946422e24e50f026a989d2c88f5ee3
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
                    FindTrack();
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
            //Console.WriteLine("Minkä aseman tiedot haluat?");
            //string asemaRaide = Console.ReadLine().ToLower();
            
            string param = "latest/" + junaNumero;
            Juna juna = await TrainsApi.GetJuna(param);
            /*
            if (juna != null)
                Console.WriteLine(juna.ToString());
            else
            {
                Console.WriteLine("Ei löytynyt :(");
            }
            */
        }

            private static async Task MisMih()
            {//Akin Väserrykset
            while (true)
            {
                try
                {
                    Console.WriteLine("Mistä olet Lähdössä");
                    string lahto = Console.ReadLine();

                    Console.WriteLine("Minne olet Menossa");
                    string saapuminen = Console.ReadLine();

                    /*Console.WriteLine("Minä päivänä olet menossa?");
                    DateTime paiva = Convert.ToDateTime(Console.ReadLine());*/

                    Reitti reitti = await TrainsApi.HaeReitti(lahto, saapuminen);

                    if (reitti == null)
                    Console.WriteLine("\nJunaa ei löydy");

                    /*else
                        Rejtti(reitti);*/
                }
                catch (FormatException)
                {

                    Console.WriteLine("-.-");
                    continue;
                }
                break;
            }

            }
            /*private static void Rejtti(Reitti rejtti)
            {
            Console.WriteLine("  Reittisi tiedot:");
            Console.WriteLine($"  Reitti lähtö: {rejtti.operatorShortCode}");
            Console.WriteLine($"  Reitti saapuminen: {rejtti.operatorShortCode}");
            Console.WriteLine($"  Reitti lähtöaika: {rejtti.departureDate}");
            Console.WriteLine($"  Reitti carbohydrates: {rejtti.timeTableRows.countryCode}");
         }*/
    }
}