using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tickets.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(
        "CanViewAllTickets",
        policyBuilder => policyBuilder.RequireClaim("IsAdmin", "true"))
    .AddPolicy(
        "CanDeleteTicket",
        policyBuilder => policyBuilder.RequireClaim("IsAdmin", "true"))
    .AddPolicy(
        "CanEditTicket",
        policyBuilder => policyBuilder.RequireClaim("IsAdmin", "true"));

builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
