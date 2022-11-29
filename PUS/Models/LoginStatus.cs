namespace PUS.Models
{
    public enum Status : int
    {
        Unknow = 999,
        Success = 0,
        WrongEmailOrPassword = 1,
        Locked = 2,
    }

    public class LoginStatus
    {
        public Status Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
