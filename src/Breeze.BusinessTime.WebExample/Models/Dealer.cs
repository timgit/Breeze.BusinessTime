using Breeze.BusinessTime.Authorization;

namespace Breeze.BusinessTime.WebExample.Models
{
    [AuthorizeEntity(Roles = "Dealer")]
    public class Dealer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool Preferred { get; set; }
    }
}