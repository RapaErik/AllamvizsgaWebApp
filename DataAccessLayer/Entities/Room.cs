using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
