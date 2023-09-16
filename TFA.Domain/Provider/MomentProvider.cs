namespace TFA.Domain.Provider;

public class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}