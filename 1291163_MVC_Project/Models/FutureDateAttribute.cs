using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace _1291163_MVC_Project.Models
{

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            DateTime bookingDate = Convert.ToDateTime(value);

            // Today অথবা future allowed, past not allowed
            return bookingDate.Date >= DateTime.Today;
        }
    }

}