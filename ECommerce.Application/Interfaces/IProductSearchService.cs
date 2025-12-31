using ECommerce.Application.DTO;

namespace ECommerce.Application.Interfaces;

public interface IProductSearchService
{
    Task<IEnumerable<ProductSearchDto>> SearchAsync(string query);
}
