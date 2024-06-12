using Entities;
using System.ComponentModel.DataAnnotations;

namespace MyApi.Models
{
    public class UserDto : IValidatableObject
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(500)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public int Age { get; set; }

        public GenderType Gender { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //var list=new List<ValidationResult>();
            if (UserName.Equals("test", StringComparison.OrdinalIgnoreCase))
                //list.Add(new ValidationResult("نام کاربری نمیتواند Test باشد", new[] { nameof(UserName) }));
                yield return new ValidationResult("نام کاربری نمیتواند Test باشد", new[] { nameof(UserName) });


            //return list;
        }
    }
}
