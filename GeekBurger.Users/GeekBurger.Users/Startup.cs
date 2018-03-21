using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GeekBurger.Users.Services;
using GeekBurger.Users.Model;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using GeekBurger.Users.Repository;
using System.IO;

namespace GeekBurger.Users
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcCoreBuilder = services.AddMvc();
            

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Users", Version = "v1" });
            });

            services.AddSingleton(Configuration);

            services.AddSingleton<IPictureValidator, PictureValidator>();

            services.AddSingleton<IFaceDetection, FaceDetectionService>();
            services.AddSingleton<IServiceBus, ServiceBusService>();
            services.AddSingleton<IDetector, Detector>();
            

            services.AddDbContext<RestrictionsContext>(o => o.UseInMemoryDatabase("geekburger-users-restrictions"));
            services.AddScoped<IRestrictionsRepository, RestrictionsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Run(async (context) =>
            //{
            //    var ms = new MemoryStream();
            //    await context.Request.Body.CopyToAsync(ms);
            //    context.Request.Body = ms;
            //});

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeekBurger.Users API V1");
            });
        }
    }
}
