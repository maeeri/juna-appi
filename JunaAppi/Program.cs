using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using APIHelpers;
using JunaAppiLatest;


namespace JunaAppi
{
    class Program
    {

        //johanna taiteili tähän taas asciiartia
        private static readonly string AsciiArt = @"
                                                                        
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


        //Johanna miettiin metodia, joka hakisi seuraavan pysäkin
        private static async Task GetNextStation()
        {
            //junan numeron perusteella, 
            Console.WriteLine("Syötä päivämäärä");
            string lähtöPäivä = Console.ReadLine();
            Console.WriteLine("Annan junan numero");
            string junanNumero = Console.ReadLine();
            TrainTrackingLatest trackedTrain = await TrainsApi.GetLocation(lähtöpäivä, junanNumero);
        }


        static async Task Main(string[] args)

        {
            bool valikko = true;
            while (valikko)
            {
                valikko = await MainMenu();
            }
        }

        private static async Task<bool> MainMenu()
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
                    await ExtraOptions();
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
            //Station asema = await TrainsApi.GetStationByNameAsync("hämeenlinna");
            //if (asema != null)
            //    Console.WriteLine(asema.stationName);
            //else
            //    Console.WriteLine("Asemaa ei löytynyt :(");



            Console.WriteLine("Minkä junan (numero) lähtöraiteen haluat hakea?");
            int.TryParse(Console.ReadLine(), out int junaNumero);
            //Console.WriteLine("Minkä aseman tiedot haluat?");
            //string asemaRaide = Console.ReadLine().ToLower();

            string tanaan = DateTime.Today.ToString("yyyy-MM-dd");
            string param = $"{tanaan}/{junaNumero.ToString()}/";
            LatestTrain[] junat = await TrainsApi.GetTrainByNumber(param);

            Console.WriteLine();
            foreach (var juna in junat)
            {
                if (junat != null)
                {
                    Console.WriteLine(juna.Property1[0].timeTableRows[0].commercialTrack);
                }
                else
                {
                    Console.WriteLine("Ei löytynyt :(");
                }

                Console.WriteLine("Hiya");
            }
        }

        //Mari-Annen metodi juna-aseman ja junan yhdistämiseen
        private static async Task StationTrack()
        {

        }

        private static async Task MisMih()
        {
            //Akin Väserrykset
            while (true)
            {
                try
                {
                    Console.WriteLine("Mistä olet Lähdössä");
                    string lahto = Console.ReadLine().ToUpper();

                    Console.WriteLine("Minne olet Menossa");
                    string saapuminen = Console.ReadLine().ToUpper();

                    JunaAppiReitit.ReittiLatest[] reitti = await TrainsApi.HaeReitti(lahto, saapuminen);

                    if (reitti == null)
                        Console.WriteLine("\nJunaa ei löydy");


                    else
                    Console.WriteLine("Lähtö asema: " + lahto);
                    Console.WriteLine("Saapuminen asemalle: " + saapuminen);
                    Rejtti(reitti);
                    
                        
                }
                catch (FormatException)
                {

                    Console.WriteLine("_(-.-)_");
                    continue;
                }

                break;
            }
        }
            private static void Rejtti(JunaAppiReitit.ReittiLatest[] reitti)
             {
            
                Console.WriteLine("  Reittisi tiedot:");
                Console.WriteLine($"  Junan numero: {reitti[1].trainNumber}");
                Console.WriteLine($"  Junan lähtöpäivä: {reitti[1].departureDate}");
                Console.WriteLine("\nVaihtoehtosi:\n1) Mistä-Mihin\n2) Ajoissa\n3) Seuraava Pysäkki\n4) Vaihtoraide\n5) Junan Palvelut\n6) Poistu");
                //Console.WriteLine($"  Reitti lähtöasema: {reitti[1].timeTableRows.stationShortCode}");
            }

        private static async Task ExtraOptions()
        {

            //var response = APIHelpers.RunAsync<Vaunu>(url, urlParams);
            Console.WriteLine("Minä päivänä juna lähtee?");
            string date = Console.ReadLine();

            Console.WriteLine("Junan numero:");
            int junanro = Convert.ToInt32(Console.ReadLine());


            Vaunu vaunu = await TrainsApi.HaeJunanPalvelut(date, junanro);

            Console.WriteLine(date + " " + junanro);

            //var pet = vaunu.journeySections[0].wagons.Any(pet => pet.pet == true);
            //Console.WriteLine(pet);

            Console.WriteLine("Haluaisin tarkistaa, onko junassa:\nA) lemmikki sallittu\nB) leikkipaikka \nC) ravintolavaunu\nD) esteettömyys");

            switch (Console.ReadLine())
            {
                    
                case "A":
                {
                    var pet = vaunu.journeySections[0].wagons.Any(pet => pet.pet == true);

                    Console.WriteLine(pet == true
                        ? "Lemmikinne on tervetullut!"
                        : "Valitettavasti lemmikit ei ole sallittuja.");

                    Console.ReadLine();
                    break;

                }
                case "B":
                {
                    var playground = vaunu.journeySections[0].wagons.Any(playground => playground.playground == true);


                    Console.WriteLine(playground == true
                        ? "Leikkipaikka löytyy. Tervetuloa!"
                        : "Valitettavasti tässä vuorossa ei ole leikkipaikkaa.");

                    Console.ReadLine();
                    break;
                }
                case "C":
                {
                    var catering = vaunu.journeySections[0].wagons.Any(catering => catering.catering == true);

                    Console.WriteLine(catering == true
                        ? "Junassa on ravintolavaunu. Tervetuloa!"
                        : "Valitettavasti tässä vuorossa ei ole ravintolavaunua.");

                        Console.ReadLine();
                    break;
                }
                case "D":
                {
                    var disabled = vaunu.journeySections[0].wagons.Any(disabled => disabled.disabled == true);

                    Console.WriteLine(disabled == true
                        ? "Valitsemanne juna on esteetön. Tervetuloa!"
                        : "Valitettavasti tämä vuoro ei ole esteetön.");

                        Console.ReadLine();

                    break;
                }


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

