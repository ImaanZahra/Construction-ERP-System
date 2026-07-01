namespace ConstructionERPSystem.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        public int ClientId { get; set; }

        public int ProjectId { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public string Status { get; set; }

        public string ClientName { get; set; }

        public string ProjectName { get; set; }
    }
}