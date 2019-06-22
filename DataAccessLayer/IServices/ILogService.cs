using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.IServices
{
    public interface ILogService
    {

        Log InsertLog(Log newData);
        void ClearLogTable();
        void InitDatabase();


        IEnumerable<Log> GetLastFiftyLogsExceptHeatersAndCooler();
        IEnumerable<Log> GetLastNLogsByDeviceType(string type="", int? number=null, int? DeviceId = null);
        List<Log> GetLogsOfTempAndHumi();
    }
}
