using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.IServices;
using System;
using System.Collections.Generic;
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
        public int InsertNewCommunicationUnit(string ipAddress)
        {
            CommunicationUnit c = new CommunicationUnit { IPAddress=ipAddress};
            _ctx.Add(c);
            _ctx.SaveChanges();
            return c.Id;
        }
    }
}
