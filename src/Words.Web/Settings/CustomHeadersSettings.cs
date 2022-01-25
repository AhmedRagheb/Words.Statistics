namespace Words.Web.Settings
{
	internal static class CustomHeadersSettings
	{
		public static IApplicationBuilder UseCustomHeadersSettings(this IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
				context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
				context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
				context.Response.Headers.Add("Referrer-Policy", "same-origin");

				await next();
			});

			return app;
		}
	}
}