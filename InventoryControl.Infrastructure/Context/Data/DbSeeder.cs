using Bogus;
using InventoryControl.Domain.Entities;
using SecureIdentity.Password;

namespace InventoryControl.Infrastructure.Context.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            Randomizer.Seed = new Random(42);

            if (!context.Users.Any())
            {
                var users = new Faker<User>("pt_BR")
                    .RuleFor(u => u.Id, f => Guid.NewGuid())
                    .RuleFor(u => u.Username, f => f.Person.UserName.ToLower())
                    .RuleFor(u => u.Email, f => f.Person.Email.ToLower())
                    .RuleFor(u => u.PasswordHash, f => PasswordHasher.Hash("P@ssw0rd"))
                    .RuleFor(u => u.Role, f => f.PickRandom<User.UserRole>().ToString().ToLower())
                    .RuleFor(u => u.CreatedAt, f => f.Date.Past(1))
                    .RuleFor(u => u.ForceChangePassword, f => false)
                    .Generate(10); // Gera 10 usuários
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            if (!context.Categories.Any())
            {
                var categories = new Faker<Category>("pt_BR")
                    .RuleFor(c => c.Id, f => Guid.NewGuid())
                    .RuleFor(c => c.Name, f => f.Commerce.Department())
                    .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
                    .RuleFor(c => c.IsActive, f => f.Random.Bool())
                    .Generate(10); // Gera 10 categorias
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                var categories = context.Categories.ToList();
                var products = new Faker<Product>("pt_BR")
                    .RuleFor(p => p.Id, f => Guid.NewGuid())
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Sku, f => $"PRD-{f.Random.AlphaNumeric(4).ToUpper()}-{f.Random.AlphaNumeric(4).ToUpper()}-{f.Random.Number(1, 999)}")
                    .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                    .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(1, 1000)))
                    .RuleFor(p => p.CurrentStock, f => f.Random.Int(0, 100))
                    .RuleFor(p => p.IsActive, f => f.Random.Bool())
                    .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).Id)
                    .Generate(200); // Gera 200 produtos
                context.Products.AddRange(products);
                context.SaveChanges();
            }

            if (!context.StockMovements.Any())
            {
                var users = context.Users.ToList();
                var products = context.Products.ToList();
                var stockMovements = new Faker<StockMovement>("pt_BR")
                    .RuleFor(sm => sm.Id, f => Guid.NewGuid())
                    .RuleFor(sm => sm.Quantity, f => f.Random.Int(0, 100))
                    .RuleFor(sm => sm.Type, f => f.PickRandom<MovementType>())
                    .RuleFor(sm => sm.MovedAt, f => f.Date.Past(1))
                    .RuleFor(sm => sm.Observation, f => f.Lorem.Sentence(5))
                    .RuleFor(sm => sm.UserId, f => f.PickRandom(users).Id)
                    .RuleFor(sm => sm.ProductId, f => f.PickRandom(products).Id)
                    .Generate(50); // Gera 50 movimentações de estoque
                context.StockMovements.AddRange(stockMovements);
                context.SaveChanges();
            }
        }
    }
}
