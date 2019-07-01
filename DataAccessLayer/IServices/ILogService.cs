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

        IEnumerable<Log> Filter(DateTime startDate, DateTime endDate, int page=1, string type = "", string roomName = "");
        int GetDbSize();
        IEnumerable<Log> GetLastFiftyLogsExceptHeatersAndCooler();
        IEnumerable<Log> GetLastFiftyLogsExceptHeatersAndCooler(int roomId);
        IEnumerable<Log> GetLastNLogsByDeviceType(string type="", int? number=null, int? DeviceId = null);

        void ReduceLogs();
        List<Log> GetLogsOfTempAndHumi();
        int GetFiltredDbSize(DateTime startDate, DateTime endDate,  string type, string roomName);
    }
}
