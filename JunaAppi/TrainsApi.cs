using System;
using System.Collections.Generic;
using System.Text;
using APIHelpers;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using JunaAppiLatest;

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
        public static async Task<TrainTrackingNext[]> GetLocation(string lähtöpäivä, string junanNumero) 
        {

            string urlParams = $"train-tracking/{lähtöpäivä}/{junanNumero}"; //muutettu 6.7. train-tracking/lähtöpäivä+junanNumero -hauksi
            TrainTrackingNext[] response = await ApiHelper.RunAsync<TrainTrackingNext[]>(url, urlParams);
            return response;
        }

        //Otetaan yhetyttä Apiin (Annan versio ryhmän avulla).

        public static async Task<Vaunu> HaeJunanPalvelut(string date, int junanro)
        {
            string urlParams = "compositions/" + date + "/" + junanro;
            Vaunu response = await ApiHelper.RunAsync<Vaunu>(url, urlParams);
            return response;
        }


        //Akin junan haku apista
        public static async Task<Reitti> HaeReitti(string lahto, string saapuminen)
        {
            string urlParams = "live-trains/station/" + lahto + "/" + saapuminen;

            Reitti reitti = await ApiHelper.RunAsync<Reitti>(url, urlParams);

            return reitti;
        }

        //Materiaalista häpeilemättä kopioitu GetStations-metodi
        public static async Task<Station[]> GetStations()
        {
            string urlParams = "metadata/stations";

            Station[] response = await ApiHelper.RunAsync<Station[]>(url, urlParams);
            return response;
        }

        //Mari-Annen tekemä muokkaus LatestTrain-olion hakuun
        public static async Task<TrainByDate[]> GetTrainByNumberAsync(string input)
        {
            string urlParams = "trains/" + input;
            TrainByDate[] response = await ApiHelper.RunAsync<TrainByDate[]>(url, urlParams);
            return response;
        }

        //Mari-Annen metodi aseman hakuun asemakoodin perusteella
        public static async Task<Station> GetStationByCodeAsync(string stationShortCode)
        {
            Station[] asemat = await GetStations();
            Station response = asemat.FirstOrDefault(x => x.stationShortCode.Equals(stationShortCode, StringComparison.OrdinalIgnoreCase));

            //jos asema ei ole null, palautetaan Station-olio ja jos on, palautetaan oletusarvo
            return (response != null ? response : default);
        }

        //metodi aseman hakuun nimen perusteella /Mari-Anne
        public static async Task<Station> GetStationByNameAsync(string stationName)
        {
            Station[] asemat = await GetStations();
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
    }
}
