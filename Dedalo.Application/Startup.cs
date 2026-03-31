using System;
using Dedalo.Infra.Interfaces.Repository;
using Dedalo.Infra.Repository;
using Dedalo.Domain.Models;
using Dedalo.Domain.Services;
using Dedalo.Domain.Interfaces;
using Dedalo.Domain.Validators;
using Dedalo.DTO.Website;
using Dedalo.DTO.Page;
using Dedalo.DTO.Menu;
using Dedalo.DTO.Content;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NAuth.ACL;
using NAuth.ACL.Interfaces;
using zTools.ACL.Interfaces;
using zTools.ACL;
using Dedalo.Infra.Mappers;

namespace Dedalo.Application
{
    public static class Startup
    {
        private static void injectDependency(Type serviceType, Type implementationType, IServiceCollection services, bool scoped = true)
        {
            if (scoped)
                services.AddScoped(serviceType, implementationType);
            else
                services.AddTransient(serviceType, implementationType);
        }

        public static void ConfigureDedalo(this IServiceCollection services, bool scoped = true)
        {
            #region Tenant
            services.AddHttpContextAccessor();
            services.AddScoped<ITenantContext, TenantContext>();
            services.AddScoped<ITenantResolver, TenantResolver>();
            services.AddScoped<TenantDbContextFactory>();
            services.AddScoped(sp => sp.GetRequiredService<TenantDbContextFactory>().CreateDbContext());
            #endregion

            #region AutoMapper
            services.AddAutoMapper(typeof(WebsiteProfile), typeof(PageProfile), typeof(MenuProfile), typeof(ContentProfile));
            #endregion

            #region Repository
            injectDependency(typeof(IWebsiteRepository<WebsiteModel>), typeof(WebsiteRepository), services, scoped);
            injectDependency(typeof(IPageRepository<PageModel>), typeof(PageRepository), services, scoped);
            injectDependency(typeof(IMenuRepository<MenuModel>), typeof(MenuRepository), services, scoped);
            injectDependency(typeof(IContentRepository<ContentModel>), typeof(ContentRepository), services, scoped);
            #endregion

            #region Client
            services.AddHttpClient();
            injectDependency(typeof(IUserClient), typeof(UserClient), services, scoped);
            injectDependency(typeof(IChatGPTClient), typeof(ChatGPTClient), services, scoped);
            injectDependency(typeof(IMailClient), typeof(MailClient), services, scoped);
            injectDependency(typeof(IFileClient), typeof(FileClient), services, scoped);
            injectDependency(typeof(IStringClient), typeof(StringClient), services, scoped);
            injectDependency(typeof(IDocumentClient), typeof(DocumentClient), services, scoped);
            #endregion

            #region Validators
            services.AddScoped<IValidator<WebsiteInsertInfo>, WebsiteInsertValidator>();
            services.AddScoped<IValidator<WebsiteUpdateInfo>, WebsiteUpdateValidator>();
            services.AddScoped<IValidator<PageInsertInfo>, PageInsertValidator>();
            services.AddScoped<IValidator<PageUpdateInfo>, PageUpdateValidator>();
            services.AddScoped<IValidator<MenuInsertInfo>, MenuInsertValidator>();
            services.AddScoped<IValidator<MenuUpdateInfo>, MenuUpdateValidator>();
            services.AddScoped<IValidator<ContentInsertInfo>, ContentInsertValidator>();
            services.AddScoped<IValidator<ContentUpdateInfo>, ContentUpdateValidator>();
            services.AddScoped<IValidator<ContentAreaInfo>, ContentAreaValidator>();
            #endregion

            #region Service
            injectDependency(typeof(IWebsiteService), typeof(WebsiteService), services, scoped);
            injectDependency(typeof(IPageService), typeof(PageService), services, scoped);
            injectDependency(typeof(IMenuService), typeof(MenuService), services, scoped);
            injectDependency(typeof(IContentService), typeof(ContentService), services, scoped);
            #endregion

            services.AddScoped<ITenantSecretProvider, NAuthTenantSecretProvider>();
            services.AddNAuth<NAuthTenantProvider>();
            services.AddNAuthAuthentication("BasicAuthentication");
        }
    }
}
