using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace EIMarketplace.Models
{

        public enum ListingType
        {
            Freelancer,
            Startup, 
            All
        }

        public enum ListingStatus
        {
            Created,
            InProgress,
            Completed
        }

        public enum ListingAcceptType
        {
            Auto,
            Manual
        }
   
        public class Listing
        {
            [Key]
            public int ID { get; set; }

            public int CreatorID { get; set; }

            public string CreatorName { get; set; }

            public string TrunTitle { get; set; }
            public string TrunDescription { get; set; }

            [Display(Name = "Contact")]
            public string CreatorContact { get; set; }

            /*
            [Display(Name = "Expiration Date")]
            public DateTime? ExpirationDate { get; set; }

            [Display(Name = "Last Activity")]
            [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime? LastActivity { get; set; }
            */
             
            [Required]
            [Display(Name = "Listing Type")]
            public ListingType Type { get; set; }

            public ListingStatus Status { get; set; }

            [Required]
            public string Title { get; set; }

            [Required]
            public string Description { get; set; }
            
            [Required]
            public string Payment { get; set; }

            /*
            [Required]
            [Display(Name = "Inquiry Selection")]
            public ListingAcceptType AcceptType { get; set; }
           
            [Range(0,100, ErrorMessage = "{0} must be a number between {1} and {2}")]
            [Display(Name = "Maximum Number of Auto Accepts")]
            public int? AutoAcceptMax { get; set; }
             */
        }

        public class ListingDBContext : DbContext
        {
            public DbSet<Listing> Listings { get; set; }
        }
    
}