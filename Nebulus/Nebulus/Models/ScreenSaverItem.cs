using Nebulus.Models.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Nebulus.Models
{
    public class ScreenSaverItem
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Modified { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Please select PowerPoint slides")]
        [Display(Name = "PowerPoint Slides")]
        [NotMapped]
        [ValidateFile]
        public HttpPostedFileBase Slides { get; set; }

        public string CreatorId { get; set; }

        public bool Active { get; set; }


        public string SlidesPath { get; set; }

        public PresenterType Presenter { get; set; }
    }
}