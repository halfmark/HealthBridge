using HealthBridge.External.Service.Interfaces;
using HealthBridge.External.Service.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthBridge.External.Service.Implementation
{
    public class RapidApiService: IRapidApiService
    {
        private readonly IConfiguration _configuration;

        public RapidApiService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ExternalCallResponse<List<StatisticsResponse>> RetrieveStatistics()
        {
            var statisticsRequestUrl = _configuration["Stats"];

            var results = Get(statisticsRequestUrl);

            if (string.IsNullOrEmpty(results.response))
                return new ExternalCallResponse<List<StatisticsResponse>>(null,$"Failed {results.errors}",false);
    
            var rootObject = JsonConvert.DeserializeObject<RootObject>(results.response);

            return new ExternalCallResponse<List<StatisticsResponse>>(rootObject.Response, string.Empty,true);
        }

        private (string response,string errors) Get(string url) 
        {
            var request = new RestRequest(Method.GET)
            {
                Resource = url,
                UseDefaultCredentials = true,
                RequestFormat = DataFormat.Json,
                Timeout = 10000

            };

            request.AddHeader("x-rapidapi-key", _configuration["RapidApiKey"]);

            request.AddHeader("x-rapidapi-host", _configuration["RapidApiHost"]);

            var client = new RestClient
            {
                BaseUrl = new Uri(_configuration["RapidApiUrl"])
            };

            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return (response.Content, string.Empty);
            }

            return (string.Empty, response.ErrorMessage);
        
        }
    }
}
