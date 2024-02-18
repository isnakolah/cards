using System.Reflection;
using AutoMapper;

namespace Cards.Application.Common.Mapping;

internal sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IViewModel<>)));

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var iMapMethodInfo = type.GetMethod("Mapping");

            iMapMethodInfo?.Invoke(instance, [this]);
        }
    }
}
