namespace TaskManagerApi.Exceptions
{
    public class ConflictException : HttpResponseException
    {
        public ConflictException(string message)
            : base(StatusCodes.Status409Conflict, message) { }
    }
}
