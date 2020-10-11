//
// Copyright © 2020 Terry Moreland
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
using System.Threading.Tasks;
using IdentityDomain.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebUI.Extensions;

namespace WebUI.Controllers
{
    [Route("api/countries")] // alternate: api/[controller] to inherit name but that can have issues if refactored
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly DemoDbContext _db;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(DemoDbContext db, ILogger<CountriesController> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Countries.ToArrayAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            id = id?.ToUpperInvariant() ?? string.Empty;
            var country = await _db.Countries.FirstOrDefaultAsync(c => c.Id == id);
            return country != null
                ? (IActionResult) Ok(country.ToModel())
                : NotFound();

        }

    }
}
