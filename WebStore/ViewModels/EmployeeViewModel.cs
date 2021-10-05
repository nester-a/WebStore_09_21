using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel : IValidatableObject
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя не указано")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Длина от 2 до 10 символов")]
        [RegularExpression(@"([А-ЯЁ][a-яё]+)|([A-Z][a-z]+)",ErrorMessage = "Имя должно начинаться с заглавной буквы")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Фамилия не указана")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Длина от 2 до 10 символов")]
        [RegularExpression(@"([А-ЯЁ][a-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Фамилия должна начинаться с заглавной буквы")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string SecondName { get; set; }

        [Display(Name = "Возраст")]
        [Range(18, 80, ErrorMessage = "Возраст должен быть от 18 до 80 лет")]
        public int Age { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (validationContext.MemberName)
            {
                //default: return Enumerable.Empty<ValidationResult>();
                default: return new[] { ValidationResult.Success };

                case nameof(Age):
                    if(Age < 15 || Age > 90) 
                        return new[] { new ValidationResult("Странный возраст", new[] { nameof(Age) } )};

                    return new[] { ValidationResult.Success };
            }
        }
    }
}
