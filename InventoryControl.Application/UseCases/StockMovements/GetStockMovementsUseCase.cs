using InventoryControl.Application.DTOs.StockMovements;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Entities;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.StockMovements
{
    public class GetStockMovementsUseCase
    {
        private readonly IStockMovementRepository _repository;

        public GetStockMovementsUseCase(IStockMovementRepository repository)
            => _repository = repository;

        public async Task<Result<IEnumerable<StockMovementResponse>>> ExecuteAsync(Guid userId, int page, int pageSize)
        {
            IEnumerable<StockMovement> stockMovements;
            if (userId == Guid.Empty)
                stockMovements = await _repository.GetStockMovementsAsync(page, pageSize);
            else
                stockMovements = await _repository.GetHistoryByUserIdAsync(userId, page, pageSize);

            if (!stockMovements.Any())
            {
                if (userId == Guid.Empty)
                    return Result<IEnumerable<StockMovementResponse>>.Failure(new Error("StockMovement.NotFound", "Não há movimentações de estoque cadastradas."));
                else
                    return Result<IEnumerable<StockMovementResponse>>.Failure(new Error("StockMovement.NotFound", "Não há movimentações de estoque para o usuário especificado."));
            }

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