using HealthBridge.BL.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthBridge.BL.Interfaces
{
    public interface ICovidService
    {
        IEnumerable<Countries> GetCountries();
        IEnumerable<Continents>  GetContinents();
    }
}
