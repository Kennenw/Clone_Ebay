using Microsoft.EntityFrameworkCore;
using Web_Clone_Ebay.Models.EBayDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var connectionString = builder.Configuration.GetConnectionString("EbayConnection");
//Kết nối db
builder.Services.AddDbContext<EBayDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));

builder.Services.AddCors(option=>{
    option.AddPolicy("allow_origin", policy => {
        policy.WithOrigins("https://localhost:5001","http://localhost:5167")
            .AllowAnyHeader() //Cho phép rq tất cả header
            .AllowAnyMethod() //Cho phép rq tất cả method (POST,PUT,GET,DELETE,OPTION)
            .AllowCredentials(); ////Cho phép cookie...
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("allow_origin");
app.UseHttpsRedirection();
app.MapControllers(); 

app.UseStaticFiles();
app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();
