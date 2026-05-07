using APM_Crate.Models;
using APM_Crate.ViewModels;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace APM_Crate.Models.RestApiModel
{
    public static class RestModel
    {
        static HttpClient client = new HttpClient();

        public const string  DeviceFamily = "PLC (Крейт)";
        public const string APM = $"АРМ проверки и настройки контроллеров ТИК-Крейт";

        private const string mes = "Configurations";

        public static string IP = "https://172.22.64.138:5000/";


        //public static void SetUri()
        //{
        //    client.BaseAddress = new Uri(IP);
        //}
        public static async Task<string> Post(Config config)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync($"{IP}{mes}", config);
            response.EnsureSuccessStatusCode();
           return await response.Content.ReadAsStringAsync();
        }
        public static async Task<ushort> GetLastSerialNumber()
        {
            HttpResponseMessage response = await client.GetAsync($"{IP}{mes}/last-serial/{DeviceFamily}");
            response.EnsureSuccessStatusCode();
            return Convert.ToUInt16( await response.Content.ReadAsStringAsync());
        }
        public static async Task<List<Config>> GetListRecord(
             int limit = 50,
             uint? serialNumber = null,
             string orderNumber = null,
             string deviceType = null,
             string deviceFamily = null,
             string username = null,
             string date = null,
             string arm = null,
             bool? isActual = true)
        {
            var queryParams = new List<string>();
            if (limit != 50) queryParams.Add($"limit={limit}");
            if (serialNumber.HasValue) queryParams.Add($"serialNumber={serialNumber.Value}");
            if (!string.IsNullOrEmpty(orderNumber)) queryParams.Add($"orderNumber={Uri.EscapeDataString(orderNumber)}");
            if (!string.IsNullOrEmpty(deviceType)) queryParams.Add($"deviceType={Uri.EscapeDataString(deviceType)}");
            if (!string.IsNullOrEmpty(deviceFamily)) queryParams.Add($"deviceFamily={Uri.EscapeDataString(deviceFamily)}");
            if (!string.IsNullOrEmpty(username)) queryParams.Add($"userName={Uri.EscapeDataString(username)}");
            if (!string.IsNullOrEmpty(date)) queryParams.Add($"date={Uri.EscapeDataString(date)}");
            if (!string.IsNullOrEmpty(arm)) queryParams.Add($"arm={Uri.EscapeDataString(arm)}");
            if (isActual.HasValue) queryParams.Add($"isActual={isActual.Value.ToString().ToLower()}");

            string queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            string requestUrl = $"{IP}{mes}/{queryString}";

            HttpResponseMessage response = await client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            // Десериализация JSON в List<Config>
            var configs = JsonSerializer.Deserialize<List<Config>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return configs;
        }
        public static async Task<Config> GetRecordByID(string ID)
        {
            return await client.GetFromJsonAsync<Config>($"{IP}{mes}/{ID}");
        }
        public static async Task Put(string Id,Config config)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"{IP}{mes}/{Id}", config);
            response.EnsureSuccessStatusCode();
        }
        public static async Task Delete(string Id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"{IP}{mes}/{Id}");
            response.EnsureSuccessStatusCode();
        }
        public static async Task<bool> GetAPIStatus()
        {
            try
            {
                List<Config> configs = await GetListRecord();
                if (configs.Count > 0)
                {
                    return true;
                }
                else return false;
            }
            catch(HttpRequestException)
            {
                return false;
            }
        }
    }
}
