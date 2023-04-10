using P013WebSite.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>(); //burada veritabaný iþlemleri için kullandýðýmýz context

// veritabaný oluþturmak için üst menüden Tools > Nuget Package Manager > Package manager console menüsünden komut yazma ekranýný açýyoruz.
// Bu ekrana add-migrations InitialCreate yazýp enter a basýyoruz.
// Migrations klasörü ve initialcreate classý oluþtuktan sonra update-database yazýp enter a basýyoruz
// Done mesajý geldiyse iþlem baþarýlýdýr.

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

app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
          );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
