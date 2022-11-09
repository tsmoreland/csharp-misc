//
// Copyright (c) 2022 Terry Moreland
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
using System.Threading;
using System.Threading.Tasks;
using IdentityDomain;
using IdentityDomain.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Extensions;

namespace WebUI.Controllers
{
    [Route("api/countries")] // alternate: api/[controller] to inherit name but that can have issues if refactored
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IDemoRepository _repository;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(IDemoRepository repository, ILogger<CountriesController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Country> countries = new();
            await foreach (Country country in _repository.GetAllCountriesAsync())
                countries.Add(country);
            return Ok(countries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            id = id?.ToUpperInvariant() ?? string.Empty;
            Country country = await _repository.FindByIdAsync(id, CancellationToken.None);
            return country != null
                ? (IActionResult) Ok(country.ToModel())
                : NotFound();

        }

    }
}
