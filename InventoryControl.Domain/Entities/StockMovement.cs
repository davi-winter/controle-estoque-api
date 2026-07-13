namespace InventoryControl.Domain.Entities
{
    public class StockMovement
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public MovementType Type { get; set; }
        public DateTime MovedAt { get; set; } = DateTime.UtcNow;
        public string Observation { get; set; } = string.Empty;
        public Guid UserId { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }

    public enum MovementType
    {
        Input = 1,
        Output = 2
    }
}
