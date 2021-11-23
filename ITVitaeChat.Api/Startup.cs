using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Interfaces;
using ITVitaeChat.ChatCore.Services;
using ITVitaeChat.ChatInfrastructure;
using Microsoft.AspNetCore.Authentication;
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
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

namespace ITVitaeChat.Api
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
            //services.AddIdentity<User, IdentityRole>();

            services.AddScoped<IAdministratorService, AdministratorService>();
            services.AddScoped<IChatDisallowedWordsService, ChatDisallowedWordsService>();
            services.AddScoped<IChatGroupService, ChatGroupService>();
            services.AddScoped<IChatGroupUserService, ChatGroupUserService>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IChatMessageService, ChatMessageService>();
            services.AddScoped<IHashAndSaltService, HashAndSaltService>();
            services.AddScoped<IRepository<Administrator>, Repository<Administrator>>();
            services.AddScoped<IRepository<ChatDisallowedWord>, Repository<ChatDisallowedWord>>();
            services.AddScoped<IRepository<ChatGroup>, Repository<ChatGroup>>();
            services.AddScoped<IRepository<ChatGroupUser>, Repository<ChatGroupUser>>();
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IUserService, UserService>();
            services.AddDbContext<ITVitaeChatDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ITVitaeChat.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ITVitaeChat.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
