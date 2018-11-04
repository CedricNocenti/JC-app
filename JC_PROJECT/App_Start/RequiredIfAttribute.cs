﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JC_PROJECT.App_Start
{
    public class RequiredIfAttribute: ValidationAttribute
    {
       
            private String PropertyName { get; set; }
            private String ErrorMessage { get; set; }
            private Object DesiredValue { get; set; }

            public RequiredIfAttribute(String propertyName, Object desiredvalue, String errormessage)
            {
                this.PropertyName = propertyName;
                this.DesiredValue = desiredvalue;
                this.ErrorMessage = errormessage;
            }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value != null) { return ValidationResult.Success; }

            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (proprtyvalue == null)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
        

    }
}