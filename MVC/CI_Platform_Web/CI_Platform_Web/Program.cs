using CI_Platform.Entities.CIPlatformDbContext;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//email
//var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
//builder.Services.AddSingleton(emailConfig);
//email
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CiPlatformDbContext>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();    
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IMissionCard, MissionCard>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IVolunteerMissionCard, VolunteerMissionCard>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


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

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
