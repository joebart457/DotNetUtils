using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task DownloadFileAsync(this HttpClient client, Uri uri, string fileName)
        {
            using (var s = await client.GetStreamAsync(uri))
            {

                using (var fs = new FileStream(fileName, FileMode.Create))
                {
                    await s.CopyToAsync(fs);
                }
            }
        }

        public static async Task<HttpResponseMessage> PostFilesAsync(this HttpClient client, string uri, string[] filePaths)
        {
            
            using (var formData = new MultipartFormDataContent())
            {
                foreach(string file in filePaths)
                {
                    var fs = File.OpenRead(file);
                    HttpContent fileStreamContent = new StreamContent(fs);
                    formData.Add(fileStreamContent, Path.GetFileName(file), Path.GetFileName(file));
                }
                
                var response = await client.PostAsync(uri, formData);
                return response;
            }
        }

        public static async Task<TyResult> ToResult<TyResult>(this HttpResponseMessage response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                TyResult? finalObject = JsonConvert.DeserializeObject<TyResult>(result);
                if (finalObject == null) throw new Exception($"unable to convert response to valid object. Response was {result}");
                return finalObject;
            }
            throw new Exception($"received [Status:{response?.StatusCode}] when attempting to retrieve resource at {response?.RequestMessage?.RequestUri}");
        }
    }
}
