using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GameStore.Core.Models;

public class ExternalGameService
{
    private readonly HttpClient _httpClient;

    public ExternalGameService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Game?> FetchExternalGameAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ExternalGameDto>("https://api.rawg.io/api/games/3498?key=YOUR_API_KEY");
        if (response == null) return null;

        return new Game
        {
            Title = response.name,
            Description = response.description_raw,
            Genre = response.genres?[0]?.name ?? "Unknown",
            Price = 59.99M,
            ReleaseDate = DateTime.Parse(response.released)
        };
    }

    private class ExternalGameDto
    {
        public string name { get; set; }
        public string description_raw { get; set; }
        public string released { get; set; }
        public List<GenreDto> genres { get; set; }
    }

    private class GenreDto
    {
        public string name { get; set; }
    }
}
