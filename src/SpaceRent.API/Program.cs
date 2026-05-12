using SpaceRent.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Allow large file uploads (videos up to 200MB)
builder.Services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 200 * 1024 * 1024; // 200MB
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register SpaceRent Services
builder.Services.AddSpaceRentServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serves uploaded files from wwwroot/uploads
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
