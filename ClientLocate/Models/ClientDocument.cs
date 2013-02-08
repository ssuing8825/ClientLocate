using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ClientLocate.Models
{
    public class ClientDocument
    {
        [Key]
        public int Id { get; set; }

      
        public string PayloadXml { get; set; }

    
        public string PayloadJson { get; set; } 

    }
}