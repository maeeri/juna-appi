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
using System.Threading.Channels;


namespace JunaAppi
{
    class Program
    {
        //johanna taiteili tähän taas asciiartia
        private static readonly string AsciiArt = @"
                                                                        
 ____________________________________________________________________________
        __   __     _  __     _  ___        ___     _____    _____      ___
        //   //    /   //|   /   // |       // |    //    )  //    )    // 
-------//---//----/---//-| -/---//__|------//__|---//____/--//____/----//--
      //   //    /   //  | /   //   | === //   |  //       //         //   
_(___//___((____/___//___|/___//____|____//____|_//_______//_______ _//_ __

  ___________   _______________________________________^__  _______________
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
            Console.WriteLine("Anna junan numero");
            string junanNumero = Console.ReadLine();
            DateTime omaDateTime = DateTime.Now; //haussa pitää olla muodossa yyyy-MM-dd eikä kellonaikaa
            string lähtöPäivä = omaDateTime.ToString("yyyy-MM-dd"); //7.7. ei tämä itseasiassa vaadi myöskään sitä to universal datetimeksi muuttamista, joten se poistettu
            TrainTrackingNext[] trainTrackingList = await TrainsApi.GetLocation(lähtöPäivä, junanNumero); // Mari-Annen metodia hyödyntäen aseman 3-kirj. koodi aseman nimeksi
            string stationShortCode = (trainTrackingList[0].nextStation); //voisi vielä parannella sillä, että ei kaadu, vaikka syöttäisi junannumeron sellaisesta junasta, joka ei nyt kulussa
            Station seuraavaAsema = await GetStationByCodeAsync(stationShortCode);
            if (seuraavaAsema == null) //lisätty ehto, sillä ohjelma kaatuu jos syöttää junannumeron, joka ei ole juuri nyt kulussa
            {
                Console.WriteLine("Valitettavasti hakemallasi tiedolla ei löytynyt kulussa olevaa junaa.");
            }
            else
            {
                Console.WriteLine($"Juna {junanNumero}, seuraava asema: {seuraavaAsema.stationName}.");
            }
            PressKey();

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
            //Johannan visuaalisuuskoodi
            Console.BackgroundColor = ConsoleColor.Red; //Akin toiveväri tähän
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(AsciiArt);
            Console.WriteLine(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Yellow;

            //Akin valikko
            Console.WriteLine("  Vaihtoehtosi:\n" +
                              "  1) Mistä-Mihin\n" +
                              "  2) Hae seuraava asema\n" +
                              "  3) Hae raide, jolla juna pysähtyy\n" +
                              "  4) Junan Palvelut\n" +
                              "  5) Poistu");
            

            switch (Console.ReadLine())
            {
                case "1":
                    await MisMih();
                    return true;
                case "2":
                    await GetNextStation();
                    return true;
                case "3":
                    await FindTrack();
                    return true;
                case "4":
                    await ExtraOptions();
                    return true;
                case "5":
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
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Minä päivänä matkustat? (VVVV/KK/PP)");
                    paiva = ValidateDateTimeInput(Console.ReadLine());
                    Console.WriteLine("Minkä junan (numero) lähtöraiteen haluat hakea?");
                    junaNumero = ValidateIntInput(Console.ReadLine());
                    Console.WriteLine("Minkä aseman tiedot haluat?");
                    asemaInput = Console.ReadLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Asemaa, junaa tai raidetta ei lötynyt. :(");
                    continue;
                }

                break;
            }

            string asema = asemaInput[0].ToString().ToUpper() + asemaInput.Substring(1).ToLower();
            string junaHaku = $"{paiva.Date:yyyy-MM-dd}/{junaNumero}";
            TrainByDate[] juna = await TrainsApi.GetTrainByNumberAsync(junaHaku);
            var asemaOlio = await GetStationByNameAsync(asema);
            var asemanKoodi = asemaOlio.stationShortCode;


            string raide = await StationTrack(juna, asemanKoodi);

            if (raide != null)
            {
                Console.WriteLine($"Junasi numerolla {junaNumero} pysähtyy aseman {asema} laiturilla {raide}.");
            }
            else
            {
                Console.WriteLine("Hmm. Jostain syystä joko junaa, asemaa tai raidetta ei löytynyt.");
            }
            PressKey();
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

        //Mari-Annen metodi aseman hakuun asemakoodin perusteella
        public static async Task<Station> GetStationByCodeAsync(string stationShortCode)
        {
            Station[] asemat = await TrainsApi.GetStations();
            Station response = asemat.FirstOrDefault(x => x.stationShortCode.Equals(stationShortCode, StringComparison.OrdinalIgnoreCase));

            //jos asema ei ole null, palautetaan Station-olio ja jos on, palautetaan oletusarvo
            return (response != null ? response : default);
        }

        //metodi aseman hakuun nimen perusteella /Mari-Anne
        public static async Task<Station> GetStationByNameAsync(string stationName)
        {
            Station[] asemat = await TrainsApi.GetStations();
            Station response;
            try
            {
                response = asemat.FirstOrDefault(x =>
                    x.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase));
                if (response != null)
                    return response;
                else
                {
                    stationName += " asema";
                    response = asemat.FirstOrDefault(x =>
                        x.stationName.Equals(stationName, StringComparison.OrdinalIgnoreCase));
                    return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Asemaa ei löytynyt");
                return default;
            }
        }

        private static async Task MisMih()
        {
            //Akin Väserrykset
            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Mistä olet Lähdössä");
                    string lahto = Console.ReadLine().ToUpper();

                    Console.WriteLine("Minne olet Menossa");
                    string saapuminen = Console.ReadLine().ToUpper();



                    JunaAppiReitit.ReittiLatest[] reitti = await TrainsApi.HaeReitti(lahto, saapuminen);

                    if (reitti == null)
                        Console.WriteLine("\nJunaa ei löydy");

                    else
                        Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\tReittisi tiedot:");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\tLähtö asema: " + lahto);
                    Console.WriteLine("\tSaapuminen asemalle: " + saapuminen);
                    Rejtti(reitti);
                    PressKey();
                    
                }
                catch (FormatException)
                {
                    Console.WriteLine("-.-");
                    continue;
                }

                break;
            }
        }

