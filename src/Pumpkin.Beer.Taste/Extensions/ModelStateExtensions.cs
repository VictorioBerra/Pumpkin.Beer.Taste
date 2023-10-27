namespace Pumpkin.Beer.Taste.Extensions;

using Microsoft.AspNetCore.Mvc.ModelBinding;

public static class ModelStateExtensions
{
    public static void AddPageError(this ModelStateDictionary modelState, string message)
        => modelState.AddModelError(string.Empty, message);
}
