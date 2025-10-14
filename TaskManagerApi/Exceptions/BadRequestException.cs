namespace TaskManagerApi.Exceptions
{
    public class BadRequestException : HttpResponseException
    {
        public BadRequestException(string message)
            : base(StatusCodes.Status400BadRequest, message) { }
    }
}
