using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace CallApis
{
    internal struct Input1
    {
        public string ContactAddress { get; set; }
        public string WarehouseAddress { get; set; }
        public IList<double> PackageDimentions { get; set; }
    }

    internal struct Input2
    {
        public string Consignee { get; set; }
        public string Consignor { get; set; }
        public IList<double> Cartons { get; set; }
    }
    internal struct Ans1
    {
        public int Total { get; set; }
    }

    internal struct Ans2
    {
        public int Amount { get; set; }
    }

    class Program
    {
        private const string URL = "https://maps.googleapis.com/maps/api/timezone/json";
        private const string urlParameters = @"?location=39.6034810%2C-119.6822510
  & timestamp = 1331161200
  & key = AIzaSyCLMrAHDUvVYWi1uxWWQ3MG60wG2 - 8sayc";
        /// AIzaSyCLMrAHDUvVYWi1uxWWQ3MG60wG2-8sayc  

        static async Task Main(string[] args)
        {
            var callApi = new Program();
            var results = await callApi.CallApiAsync();
            var ans = results.Min();
           
        }

        private Task<int> Call1Async()
        {
            Uri uri = new Uri("https://httpbin.org/post");
            Input1 input = new Input1 { ContactAddress = "", PackageDimentions = new List<double> { 10, 20 }, WarehouseAddress = "" };
            var response = MakeRestCallAsync(uri, JsonConvert.SerializeObject(input));
            if (!response.Result.IsSuccessStatusCode)
            {
                // add error handling here - call again? log error? 
                return Task.FromResult(-1);
            }
            var result = "{total : 20}"; //response.Result.Content.ReadAsStringAsync().Result
            var json = DeserializeObjectFromJson<Ans1>(result);

            return Task.FromResult(json.Total);
        }
        private Task<int> Call2Async()
        {
            Uri uri = new Uri("https://httpbin.org/post");
            Input2 input = new Input2 { Consignee = "", Cartons = new List<double> { 10, 20 }, Consignor = "" };
            var response = MakeRestCallAsync(uri, JsonConvert.SerializeObject(input));
            if (!response.Result.IsSuccessStatusCode)
            {
                // add error handling here - call again? log error? 
                return Task.FromResult(-1);
            }
            var result = "{amount : 30}"; //response.Result.Content.ReadAsStringAsync().Result
            var json = DeserializeObjectFromJson<Ans2>(result);

            return Task.FromResult(json.Amount);
        }

        private Task<int> Call3Async()
        {
            Uri uri = new Uri("https://httpbin.org/post");
            string input = "<xml><source/><destination/><packages><package/></packages><xml>";
            var response = MakeRestCallAsync(uri, input);
            if (!response.Result.IsSuccessStatusCode)
            {
                // add error handling here - call again? log error? 
                return Task.FromResult(-1);
            }

            var result = "<xml><quote>40</quote></xml>"; //response.Result.Content.ReadAsStringAsync().Result
            XmlDocument xmltest = new XmlDocument();
            xmltest.LoadXml(result);

            XmlNodeList elemlist = xmltest.GetElementsByTagName("quote");

            var resultXml = elemlist[0].InnerXml;
            if (int.TryParse(resultXml, out int res))
            {
                return Task.FromResult(res);
            }
            return Task.FromResult(-1);
        }

        private async Task<HttpResponseMessage> MakeRestCallAsync(Uri uri, string input)
        {
            using (var httpClient = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    //add content to form data
                    formData.Add(new StringContent(input), "Input");


                    return await httpClient.PostAsync(uri, formData);
                    
                }
            }
        }
        private T DeserializeObjectFromJson<T>(string result)
        {
            return JsonConvert.DeserializeObject<T>(result);
        }

        
        private async Task<IEnumerable<int>> CallApiAsync()
        {

            var task1 = Call1Async();
            var task2 = Call2Async();
            var task3 = Call3Async();

            await Task.WhenAll(task1, task2, task3);

            return new List<int> { task1.Result, task2.Result, task3.Result };
        }
    }
}
  
