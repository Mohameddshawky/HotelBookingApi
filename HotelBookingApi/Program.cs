using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Application.Interfaces.Services;
using HotelBookingApi.Application.Interfaces.Notifications;
using HotelBookingApi.Application.Mappings;
using HotelBookingApi.Application.Services;
using HotelBookingApi.Application.Strategies.Sorting;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Infrastructure.Data;
using HotelBookingApi.Infrastructure.Configuration;
using HotelBookingApi.Infrastructure.Repositories;
using HotelBookingApi.Infrastructure.Services;
using HotelBookingApi.Infrastructure.Notifications;
using HotelBookingApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using HotelBookingApi.Features.Bookings.CreateBooking;
using HotelBookingApi.Features.Bookings.GetBookingById;
using HotelBookingApi.Features.Bookings.GetAllBookings;
using HotelBookingApi.Features.Bookings.GetGuestBookings;
using HotelBookingApi.Features.Bookings.GetGuestBookingsByEmail;
using HotelBookingApi.Features.Bookings.ConfirmBooking;
using HotelBookingApi.Features.Bookings.CancelBooking;
using HotelBookingApi.Features.Bookings.CheckInBooking;
using HotelBookingApi.Features.Bookings.CheckOutBooking;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuration
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Repositories
builder.Services.AddScoped<IAmenityRepository, AmenityRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IAmenityService, AmenityService>();
// Bookings VSA Handlers
builder.Services.AddScoped<CreateBookingHandler>();
builder.Services.AddScoped<GetBookingByIdHandler>();
builder.Services.AddScoped<GetAllBookingsHandler>();
builder.Services.AddScoped<GetGuestBookingsHandler>();
builder.Services.AddScoped<GetGuestBookingsByEmailHandler>();
builder.Services.AddScoped<ConfirmBookingHandler>();
builder.Services.AddScoped<CancelBookingHandler>();
builder.Services.AddScoped<CheckInBookingHandler>();
builder.Services.AddScoped<CheckOutBookingHandler>();
builder.Services.AddScoped<IGuestService, GuestService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Sorting Strategies
builder.Services.AddScoped<IRoomSortStrategy, SortByPriceStrategy>();
builder.Services.AddScoped<IRoomSortStrategy, SortByNameStrategy>();
builder.Services.AddScoped<IRoomSortStrategy, SortByRatingStrategy>();
builder.Services.AddScoped<RoomSortStrategyFactory>();

// Notifications
builder.Services.AddScoped<INotificationStrategy, EmailNotificationStrategy>();

// Identity
builder.Services.AddIdentity<Staff, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// AutoMapper
builder.Services.AddAutoMapper(config => {
    config.AddProfile<MappingProfile>();
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new HotelBookingApi.Converters.DateFormatConverter());
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelBookingApi", Version = "v1" });
    c.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

app.UseMiddleware<ExceptionMiddleware>();

// Seed Default Roles and Admin User
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Staff>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));
        
    if (!await roleManager.RoleExistsAsync("Staff"))
        await roleManager.CreateAsync(new IdentityRole("Staff"));

    if (await userManager.FindByNameAsync("admin") == null)
    {
        var adminUser = new Staff
        {
            UserName = "admin",
            Email = "admin@hotelbooking.com"
        };
        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Staff");
        }
    }
}

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
// Bookings VSA Endpoints
HotelBookingApi.Features.Bookings.CreateBooking.CreateBookingEndpoint.MapEndpoint(app);
HotelBookingApi.Features.Bookings.GetBookingById.GetBookingByIdEndpoint.MapEndpoint(app);
HotelBookingApi.Features.Bookings.GetAllBookings.GetAllBookingsEndpoint.MapEndpoint(app);
HotelBookingApi.Features.Bookings.GetGuestBookings.GetGuestBookingsEndpoint.MapEndpoint(app);
HotelBookingApi.Features.Bookings.GetGuestBookingsByEmail.GetGuestBookingsByEmailEndpoint.MapEndpoint(app);
HotelBookingApi.Features.Bookings.ConfirmBooking.ConfirmBookingEndpoint.MapEndpoint(app);
HotelBookingApi.Features.Bookings.CancelBooking.CancelBookingEndpoint.MapEndpoint(app);
HotelBookingApi.Features.Bookings.CheckInBooking.CheckInBookingEndpoint.MapEndpoint(app);
HotelBookingApi.Features.Bookings.CheckOutBooking.CheckOutBookingEndpoint.MapEndpoint(app);

app.Run();
