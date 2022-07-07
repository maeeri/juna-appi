using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using APIHelpers;
using JunaAppiLatest;
using System.IO;


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

        //Johanna miettii metodia, joka hakisi seuraavan pysäkin

        private static async Task GetNextStation()
        {
            //junan numeron perusteella, oletuksena, että hakijaa kiinnostaa esim. juna jossa itse matkustaa, joten ohjelma antaa automaattisesti päivämääräksi /klonajaksi sen hetkisen ajan
            DateTime omaDateTime = DateTime.Now; //haussa pitää olla muodossa yyyy-MM-dd eikä kellonaikaa
            string lähtöPäivä = omaDateTime.ToString("yyyy-MM-dd"); //7.7. ei tämä itseasiassa vaadi myöskään sitä to universal datetimeksi muuttamista, joten se poistettu
            Console.WriteLine("Annan junan numero");
            string junanNumero = Console.ReadLine();
            TrainTrackingNext[] trainTrackingList = await TrainsApi.GetLocation(lähtöPäivä, junanNumero);
            Console.WriteLine(trainTrackingList[0].nextStation); //tää toimii nyt, mutta palauttaa vain sen lyhenteen!
            //string stationShortCode = Console.ReadLine();
           
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
            Console.BackgroundColor = ConsoleColor.Red; //Akin toiveväri tähän
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
                    ExtraOptions();
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
            DateTime paiva;
            int junaNumero;
            string asemaInput;

            while (true)
            {
                try
                {
                    Console.WriteLine("Minä päivänä matkustat? (VVVV/KK/PP)");
                    DateTime.TryParse(Console.ReadLine(), out paiva);
                    Console.WriteLine("Minkä junan (numero) lähtöraiteen haluat hakea?");
                    int.TryParse(Console.ReadLine(), out junaNumero);
                    Console.WriteLine("Minkä aseman tiedot haluat?");
                    asemaInput = Console.ReadLine();
                }
                catch
                {
                    continue;
                }
                break;
            }

            string asema = asemaInput[0].ToString().ToUpper() + asemaInput.Substring(1).ToLower();
            string junaHaku = $"{paiva.Date:yyyy-MM-dd}/{junaNumero}";
            TrainByDate[] juna = await TrainsApi.GetTrainByNumberAsync(junaHaku);
            var asemaOlio = await TrainsApi.GetStationByNameAsync(asema);
            var asemanKoodi = asemaOlio.stationShortCode;

            string raide = await StationTrack(juna, asemanKoodi);

            if (raide != null)
            {
                Console.WriteLine($"Junasi {junaNumero} pysähtyy aseman {asema} laiturilla {raide}.");
            }
            else
            {
                Console.WriteLine("Hmm. Jostain syystä joko junaa, asemaa tai raidetta ei löytynyt.");
            }
        }

        //Mari-Annen metodi juna-aseman ja junan yhdistämiseen juna = TrainByDate-olio, koodi = aseman koodi
        private static async Task<string> StationTrack(TrainByDate[] juna, string koodi)
        {
            string raide = null;

            foreach (var vali in juna)
            {
                foreach (var pysahdys in vali.timeTableRows)
                {
                    if (pysahdys.stationShortCode == koodi)
                        raide = pysahdys.commercialTrack;
                }
            }
            return raide;
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

            private static async Task ExtraOptions()
            {

            //var response = APIHelpers.RunAsync<Vaunu>(url, urlParams);
            Console.WriteLine("Minä päivänä juna lähtee?");
            string date = Console.ReadLine();

            Console.WriteLine("Junan numero:");
            int junanro = Convert.ToInt32(Console.ReadLine());


            Vaunu vaunu = await TrainsApi.HaeJunanPalvelut(date, junanro);

            Console.WriteLine(date + junanro);

            var pet = vaunu.journeySections[0].wagons.Any(pet => pet.pet == true);
            Console.WriteLine(pet);

            /*Console.WriteLine("Haluaisin tarkistaa, onko junassa:\n1) lemmikki sallittu\n2) leikkipaikka \n3) ravintolavaunu\n4) inva-paikat ");
            string inputChoice = Console.ReadLine();

            switch (inputChoice)
            {
                case "1":
                {
                    var pet = vaunu.journeySections[0].wagons.Any(pet => pet.pet == true);
                    Console.WriteLine(pet);
                    break;
                }
                case "2":
                {
                    var pet = vaunu.journeySections[0].wagons.Any(pet => pet.pet == true);
                    Console.WriteLine(pet);
                    break;
                }
                case "3":
                {
                    var pet = vaunu.journeySections[0].wagons.Any(pet => pet.pet == true);
                    Console.WriteLine(pet);
                    break;
                }
                case "4":
                {
                    var pet = vaunu.journeySections[0].wagons.Any(pet => pet.pet == true);
                    Console.WriteLine(pet);
                    break;
                }*/

            }



            


            //Console.WriteLine("Mitä pitäisi olla vaunussa?");
            //string answer = Console.ReadLine();

            //Console.WriteLine(TrainsApi.HaeJunanPalvelut());
            //Console.ReadLine();
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

    
