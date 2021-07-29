using System;
using System.Collections.Generic;
using System.Text;

namespace HealthBridge.BL.Contracts
{
    public class Continents
    {
        public string Continent { get; set; }
        public New New { get; set; }
        public Active Active { get; set; }
        public Deaths Deaths { get; set; }
    }
}
