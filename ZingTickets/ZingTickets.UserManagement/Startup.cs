using Amazon;
using Amazon.CognitoIdentityProvider;
using ZingTickets.API;

namespace ZingTickets.UserManagement;

public class Startup : BaseStartup
{
    public Startup(IConfiguration configuration) : base(configuration)
    {
    }


    // This method gets called by the runtime. Use this method to add services to the container
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddCognitoIdentity();
        //services.AddSingleton<IAmazonCognitoIdentityProvider, AmazonCognitoIdentityProviderClient>(options =>
        //{
        //    var client = new AmazonCognitoIdentityProviderClient(RegionEndpoint.USEast1);
        //    return client;
        //});
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        base.Configure(app, env);
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            });
        });
    }
}