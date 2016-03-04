using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Windows.Data.Json;

namespace SharkData.Models
{
    class DataRetriever
    {
        private string _apiRootUrl;

        public DataRetriever(string apiRootUrl)
        {
            _apiRootUrl = apiRootUrl.TrimEnd('/');
        }

        public async Task<IEnumerable<Shark>> GetShark()
        {
            HttpClient client = new HttpClient();

            Uri requestUri = new Uri(string.Format("{0}/{1}", _apiRootUrl, "sharks"));

            HttpResponseMessage response = new HttpResponseMessage();
            string responseBody = "";

            try
            {
                response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                responseBody = ex.ToString();
            }

            return ParseSharks(responseBody);
        }

        public async Task<Shark> GetShark(int id)
        {
            return (await GetShark()).Where(s => s.Id == id).First();
        }

        private IEnumerable<Shark> ParseSharks(string sharkJson)
        {
            JsonArray root = JsonValue.Parse(sharkJson).GetArray();

            uint i;
            for (i = 0; i < root.Count; i++)
                yield return new Shark()
                {
                    Id = (int)root.GetObjectAt(i).GetNamedNumber("Id"),
                    Name = root.GetObjectAt(i).GetNamedString("Name"),
                    Binomial = root.GetObjectAt(i).GetNamedString("Binomial"),
                    MaxLength = (int)root.GetObjectAt(i).GetNamedNumber("MaxLength")
                };
        }

        public async Task<Shark> ModifyShark(Shark shark)
        {
            HttpClient client = new HttpClient();

            Uri requestUri = new Uri(string.Format("{0}/{1}/{2}", _apiRootUrl, "sharks", shark.Id));
            JsonObject jsonShark = shark.ToJson();

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Put, requestUri);
            message.Content = new HttpStringContent(jsonShark.Stringify(), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");

            HttpResponseMessage response = new HttpResponseMessage();
            string responseBody = "";

            try
            {
                response = await client.SendRequestAsync(message);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
                return Shark.ConvertFromJson(responseBody);
            }
            catch (Exception ex)
            {
                responseBody = ex.ToString();
                return null;
            }
        }
    }
}
