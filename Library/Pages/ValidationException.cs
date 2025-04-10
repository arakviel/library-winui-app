using System;
using System.Collections.Generic;

namespace Library.Pages;

internal class ValidationException : Exception
{
    public Dictionary<string, List<string>> Errors { get; } = new Dictionary<string, List<string>>();

    public ValidationException(Dictionary<string, List<string>> errors)
    {
        Errors = errors;
    }
}