using AutoMapper;
using Dedalo.Domain.Interfaces;
using Dedalo.DTO.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NAuth.ACL.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Dedalo.API.Controllers
{
    [Route("website/{websiteId}/menu")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly IUserClient _userClient;
        private readonly IMapper _mapper;

        public MenuController(
            IMenuService menuService,
            IUserClient userClient,
            IMapper mapper
        )
        {
            _menuService = menuService;
            _userClient = userClient;
            _mapper = mapper;
        }

        [HttpGet("/menu")]
        [AllowAnonymous]
        public async Task<IActionResult> ListPublic([FromQuery] string websiteSlug, [FromQuery] string domain)
        {
            var models = await _menuService.ListPublicAsync(websiteSlug, domain);
            var result = models.Select(m => _mapper.Map<MenuInfo>(m));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> List(long websiteId)
        {
            var models = await _menuService.ListByWebsiteAsync(websiteId);
            var result = models.Select(m => _mapper.Map<MenuInfo>(m));
            return Ok(result);
        }

        [HttpGet("{menuId}")]
        public async Task<IActionResult> GetById(long websiteId, long menuId)
        {
            var model = await _menuService.GetByIdAsync(menuId);
            if (model == null)
                return NotFound();
            return Ok(_mapper.Map<MenuInfo>(model));
        }

        [HttpPost]
        public async Task<IActionResult> Insert(long websiteId, [FromBody] MenuInsertInfo menu)
        {
            menu.WebsiteId = websiteId;
            var user = _userClient.GetUserInSession(HttpContext);
            var model = await _menuService.InsertAsync(menu, user.UserId);
            return Created($"website/{websiteId}/menu/{model.MenuId}", _mapper.Map<MenuInfo>(model));
        }

        [HttpPut("{menuId}")]
        public async Task<IActionResult> Update(long websiteId, long menuId, [FromBody] MenuUpdateInfo menu)
        {
            menu.MenuId = menuId;
            var user = _userClient.GetUserInSession(HttpContext);
            var model = await _menuService.UpdateAsync(menu, user.UserId);
            return Ok(_mapper.Map<MenuInfo>(model));
        }

        [HttpDelete("{menuId}")]
        public async Task<IActionResult> Delete(long websiteId, long menuId)
        {
            var user = _userClient.GetUserInSession(HttpContext);
            await _menuService.DeleteAsync(menuId, user.UserId);
            return NoContent();
        }
    }
}
