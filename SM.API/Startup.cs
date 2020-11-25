using FluentValidation.AspNetCore;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Shared.Contract.Configs;
using Sieve.Models;
using Sieve.Services;
using SM.API.Extensions;
using SM.API.Services;
using SM.Application;
using SM.Application.Common.Interfaces;
using SM.Application.Saga.StudentRegistration;
using SM.Infrastructure;
using SM.Infrastructure.StatePersistence;
using System.Reflection;

namespace SM.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            services.AddSwaggerExtension();
            services.AddControllers();
            services.AddApiVersioningExtension();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddControllersWithViews()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IApplicationDbContext>())
                .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddSwaggerGenNewtonsoftSupport();

            #region Sieve

            services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();
            services.AddScoped<ISieveCustomFilterMethods, ApplicationCustomFilterMethods>();
            services.Configure<SieveOptions>(Configuration.GetSection("Sieve"));

            #endregion

            var massTransitSettingSection = Configuration.GetSection("MassTransitConfig");
            var massTransitConfig = massTransitSettingSection.Get<MassTransitConfig>();

            services.AddDbContext<StudentStateDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(StudentStateDbContext).Assembly.FullName);
                        b.MigrationsHistoryTable($"__{nameof(StudentStateDbContext)}");
                    }));

            services.AddMassTransit(x =>
            {
                x.AddApplication();
                x.SetKebabCaseEndpointNameFormatter();
                x.AddSagaStateMachine<StudentRegistrationStateMachine, StudentRegistrationState>()
                .EntityFrameworkRepository(r =>
                {
                    r.AddStudentSaga(Configuration);
                });
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                    cfg.Host(massTransitConfig.Host, massTransitConfig.VirtualHost,
                        h =>
                        {
                            h.Username(massTransitConfig.Username);
                            h.Password(massTransitConfig.Password);
                        }
                    );
                });
            });
            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerExtension();
            app.UseErrorHandlingMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
