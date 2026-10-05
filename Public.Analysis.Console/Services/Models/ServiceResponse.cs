using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.Services.Models
{
    public class ServiceResponse<T> where T:class
    {
        public T ResponseData { get; }

        private readonly string[] validationErrors;

        public IEnumerable<string> ValidationErrors => this.validationErrors.Select(e => e).ToList();

        public ServiceResponse(T responseData, string[] validationErrors)
        {
            this.ResponseData = responseData;
            this.validationErrors = validationErrors;
        }
    }
}
