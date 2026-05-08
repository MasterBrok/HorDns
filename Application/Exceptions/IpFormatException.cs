using Application.Models;
using Application.Services;

namespace Application.Exceptions;

public class IpFormatException : Exception
{
    public IpFormatException() : base(MessageText.IpFormatError)
    {

    }
    public static void ThrowIfNotValid(Ip value)
    {
        if (string.IsNullOrWhiteSpace(value.Value) || !DnsService.IsValidIp(value.Value))
            throw new IpFormatException();
    }
}
