using Words.Services;

namespace Words.Web.Settings
{
	internal static class DependencyInjectionSettings
	{
		public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
		{
			services.AddTransient<IWordsParserService, WordsParserService>();
			services.AddTransient<IWordsService, WordsService>();

			return services;
		}
	}
}
