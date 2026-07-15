using InventoryControl.Application.UseCases.Categories;
using InventoryControl.Application.UseCases.Products;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryControl.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationHierarchy(this IServiceCollection services)
        {
            services.AddScoped<CreateProductUseCase>();
            services.AddScoped<CreateCategoryUseCase>();
            // ...

            return services;
        }
    }
}
