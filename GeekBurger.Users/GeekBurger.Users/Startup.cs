using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GeekBurger.Users.Services;
using GeekBurger.Users.Model;

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
            var builder = services.AddMvcCore()
                    .AddFormatterMappings()
                    .AddJsonFormatters()
                    .AddCors();
            services.AddSingleton<IFaceDetection>((_) => new FaceDetectionService());
            services.AddSingleton<IServiceBus>((_) => new ServiceBusService());
            services.AddSingleton<IDetector>((sp) => new Detector(sp.GetService<IFaceDetection>(), sp.GetService<IServiceBus>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
