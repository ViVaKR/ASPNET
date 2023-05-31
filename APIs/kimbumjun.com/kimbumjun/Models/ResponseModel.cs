using kimbumjun.Enums;

namespace kimbumjun.Models;

public class ResponseModel
{
    
    public ResponseCode ResponseCode    { get; set; }
    public string       ResponseMessage { get; set; }
    public object       DataSet         { get; set; }

    public ResponseModel(ResponseCode code, string message,object data)
    {
        ResponseCode    = code;
        ResponseMessage = message;
        DataSet         = data;
    }
}