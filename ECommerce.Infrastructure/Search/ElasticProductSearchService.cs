using ECommerce.Application.DTO;
using ECommerce.Application.Interfaces;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
namespace ECommerce.Infrastructure.Search;

public class ElasticProductSearchService : IProductSearchService
{
    private readonly ElasticsearchClient _elasticClient;

    public ElasticProductSearchService(ElasticsearchClient elasticClient)
    {
        _elasticClient = elasticClient;
    }


    public async Task<IEnumerable<ProductSearchDto>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 1) 
            return Enumerable.Empty<ProductSearchDto>();

        var response = await _elasticClient.SearchAsync<ProductSearchDto>(s => s
            .Index("products")
            .Size(20)
            .Query(q => q
                .Bool(b => b
                    .Should(
                        sh => sh.Prefix(p => p
                            .Field(f => f.Name)
                            .Value(query.ToLower())
                        ),
                        sh => sh.Prefix(p => p
                            .Field(f => f.CategoryName)
                            .Value(query.ToLower())
                        )
                    )
                )
            )
        );

        return response.Documents;
    }





}
