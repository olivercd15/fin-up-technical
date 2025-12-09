using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public T? Value { get; }
        public List<string> Errors { get; }

        private Result(bool success, T? value, List<string>? errors)
        {
            IsSuccess = success;
            Value = value;
            Errors = errors ?? new List<string>();
        }

        public static Result<T> Ok(T value) =>
            new(true, value, null);

        public static Result<T> Fail(string error) =>
            new(false, default, new List<string> { error });

        public static Result<T> Fail(List<string> errors) =>
            new(false, default, errors);
    }
}
