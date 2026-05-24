namespace CRM_API.Models
{
    public class Customer
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string name { get; set; }
        public string title { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }

        public SalesRep salesRep { get; set; }
    }
}
