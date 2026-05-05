using Application.Models;

namespace Application.Exceptions;

public class AccessDeniedException : Exception
{
    public AccessDeniedException() : base(MessageText.AccessDenied)
    {

    }
}
