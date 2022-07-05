using System;
using System.Threading.Tasks;


namespace JunaAppi
{
    class Program
    {
        //johanna taiteili tähän taas asciiartia
        private static string AsciiArt = @"  
   OO  Oo o o o...     ______________________ _________________
  O      ____          |                    | |               |
 ][_^_||_| (   ***___  | TIIMI KUTOSEN UPEA | |  JUNA-APPI <3 |
(__________|_[______]__|____________________|_|_______________|
  0--0--0      0  0      0       0     0        0        0  ";

        //Johanna miettii metodia, joka hakisi seuraavan pysäkin
        private static async Task GetNextStation()
        {
            //junan numeron perusteella 
            Console.WriteLine("Annan junan numero");
            string junanNumero = Console.ReadLine(); 
            TrainTrackingLatest trackedTrain = await TrainsApi.GetLocation(junanNumero);
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
            //johanna lisää vähän visuaalisuutta
            Console.BackgroundColor = ConsoleColor.Red; //koska Aki toivoi <3
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(AsciiArt);
            Console.WriteLine(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Yellow;
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
                    GetNextStation();
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
            Console.WriteLine("Minkä aseman tiedot haluat?");
            string asemaRaide = Console.ReadLine().ToLower();



            //string param = "latest/" + junaNumero + "?version=0";
            //Juna juna = await TrainsApi.GetJuna(param);

            //if (juna != null)
            //    Console.WriteLine(juna.timeTableRows);
            //else
            //{
            //    Console.WriteLine("Ei löytynyt :(");
            //}

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

                    //if (reitti == null)
                    //Console.WriteLine("\nJunaa ei löydy");
                    //else
                    
                }
                catch (FormatException)
                {

                    Console.WriteLine("-.-");
                    continue;
                }
                break;
            }


        }
    }
}