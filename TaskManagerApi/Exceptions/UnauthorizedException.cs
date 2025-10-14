namespace TaskManagerApi.Exceptions
{
    public class UnauthorizedException : HttpResponseException
    {
        public UnauthorizedException(string message)
            : base(StatusCodes.Status401Unauthorized, message) { }
    }
}
