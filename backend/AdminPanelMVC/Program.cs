
using AdmipPanel.BL.Extensions;
using Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); // Включение опции перекомпиляции представления после изменения и сохранения cshtml файла
builder.Services.AddAuthContext();
builder.Services.AddBackendContext();

builder.Services.AddIdentityManagers();
builder.Services.AddAccountServiceDependency();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Restaraunt/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Restaraunt}/{action=Index}/{id?}");

app.Run();
