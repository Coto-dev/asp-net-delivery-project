using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Common.Exceptions {
	public class ErrorHandlingMiddleware {
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;
		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger) {
			_next = next;
			_logger= logger;
		}

		public async Task Invoke(HttpContext context) {
			try {
				await _next(context);
			}
			catch (Exception ex) {
				await HandleExceptionAsync(context, ex);
				_logger.LogError(ex,
				  $"Message: {ex.Message}");
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception) {
			HttpStatusCode status;
			string message;
			var stackTrace = String.Empty;

			var exceptionType = exception.GetType();
			if (exceptionType == typeof(BadRequestException)) {
				message = exception.Message;
				status = HttpStatusCode.BadRequest;
			}
			else if (exceptionType == typeof(NotFoundException)) {
				message = exception.Message;
				status = HttpStatusCode.NotFound;
			}
			else if (exceptionType == typeof(InvalidOperationException)) {
				message = exception.Message;
				status = HttpStatusCode.BadRequest;
			}
			else if (exceptionType == typeof(NotAllowedException)) {
				message = exception.Message;
				status = HttpStatusCode.Forbidden;
			}
			else if (exceptionType == typeof(ConflictException)) {
				message = exception.Message;
				status = HttpStatusCode.Conflict;
			}
			else if (exceptionType == typeof(ArgumentException)) {
				message = exception.Message;
				status = HttpStatusCode.BadRequest;
			}
			else if (exceptionType == typeof(ArgumentNullException)) {
				message = exception.Message;
				status = HttpStatusCode.BadRequest;
			}
			else if (exceptionType == typeof(KeyNotFoundException)) {
				message = exception.Message;
				status = HttpStatusCode.BadRequest;
			}
			else {
				status = HttpStatusCode.InternalServerError;
				message = exception.Message;
				//if (env.IsEnvironment("Development"))
					stackTrace = exception.StackTrace;
			}

			var result = JsonSerializer.Serialize(new { error = message, stackTrace });
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)status;
			return context.Response.WriteAsync(result);
		}
	}
}
