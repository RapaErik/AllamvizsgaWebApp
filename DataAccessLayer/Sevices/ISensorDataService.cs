using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Sevices
{
    public interface ISensorDataService
    {

        SensorData InsertSensorData(SensorData newData);
        void ClearSensorDataTable();
        void InitDatabase();


        IEnumerable<SensorData> GetLastFiftySensorDatasExceptHeaters();
        IEnumerable<SensorData> GetLastNSensorDatasBySensorType(string type="", int? number=null, int? sensorId = null);
    }
}
