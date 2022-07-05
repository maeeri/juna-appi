using System;
using System.Collections.Generic;
using System.Text;
using APIHelpers;
using System.Threading.Tasks;

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
        public static async Task<TrainTrackingLatest> GetLocation(string input)
        {
            string urlParams = "train-tracking/" + input;
            TrainTrackingLatest response = await ApiHelper.RunAsync<TrainTrackingLatest>(url, urlParams);
            return response;
        }

        //Otetaan yhetyttä Apiin (Annan versio ryhmän avulla).
        public static async Task<Vaunu> HaeJunanPalvelut(string input)
        {
            string urlParams = "compositions/" + input;
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
    }
}
