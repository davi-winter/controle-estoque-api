using InventoryControl.Application.DTOs.StockMovements;
using InventoryControl.Application.Validations;
using InventoryControl.Domain.Interfaces.Repositories;

namespace InventoryControl.Application.UseCases.StockMovements
{
    public class GetHistoryByPeriodUseCase
    {
        private readonly IStockMovementRepository _repository;

        public GetHistoryByPeriodUseCase(IStockMovementRepository repository)
            => _repository = repository;

        public async Task<Result<IEnumerable<StockMovementResponse>>> ExecuteAsync(DateOnly startDate, DateOnly endDate, int page, int pageSize)
        {
            if (startDate > endDate)
                return Result<IEnumerable<StockMovementResponse>>.Failure(new Error("StockMovement.InvalidPeriod", "A data de início não pode ser posterior à data de término."));

            var stockMovements = await _repository.GetHistoryByPeriodAsync(startDate, endDate, page, pageSize);

            if (!stockMovements.Any())
                return Result<IEnumerable<StockMovementResponse>>.Failure(new Error("StockMovement.NotFound", "Não há movimentações de estoque para o período informado."));

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