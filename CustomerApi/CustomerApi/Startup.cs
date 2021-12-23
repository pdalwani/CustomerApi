using CustomerApi.Data;
using CustomerApi.Models;
using CustomerApi.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;


namespace CustomerApi
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
            services.AddDbContext<EntityContext>(opt => opt.UseInMemoryDatabase(databaseName: "CustomersData"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler();

            app.UseSwagger();
           
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Customer API V1");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            var context = serviceProvider.GetService<EntityContext>();
            AddTestData(context);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddTestData(EntityContext context)
        {
            var testCustomer1 = new Customer
            {
                CustomerId = 123,
                FirstName = "Luke",
                LastName = "Skywalker",
                DateOfBirth = new DateTime(2015, 12, 31)
            };

            context.Customers.Add(testCustomer1);

            var testCustomer2 = new Customer
            {
                CustomerId = 456,
                FirstName = "Mark",
                LastName = "Robinson",
                DateOfBirth = new DateTime(2011, 11, 29)
            };

            context.Customers.Add(testCustomer2);
            context.SaveChanges();
        }

        }
    }