        private static void Rejtti(JunaAppiReitit.ReittiLatest[] reitti)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\tJunan numero: {reitti[0].trainNumber}");
            Console.WriteLine($"\tJunan lähtöpäivä: {reitti[0].departureDate}");
           
        }


        //Annan rakentama metodi
        private static async Task ExtraOptions()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Minä päivänä juna lähtee? esim. YYYY-MM-DD");
            string date = Console.ReadLine();

            Console.WriteLine("Junan numero:");
            int junanro = Convert.ToInt32(Console.ReadLine());


            Vaunu vaunu = await TrainsApi.HaeJunanPalvelut(date, junanro);

            Console.WriteLine(date + junanro);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Haluaisin tarkistaa, onko junassa:\n" +
                              "A) lemmikki sallittu\n" +
                              "B) leikkipaikka\n" +
                              "C) ravintolavaunu\n" +
                              "D) esteettömyys\n");
            switch (Console.ReadLine().ToUpper())
            {

                case "A":
                {
                    Console.ForegroundColor = ConsoleColor.White;
                        var pet = vaunu.journeySections[0].wagons.Any(pet => pet.pet == true);
                    Console.WriteLine(pet
                        ? "Lemmikinne on tervetullut!"
                        : "Valitettavasti lemmikit ei ole sallittuja.");
                    PressKey();
                    break;
                }
                case "B":
                {
                    var playground = vaunu.journeySections[0].wagons.Any(playground => playground.playground == true);
                    Console.WriteLine(playground
                        ? "Leikkipaikka löytyy. Tervetuloa!"
                        : "Valitettavasti tässä vuorossa ei ole leikkipaikkaa.");
                    PressKey();
                    break;
                }
                case "C":
                {
                    var catering = vaunu.journeySections[0].wagons.Any(catering => catering.catering == true);
                    Console.WriteLine(catering
                        ? "Junassa on ravintolavaunu. Tervetuloa!"
                        : "Valitettavasti tässä vuorossa ei ole ravintolavaunua.");
                    PressKey();
                    break;
                }
                case "D":
                {
                    var disabled = vaunu.journeySections[0].wagons.Any(disabled => disabled.disabled == true);
                    Console.WriteLine(disabled
                        ? "Valitsemanne juna on esteetön. Tervetuloa!"
                        : "Valitettavasti tämä vuoro ei ole esteetön.");
                    PressKey();
                    break;
                }
            }

        }

        //validates int input /Mari-Anne
        public static int ValidateIntInput(string input)
        {
            int newInput;
            while (true)
            {
                if (int.TryParse(input, out newInput))
                {
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Ole hyvä ja anna pelkästään numero:");
                    input = Console.ReadLine();
                    continue;
                }
            }
            return newInput;
        }

        //validates input for datetime/Mari-Anne
        public static DateTime ValidateDateTimeInput(string input)
        {
            DateTime newInput;
            while (true)
            {
                if (DateTime.TryParse(input, out newInput))
                {
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Ole hyvä ja anna päivämäärä muodossa (VVVV/KK/PP):");
                    input = Console.ReadLine();
                    continue;
                }
            }
            return newInput;
        }

        //Johanna toi tänne omasta vanhasta koodistaan PressKey -toiminnon, joka helpottaa await -async käyttöä
        private static void PressKey()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nPaina 'Enter' jatkaaksesi... ");
            Console.ReadKey(true);

        }
    }
}
