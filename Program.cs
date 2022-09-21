using MoviesWebApplication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DBMoviesContext>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseFileServer();
var options = new StaticFileOptions();
var contentTypeProvider = (FileExtensionContentTypeProvider)options.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
contentTypeProvider.Mappings.Add(".data", "application/octet-stream");
options.ContentTypeProvider = contentTypeProvider;
app.UseStaticFiles(options);

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider
//    (Path.Combine(builder.Environment.ContentRootPath, "GameFiles")),
//    RequestPath = "/GameFiles"
//});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
