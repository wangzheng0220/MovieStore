using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Models.Response
{
    // ViewModels, we are creating these model classes just for the Views/Client, they will have only properties that are required
    //  by the View, No more no less.
    // ViewModels are also useful when you want to combine multiple models, like different properties from different classes
    // they are called ViewModels in MVC
    // DTO (Data transfer Objects) in API's
    public  class UserRegisterResponseModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
