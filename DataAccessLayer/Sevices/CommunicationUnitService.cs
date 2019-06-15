using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Sevices
{
    public class CommunicationUnitService : ICommunicationUnitService
    {
        private readonly HeatingContext _ctx;
        private int Constant;

        public CommunicationUnitService(HeatingContext ctx)
        {
            _ctx = ctx;
        }

        public CommunicationUnit GetCommunicationUnit(string chipId)
        {
            return _ctx.CommunicationUnits.Where(w => w.Code == chipId).FirstOrDefault();
        }

        public void InitNewCommunicationUnitAddIpAddress(CommunicationUnit c)
        {
            var a = _ctx.CommunicationUnits.Where(w => w.Id == c.Id).FirstOrDefault();
            a.IPAddress = c.IPAddress;
            _ctx.SaveChanges();

        }

        public int InsertNewCommunicationUnit(string chipId)
        {
            
            CommunicationUnit c = _ctx.CommunicationUnits.Where(w => w.Code == chipId).FirstOrDefault();
            if(c==null)
            {
                c = new CommunicationUnit { Code = chipId };
                _ctx.Add(c);
                _ctx.SaveChanges();
            }
            return c.Id;
        }
    }
}
