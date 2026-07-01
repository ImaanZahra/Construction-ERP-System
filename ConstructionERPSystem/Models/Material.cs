namespace ConstructionERPSystem.Models
{
    public class Material
    {
        public int MaterialId { get; set; }

        public string MaterialName { get; set; }

        public string Unit { get; set; }

        public int CurrentStock { get; set; }

        public int MinimumStock { get; set; }
    }
}