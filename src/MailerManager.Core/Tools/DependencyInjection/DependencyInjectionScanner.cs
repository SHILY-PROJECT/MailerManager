using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MailerManager.Core.Tools.DependencyInjection;

public static class DependencyInjectionScanner
{
    private static readonly Dictionary<Type, ServiceLifetime> InterfaceToLifetimeMap = new()
    {
        [typeof(ITransientDependency)] = ServiceLifetime.Transient,
        [typeof(IScopedDependency)] = ServiceLifetime.Scoped,
        [typeof(ISingletonDependency)] = ServiceLifetime.Singleton,
    };
    
    public static IServiceCollection ScanDependencies(this IServiceCollection services, Assembly[] assemblies)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableToAny(InterfaceToLifetimeMap.Keys))
            .AsImplementedInterfaces()
            .WithLifetime(GetLifetime)
            .AddClasses(classes => classes.AssignableToAny(InterfaceToLifetimeMap.Keys))
            .AsSelf()
            .WithLifetime(GetLifetime));
            
        return services;
    }
    
    private static IEnumerable<Type> GetAllBaseInterfaces(Type? type)
    {
        if (type == null || type == typeof(object)) yield break;

        foreach (var interfaceType in type.GetInterfaces())
        {
            yield return interfaceType;

            foreach (var baseInterface in GetAllBaseInterfaces(interfaceType))
            {
                yield return baseInterface;
            }
        }
    }

    private static ServiceLifetime GetLifetime(Type type)
    {
        var implementedInterfaces = type.GetInterfaces();

        foreach (var interfaceType in implementedInterfaces)
        {
            if (InterfaceToLifetimeMap.TryGetValue(interfaceType, out var lifetime)) return lifetime;

            var baseInterfaces = GetAllBaseInterfaces(interfaceType);

            if (baseInterfaces.Any(baseInterface => InterfaceToLifetimeMap.TryGetValue(baseInterface, out lifetime))) return lifetime;
        }

        throw new InvalidOperationException($"Type '{type.Name}' does not match any known interface");
    }
}