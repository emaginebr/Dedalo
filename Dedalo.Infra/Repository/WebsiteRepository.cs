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
    public class WebsiteRepository : IWebsiteRepository<WebsiteModel>
    {
        private readonly DedaloContext _context;
        private readonly IMapper _mapper;

        public WebsiteRepository(DedaloContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<WebsiteModel> InsertAsync(WebsiteModel model)
        {
            var row = _mapper.Map<Website>(model);
            _context.Add(row);
            await _context.SaveChangesAsync();
            model.WebsiteId = row.WebsiteId;
            return model;
        }

        public async Task<WebsiteModel> UpdateAsync(WebsiteModel model)
        {
            var row = await _context.Websites.FindAsync(model.WebsiteId);
            _mapper.Map(model, row);
            _context.Websites.Update(row);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<WebsiteModel> GetByIdAsync(long id)
        {
            var row = await _context.Websites.FindAsync(id);
            if (row == null)
                return null;
            return _mapper.Map<WebsiteModel>(row);
        }

        public async Task<WebsiteModel> GetBySlugAsync(string slug)
        {
            var row = await _context.Websites.FirstOrDefaultAsync(x => x.WebsiteSlug == slug);
            if (row == null)
                return null;
            return _mapper.Map<WebsiteModel>(row);
        }

        public async Task<WebsiteModel> GetByDomainAsync(string domain)
        {
            var row = await _context.Websites.FirstOrDefaultAsync(x => x.CustomDomain == domain);
            if (row == null)
                return null;
            return _mapper.Map<WebsiteModel>(row);
        }

        public async Task<IEnumerable<WebsiteModel>> ListByUserAsync(long userId)
        {
            var rows = await _context.Websites
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
            return rows.Select(r => _mapper.Map<WebsiteModel>(r));
        }

        public async Task UpdateLogoAsync(long id, string logoUrl)
        {
            var row = await _context.Websites.FindAsync(id);
            if (row != null)
            {
                row.LogoUrl = logoUrl;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(long id)
        {
            var row = await _context.Websites.FindAsync(id);
            if (row != null)
            {
                _context.Websites.Remove(row);
                await _context.SaveChangesAsync();
            }
        }
    }
}
