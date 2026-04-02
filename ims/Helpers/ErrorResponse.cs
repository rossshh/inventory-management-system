using System.Collections.Generic;

namespace ims.Helpers;

/// <summary>
/// A standardized error response returned by the API when something goes wrong.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// The HTTP status code of the error (e.g., 400, 401, 404, 500).
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// A human-readable description of the error.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Additional detailed error messages (e.g., specific validation failures).
    /// </summary>
    public object? Errors { get; set; }

    /// <summary>
    /// Initializes a new instance of the ErrorResponse class.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="message">A descriptive error message.</param>
    /// <param name="errors">Optional details or validation errors.</param>
    public ErrorResponse(int statusCode, string message, object? errors = null)
    {
        StatusCode = statusCode;
        Message = message;
        Errors = errors;
    }
}
