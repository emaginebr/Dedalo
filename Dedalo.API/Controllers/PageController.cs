using AutoMapper;
using Dedalo.Domain.Interfaces;
using Dedalo.DTO.Page;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NAuth.ACL.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Dedalo.API.Controllers
{
    [Route("website/{websiteId}/page")]
    [ApiController]
    [Authorize]
    public class PageController : ControllerBase
    {
        private readonly IPageService _pageService;
        private readonly IUserClient _userClient;
        private readonly IMapper _mapper;

        public PageController(
            IPageService pageService,
            IUserClient userClient,
            IMapper mapper
        )
        {
            _pageService = pageService;
            _userClient = userClient;
            _mapper = mapper;
        }

        [HttpGet("/page/{pageSlug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBySlug(string pageSlug, [FromQuery] string websiteSlug, [FromQuery] string domain)
        {
            var model = await _pageService.GetBySlugAsync(pageSlug, websiteSlug, domain);
            return Ok(_mapper.Map<PagePublicInfo>(model));
        }

        [HttpGet]
        public async Task<IActionResult> List(long websiteId)
        {
            var models = await _pageService.ListByWebsiteAsync(websiteId);
            var result = models.Select(m => _mapper.Map<PageInfo>(m));
            return Ok(result);
        }

        [HttpGet("{pageId}")]
        public async Task<IActionResult> GetById(long websiteId, long pageId)
        {
            var model = await _pageService.GetByIdAsync(pageId);
            if (model == null)
                return NotFound();
            return Ok(_mapper.Map<PageInfo>(model));
        }

        [HttpPost]
        public async Task<IActionResult> Insert(long websiteId, [FromBody] PageInsertInfo page)
        {
            page.WebsiteId = websiteId;
            var user = _userClient.GetUserInSession(HttpContext);
            var model = await _pageService.InsertAsync(page, user.UserId);
            return Created($"website/{websiteId}/page/{model.PageId}", _mapper.Map<PageInfo>(model));
        }

        [HttpPut("{pageId}")]
        public async Task<IActionResult> Update(long websiteId, long pageId, [FromBody] PageUpdateInfo page)
        {
            page.PageId = pageId;
            var user = _userClient.GetUserInSession(HttpContext);
            var model = await _pageService.UpdateAsync(page, user.UserId);
            return Ok(_mapper.Map<PageInfo>(model));
        }

        [HttpDelete("{pageId}")]
        public async Task<IActionResult> Delete(long websiteId, long pageId)
        {
            var user = _userClient.GetUserInSession(HttpContext);
            await _pageService.DeleteAsync(pageId, user.UserId);
            return NoContent();
        }
    }
}
