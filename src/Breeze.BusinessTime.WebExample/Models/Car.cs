using Breeze.BusinessTime.Authorization;

namespace Breeze.BusinessTime.WebExample.Models
{
    [AuthorizeEntity(Roles = "Owner, Dealer")]
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
    }
}