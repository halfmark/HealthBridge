using HealthBridge.BL.Contracts;
using HealthBridge.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthBridge.RapidApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    public class Covid19 : ControllerBase
    {
        private readonly ILogger<Covid19> _logger;

        private readonly ICovidService _covidService;

        //inject component/services
        public Covid19(ILogger<Covid19> logger, ICovidService covidService) 
        {
            _covidService = covidService;

            _logger = logger;
        }

        [HttpGet("Countries")]
        [ProducesResponseType(200, Type = typeof(Countries))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<Countries>> GetCountries()
        {
            try
            {
                _logger.LogInformation($"Getting Countries");

                var result = _covidService.GetCountries();

                if (result.Any())
                    return Ok(result);
                else
                {
                    _logger.LogInformation($"NOT FOUND");

                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }

        }


        [HttpGet("Continents")]
        [ProducesResponseType(200, Type = typeof(Continents))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<Continents>> GetContinents()
        {
            try
            {
                _logger.LogInformation($"Getting Continents");

                var result = _covidService.GetContinents();

                if (result.Any())
                    return Ok(result);
                else
                {
                    _logger.LogInformation($"NOT FOUND");

                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }
    }
}
