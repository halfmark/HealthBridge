using HealthBridge.External.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthBridge.External.Service.Interfaces
{
    public interface IRapidApiService
    {
        ExternalCallResponse<List<StatisticsResponse>> RetrieveStatistics();
    }
}
