namespace ConstructionERPSystem.Models
{
    public class PurchaseOrder
    {
        public int PurchaseOrderId { get; set; }

        public int VendorId { get; set; }

        public int MaterialId { get; set; }

        public int Quantity { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime? OrderDate { get; set; }

        public string Status { get; set; }

        public string VendorName { get; set; }

        public string MaterialName { get; set; }
    }
}