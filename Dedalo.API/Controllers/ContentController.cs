using AutoMapper;
using Dedalo.Domain.Interfaces;
using Dedalo.DTO.Content;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NAuth.ACL.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Dedalo.API.Controllers
{
    [Route("website/{websiteId}/page/{pageId}/content")]
    [ApiController]
    [Authorize]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly IUserClient _userClient;
        private readonly IMapper _mapper;

        public ContentController(
            IContentService contentService,
            IUserClient userClient,
            IMapper mapper
        )
        {
            _contentService = contentService;
            _userClient = userClient;
            _mapper = mapper;
        }

        [HttpGet("/content/{pageSlug}")]
        [AllowAnonymous]
        public async Task<IActionResult> ListPublic(string pageSlug, [FromQuery] string websiteSlug, [FromQuery] string domain)
        {
            var models = await _contentService.ListPublicAsync(pageSlug, websiteSlug, domain);
            var result = models.Select(m => _mapper.Map<ContentInfo>(m));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> List(long websiteId, long pageId)
        {
            var models = await _contentService.ListByPageAsync(pageId);
            var result = models.Select(m => _mapper.Map<ContentInfo>(m));
            return Ok(result);
        }

        [HttpGet("{contentId}")]
        public async Task<IActionResult> GetById(long websiteId, long pageId, long contentId)
        {
            var model = await _contentService.GetByIdAsync(contentId);
            if (model == null)
                return NotFound();
            return Ok(_mapper.Map<ContentInfo>(model));
        }

        [HttpPut("area")]
        public async Task<IActionResult> SaveArea(long websiteId, long pageId, [FromBody] ContentAreaInfo area)
        {
            area.WebsiteId = websiteId;
            area.PageId = pageId;
            var user = _userClient.GetUserInSession(HttpContext);
            var models = await _contentService.SaveAreaAsync(area, user.UserId);
            var result = models.Select(m => _mapper.Map<ContentInfo>(m));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(long websiteId, long pageId, [FromBody] ContentInsertInfo content)
        {
            content.WebsiteId = websiteId;
            content.PageId = pageId;
            var user = _userClient.GetUserInSession(HttpContext);
            var model = await _contentService.InsertAsync(content, user.UserId);
            return Created($"website/{websiteId}/page/{pageId}/content/{model.ContentId}", _mapper.Map<ContentInfo>(model));
        }

        [HttpPut("{contentId}")]
        public async Task<IActionResult> Update(long websiteId, long pageId, long contentId, [FromBody] ContentUpdateInfo content)
        {
            content.ContentId = contentId;
            var user = _userClient.GetUserInSession(HttpContext);
            var model = await _contentService.UpdateAsync(content, user.UserId);
            return Ok(_mapper.Map<ContentInfo>(model));
        }

        [HttpDelete("{contentId}")]
        public async Task<IActionResult> Delete(long websiteId, long pageId, long contentId)
        {
            var user = _userClient.GetUserInSession(HttpContext);
            await _contentService.DeleteAsync(contentId, user.UserId);
            return NoContent();
        }
    }
}
