using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAppTinhVanCat_aspnetcore.Models;

[assembly: HostingStartup(typeof(WebAppTinhVanCat_aspnetcore.Areas.Identity.IdentityHostingStartup))]
namespace WebAppTinhVanCat_aspnetcore.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}