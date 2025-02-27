using Docsnap;
using Docsnap.Models;

namespace docsnapAPI;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configura a aplicação para usar a funcionalidade da biblioteca docsnapLib
        app.UseDocsnap();
        //app.ChangeDocsnapPath("/md");

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
