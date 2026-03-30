using AutoMapper;
using Dedalo.Domain.Interfaces;
using Dedalo.DTO.Website;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NAuth.ACL.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Dedalo.API.Controllers
{
    [Route("website")]
    [ApiController]
    [Authorize]
    public class WebsiteController : ControllerBase
    {
        private readonly IWebsiteService _websiteService;
        private readonly IUserClient _userClient;
        private readonly IMapper _mapper;

        public WebsiteController(
            IWebsiteService websiteService,
            IUserClient userClient,
            IMapper mapper
        )
        {
            _websiteService = websiteService;
            _userClient = userClient;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var user = _userClient.GetUserInSession(HttpContext);
            var models = await _websiteService.ListByUserAsync(user.UserId);
            var result = models.Select(m => _mapper.Map<WebsiteInfo>(m));
            return Ok(result);
        }

        [HttpGet("{websiteId}")]
        public async Task<IActionResult> GetById(long websiteId)
        {
            var user = _userClient.GetUserInSession(HttpContext);
            var model = await _websiteService.GetByIdAsync(websiteId, user.UserId);
            if (model == null)
                return NotFound();
            return Ok(_mapper.Map<WebsiteInfo>(model));
        }

        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var model = await _websiteService.GetBySlugAsync(slug);
            return Ok(_mapper.Map<WebsiteInfo>(model));
        }

        [HttpGet("domain/{domain}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByDomain(string domain)
        {
            var model = await _websiteService.GetByDomainAsync(domain);
            return Ok(_mapper.Map<WebsiteInfo>(model));
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] WebsiteInsertInfo website)
        {
            var user = _userClient.GetUserInSession(HttpContext);
            var model = await _websiteService.InsertAsync(website, user.UserId);
            return Created($"website/{model.WebsiteId}", _mapper.Map<WebsiteInfo>(model));
        }

        [HttpPut("{websiteId}")]
        public async Task<IActionResult> Update(long websiteId, [FromBody] WebsiteUpdateInfo website)
        {
            website.WebsiteId = websiteId;
            var user = _userClient.GetUserInSession(HttpContext);
            var model = await _websiteService.UpdateAsync(website, user.UserId);
            return Ok(_mapper.Map<WebsiteInfo>(model));
        }
    }
}
