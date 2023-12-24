namespace MenuWebapi.Models.Entities
{

    public class Carrier
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Cost { get; set; } = 2;
        public bool Deleted { get; set; } = false;
    }
}
