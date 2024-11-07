using Microsoft.EntityFrameworkCore;
using FinancialManagementApp.Data;
using FinancialManagementApp.Services.Interfaces;
using FinancialManagementApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DotNetEnv();

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policyBuilder =>
    
     policyBuilder.WithOrigins("https://financial-management-app-beta.vercel.app")
                      .AllowAnyHeader()
                      .AllowAnyMethod());

});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Obt�m a senha a partir da vari�vel de ambiente
var password = Environment.GetEnvironmentVariable("DATABASE_KEY");

if (!string.IsNullOrEmpty(password))
{
    // Substitui o marcador pela senha da vari�vel de ambiente
    connectionString = connectionString.Replace("REPLACE_WITH_ENV_VAR", password);
}

builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = "https://dev-tomuar4vp6ixaauc.us.auth0.com";
            options.Audience = "https://financialapi.com";
        });

       

builder.Services.AddControllers();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


