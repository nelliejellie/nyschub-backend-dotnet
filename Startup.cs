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
using System.Threading.Tasks;

namespace nyschub
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllers();
            services.AddScoped<ICorperRepository, CorperRepository>();
            services.AddScoped<IPostRepository, ForumPostRepository>();

            // for database linking
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("default")));

            // for emails service
            services.AddScoped<EmailService>();

            // for image service
            services.AddScoped<IImageService>(x => new ImageService(
                Configuration.GetSection("Cloudinary")["Name"],
                Configuration.GetSection("Cloudinary")["Key"],
                Configuration.GetSection("Cloudinary")["Secret"]
            ));

            // for identity user
            services.AddIdentity<Corper, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
            }
                ).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            // for email service
            services.Configure<MailjetOptions>(Configuration.GetSection("mailjetOptions"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "nyschub", Version = "v1" });
            });

            services.AddDefaultIdentity<Corper>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "nyschub v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
