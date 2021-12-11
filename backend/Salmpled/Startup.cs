using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Salmpled.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.HttpOverrides;
using Amazon.S3;
using Amazon.Runtime;

using Salmpled.Services;

namespace Salmpled
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

            //services.AddDbContext<SalmpledContext>(opt => opt.UseInMemoryDatabase("SalmpledList"));

            services.AddHttpContextAccessor();
            
            services.AddDbContext<SalmpledContext>();

            services.AddScoped<ISampleService, SampleService>();

            services.AddScoped<IMyS3Service, MyS3Service>();

            services.AddScoped<ISamplePackService, SamplePackService> ();

            services.AddScoped<ISamplePackGenreService, SamplePackGenreService>();

            services.AddScoped<ISamplePlaylistService, SamplePlaylistService>();

            services.AddScoped<IUserService, UserService>();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers().AddNewtonsoftJson(x => 
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddSwaggerGen();

            services.AddCors(options =>
                options.AddPolicy("AllowEverything", p =>
                p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().SetIsOriginAllowed(_ => true)));


            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("./firebase-sdk.json"),
            });


            var awsOptions = Configuration.GetAWSOptions();
            awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://securetoken.google.com/sampled-5fe52";
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = options.Authority,
                    ValidateAudience = true,
                    ValidAudience = "sampled-5fe52",
                    ValidateLifetime = true
                };

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                }
                );
            }

            app.UseHttpsRedirection();
            //FARGATE NGINX SET UP
            // app.UseForwardedHeaders(new ForwardedHeadersOptions
            // {
            //     ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            // });

            app.UseRouting();

            app.UseCors("AllowEverything");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
