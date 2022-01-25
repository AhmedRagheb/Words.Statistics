using System.Net;

namespace Words.Web.Middlewares
{
	internal class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			var httpMethod = context.Request?.Method.ToUpperInvariant();
			var requestPath = context.Request?.Path;

			try
			{
				await _next(context);
			}

			catch (OperationCanceledException)
			{
				context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex,
					"Request: {HttpMethod} {RequestPath}; status: {HttpStatusCode};",
					httpMethod, requestPath, 500);

				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			}
		}
	}
}