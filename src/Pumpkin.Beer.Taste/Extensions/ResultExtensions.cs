namespace Pumpkin.Beer.Taste.Extensions;

using Microsoft.AspNetCore.Mvc.ModelBinding;

public static class ResultExtensions
{
    public static void AddPageError(this ModelStateDictionary modelState, Ardalis.Result.IResult result)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(string.Empty, error);
        }
    }
}
