using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Core.ViewModels
{
    public class AuthorFormViewModel
    {
        public int Id { set; get; }
        [MaxLength(100 , ErrorMessage = "Max Length cannot be more than 100 chr.") ]
        [Remote("AllowItems", null , AdditionalFields = "Id", ErrorMessage = "Author with the same name is already existed!")]
        public string Name { get; set; } = null!;
    }
}
