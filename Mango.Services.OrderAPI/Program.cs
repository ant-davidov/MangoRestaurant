using Mango.MessageBus;
using Mango.Services.OrderAPI.DbContexts;
using Mango.Services.OrderAPI.Messaging;
using Mango.Services.OrderAPI.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<IOrderRepository,OrderRepository>();
var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionBuilder.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(new OrderRepository(optionBuilder.Options));
builder.Services.AddHostedService<ConsumeRabbitMQHostedService>();
builder.Services.AddHostedService<ConsumeRabbitMGetResultPaymentStatus>();
builder.Services.AddSingleton<IMessageBus, MessageBus>();

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:7143/";
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "mango");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
