using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using nyschub.Contracts;
using nyschub.DataAccess;
using nyschub.Entities;
using nyschub.Repositories;
using nyschub.Services;
using nyschub.Services.Entities;
using nyschub.Services.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nyschub
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //for jwt
            
            //jwt ends

            // for identity user

            services.AddIdentity<Corper, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.SignIn.RequireConfirmedAccount = true;
            }
                ).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            
            services.AddScoped<ICorperRepository, CorperRepository>();
            services.AddScoped<IPostRepository, ForumPostRepository>();
            services.AddScoped<ICorperRepository, CorperRepository>();
            services.AddScoped<IMarketPostRepository, MarketPostRepository>();
            services.AddScoped<ICommentRepository<ForumComment>, ForumCommentRepository>();
            services.AddScoped<ICommentRepository<MarketComment>, MarketCommentRepository>();
            services.AddScoped<IVoteRepository<UpVote>, VoteRepository>();
            services.AddScoped<IVoteRepository<DownVote>, DownVoteRepository>();
            services.AddScoped<TokenRepository>();
            services.AddScoped<IAuthManager, AuthManager>();
            // add cors policy
            services.AddCors(o => {
                o.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            });

            // for database linking
            if (_env.IsDevelopment())
            {
                services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("default")));
            }
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("PostGreSql")));

            // for emails service
            services.AddScoped<EmailService>();

            // for image service
            services.AddScoped<IImageService>(x => new ImageService(
                Environment.GetEnvironmentVariable("CloudinaryName"),
                Environment.GetEnvironmentVariable("CloudinaryKey"),
                Environment.GetEnvironmentVariable("CloudinarySecret")
            ));
            services.AddControllers();
            var jwtSettings = Configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JwtKey"));

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    RequireExpirationTime = false
                };
            });

            
            // for email service
            services.Configure<MailjetOptions>(Configuration.GetSection("mailjetOptions"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "nyschub", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT authorization header using the bearer scheme.
                        Enter 'Bearer' [Space] and then your token in the text input below
                        Example: 'Bearer 12343abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "0auth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },

                    new List<string>()
                }});
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext database)
        {
           
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "nyschub v1"));

           
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseRouting();

            app.UseAuthorization();

            database.Database.EnsureCreated();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        } 
    }
}
