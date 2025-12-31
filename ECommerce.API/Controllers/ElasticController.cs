using ECommerce.Infrastructure.Search;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Analysis;
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

        try
        {
            var response = await _elasticClient.Indices.CreateAsync("products", c => c
                .Settings(s => s
                    .Analysis(a => a
                        .Tokenizers(t => t
                            .EdgeNGram("autocomplete_tokenizer", e => e
                                .MinGram(2)
                                .MaxGram(20)
                                .TokenChars(new[] { TokenChar.Letter, TokenChar.Digit })
                            )
                        )
                        .Analyzers(an => an
                            .Custom("autocomplete", ca => ca
                                .Tokenizer("autocomplete_tokenizer")
                                .Filter(new[] { "lowercase" })
                            )
                        )
                    )
                )
                .Mappings(m => m
                    .Properties(new Properties
                    {
                    {
                        "name",
                        new TextProperty
                        {
                            Analyzer = "autocomplete",
                            SearchAnalyzer = "standard"
                        }
                    },
                    {
                        "categoryName",
                        new TextProperty
                        {
                            Analyzer = "autocomplete",
                            SearchAnalyzer = "standard"
                        }
                    }
                    })
                )
            );

            return Ok("Index oluşturuldu (Edge N-gram hazır)");
        }
        catch (Exception ex)
        {
            return BadRequest($"Index oluşturulamadı: {ex.Message}");
        }
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
