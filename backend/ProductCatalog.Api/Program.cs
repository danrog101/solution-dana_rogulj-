using ProductCatalog.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IProductSource, DummyJsonProductSource>(client =>
{
    string baseUrl = builder.Configuration["ProductSource:BaseUrl"]
        ?? "https://dummyjson.com/";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
{
    policy.WithOrigins("http://localhost:5173");
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
});

app.MapControllers();

app.Run();