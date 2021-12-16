using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnlineShop.Context;
using OnlineShop.Repository;
using OnlineShop.Services;

namespace OnlineShop
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

            services.AddEntityFrameworkSqlServer();
            //services.AddDbContextPool<OnlineShopDbContext>((serviceProvider, optionsBuilder) =>
            //{
            //    optionsBuilder.UseSqlServer("DevConnection");
            //    optionsBuilder.UseInternalServiceProvider(serviceProvider);
            //});
            services.AddDbContext<OnlineShopDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DevConnection"));
                options.UseInternalServiceProvider(serviceProvider);
            });

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            //services.AddDbContextPool<OnlineShopDbContext>(x => x.UseSqlServer("DevConnection", b => b.MigrationsAssembly("OnlineShop")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "My API", Version = "v2" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, OnlineShopDbContext onlineShopDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            onlineShopDbContext.Seed();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
