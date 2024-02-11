using System.ComponentModel.DataAnnotations;

namespace dz5.Models
{
    public class Contact
    {
        [Required(ErrorMessage = "Поле ім'я є обов'язковим")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле мобільний телефон є обов'язковим")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Недійсний номер телефону")]
        public string MobilePhone { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Недійсний номер телефону")]
        public string AlternativeMobilePhone { get; set; }

        [Required(ErrorMessage = "Поле електронна пошта є обов'язковим")]
        [EmailAddress(ErrorMessage = "Недійсна адреса електронної пошти")]
        public string Email { get; set; }

        public string Description { get; set; }
    }
}
