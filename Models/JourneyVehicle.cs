namespace Atut.Models
{
    public class JourneyVehicle
    {
        public int JourneyId { get; set; }
        public Journey Journey { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
