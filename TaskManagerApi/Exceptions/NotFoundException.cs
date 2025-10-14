namespace TaskManagerApi.Exceptions
{
    public class NotFoundException : HttpResponseException
    {
        public NotFoundException(string message)
            : base(StatusCodes.Status404NotFound, message) { }
    }
}
