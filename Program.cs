var builder = WebApplication.CreateBuilder(args);

// Adiciona política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend",
        policy => policy
            .WithOrigins("http://localhost:5173","https://my-store-digi-market.vercel.app")
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

builder.Services.AddControllers();

var app = builder.Build();

// Usa a política de CORS
app.UseCors("PermitirFrontend");

app.UseAuthorization();
app.MapControllers();

app.Run();