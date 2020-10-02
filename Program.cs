using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iSAMSData
{

    class Program
    {
        //Variables
        private static readonly HttpClient client = new HttpClient();
        private static JObject _token;
        private static JArray _extractedData;
        private static string _tokenURL;
        private static string _clientID;
        private static string _clientSecret;
        private static string _apiEndpoint;
        private static string _elementName;
        private static string _saveLocation;

        static void Main(string[] args)
        {

            //Assign console parameters to variables and replace "/" & "\" with "//" & "\\" where appropriate
            _tokenURL = Regex.Replace(args[0], @"//", "////");
            _clientID = args[1];
            _clientSecret = args[2];
            _apiEndpoint = Regex.Replace(args[3], @"//", "////");
            _elementName = args[4];
            _saveLocation = Regex.Replace(args[5], @"\\", "\\\\");

            _extractedData = new JArray();

            _token = GetToken(_tokenURL, _clientID, _clientSecret).GetAwaiter().GetResult();

            var iSAMSDataTotalPages = GetData(1, _apiEndpoint).GetAwaiter().GetResult();

            JValue _totalPages = (JValue)iSAMSDataTotalPages["totalPages"];

            //Check if _totalPages exists - if not then set a default of 1
            if (_totalPages is null)
            {
                _totalPages = (JValue)1;
            }
               
            //Where paging is required loop through to build up results set then append together as a single entity
            for (int page = 1; page <= (int)_totalPages; page ++)
            {               
                var iSAMSData = GetData(page, _apiEndpoint).GetAwaiter().GetResult();
                JArray SubElement = (JArray)iSAMSData[_elementName];
                _extractedData.Add(SubElement);          

            }

            //Save final results to local file (e.g. data.json)
            string savedData = @"" + _saveLocation + "";
            using (StreamWriter file = File.CreateText(savedData))
            {            
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, _extractedData);
            }
        }

        //Process for getting authorisation token which is sent with each subsequent request
        static async Task<JObject> GetToken(string _clientUrl = "youraddressfortoken", string _clientID = "yourclientid", string _clientSecret = "yourclientsecret")
        {
  
            var _tokenUrl = "https://" + _clientUrl;
            var _tokenSecretRaw = string.Format("client_id=" + _clientID + "&client_secret=" + _clientSecret + "&grant_type=client_credentials&scope=restapi");
            StringContent theContent = new StringContent(_tokenSecretRaw, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");            
            HttpResponseMessage aResponse = await client.PostAsync(new Uri(_tokenUrl), theContent);
            aResponse.EnsureSuccessStatusCode();
            string content = await aResponse.Content.ReadAsStringAsync();

            return (JObject)await Task.Run(() => JObject.Parse(content));
        }

        //Process for getting data
        static async Task<JObject> GetData(int _page = 1, string urlApi = "yourchosenapiendpoint")
        {
                JObject Token = _token; //Use cached Token
                var _extractedToken = Token["access_token"].ToString();
                var _baseUrl = "https://" + urlApi + "?page=" + _page;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _extractedToken);
                var response = await client.GetStringAsync(_baseUrl);

            return (JObject)await Task.Run(() => JObject.Parse(response));        
        }

    }

}
