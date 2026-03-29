using Dedalo.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NAuth.ACL.Interfaces;
using zTools.ACL.Interfaces;
using System.Threading.Tasks;

namespace Dedalo.API.Controllers
{
    [Route("image")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IFileClient _fileClient;
        private readonly IUserClient _userClient;
        private readonly ITenantResolver _tenantResolver;
        private readonly IWebsiteService _websiteService;

        public ImageController(
            IFileClient fileClient,
            IUserClient userClient,
            ITenantResolver tenantResolver,
            IWebsiteService websiteService
        )
        {
            _fileClient = fileClient;
            _userClient = userClient;
            _tenantResolver = tenantResolver;
            _websiteService = websiteService;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            var bucketName = _tenantResolver.BucketName;
            var url = await _fileClient.UploadFileAsync(bucketName, file);

            return Ok(new { url });
        }

        [HttpPost("upload/logo/{websiteId}")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> UploadLogo(long websiteId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            var user = _userClient.GetUserInSession(HttpContext);
            var website = await _websiteService.GetByIdAsync(websiteId, user.UserId);
            if (website == null)
                return NotFound();

            var bucketName = _tenantResolver.BucketName;
            var url = await _fileClient.UploadFileAsync(bucketName, file);

            website.LogoUrl = url;
            website.MarkUpdated();
            await _websiteService.UpdateLogoAsync(websiteId, url, user.UserId);

            return Ok(new { url });
        }
    }
}
