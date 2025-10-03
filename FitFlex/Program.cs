using FitFlex.Application.Interfaces;
using FitFlex.Infrastructure.Db_context;
using FitFlex.Infrastructure.Repository_service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FitFlex.Application.DTO_s.Dto_validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using FitFlex.Application.DTOs.Dto_validation;
using FitFlex.Application.DTO_s.Trainers_dto;
using FitFlex.Infrastructure.Interfaces;
using FitFlex.Application.services;
using FitFlex.Middleware;
using Microsoft.OpenApi.Models;
using FitFlex.Application.Mapper;
using Microsoft.AspNetCore.SignalR;
using FitFlex.Chatting;
using FitFlex.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// JWT Settings
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

    // SignalR JWT token from query string
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// Authorization
builder.Services.AddAuthorization();

// Controllers
builder.Services.AddControllers();

// DbContext
builder.Services.AddDbContext<MyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection"))
);

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterdtoValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<TrainersValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<TrainerRegisterDtoValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<TrainerLoginDtoValidation>();

// HttpContext accessor
builder.Services.AddHttpContextAccessor();

// Repositories & Services
builder.Services.AddScoped(typeof(IRepository<>), typeof(repository<>));
builder.Services.AddScoped<IAuth, AuthService>();
builder.Services.AddScoped<ITrainerservice, TrainerService>();
builder.Services.AddScoped<ISubscription, SubscriptionService>();
builder.Services.AddScoped<IUserSubscription, IuserSelectionService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<Ibooking, BookingService>();
builder.Services.AddScoped<IWorkoutPlanService, WorkoutPlanService>();
builder.Services.AddScoped<IUserWorkoutAssignmentService, UserWorkoutAssignmentService>();
builder.Services.AddScoped<IAdditionalSubscriptionService, AdditionalSubscriptionService>();
builder.Services.AddScoped<IAttendance, AttendanceService>();
builder.Services.AddScoped<ISessions, SessionService>();
builder.Services.AddScoped<IUserSession, UserSessionService>();
builder.Services.AddScoped<IDietPlanService, DietPlanService>();





// SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
});
builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

// Swagger with JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "FitFlex API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Stripe
Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS
app.UseHttpsRedirection();

// CORS
app.UseCors("AllowReactApp");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Custom Middleware — now after Authentication
app.UseMiddleware<UserIdMiddleware>();

// Controllers
app.MapControllers();

// SignalR Hub
app.MapHub<ChatHub>("/chatHub");

app.Run();
