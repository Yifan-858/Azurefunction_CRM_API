using System;
using System.Collections.Generic;
using System.Text;

namespace CRM_Trigger.Models
{
    public class Customer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }

        public SalesRep salesRep { get; set; }
    }
}
