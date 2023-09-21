using Gamble_On.Services;

public class BaseService
{
    protected readonly HttpClient _httpClient;
    protected readonly IAuthorizationService _authorizationService;

    public BaseService(HttpClient httpClient, IAuthorizationService authorizationService)
    {
        _httpClient = httpClient;
        _authorizationService = authorizationService;
    }

    protected async Task<HttpResponseMessage> ExecuteHttpRequestAsync(Func<Task<HttpResponseMessage>> httpRequest)
    {
        var token = await _authorizationService.GetAuthorizationTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        try
        {
            return await httpRequest();
        }
        catch (HttpRequestException e)
        {
            throw new Exception("Network error occurred", e);
        }
        catch (TaskCanceledException e)
        {
            if (e.CancellationToken.IsCancellationRequested)
                throw new Exception("Request was cancelled", e);
            else
                throw new Exception("Request timed out", e);
        }
    }
}
