namespace Fintrak.CustomerPortal.Blazor.Shared.Models
{
    public class ResponseModel<TResponse>
    {
        public TResponse Response { get; set; }
        public string Message { get; set; }
        public IList<string> Errors { get; set; } = new List<string>();
        public bool Successful { get; set; }
        public List<string>? ValidationErrors { get; set; } = new List<string>();
    }

    public class  ResultModel<TResponse>
    {
        public ResponseModel<TResponse> Result { get; set; }

        public bool Success { get; set; }
    }
}
