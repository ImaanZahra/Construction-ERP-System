namespace ConstructionERPSystem.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }

        public int ProjectId { get; set; }

        public string ExpenseType { get; set; }

        public decimal Amount { get; set; }

        public DateTime? ExpenseDate { get; set; }

        public string Description { get; set; }

        public string ProjectName { get; set; }
    }
}