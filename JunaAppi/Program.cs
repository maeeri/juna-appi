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
<<<<<<< HEAD
            TrainTrackingLatest trackedTrain = await TrainsApi.GetLocation(lähtöpäivä, junanNumero);
        }


        static async Task Main(string[] args)

=======
            TrainTrackingNext[] trainTrackingList = await TrainsApi.GetLocation(lähtöPäivä, junanNumero);
            Console.WriteLine(trainTrackingList[0].nextStation); //tää toimii nyt, mutta palauttaa vain sen lyhenteen!
            
        }

        static void Main(string[] args)
>>>>>>> 668e5c998ce9ca4d4fe2408155bd80d34fcc9288
        {
            bool valikko = true;
            while (valikko)
            {
                valikko = await MainMenu();
            }
        }

<<<<<<< HEAD
        private static async Task<bool> MainMenu()
=======
        private static bool MainMenu()
>>>>>>> ce1b69605d02d3aa3c0ba025b174b9bd69c02dff
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
<<<<<<< HEAD
            Console.WriteLine("Vaihtoehtosi:\n1) Mistä-Mihin\n2) Ajoissa\n3) Seuraava Pysäkki\n4) Vaihtoraide\n5) Junan Palvelut\n6) Poistu");

=======
            Console.WriteLine("Vaihtoehtosi:\n" +
                              "1) Mistä-Mihin\n" +
                              "2) Ajoissa\n" +
                              "3) Seuraava Pysäkki\n" +
                              "4) Hae raide, jolla juna pysähtyy\n" +
                              "5) Junan Palvelut\n" +
                              "6) Poistu");
>>>>>>> ce1b69605d02d3aa3c0ba025b174b9bd69c02dff
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

<<<<<<< HEAD
        private static async Task ExtraOptions()
=======
        private static void Rejtti(JunaAppiReitit.ReittiLatest[] reitti)
>>>>>>> ce1b69605d02d3aa3c0ba025b174b9bd69c02dff
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
<<<<<<< HEAD


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




            


            //Console.WriteLine("Mitä pitäisi olla vaunussa?");
            //string answer = Console.ReadLine();

            //Console.WriteLine(TrainsApi.HaeJunanPalvelut());
            //Console.ReadLine();
=======
                case "4":
                {
                    var pet = vaunu.journeySections[0].wagons.Any(pet => pet.pet == true);
                    Console.WriteLine(pet);
                    break;
                }*/
>>>>>>> ce1b69605d02d3aa3c0ba025b174b9bd69c02dff
        }

<<<<<<< HEAD
<<<<<<< HEAD
=======
    
>>>>>>> 668e5c998ce9ca4d4fe2408155bd80d34fcc9288
=======
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
    }
}
>>>>>>> ce1b69605d02d3aa3c0ba025b174b9bd69c02dff
