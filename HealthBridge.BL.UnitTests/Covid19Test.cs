using HealthBridge.BL.Interfaces;
using HealthBridge.BL.Services;
using HealthBridge.External.Service.Interfaces;
using HealthBridge.External.Service.Models;
using Lamar;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HealthBridge.BL.UnitTests
{
    public class Covid19Test
    {
        private IContainer _oicContainer;
        private Mock<ICovidService> _covidServiceMock;
        private Mock<IRapidApiService> _rapidApiServiceMock;

        [OneTimeSetUp]
        public void Initialise()
        {
            SetupMock();
            _oicContainer = BootstrapStructureMap();
        }
        private void SetupMock()
        {
            _covidServiceMock = new Mock<ICovidService>();
            _rapidApiServiceMock = new Mock<IRapidApiService>();
        }
        private IContainer BootstrapStructureMap()
        {
            return new Container(x => {
                x.For<IRapidApiService>().Use(_rapidApiServiceMock.Object);
                x.For<ICovidService>().Use<CovidService>();
                x.For<IConfiguration>().Use(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build());
            });
        }
        [SetUp]
        public void TestSetup()
        {
            _covidServiceMock.Reset();
            _rapidApiServiceMock.Reset();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetContinents(bool isSuccessfull) 
        {
            var mockdata = GetMockData(isSuccessfull);

            _rapidApiServiceMock.Setup(x => x.RetrieveStatistics()).Returns(mockdata);

            var covidService = _oicContainer.GetInstance<ICovidService>();

            var results = covidService.GetContinents();

            _rapidApiServiceMock.Verify(x => x.RetrieveStatistics(), Times.Once);

            if(isSuccessfull == true)
                Assert.IsTrue(results.Count() > 0);
            else
                Assert.IsTrue(results.Count() == 0);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetCountries(bool isSuccessfull)
        {
            var mockdata = GetMockData(isSuccessfull);

            _rapidApiServiceMock.Setup(x => x.RetrieveStatistics()).Returns(mockdata);

            var covidService = _oicContainer.GetInstance<ICovidService>();

            var results = covidService.GetCountries();

            _rapidApiServiceMock.Verify(x => x.RetrieveStatistics(), Times.Once);

            if (isSuccessfull == true)
                Assert.IsTrue(results.Count() > 0);
            else
                Assert.IsTrue(results.Count() == 0);
        }

        private ExternalCallResponse<List<StatisticsResponse>> GetMockData(bool isSuccessfull)
        {
            if (isSuccessfull == false)
                return new ExternalCallResponse<List<StatisticsResponse>>(null, "Failed!", isSuccessfull);
       
            var path = Directory.GetCurrentDirectory();

            string jsonFilePath = Path.Combine(path,"StatisticsData.json");

            string json = File.ReadAllText(jsonFilePath);

            var mockData = JsonConvert.DeserializeObject<RootObject>(json);
           
            return new ExternalCallResponse<List<StatisticsResponse>>(mockData.Response, string.Empty, true);

        }

    }
}
