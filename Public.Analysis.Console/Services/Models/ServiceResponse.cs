namespace Public.Analysis.Console.Services.Models
{
    public class ServiceResponse<T> where T:class
    {
        public T? ResponseData { get; }

        private readonly string[] validationErrors;

        public IEnumerable<string> ValidationErrors => this.validationErrors.Select(e => e).ToList();

        public ServiceResponse(T? responseData, string[] validationErrors)
        {
            this.ResponseData = responseData;
            this.validationErrors = validationErrors;
        }
        public ServiceResponse(T? responseData)
        {
            this.ResponseData = responseData;
            this.validationErrors = [];
        }

        public static ServiceResponse<T> Failure(string reason)
        {
            return new ServiceResponse<T>(null, [ reason ]);
        }
    }
}
