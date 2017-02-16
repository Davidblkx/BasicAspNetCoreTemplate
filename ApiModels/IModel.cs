using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OwnAspNetCore.ApiModels
{
    public interface IModel
    {
         void Validate(ModelStateDictionary model);
    }
}