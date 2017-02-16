using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OwnAspNetCore.ApiModels
{
    public class ModelUserLogin : IModel
    {
        public string User { get; set; }
        public string Pass { get; set; }

        public void Validate(ModelStateDictionary model)
        {
            if(string.IsNullOrEmpty(User))
                model.AddModelError("User", "User field can't be empty");

            if(string.IsNullOrEmpty(Pass))
                model.AddModelError("Pass", "Password field can't be empty");
        }
    }
}