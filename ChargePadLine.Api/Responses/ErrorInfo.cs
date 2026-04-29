namespace X509Data.ChargePadLine.Api.Responses
{
  public class ErrorField
  {
    public string Name { get; set; } = "";
    public string[] Errors { get; set; } = new string[0];
  }

  public class ErrorInfo
  {
    public string Code { get; set; } = "";
    public string Message { get; set; } = "";

    public IList<ErrorField>? ErrorFields { get; set; }

    public static ErrorInfo MakeNew(int Code, string Message)
    {
      return new ErrorInfo { Code = Code.ToString(), Message = Message };
    }
  }
}
