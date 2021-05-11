using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hands_on_Programming_5
{
    [MetadataType(typeof(VolunteerMD))]
    public partial class VolunteerList
    {

        public string Fullname
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }
               
    }


    public class VolunteerMD
    {

        public int VolunteerId { get; set; }
        [Required(ErrorMessage = "Staff Name is required.")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Staff Name is required.")]
        public string Lastname { get; set; }
        public Nullable<int> Age { get; set; }
        [Required(ErrorMessage = "Birthdate is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> Birthdate { get; set; }
        public Nullable<int> sex_id { get; set; }
        public string region_code { get; set; }
       
    }
}