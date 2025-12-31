using ECommerce.Application.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Bulk;

namespace ECommerce.Infrastructure.Search;

public class ElasticProductIndexer
{
    private readonly ElasticsearchClient _elasticClient;
    private readonly IUnitOfWork _unitOfWork;

    public ElasticProductIndexer(
        ElasticsearchClient elasticClient,
        IUnitOfWork unitOfWork)
    {
        _elasticClient = elasticClient;
        _unitOfWork = unitOfWork;
    }

    public async Task IndexAllAsync()
    {
        var products = await _unitOfWork.Products.FindAsync(
            p => true,
            p => p.Category
        );

        var response = await _elasticClient.BulkAsync(b => b
            .Index("products")
            .IndexMany(products.Select(product => new ProductSearchDto
            {
                Id = product.Id,
                Name = product.Name,
                CategoryName = product.Category.Name,
                Picture = product.Picture,
                Price = product.Price
            }))
        );

        if (response.Errors)
        {
            throw new Exception("Elastic bulk indexleme sırasında hata oluştu");
        }
    }



}
