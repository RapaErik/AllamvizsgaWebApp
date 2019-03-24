using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Sevices
{
    public class SensorDataService : ISensorDataService
    {
        private readonly HeatingContext _ctx; 

        public SensorDataService(HeatingContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<SensorData> GetLastFiftySensorDatas()
        {
            return _ctx.SensorDatas.OrderByDescending(c=>c.Id).Take(50).ToList();
        }

        public IEnumerable<SensorData> GetSensorDatas()
        {
            return _ctx.SensorDatas.ToList();
        }

        public SensorData InsertSensorData(SensorData newData)
        {
            newData.TimeStamp = DateTime.Now;
            _ctx.SensorDatas.Add(newData);
            _ctx.SaveChanges();

            return newData;

        }
    }
}
