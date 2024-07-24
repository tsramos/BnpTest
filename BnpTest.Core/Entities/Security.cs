namespace BnpTest.Core.Entities
{
    public class Security : Entity
    {
        public string ISIN { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
