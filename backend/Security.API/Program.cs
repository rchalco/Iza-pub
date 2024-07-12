using PlumbingProps.Config;
using System.Net;


///Configuration host
var MyAllowSpecificOrigins = "MyPolicy";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                      });
});

if (ConfigManager.GetConfiguration().GetSection("mode").Value.Equals("server"))
{
    IPAddress iPAddress = IPAddress.Parse(ConfigManager.GetConfiguration().GetSection("iplookup").Value!);
    int portServer = Convert.ToInt32(ConfigManager.GetConfiguration().GetSection("port").Value!);
    builder.WebHost.ConfigureKestrel((context, serverOptions) =>
    {
        serverOptions.Listen(iPAddress, portServer);
    });
}

builder.Services.AddControllers();

///Configuration application
var app = builder.Build();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
