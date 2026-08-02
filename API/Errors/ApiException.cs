using System;

namespace API.Errors;

public class ApiException(int statuscode, string Message, string? Details)
{
    public int statuscode {get; set;} = statuscode;

    public string Message {get; set;} = Message;

    public string? Details {get; set;} = Details;
}