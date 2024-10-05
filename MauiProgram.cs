using Microsoft.Extensions.Logging;
using Task.Management.Service;
using Task.Management.ViewModels;
using Task.Management.Views;

namespace Task.Management
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UsePrism(prism =>
                          prism.RegisterTypes(registry =>
                          {
                              registry.RegisterForNavigation<TasksPage, TasksPageViewModel>();
                              registry.RegisterForNavigation<TaskPage, TaskPageViewModel>();
                              registry.Register<ILocalDbService, LocalDbService>();
                          })
                          .CreateWindow("/TasksPage"))
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
