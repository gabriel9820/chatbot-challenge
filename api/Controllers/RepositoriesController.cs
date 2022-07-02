using api.Helpers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoriesController : ControllerBase
    {
        private const string BASE_URL = "https://api.github.com";

        public RepositoriesController() { }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepositoryViewModel>>> Get()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("challenge-api");

                var endpoint = "/orgs/takenet/repos";
                var repositories = Enumerable.Empty<RepositoryViewModel>();

                while (!string.IsNullOrEmpty(endpoint))
                {
                    var response = await client.GetAsync(endpoint);

                    if (!response.IsSuccessStatusCode)
                    {
                        return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                    }

                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<RepositoryViewModel>>();

                    if (content?.Count() > 0)
                    {
                        repositories = repositories.Concat(content);
                    }

                    var link = response.Headers.GetValues("Link").FirstOrDefault();
                    var parsedLink = LinkHeader.LinksFromHeader(link);
                    endpoint = parsedLink.NextLink;
                }

                repositories = repositories.Where(r => r.Language == "C#").OrderBy(r => r.CreatedAt).Take(5);

                return Ok(repositories);
            }
        }
    }
}

