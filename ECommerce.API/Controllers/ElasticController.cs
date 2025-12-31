using ECommerce.Infrastructure.Search;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/elastic")]
public class ElasticController : ControllerBase
{
    private readonly ElasticsearchClient _elasticClient;
    private readonly ElasticProductIndexer _indexer;

    public ElasticController(
        ElasticsearchClient elasticClient,
        ElasticProductIndexer indexer)
    {
        _elasticClient = elasticClient;
        _indexer = indexer;
    }

    [HttpPost("create-index")]
    public async Task<IActionResult> CreateIndex()
    {
        var exists = await _elasticClient.Indices.ExistsAsync("products");
        if (exists.Exists)
            return Ok("Index zaten var");

        var response = await _elasticClient.Indices.CreateAsync("products", c => c
            .Mappings(m => m
                .Properties(new Properties
                {
                {
                    "name",
                    new TextProperty()
                },
                {
                    "categoryName",
                    new KeywordProperty()
                }
                })
            )
        );

        return Ok("Index oluşturuldu");
    }



    [HttpPost("reindex-products")]
    public async Task<IActionResult> ReindexProducts()
    {
        await _indexer.IndexAllAsync();
        return Ok("Ürünler Elastic’e indexlendi");
    }

    [HttpDelete("delete-index")]
    public async Task<IActionResult> DeleteIndex()
    {
        var exists = await _elasticClient.Indices.ExistsAsync("products");
        if (!exists.Exists)
            return Ok("Index zaten yok");

        await _elasticClient.Indices.DeleteAsync("products");
        return Ok("Index silindi");
    }

}
