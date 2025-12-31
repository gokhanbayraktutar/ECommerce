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
        if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
            return Enumerable.Empty<ProductSearchDto>();

        var response = await _elasticClient.SearchAsync<ProductSearchDto>(s => s
            .Index("products")
            .Size(20)
            .Query(q => q
                .MultiMatch(m => m
                    .Query(query)
                    .Fields(new[] { "name", "categoryName" })
                    .Fuzziness(new Fuzziness("AUTO"))
                    .Operator(Operator.And)
                )
            )
        );

        if (!response.IsValidResponse)
        {
            throw new Exception($"Elastic search hatası: {response.ElasticsearchServerError?.Error.Reason}");
        }

        return response.Documents;
    }




}
