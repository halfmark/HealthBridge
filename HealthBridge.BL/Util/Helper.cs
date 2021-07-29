using HealthBridge.BL.Contracts;
using HealthBridge.BL.Enum;
using HealthBridge.External.Service.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HealthBridge.BL.Util
{
    public static class Helper
    {
        public static IEnumerable<T> AggregateNumbers<T>(IList list,string continent, PurposeType purposeType)
        {
            switch (purposeType)
            {
                case PurposeType.Continents:
                   return ContinentCalculation<T>(list, continent);
                case PurposeType.Countrie:
                    return CountryCalculation<T>(list);
                default:
                    return (List<T>)Convert.ChangeType(null, typeof(T)); ;
                    
            }
        }

        private static IEnumerable<T> ContinentCalculation<T>(IList list, string continent)
        {
            var statistics = list as IList<StatisticsResponse>;

            int newCases = 0;

            int totalDeaths = 0;

            int active = 0;

            List<Continents> results = new List<Continents>();

            foreach (var item in statistics)
            {
                int x = 0;
                if (!string.IsNullOrEmpty(item.Cases.New))
                {
                    var number = item.Cases.New.Split('+')[1];
                    if (int.TryParse(number, out x))
                        newCases += x;

                }

                if (item.Deaths.Total.HasValue)
                    totalDeaths += item.Deaths.Total.Value;

                if (item.Cases.Active.HasValue)
                    active += item.Cases.Active.Value;
            }

            var c = new Continents
            {
                Active = new Active 
                {
                    //number of active cases in continent
                    Total = active
                },
                New = new New 
                {
                    //number of new cases in continent
                    Total = newCases              
                },
                Deaths = new Deaths 
                {
                    // number of deaths in continent
                    Total = totalDeaths 
                },
                Continent = continent

            };

            results.Add(c);

            var castedList = results.Select(x => (T)Convert.ChangeType(x, typeof(T)));

            return castedList;
        }
        private static IEnumerable<T> CountryCalculation<T>(IList list)
        {
            var statistics = list as IList<StatisticsResponse>;

            int newCasesPerContinent = 0;

            int newDeathsPerContinent = 0;

            int activeCasePerContinent = 0;

            List<Countries>  results = new List<Countries>();

            //sum up cases,deaths and active cases to get continent values
            foreach (var item in statistics)
            {
                int x = 0;

                if (!string.IsNullOrEmpty(item.Cases.New))
                {
                    //convert new case into an integer
                    var number = item.Cases.New.Split('+')[1];

                    if (int.TryParse(number, out x))
                        newCasesPerContinent += x;
                }

                if (item.Deaths.Total.HasValue)
                    newDeathsPerContinent += item.Deaths.Total.Value;

                if (item.Cases.Active.HasValue)
                    activeCasePerContinent += item.Cases.Active.Value;

            }

            foreach (var item in statistics)
            {             
                var country = new Countries
                {
                    Active = new Active 
                    {
                        //number of active cases in country
                        Total = item.Cases.Active.HasValue ? item.Cases.Active.Value : 0,

                        //Active cases in country as a percentage of active cases in continent Round to 2 decimal places
                        Percent = Round(((double)(item.Cases.Active.HasValue ? item.Cases.Active.Value : 0) / (double)activeCasePerContinent) * 100, 2)
                    },
                    New = new New 
                    {
                        //number of new cases in country
                        Total = !string.IsNullOrEmpty(item.Cases.New) ? int.Parse(item.Cases.New.Split('+')[1]): 0,

                        // New cases in country as a percentage of new cases in continent Round to 2 decimal places
                        Percent = Round(((double)(!string.IsNullOrEmpty(item.Cases.New) ? int.Parse(item.Cases.New.Split('+')[1]) : 0) / (double)newCasesPerContinent) * 100, 2)
                    },
                    Deaths = new Deaths 
                    {
                        // number of deaths in country
                        Total = item.Deaths.Total.HasValue ? item.Deaths.Total.Value: 0  ,

                        //Deaths in country as a percentage of deaths cases in continent Round to 2 decimal places
                        Percent = Round(((double)(item.Deaths.Total.HasValue ? item.Deaths.Total.Value : 0) / (double)newDeathsPerContinent) * 100, 2)
                    },
                    Continent = item.Continent,
                    Country = item.Country

                };

                results.Add(country);

            }
       
            var castedList = results.Select(x => (T)Convert.ChangeType(x, typeof(T)));

            return castedList;
        }


      
        public static IDictionary<string, IEnumerable<Continents>> AggregatePercentages(IDictionary<string,IEnumerable<Continents>> keyValuePairs)
        {
            int newCasesGlobally = 0;

            int newDeathsGlobally = 0;

            int activeGlobally = 0;

            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                string key = keyValuePairs.Keys.ElementAt(i);

                var continents = keyValuePairs[key];
                
                //sum up cases,deaths and active cases to get global values
                newCasesGlobally += continents.ToList().FirstOrDefault().New.Total;

                newDeathsGlobally += continents.ToList().FirstOrDefault().Deaths.Total;

                activeGlobally += continents.ToList().FirstOrDefault().Active.Total;
            }

            for (int i = 0; i < keyValuePairs.Count; i++)
            {
                string key = keyValuePairs.Keys.ElementAt(i);

                var continents = keyValuePairs[key];

                //New cases in continent as a percentage of new cases globally Round to 2 decimal places
                continents.ToList().FirstOrDefault().New.Percent = Round(((double)continents.ToList().FirstOrDefault().New.Total / (double)newCasesGlobally) * 100, 2);

                // Active cases in continent as a percentage of active cases globally Round to 2 decimal places
                continents.ToList().FirstOrDefault().Active.Percent = Round(((double)continents.ToList().FirstOrDefault().Active.Total / (double)activeGlobally) * 100, 2);

                //Deaths in continent as a percentage of deaths cases globally Round to 2 decimal places
                continents.ToList().FirstOrDefault().Deaths.Percent = Round(((double)continents.ToList().FirstOrDefault().Deaths.Total / (double)newDeathsGlobally) * 100, 2);
            }

            return keyValuePairs;
        }
   
        private static double Round(double value,int places) 
        {
            if (places < 0)
                throw new InvalidExpressionException();
  
            long factor = (long)Math.Pow(10, places);

            value *= factor;

            double temp = Math.Round(value);

            return (double)temp / factor;
        }
    }
}
