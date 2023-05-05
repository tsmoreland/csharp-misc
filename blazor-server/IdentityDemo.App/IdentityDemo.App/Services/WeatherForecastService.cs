//
// Copyright (c) 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityDemo.Shared;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;

namespace IdentityDemo.App.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly HttpClient _client;
    private readonly ITokenManager _tokenManager;
    private readonly ILogger<WeatherForecastService> _logger;

    public WeatherForecastService(HttpClient client, ITokenManager tokenManager, ILogger<WeatherForecastService> logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
    {
        var accessToken = await _tokenManager.GetAcccessTokenOrEmptyAsync();
        if (string.IsNullOrEmpty(accessToken))
            return Array.Empty<WeatherForecast>();

        _client.SetBearerToken(accessToken);
        return (await JsonSerializer.DeserializeAsync<IEnumerable<WeatherForecast>>(
                await _client.GetStreamAsync("api/weatherforecast"),
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true}))
            .ToArray();

    }
}
