namespace InventoryControl.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CurrentStock { get; set; }
        public Guid CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

        public void AddToStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantity));

            CurrentStock += quantity;
        }

        public void RemoveFromStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantity));

            if (quantity > CurrentStock)
                throw new InvalidOperationException("Estoque insuficiente.");

            CurrentStock -= quantity;
        }
    }
}
