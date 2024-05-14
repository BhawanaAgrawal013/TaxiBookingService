using Microsoft.OpenApi.Models;

namespace TaxiBookingApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "TaxiBookingServiceApi", Version = "v1" });
            });

            LogManager.LoadConfiguration(System.String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            var connectionstring = builder.Configuration.GetConnectionString("myconn");
            builder.Services.AddDbContext<TaxiBookingContext>(options => options.UseSqlServer(connectionstring));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        //  ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context => {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                }
            );

            var corspolicy = "corspolicy";

            builder.Services.AddCors(p => {
                p.AddPolicy(name: corspolicy, build =>
                {
                    build.WithOrigins("https://localhost:7062/").WithMethods("PUT").WithHeaders();

                });
            });

            builder.Services.AddScoped<TaxiBookingContext>();
            builder.Services.AddScoped<ILog, LogNLog>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IHashingService, HashingService>();
            builder.Services.AddScoped<IBookingStatusService, BookingStatusService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IPaymentModeService, PaymentModeService>();
            builder.Services.AddScoped<ICancellationReasonService, CancellationReasonService>();
            builder.Services.AddScoped<ICancelledBookingService, CancelledBookingService>();
            builder.Services.AddScoped<ICityService, CityService>();
            builder.Services.AddScoped<IStateService, StateService>();
            builder.Services.AddScoped<IAreaService, AreaService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IDriverService, DriverService>();
            builder.Services.AddScoped<IVehicleTypeService, VehicleTypeService>();
            builder.Services.AddScoped<IVehicleDetailsService, VehicleDetailsService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IPaymentModeService, PaymentModeService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IRatingService, RatingService>();

            builder.Services.AddScoped<IStateUnitOfWork, StateUnitOfWork>();
            builder.Services.AddScoped<IAreaUnitOfWork, AreaUnitOfWork>();
            builder.Services.AddScoped<IBookingStatusUnitOfWork,  BookingStatusUnitOfWork>();
            builder.Services.AddScoped<IBookingUnitOfWork,  BookingUnitOfWork>();
            builder.Services.AddScoped<ICancellationBookingUOW,  CancellationBookingUOW>();
            builder.Services.AddScoped<ICityUnitOfWork, CityUnitOfWork>();
            builder.Services.AddScoped<IDriverUnitOfWork, DriverUnitOfWork>();
            builder.Services.AddScoped<ILocationUnitOfWork, LocationUnitOfWork>();
            builder.Services.AddScoped<IPaymentUnitOfWork, PaymentUnitOfWork>();
            builder.Services.AddScoped<IRatingUnitOfWork, RatingUnitOfWork>();
            builder.Services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
            builder.Services.AddScoped<IVehicleUnitOfWork, VehicleUnitOfWork>();

           /* builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "TaxiBookingServiceSession";
                options.IdleTimeout = TimeSpan.FromMinutes(5);
            });*/

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.UseSession();

            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseCors(corspolicy);

            app.UseAuthentication();

            app.UseAuthorization();

            //app.MapControllers();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                         .RequireCors(corspolicy);
            });

            app.Run();
        }
    }
}