using AutoMapper;
using Dedalo.Infra.Interfaces.Repository;
using Dedalo.Infra.Context;
using Dedalo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dedalo.Infra.Repository
{
    public class MenuRepository : IMenuRepository<MenuModel>
    {
        private readonly DedaloContext _context;
        private readonly IMapper _mapper;

        public MenuRepository(DedaloContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MenuModel> InsertAsync(MenuModel model)
        {
            var row = _mapper.Map<Menu>(model);
            _context.Add(row);
            await _context.SaveChangesAsync();
            model.MenuId = row.MenuId;
            return model;
        }

        public async Task<MenuModel> UpdateAsync(MenuModel model)
        {
            var row = await _context.Menus.FindAsync(model.MenuId);
            _mapper.Map(model, row);
            _context.Menus.Update(row);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<MenuModel> GetByIdAsync(long id)
        {
            var row = await _context.Menus.FindAsync(id);
            if (row == null)
                return null;
            return _mapper.Map<MenuModel>(row);
        }

        public async Task<IEnumerable<MenuModel>> ListByWebsiteAsync(long websiteId)
        {
            var rows = await _context.Menus
                .Where(x => x.WebsiteId == websiteId)
                .OrderBy(x => x.Name)
                .ToListAsync();
            return rows.Select(r => _mapper.Map<MenuModel>(r));
        }

        public async Task<IEnumerable<MenuModel>> ListByWebsiteSlugOrDomainAsync(string websiteSlug, string domain)
        {
            var query = _context.Menus
                .Include(m => m.Website)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(domain))
                query = query.Where(m => m.Website.CustomDomain == domain);
            else
                query = query.Where(m => m.Website.WebsiteSlug == websiteSlug);

            var rows = await query.OrderBy(m => m.Name).ToListAsync();
            return rows.Select(r => _mapper.Map<MenuModel>(r));
        }

        public async Task DeleteAsync(long id)
        {
            var row = await _context.Menus.FindAsync(id);
            if (row != null)
            {
                _context.Menus.Remove(row);
                await _context.SaveChangesAsync();
            }
        }
    }
}
