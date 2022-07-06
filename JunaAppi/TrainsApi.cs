using System;
using System.Collections.Generic;
using System.Text;
using APIHelpers;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace JunaAppi
{
    public static class TrainsApi
    {
        const string url = "https://rata.digitraffic.fi/api/v1/";

        //tehty ryhmäkoodauksena, Johanna kirjoitusoperaattorina muu ryhmä neuvoo
        public static async Task<Juna> GetJuna(string input)
        {
            string urlParams = "trains/" + input;
            Juna response = await ApiHelper.RunAsync<Juna>(url, urlParams);
            return response;
              
        }
        //johanna teki edellisen mallin mukaan
        public static async Task<TrainTrackingLatest> GetLocation(string junanNumero)
        {

            string urlParams = $"train-tracking/latest/{junanNumero}"; //muutettu train-trackinginsta train-tracking/latest/ koska tämän perään voi sijoittaa {train_number} arvon
            TrainTrackingLatest response = await ApiHelper.RunAsync<TrainTrackingLatest>(url, urlParams);
            return response;
        }

        //Otetaan yhetyttä Apiin (Annan versio ryhmän avulla).
        public static async Task<Wagon> HaeJunanPalvelut(string input)
        {
            string urlParams = "compositions/" + input;
            Wagon response = await ApiHelper.RunAsync<Wagon>(url, urlParams);
            return response;
        }


        //Akin junan haku apista
        public static async Task<Reitti> HaeReitti(string lahto, string saapuminen)
        {
            string urlParams = "live-trains/station/" + lahto + "/" + saapuminen;

            Reitti reitti = await ApiHelper.RunAsync<Reitti>(url, urlParams);

            return reitti;
        }

        //Materiaalista kopioitu GetStations-metodi
        public static async Task<Station[]> GetStations()
        {
            string urlParams = "metadata/stations";

            var response = await ApiHelper.RunAsync<Station[]>(url, urlParams);
            return response;
        }

        //Mari-Annen metodi aseman nimihakuun
        public static async Task<string> HaeAsemanNimi(string stationShortCode)
        {
            Station[] asemat = await GetStations();
            string response = null;
            var asemanNimi = asemat
                .Where(x => x.stationShortCode == stationShortCode)
                .Select(x => x.stationName);

            //koska voidaan olettaa, että yhdellä asemakoodilla saadaan tulokseksi yksi asema, haetaan aseman nimi foreach-lauseella sijoittaen se muuttujaan
            foreach (var asema in asemanNimi)
            {
                response = asema;
            }

            //jos aseman nimi ei ole null, palautetaan nimi, jo on, palautetaan viesti, ettei asemaa löytynyt
            return (response != null ? response : "Ei löytynyt :(");
        }
    }
}
