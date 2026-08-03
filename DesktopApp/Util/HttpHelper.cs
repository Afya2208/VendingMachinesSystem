using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopApp.Util;

public static class HttpHelper
{
    
    public static async Task<HttpResult<TResult>> SafeGetAsync<TResult>(this HttpClient httpClient, string requestUri, CancellationToken ct = default)
    {
        try
        {
            var response = await httpClient.GetAsync(requestUri, ct);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<TResult>(ct);
                return new HttpResult<TResult>(data, null);
            }
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(ct));
        }
        catch (Exception ex)
        {
            return new HttpResult<TResult>(default, ex);
        }
    }
    
}