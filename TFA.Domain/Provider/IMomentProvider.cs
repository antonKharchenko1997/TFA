namespace TFA.Domain.Provider;

public interface IMomentProvider
{
    DateTimeOffset Now { get; }
}