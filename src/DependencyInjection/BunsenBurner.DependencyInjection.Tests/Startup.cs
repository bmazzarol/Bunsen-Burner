using Microsoft.Extensions.DependencyInjection;

namespace BunsenBurner.DependencyInjection.Tests;

public interface IA { }

public interface IB { }

public sealed class A : IA
{
    private readonly IB _b;

    public A(IB b) => _b = b;
}

public sealed class B : IB { }

public class Startup
{
    private readonly Action<IServiceCollection> _configure;

    public Startup(Action<IServiceCollection> configure) => _configure = configure;

    public void Configure(IServiceCollection serviceCollection)
    {
        _configure(serviceCollection);
    }
}
