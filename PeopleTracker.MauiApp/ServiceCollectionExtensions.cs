using System.Reflection;

namespace PeopleTracker.MauiApp;

/// <summary>Расширения для автоматической регистрации ViewModel'ов</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>Зарегистрировать все ViewModel'ы как Transient</summary>
    public static IServiceCollection AddViewModels(this IServiceCollection services, Assembly assembly)
    {
        var viewModelTypes = assembly.GetTypes()
            .Where(t => t.IsClass &&
                        !t.IsAbstract &&
                        t.Name.EndsWith("Vm") &&
                        t.BaseType != null &&
                        t.BaseType.Name == "ViewModel"); // базовый ViewModel

        foreach (var type in viewModelTypes)
        {
            services.AddTransient(type);
        }

        return services;
    }
}