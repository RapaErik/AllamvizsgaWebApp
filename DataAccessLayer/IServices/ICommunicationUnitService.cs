using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.IServices
{
    public interface ICommunicationUnitService
    {
        int InsertNewCommunicationUnit(string ipAddress);
    }
}
