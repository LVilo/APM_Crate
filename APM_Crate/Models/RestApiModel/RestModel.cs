using APM_Crate.Models;
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
        public const string APM = "АРМ проверки и настройки контроллеров ТИК-Крейт";

        private const string mes = "Configurations";

        public static string IP = "https://172.22.64.138:5000/";

        public static void SetUri()
        {
            client.BaseAddress = new Uri(IP);
        }
        public static async Task<string> Post(Config config)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(mes, config);
            response.EnsureSuccessStatusCode();
           return await response.Content.ReadAsStringAsync();
        }
        public static async Task<string> GetLastSerialNumber()
        {
            HttpResponseMessage response = await client.GetAsync($"{mes}/last-serial/{DeviceFamily}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public static async Task<List<Config>> GetListRecord(
             int limit = 50,
             uint? serialNumber = null,
             string orderNumber = null,
             string deviceType = null,
             string deviceFamily = DeviceFamily,
             string arm = APM,
             bool? isActual = true)
        {
            var queryParams = new List<string>();
            if (limit != 50) queryParams.Add($"limit={limit}");
            if (serialNumber.HasValue) queryParams.Add($"serialNumber={serialNumber.Value}");
            if (!string.IsNullOrEmpty(orderNumber)) queryParams.Add($"orderNumber={Uri.EscapeDataString(orderNumber)}");
            if (!string.IsNullOrEmpty(deviceType)) queryParams.Add($"deviceType={Uri.EscapeDataString(deviceType)}");
            if (!string.IsNullOrEmpty(deviceFamily)) queryParams.Add($"deviceFamily={Uri.EscapeDataString(deviceFamily)}");
            if (!string.IsNullOrEmpty(arm)) queryParams.Add($"arm={Uri.EscapeDataString(arm)}");
            if (isActual.HasValue) queryParams.Add($"isActual={isActual.Value.ToString().ToLower()}");

            string queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            string requestUrl = $"{mes}/{queryString}";

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
            return await client.GetFromJsonAsync<Config>($"{mes}/{ID}");
        }
        public static async Task Put(string Id,Config config)
        {
            await client.PutAsJsonAsync($"{mes}/{Id}", config);
        }
        public static async Task Delete(string Id)
        {
            await client.DeleteAsync($"{mes}/{Id}");
        }
        
    }
}
