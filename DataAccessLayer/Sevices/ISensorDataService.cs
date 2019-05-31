﻿using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Sevices
{
    public interface ISensorDataService
    {
        IEnumerable<SensorData> GetSensorDatas();
        IEnumerable<SensorData> GetLastFiftySensorDatas();

        IEnumerable<SensorData> GetLastFiftySensorDatasSortedByTime();

        SensorData InsertSensorData(SensorData newData);
        void ClearSensorDataTable();
        void InitDatabase();

        IEnumerable<SensorData> GetSensorDatasOfHeater();
        IEnumerable<SensorData> GetLastFiftySensorDatasOfHeater();
        IEnumerable<SensorData> GetSensorDatasOfTemperature();
        string GetLastTemperatureSensorData();
        IEnumerable<SensorData> GetLastFiftySensorDatasOfTemperature();

        IEnumerable<SensorData> GetSensorDatasOfHumidity();
        IEnumerable<SensorData> GetLastFiftySensorDatasOfHumidity();

        IEnumerable<SensorData> GetSensorDatasExceptHeaters();
        IEnumerable<SensorData> GetLastFiftySensorDatasExceptHeaters();

    }
}
