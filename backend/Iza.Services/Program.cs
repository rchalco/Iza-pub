using PlumbingProps.Config;
using System.Net;

const string CorsPolicyName = "MyPolicy";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

ConfigureServerEndpoint(builder);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "HiddenVilla_Api v1"));
}

app.UseCors(CorsPolicyName);
app.UseAuthorization();
app.MapControllers();
app.Run();

static void ConfigureServerEndpoint(WebApplicationBuilder webBuilder)
{
    var config = ConfigManager.GetConfiguration();
    var mode = config.GetSection("mode").Value;

    if (!string.Equals(mode, "server", StringComparison.OrdinalIgnoreCase))
    {
        return;
    }

    var ipText = config.GetSection("iplookup").Value;
    var portText = config.GetSection("port").Value;

    if (!IPAddress.TryParse(ipText, out var ipAddress) || !int.TryParse(portText, out var port))
    {
        return;
    }

    webBuilder.WebHost.ConfigureKestrel((_, serverOptions) =>
    {
        serverOptions.Listen(ipAddress, port);
    });
}

