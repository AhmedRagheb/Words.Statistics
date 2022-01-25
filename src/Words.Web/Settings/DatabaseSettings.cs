using Microsoft.EntityFrameworkCore;
using Words.DataAccess;

namespace Words.Web.Settings
{
	internal static class DatabaseSettings
	{
		public static IServiceCollection AddDatabase(this IServiceCollection services)
		{
			services.AddDbContext<WordsDbContext>(options =>
			{
				options.UseInMemoryDatabase("words-db");
			});

			return services;
		}

		public static IApplicationBuilder UseDatabase(this IApplicationBuilder app)
		{
			using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
			var context = serviceScope?.ServiceProvider.GetRequiredService<WordsDbContext>();
			
			context?.Database.EnsureCreated();

			return app;
		}
    }
}