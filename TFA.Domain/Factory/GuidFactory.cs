namespace TFA.Domain.Factory;

public class GuidFactory : IGuidFactory
{
    public Guid Create()=> Guid.NewGuid();
}