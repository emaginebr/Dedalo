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
    public class PageRepository : IPageRepository<PageModel>
    {
        private readonly DedaloContext _context;
        private readonly IMapper _mapper;

        public PageRepository(DedaloContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PageModel> InsertAsync(PageModel model)
        {
            var row = _mapper.Map<Page>(model);
            _context.Add(row);
            await _context.SaveChangesAsync();
            model.PageId = row.PageId;
            return model;
        }

        public async Task<PageModel> UpdateAsync(PageModel model)
        {
            var row = await _context.Pages.FindAsync(model.PageId);
            _mapper.Map(model, row);
            _context.Pages.Update(row);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<PageModel> GetByIdAsync(long id)
        {
            var row = await _context.Pages.FindAsync(id);
            if (row == null)
                return null;
            return _mapper.Map<PageModel>(row);
        }

        public async Task<PageModel> GetBySlugAsync(string pageSlug, string websiteSlug, string domain)
        {
            var query = _context.Pages
                .Include(p => p.Website)
                .Include(p => p.Contents.OrderBy(c => c.Index))
                .Where(p => p.PageSlug == pageSlug);

            if (!string.IsNullOrWhiteSpace(domain))
                query = query.Where(p => p.Website.CustomDomain == domain);
            else
                query = query.Where(p => p.Website.WebsiteSlug == websiteSlug);

            var row = await query.FirstOrDefaultAsync();
            if (row == null)
                return null;
            return _mapper.Map<PageModel>(row);
        }

        public async Task<IEnumerable<PageModel>> ListByWebsiteAsync(long websiteId)
        {
            var rows = await _context.Pages
                .Where(x => x.WebsiteId == websiteId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
            return rows.Select(r => _mapper.Map<PageModel>(r));
        }

        public async Task<IEnumerable<PageModel>> ListByWebsiteSlugOrDomainAsync(string websiteSlug, string domain)
        {
            var query = _context.Pages
                .Include(p => p.Website)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(domain))
                query = query.Where(p => p.Website.CustomDomain == domain);
            else
                query = query.Where(p => p.Website.WebsiteSlug == websiteSlug);

            var rows = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return rows.Select(r => _mapper.Map<PageModel>(r));
        }

        public async Task DeleteAsync(long id)
        {
            var row = await _context.Pages.FindAsync(id);
            if (row != null)
            {
                _context.Pages.Remove(row);
                await _context.SaveChangesAsync();
            }
        }
    }
}
