﻿using System;
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

<<<<<<< HEAD
        //Johanna miettii metodia, joka hakisi seuraavan pysäkin
=======

        //Johanna miettiin metodia, joka hakisi seuraavan pysäkin
>>>>>>> b7ed01be549713335ddf39bd2612ef8c45945d32
        private static async Task GetNextStation()
        {
            //junan numeron perusteella, 
            DateTime omaDateTime = DateTime.Now; //haussa pitää olla muodossa yyyy-MM-dd eikä kellonaikaa
            string lähtöPäivä = omaDateTime.ToUniversalTime().ToString("yyyy-MM-dd");
            Console.WriteLine("Annan junan numero");
            string junanNumero = Console.ReadLine();
            TrainTrackingNext[] trainTrackingList = await TrainsApi.GetLocation(lähtöPäivä, junanNumero);
            Console.WriteLine(trainTrackingList[0].nextStation); //tää toimii nyt, mutta palauttaa vain sen lyhenteen!


            


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
            {//Akin Väserrykset
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


    }
}







//Console.WriteLine("Mitä pitäisi olla vaunussa?");
//string answer = Console.ReadLine();

//Console.WriteLine(TrainsApi.HaeJunanPalvelut());
//Console.ReadLine();
