using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Sevices
{
    public interface ISensorDataService
    {
        IEnumerable<SensorData> GetSensorDatas();
        IEnumerable<SensorData> GetLastFiftySensorDatas();
        SensorData InsertSensorData(SensorData newData);
    }
}
