using System.ComponentModel.DataAnnotations;

namespace Lean.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
