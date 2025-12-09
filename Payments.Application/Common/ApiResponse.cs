using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Payments.Application.Common
{
    public class ApiResponse
    {
        [JsonProperty(Order = 1)]
        public bool Success { get; set; }

        [JsonProperty(Order = 2)]
        public int Code { get; set; }

        [JsonProperty(Order = 3)]
        public string? Message { get; set; }

        public static ApiResponse Ok(string message = "Success")
            => new ApiResponse { Success = true, Code = 200, Message = message };

        public static ApiResponse Created(string message = "Created")
            => new ApiResponse { Success = true, Code = 201, Message = message };

        public static ApiResponse Fail(int code, string message)
            => new ApiResponse { Success = false, Code = code, Message = message };
    }

    public class ApiResponse<T> : ApiResponse
    {
        [JsonProperty(Order = 9)]
        public T? Data { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "Success")
            => new ApiResponse<T> { Success = true, Code = 200, Message = message, Data = data };

        public static ApiResponse<T> Created(T data, string message = "Created")
            => new ApiResponse<T> { Success = true, Code = 201, Message = message, Data = data };

        public static ApiResponse<T> Fail(int code, string message)
            => new ApiResponse<T> { Success = false, Code = code, Message = message };
    }
}
