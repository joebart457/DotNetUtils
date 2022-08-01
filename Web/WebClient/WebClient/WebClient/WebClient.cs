using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Extensions;

namespace Web
{
    public static class WebClient
    {
        private static string _baseUrl = "";
        public static void ConfigureBaseUrl(string newBase)
        {
            _baseUrl = newBase;
        }

        public static async Task DownloadAsync(string resourceUri, string filePath)
        {
            HttpClient httpClient = new HttpClient();
            await httpClient.DownloadFileAsync(CreateUri(resourceUri), filePath);
        }

        public static async Task<HttpResponseMessage> GetAsync(string resourceUri)
        {
            HttpClient httpClient = new HttpClient();
            return await httpClient.GetAsync(AddBase(resourceUri)).ConfigureAwait(false);
        }
        public static async Task<TyResult> GetAsync<TyResult>(string resourceUri)
        {
            return await (await GetAsync(resourceUri)).ToResult<TyResult>();
        }

        private static async Task<HttpResponseMessage> PostAsync<TyRequestData>(string resourceUri, TyRequestData data)
        {
            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = CreateUri(resourceUri),
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"),
            };

            return await httpClient.SendAsync(request).ConfigureAwait(false);
        }

        private static async Task<TyResult> PostAsync<TyRequestData, TyResult>(string resourceUri, TyRequestData data)
        {
            return await (await PostAsync(resourceUri, data)).ToResult<TyResult>();
        }

        public static async Task<HttpResponseMessage> PostFilesAsync(string resourceUri, string[] filePaths)
        {
            HttpClient httpClient = new HttpClient();
            return await httpClient.PostFilesAsync(AddBase(resourceUri), filePaths);
        }

        public static async Task<TyResult> PostFilesAsync<TyResult>(string resourceUri, string[] filePaths)
        {
            return await (await PostFilesAsync(resourceUri, filePaths)).ToResult<TyResult>();
        }

        public static async Task<HttpResponseMessage> DeleteAsync(string resourceUri)
        {
            HttpClient httpClient = new HttpClient();
            return await httpClient.DeleteAsync(AddBase(resourceUri)).ConfigureAwait(false);;
        }

        public static async Task<TyResult> DeleteAsync<TyResult>(string resourceUri)
        {
            var response = await DeleteAsync(resourceUri);
            return await response.ToResult<TyResult>();
        }

        private static Uri CreateUri(string resourceUri)
        {
            return new Uri(AddBase(resourceUri));
        }

        private static string AddBase(string resourceUri)
        {
            return $"{_baseUrl}{resourceUri}";
        }

    }
}
