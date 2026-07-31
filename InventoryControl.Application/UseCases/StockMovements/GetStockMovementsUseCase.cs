using InventoryControl.Application.DTOs.StockMovements;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.StockMovements
{
    public class GetStockMovementsUseCase
    {
        private readonly IStockMovementRepository _repository;

        public GetStockMovementsUseCase(IStockMovementRepository repository)
            => _repository = repository;

        public async Task<Result<IEnumerable<StockMovementResponse>>> ExecuteAsync(int page, int pageSize)
        {
            var stockMovements = await _repository.GetStockMovementsAsync(page, pageSize);

            if (!stockMovements.Any())
                return Result<IEnumerable<StockMovementResponse>>.Failure(new Error("StockMovement.NotFound", "Não há movimentações de estoque cadastradas."));

            return Result<IEnumerable<StockMovementResponse>>.Success(
                stockMovements.Select(sm => new StockMovementResponse(
                    sm.Id,
                    sm.ProductId,
                    sm.Product?.Name!,
                    sm.Quantity,    
                    sm.Type.ToString(),
                    sm.MovedAt,
                    sm.Observation,
                    sm.UserId,
                    sm.User?.Username!
                )).ToList());
        }
    }
}