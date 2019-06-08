using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Sevices
{
    public interface ILogService
    {

        Log InsertLog(Log newData);
        void ClearLogTable();
        void InitDatabase();


        IEnumerable<Log> GetLastFiftyLogsExceptHeaters();
        IEnumerable<Log> GetLastNLogsByDeviceType(string type="", int? number=null, int? DeviceId = null);
    }
}
