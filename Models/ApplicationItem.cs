using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Alpha_Sharp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationItem : BaseEntity
    {

        [Key]
        public int ItemId { get; set; }

        public string ItemName {get; set;}

        public ApplicationUser CreatedBy { get; set; }

        public string ItemDetails { get; set; }

        public int StartingBid { get; set; }
        
        public DateTime EndDate { get; set; }

        public List<ApplicationAuction> ApplicationAuction { get; set; }

        public ApplicationItem()
        {
            ApplicationAuction = new List<ApplicationAuction>();
        }
    }
}