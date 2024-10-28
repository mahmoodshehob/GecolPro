using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GecolPro.Models.Models
{
    public class Result<TSuccess, TFailure>
    {
        public bool IsSuccess { get; private set; }
        public TSuccess Success { get; private set; }
        public TFailure Failure { get; private set; }

        private Result() { }

        // Method to create a success result
        public static Result<TSuccess, TFailure> SuccessResult(TSuccess success)
        {
            return new Result<TSuccess, TFailure>
            {
                IsSuccess = true,
                Success = success
            };
        }

        // Method to create a failure result
        public static Result<TSuccess, TFailure> FailureResult(TFailure failure)
        {
            return new Result<TSuccess, TFailure>
            {
                IsSuccess = false,
                Failure = failure
            };
        }
    }

}
