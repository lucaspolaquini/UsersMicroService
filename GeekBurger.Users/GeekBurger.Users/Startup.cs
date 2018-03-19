using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GeekBurger.Users.Services;
using GeekBurger.Users.Model;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using GeekBurger.Users.Repository;

namespace GeekBurger.Users
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
            var mvcCoreBuilder = services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Users", Version = "v1" });
            });

            services.AddSingleton<IFaceDetection>((_) => new FaceDetectionService());
            services.AddSingleton<IServiceBus>((_) => new ServiceBusService());
            services.AddSingleton<IDetector>((sp) => new Detector(sp.GetService<IFaceDetection>(), sp.GetService<IServiceBus>()));

            services.AddDbContext<RestrictionsContext>(o => o.UseInMemoryDatabase("geekburger-users-restrictions"));
            services.AddScoped<IRestrictionsRepository, RestrictionsRepository>();
            services.AddSingleton<IRestrictionChangedService, RestrictionChangedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeekBurger.Users API V1");
            });
        }
    }
}
