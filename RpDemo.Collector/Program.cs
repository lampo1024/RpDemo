var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/hello", (HttpContext context) =>
{
    var host = context.Request.Host.Host;
    var port = context.Request.Host.Port;
    return Results.Ok(new
    {
        host,
        port
    });
});

app.MapGet("/api/upload", async (HttpRequest req) =>
 {
     if (!req.HasFormContentType)
     {
         return Results.Ok(new { success = false, message = "Upload failed." });
     }

     var form = await req.ReadFormAsync();
     var file = form.Files["file"];
     if (file is null)
     {
         return Results.Ok(new { success = false, message = "Upload failed." });
     }

     var uploads = Path.Combine(@"d:\uploads", file.FileName);
     await using var fileStream = File.OpenWrite(uploads);
     await using var uploadStream = file.OpenReadStream();
     await uploadStream.CopyToAsync(fileStream);
     return Results.Ok(new
     {
         success = true,
         message = "Upload successful."
     });
 }).Accepts<IFormFile>("multipart/form-data");

app.MapGet("/health/check", () => Results.Ok());

app.Run();