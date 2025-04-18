using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Splitwise.Model;
using Splitwise.Model.Chat;
using Splitwise.Model.Helper;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connection String

var connectionString = builder.Configuration.GetConnectionString("SplitwiseConnection") ?? throw new InvalidOperationException("Connection string 'SplitwiseConnection' not found.");
builder.Services.AddDbContext<SplitwiseContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
}).AddEntityFrameworkStores<SplitwiseContext>();


builder.Services.AddScoped<IGroupRepository,GroupRepository>();
builder.Services.AddScoped<IExpenseRepository,ExpenseRepository>();
builder.Services.AddScoped<ISettleRepository,SettleRepository>();
builder.Services.AddScoped(typeof(Lazy<>), typeof(LazyService<>));

//SignalR

builder.Services.AddSignalR();

//JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true

    };

});
builder.Services.AddAuthentication();




// Add CORS policy
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        policy =>
//        {
//            policy.AllowAnyOrigin()
//                  .AllowAnyMethod()
//                  .AllowAnyHeader(); 
//        });
//});

// Add CORS policy with chat
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Angular's dev server
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
        });
});
var app = builder.Build();
// Enable CORS
app.UseCors("AllowAll");
 

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

//app.UseEndpoints(endpoint =>
//{
//    endpoint.MapHub<ChatHub>("/chats");
//});

//chatgpt
//app.UseEndpoints(endpoints =>
//{
//    //endpoints.MapControllers();
//    endpoints.MapHub<ChatHub>("/chatHub");
//});
app.MapHub<ChatHub>("/chatHub");

app.Run();
