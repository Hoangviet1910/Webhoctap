using Microsoft.EntityFrameworkCore;
using HoangViet.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Đăng ký EF Core SQL Server LocalDB (HoangViet_DevLearningDb)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=(localdb)\\mssqllocaldb;Database=HoangViet_DevLearningDb;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Đăng ký MVC Controllers & Views + MemoryCache (tiết kiệm Token AI) + HttpClient
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();

var app = builder.Build();

// 3. Tự động khởi tạo DB & Seed dữ liệu Đợt 1 (0% tiến độ, 0 điểm kỹ năng)
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.InitializeAsync(context);
    await InternTopic1Seed.EnsureSeededAsync(context);
    await InternTopic2Seed.EnsureSeededAsync(context);
    await InternTopic3Seed.EnsureSeededAsync(context);
    await InternTopic4Seed.EnsureSeededAsync(context);
    await InternTopic5Seed.EnsureSeededAsync(context);
    await DetailedTopic1Seed.EnsureSeededAsync(context);
    await DetailedTopic2Seed.EnsureSeededAsync(context);
    await DetailedTopic3Seed.EnsureSeededAsync(context);
    await DetailedTopic4Seed.EnsureSeededAsync(context);
    await DetailedTopic5Seed.EnsureSeededAsync(context);
    await DetailedTopic6Seed.EnsureSeededAsync(context);
    await DetailedTopic7Seed.EnsureSeededAsync(context);
    await DetailedTopic8Seed.EnsureSeededAsync(context);
    await DetailedTopic9Seed.EnsureSeededAsync(context);
    await DetailedTopic10Seed.EnsureSeededAsync(context);
    await DetailedTopic11Seed.EnsureSeededAsync(context);
    await DetailedTopic12Seed.EnsureSeededAsync(context);
    await DetailedTopic13Seed.EnsureSeededAsync(context);
    await DetailedTopic14Seed.EnsureSeededAsync(context);
    await DetailedTopic15Seed.EnsureSeededAsync(context);
    await DetailedTopic16Seed.EnsureSeededAsync(context);
}
catch (Exception ex)
{
    app.Logger.LogWarning(ex, "Lưu ý: Chưa thể tự động khởi tạo LocalDB lúc khởi động.");
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

// Vào thẳng trang học tập (Course Player tại Home/Index) không cần đăng nhập
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
