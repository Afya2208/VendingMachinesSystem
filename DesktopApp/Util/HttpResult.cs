using System;
using System.Diagnostics.CodeAnalysis;

namespace DesktopApp.Util;

public record HttpResult<TResult>(TResult? Result, Exception? Exception)
{
    public bool TryGetResult([NotNullWhen(true)] out TResult? result)
    {
        result = Result;
        return Result is not null;
    }

    public bool TryGetException([NotNullWhen(true)] out Exception? exception)
    {
        exception = Exception;
        return Exception is not null;
    }
}