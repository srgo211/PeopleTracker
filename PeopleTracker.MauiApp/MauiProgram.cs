

using PeopleTracker.Application.Interfaces;
using PeopleTracker.Application.UseCases;
using PeopleTracker.Infrastructure.InMemory;
using PeopleTracker.MauiApp.ViewModels;
using PeopleTracker.MauiApp.Views;
using Telerik.Maui.Controls.Compatibility;

namespace PeopleTracker.MauiApp;

public static class MauiProgram
{
    public static Microsoft.Maui.Hosting.MauiApp CreateMauiApp()
    {
        var builder = Microsoft.Maui.Hosting.MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseTelerik()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // ✅ Репозитории (InMemory)
        builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        builder.Services.AddSingleton<IJournalRepository, InMemoryJournalRepository>();

        // ✅ UseCases
        builder.Services.AddScoped<GenerateUserJournalSummaryUseCase>();
        builder.Services.AddScoped<GenerateCalendarGridUseCase>();

        // ✅ ViewModels
        //builder.Services.AddTransient<CalendarVm>();
        builder.Services.AddViewModels(typeof(CalendarVm).Assembly);

        // ✅ Pages
        builder.Services.AddTransient<CalendarPageView>();




        return builder.Build();
    }
}
