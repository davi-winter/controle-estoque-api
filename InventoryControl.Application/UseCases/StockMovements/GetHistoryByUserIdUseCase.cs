using InventoryControl.Application.DTOs.StockMovements;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.StockMovements
{
    public class GetHistoryByUserIdUseCase
    {
        private readonly IStockMovementRepository _repository;

        public GetHistoryByUserIdUseCase(IStockMovementRepository repository)
            => _repository = repository;

        public async Task<Result<IEnumerable<StockMovementResponse>>> ExecuteAsync(Guid userId, int page, int pageSize)
        {
            var stockMovements = await _repository.GetHistoryByUserIdAsync(userId, page, pageSize);

            if (!stockMovements.Any())
                return Result<IEnumerable<StockMovementResponse>>.Failure(new Error("StockMovement.NotFound", "Não há movimentações de estoque para este usuário."));

            return Result<IEnumerable<StockMovementResponse>>.Success(
                stockMovements.Select(sm => 
                    new StockMovementResponse(
                        sm.Id,
                        sm.ProductId,
                        sm.Product?.Name!,
                        sm.Quantity,
                        sm.Type.ToString(),
                        sm.MovedAt,
                        sm.Observation,
                        sm.UserId,
                        sm.User?.Username!
                    )
                ).ToList()
            );
        }
    }
}