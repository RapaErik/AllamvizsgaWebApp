using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.IServices
{
    public interface ICommunicationUnitService
    {
        int InsertNewCommunicationUnit(string chipId);
        void InitNewCommunicationUnitAddIpAddress(CommunicationUnit c);
        CommunicationUnit GetCommunicationUnit(string chipId);
    }
}
