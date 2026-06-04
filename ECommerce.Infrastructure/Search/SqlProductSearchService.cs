using ECommerce.Application.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Core.Interfaces;

namespace ECommerce.Infrastructure.Search
{
    public class SqlProductSearchService : IProductSearchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlProductSearchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductSearchDto>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return Enumerable.Empty<ProductSearchDto>();

            var products = await _unitOfWork.Products.FindAsync(
                p => p.Name.Contains(query)
                //p => p.Category
            );

            return products.Select(p => new ProductSearchDto
            {
                Id = p.Id,
                Name = p.Name,
                //CategoryName = p.Category.Name,
                //Picture = p.Picture,
                Price = p.Price
            });
        }
    }
}
