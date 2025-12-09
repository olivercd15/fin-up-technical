using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Application.Common;

public class AppValidationException : Exception
{
    public List<string> Errors { get; }

    public AppValidationException(List<string> errors)
        : base("Validation error")
    {
        Errors = errors;
    }
}
