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
            Console.WriteLine("Annan junan numero");
            string junanNumero = Console.ReadLine();
            DateTime omaDateTime = DateTime.Now; //haussa pitää olla muodossa yyyy-MM-dd eikä kellonaikaa
            string lähtöPäivä = omaDateTime.ToString("yyyy-MM-dd"); //7.7. ei tämä itseasiassa vaadi myöskään sitä to universal datetimeksi muuttamista, joten se poistettu
            TrainTrackingNext[] trainTrackingList = await TrainsApi.GetLocation(lähtöPäivä, junanNumero); // Mari-Annen metodia hyödyntäen aseman 3-kirj. koodi aseman nimeksi
            string stationShortCode = (trainTrackingList[0].nextStation); //tää toimii nyt, mutta palauttaa vain sen lyhenteen!
            Station seuraavaAsema = await GetStationByCodeAsync(stationShortCode);
            Console.WriteLine($"Juna {junanNumero}, seuraava asema: {seuraavaAsema.stationName}.");
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
            Console.WriteLine("Vaihtoehtosi:\n" +
                              "1) Mistä-Mihin\n" +
                              "2) Hae seuraava asema\n" +
                              "3) Hae raide, jolla juna pysähtyy\n" +
                              "4) Junan Palvelut\n" +
                              "5) Poistu");
            

            switch (Console.ReadLine())
            {
                case "1":
                    MisMih();
                    return true;
                case "2":
                    await GetNextStation();
                    return true;

                case "3":
                    FindTrack();

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
                    Console.WriteLine("Mistä olet Lähdössä");
                    string lahto = Console.ReadLine().ToUpper();

                    Console.WriteLine("Minne olet Menossa");
                    string saapuminen = Console.ReadLine().ToUpper();



                    JunaAppiReitit.ReittiLatest[] reitti = await TrainsApi.HaeReitti(lahto, saapuminen);

                    if (reitti == null)
                        Console.WriteLine("\nJunaa ei löydy");

                    else

                    Console.WriteLine("\tReittisi tiedot:");
                    Console.WriteLine("\tLähtö asema: " + lahto);
                    Console.WriteLine("\tSaapuminen asemalle: " + saapuminen);
                    Rejtti(reitti);
                    
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

            Console.WriteLine($"\tJunan numero: {reitti[0].trainNumber}");
            Console.WriteLine($"\tJunan lähtöpäivä: {reitti[0].departureDate}");
            Console.WriteLine("\nVaihtoehtosi:\n1) Mistä-Mihin\n2) Ajoissa\n3) Seuraava Pysäkki\n4) Vaihtoraide\n5) Junan Palvelut\n6) Poistu");
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

<<<<<<< HEAD
            Console.WriteLine("\nHaluaisin tarkistaa, onko junassa:\n" +
                              "A) lemmikki sallittu\n" +
                              "B) leikkipaikka\n" +
                              "C) ravintolavaunu\n" +
                              "D) esteettömyys\n");


            switch (Console.ReadLine())
=======
            switch (inputChoice)
>>>>>>> 7f137c6de878de3610305430397c1c82546bbbfd
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
                    Console.WriteLine("Ole hyvä ja anna päivämäärä muodossa (VVVV/KK/PP):");
                    input = Console.ReadLine();
                    continue;
                }
            }
            return newInput;
        }

        //Console.WriteLine("Mitä pitäisi olla vaunussa?");
        //string answer = Console.ReadLine();

        //Console.WriteLine(TrainsApi.HaeJunanPalvelut());
        //Console.ReadLine();

        //Johanna toi tänne omasta vanhasta koodistaan PressKey -toiminnon, joka helpottaa await -async käyttöä
        private static void PressKey()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nPaina 'Enter' jatkaaksesi... ");
            Console.ReadKey(true);

        }
    }
}
