using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _1291163_MVC_Project.Models.ViewModels
{
    public class GuestVM
    {
        public int guestsId { get; set; }
        [Required, StringLength(50), Display(Name = "Guest Name")]
        public string guestsName { get; set; }
        [Required, Display(Name = "Booking Date"), Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime bookingDate { get; set; }
        [Display(Name = "Number Of Guest")]
        public int numberOfGueist { get; set; }
        public string picture { get; set; }
        [Display(Name = "Picture")]
        public HttpPostedFileBase pictureFile { get; set; }
        [Display(Name = "Couple Status")]
        public bool coupleStatus { get; set; }
        public List<int> RoomList { get; set; } = new List<int>();
        public List<string> RoomNames { get; set; } = new List<string>();

    }
}