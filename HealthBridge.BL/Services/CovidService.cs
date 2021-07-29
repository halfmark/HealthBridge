using HealthBridge.BL.Contracts;
using HealthBridge.BL.Interfaces;
using HealthBridge.BL.Util;
using HealthBridge.External.Service.Interfaces;
using HealthBridge.External.Service.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthBridge.BL.Services
{
    public class CovidService : ICovidService
    {
        private readonly IRapidApiService _rapidApiService;

        public CovidService(IRapidApiService rapidApiService)
        {
            _rapidApiService = rapidApiService;
        }

        public IEnumerable<Continents> GetContinents()
        {
            //retrieve data from Rapid api (external call)
            var results = _rapidApiService.RetrieveStatistics();

            var continents = new List<Continents>();

            if (results.Success)
            {
                Dictionary<string, IEnumerable<Continents>> keyValuePairs = new Dictionary<string, IEnumerable<Continents>>();
                
                //group by continent
                var groupByContinent = from item in results.Response
                                       group item by item.Continent into sg
                                       orderby sg.Key
                                       select new { sg.Key, sg };
               
                foreach (var item in groupByContinent)
                {
                    //remove outlier e.g Null/All is not a country or continent
                    if (!string.IsNullOrEmpty(item.Key) && item.Key != "All")
                    {
                        //remove outlier e.g Africa is not a country but a continent
                        var outlier = item.sg.ToList();

                        outlier.Remove(item.sg.Where(x => x.Country == item.Key).FirstOrDefault());

                        //Create value key pair by calling AggregateNumbers helper
                        keyValuePairs.Add(item.Key, Helper.AggregateNumbers<Continents>(outlier, item.Key, Enum.PurposeType.Continents));
                    }
                }
                
                //calculate percenteges
                var newKeyValuePairs = Helper.AggregatePercentages(keyValuePairs);
          
                foreach (var item in newKeyValuePairs.Values.ToList())
                    continents.AddRange(item);

                return continents;
            }

            return continents;
        }

        public IEnumerable<Countries> GetCountries()
        {
            //retrieve data from Rapid api (external call)
            var results = _rapidApiService.RetrieveStatistics();

            var countries = new List<Countries>();

            if (results.Success) 
            {
                Dictionary<string, IEnumerable<Countries>> keyValuePairs = new Dictionary<string, IEnumerable<Countries>>();

                //group by continent
                var groupByContinent = from item in results.Response
                        group item by item.Continent into sg
                        orderby sg.Key
                        select new { sg.Key, sg };
          
                foreach (var item in groupByContinent)
                {
                    //remove outlier e.g Null/All is not a country or continent
                    if (!string.IsNullOrEmpty(item.Key)&& item.Key != "All")
                    {
                        //remove outlier e.g Africa is not a country but a continent
                        var outlier = item.sg.ToList();

                        outlier.Remove(item.sg.Where(x => x.Country == item.Key).FirstOrDefault());
                        
                        //Create value key pair by calling AggregateNumbers helper
                        keyValuePairs.Add(item.Key,Helper.AggregateNumbers<Countries>(outlier, item.Key, Enum.PurposeType.Countrie));
                    }
                }
 
                foreach (var item in keyValuePairs.Values.ToList())
                    countries.AddRange(item);
               
                return countries;
            }

            return countries;
        }
    }
}
