using InventoryControl.Application.UseCases.Categories;
using InventoryControl.Application.UseCases.Products;
using InventoryControl.Application.UseCases.StockMovements;
using InventoryControl.Application.UseCases.Users;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryControl.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationHierarchy(this IServiceCollection services)
        {
            services.AddScoped<CreateUserUseCase>();
            services.AddScoped<UpdateUserUseCase>();
            services.AddScoped<DeleteUserUseCase>();
            services.AddScoped<LoginUseCase>();
            services.AddScoped<GetByUsernameUseCase>();
            services.AddScoped<GetByEmailUseCase>();
            services.AddScoped<GetAllUsersUseCase>();
            services.AddScoped<CreateProductUseCase>();
            services.AddScoped<UpdateProductUseCase>();
            services.AddScoped<DeleteProductUseCase>();
            services.AddScoped<ChangeStatusProductUseCase>();
            services.AddScoped<GetBySkuUseCase>();
            services.AddScoped<GetLowStockProductsUseCase>();
            services.AddScoped<GetProductsByCategoryIdUseCase>();
            services.AddScoped<GetProductsByNameUseCase>();
            services.AddScoped<CreateCategoryUseCase>();
            services.AddScoped<UpdateCategoryUseCase>();
            services.AddScoped<DeleteCategoryUseCase>();
            services.AddScoped<ChangeStatusCategoryUseCase>();
            services.AddScoped<GetByCategoryIdUseCase>();
            services.AddScoped<GetAllCategoriesUseCase>();
            services.AddScoped<CreateStockMovementUseCase>();
            services.AddScoped<GetStockMovementsUseCase>();
            services.AddScoped<GetHistoryByProductIdUseCase>();
            services.AddScoped<GetHistoryByUserIdUseCase>();
            services.AddScoped<GetHistoryByPeriodUseCase>();
            // ...

            return services;
        }
    }
}
