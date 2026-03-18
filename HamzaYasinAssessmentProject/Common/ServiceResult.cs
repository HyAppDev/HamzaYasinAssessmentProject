namespace HamzaYasinAssessmentProject.Server.Common
{
    public class ServiceResult<T>
    {

        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public ServiceResultStatus Status { get; set; }

        public static ServiceResult<T> Ok(T data) => new()
        {
            Success = true,
            Data = data,
            Status = ServiceResultStatus.Ok
        };

        public static ServiceResult<T> NotFound(string message) => new()
        {
            Success = false,
            ErrorMessage = message,
            Status = ServiceResultStatus.NotFound
        };

        public static ServiceResult<T> Forbidden(string message) => new()
        {
            Success = false,
            ErrorMessage = message,
            Status = ServiceResultStatus.Forbidden
        };
    }

    public enum ServiceResultStatus
    {
        Ok,
        NotFound,
        Forbidden
    }
}
