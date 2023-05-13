using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using OrchardCore.Modules;
using System;

namespace Account
{
    public class Startup : StartupBase
    {

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                name: "Home",
                areaName: "Account",
                pattern: "{controller=Account}/{action=Index}/{id?}"
            );
        }
    }
}