using docsnapLib;

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
        StartupDocsnap.ConfigureDocsnap(app);

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
