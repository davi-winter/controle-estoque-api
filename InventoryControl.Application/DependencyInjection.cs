using InventoryControl.Application.UseCases.Categories;
using InventoryControl.Application.UseCases.Products;
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
            services.AddScoped<CreateProductUseCase>();
            services.AddScoped<UpdateProductUseCase>();
            services.AddScoped<UpdateStockUseCase>();
            services.AddScoped<DeleteProductUseCase>();
            services.AddScoped<GetBySkuUseCase>();
            services.AddScoped<GetLowStockProductsUseCase>();
            services.AddScoped<GetProductsWithCategoryUseCase>();
            services.AddScoped<CreateCategoryUseCase>();
            // ...

            return services;
        }
    }
}
