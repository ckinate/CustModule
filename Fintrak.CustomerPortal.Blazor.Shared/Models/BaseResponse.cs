namespace Fintrak.CustomerPortal.Blazor.Shared.Models
{
	public class BaseResponse<TResult>
	{
		public BaseResponse()
		{
			Success = true;
		}
		public BaseResponse(string message)
		{
			Success = true;
			Message = message;
		}

		public BaseResponse(string message, bool success)
		{
			Success = success;
			Message = message;
		}

		public bool Success { get; set; }
		public string Message { get; set; } = string.Empty;
		public List<string>? ValidationErrors { get; set; } = new List<string>();

		public TResult Result { get; set; } = default!;

		public void EnsureSuccessStatusCode()
		{
			throw new NotImplementedException();
		}
	}
}
