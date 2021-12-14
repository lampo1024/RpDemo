var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((_, bd) =>
{
    bd.AddAgileConfig();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapReverseProxy();
app.Run();