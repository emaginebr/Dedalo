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
    public class ContentRepository : IContentRepository<ContentModel>
    {
        private readonly DedaloContext _context;
        private readonly IMapper _mapper;

        public ContentRepository(DedaloContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ContentModel> InsertAsync(ContentModel model)
        {
            var row = _mapper.Map<Content>(model);
            _context.Add(row);
            await _context.SaveChangesAsync();
            model.ContentId = row.ContentId;
            return model;
        }

        public async Task<ContentModel> UpdateAsync(ContentModel model)
        {
            var row = await _context.Contents.FindAsync(model.ContentId);
            _mapper.Map(model, row);
            _context.Contents.Update(row);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<ContentModel> GetByIdAsync(long id)
        {
            var row = await _context.Contents.FindAsync(id);
            if (row == null)
                return null;
            return _mapper.Map<ContentModel>(row);
        }

        public async Task<IEnumerable<ContentModel>> ListByPageAsync(long pageId)
        {
            var rows = await _context.Contents
                .Where(x => x.PageId == pageId)
                .OrderBy(x => x.Index)
                .ToListAsync();
            return rows.Select(r => _mapper.Map<ContentModel>(r));
        }

        public async Task<IEnumerable<ContentModel>> ListByPageSlugAsync(string pageSlug, string websiteSlug, string domain)
        {
            var query = _context.Contents
                .Include(c => c.Page)
                    .ThenInclude(p => p.Website)
                .Where(c => c.Page.PageSlug == pageSlug);

            if (!string.IsNullOrWhiteSpace(domain))
                query = query.Where(c => c.Page.Website.CustomDomain == domain);
            else
                query = query.Where(c => c.Page.Website.WebsiteSlug == websiteSlug);

            var rows = await query.OrderBy(c => c.Index).ToListAsync();
            return rows.Select(r => _mapper.Map<ContentModel>(r));
        }

        public async Task<IEnumerable<ContentModel>> ListBySlugAsync(long pageId, string contentSlug)
        {
            var rows = await _context.Contents
                .Where(x => x.PageId == pageId && x.ContentSlug == contentSlug)
                .OrderBy(x => x.Index)
                .ToListAsync();
            return rows.Select(r => _mapper.Map<ContentModel>(r));
        }

        public async Task InsertBatchAsync(IEnumerable<ContentModel> models)
        {
            foreach (var model in models)
            {
                var row = _mapper.Map<Content>(model);
                _context.Add(row);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBatchAsync(IEnumerable<ContentModel> models)
        {
            foreach (var model in models)
            {
                var row = await _context.Contents.FindAsync(model.ContentId);
                if (row != null)
                {
                    _mapper.Map(model, row);
                    _context.Contents.Update(row);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBatchAsync(IEnumerable<long> ids)
        {
            var rows = await _context.Contents
                .Where(x => ids.Contains(x.ContentId))
                .ToListAsync();
            _context.Contents.RemoveRange(rows);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var row = await _context.Contents.FindAsync(id);
            if (row != null)
            {
                _context.Contents.Remove(row);
                await _context.SaveChangesAsync();
            }
        }
    }
}
