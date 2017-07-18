using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nebulus.Models.CustomValidation
{
    public class ValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int maxContent = 5242880; //5mg

            string[] sAllowedExt = new string[] { ".pptx", ".ppt" };

            var file = value as HttpPostedFileBase;
            if (file == null)

                return false;

            else if (!sAllowedExt.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {

                ErrorMessage = "Please upload PowerPoint of type: " + string.Join(", ", sAllowedExt);
                return false;
            }

            else if (file.ContentLength > maxContent)
            {
                ErrorMessage = "Your PowerPoint is too large, maximum allowed size is : " + (maxContent / 1024).ToString() + "MB";
                return false;
            }
            else
                return true;
        }
    }
}