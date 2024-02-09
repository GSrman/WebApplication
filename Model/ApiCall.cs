using System.Web;
using System.Text.Json;

namespace ApiCalls;

public class CallTwitter
{
    // Deze method voert de api call uit.
    public static async Task<string> GetBearerToken()
    {
        // Setup keys en strings etc.
        string ApiKey = HttpUtility.UrlEncode("<<your consumer api key here>>");  // Consumer Api Key
        string SecretKey = HttpUtility.UrlEncode("<<your secret consumer api key here>>");  // Consumer Api Key Secret
        string Credentials = "Basic " + Base64Encode(ApiKey + ":" + SecretKey);

        using (var client = new HttpClient())
        {
            // setup http headers
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", Credentials);
            // setup http body
            var values = new Dictionary<string, string> { { "grant_type", "client_credentials" } };
            var body = new FormUrlEncodedContent(values);
            // call api
            var response = await client.PostAsync("https://api.twitter.com/oauth2/token", body);

            // read api output en return de bearer token.
            var serializedBearer = await response.Content.ReadAsStringAsync();
            return "Bearer " + JsonSerializer.Deserialize<Bearer>(serializedBearer)!.access_token;

            // Deze Bearer token is nu kant en klaar om een nieuwe twitter api call mee uit te voeren om bijvoorbeeld
            // tweets op te halen, maar daarvoor moet je een betaald pakket hebben en die zijn best duur.
        }
    }

    // Deze method convert strings naar een base64 string,
    // dat vindt twitter fijn vgm.
    public static string Base64Encode(string Text)
    {
        return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Text));
    }

    // Deze class geeft aan hoe de output van de deserialized bearer er uit ziet.
    public class Bearer
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
    }
}