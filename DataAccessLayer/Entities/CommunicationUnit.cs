using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class CommunicationUnit
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string IPAddress  { get; set; }
    }
}
