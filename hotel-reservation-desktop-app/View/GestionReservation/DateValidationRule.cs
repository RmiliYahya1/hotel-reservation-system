using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace hotel_reservation_desktop_app.View.GestionReservation
{
    public class DateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "La date est obligatoire.");

            if (value is DateTime date && date < DateTime.Now)
                return new ValidationResult(false, "La date ne peut pas être dans le passé.");

            return ValidationResult.ValidResult;
        }
    }
}
