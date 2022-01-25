using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

public class EmptyBodyModelBinder<T> : IModelBinder where T : class, new()
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var stream = bindingContext.HttpContext.Request.Body;
        using var reader = new StreamReader(stream);
        var jsonbody = await reader.ReadToEndAsync();
        var obj = !string.IsNullOrEmpty(jsonbody)? 
            JsonSerializer.Deserialize<T>(jsonbody):
            new T();
        bindingContext.Result = ModelBindingResult.Success(obj);
    }
}