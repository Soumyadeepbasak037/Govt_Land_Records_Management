namespace govt_land_service.Models
{
    public class LocationData
    {
        public string type { get; set; }
        public List<object> data { get; set; } = new List<object>();
        public int count {  get; set; }
    }
}
