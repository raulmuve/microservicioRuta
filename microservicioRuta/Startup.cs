using microservicioRuta.Repository;
using microservicioRuta.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;


namespace microservicioRuta
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
			services.AddScoped<IRepositoryRutes, RepositoryRutes>();
			services.AddSingleton<MongoDBContext>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			//Creamos un hilo nuevo para el receptor de los mensajes (RabbitMQ)
			RabbitMQ.Rabbit rabbit1 = new RabbitMQ.Rabbit();
			Thread rabbit = new Thread(rabbit1.consumidor);
			rabbit.Start();
		}
	}
}
