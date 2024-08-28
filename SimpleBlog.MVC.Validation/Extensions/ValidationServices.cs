using Microsoft.Extensions.DependencyInjection;
using SimpleBlog.MVC.Validation.ValidationV2.ViewModel;
using SimpleBlog.MVC.Validation.ViewModels.Account;

namespace SimpleBlog.MVC.Validation.Extensions;

public static class ValidationServices
{
    public static void AddValidationServices(this IServiceCollection collection)
    {
        collection.AddScoped<CreatingAccountModelValidator>();
        collection.AddScoped<SignUpModelValidator>();
        collection.AddScoped<UpdatingAccountModelValidator>();
    }
}