using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Alpha_Sharp.Models;

namespace Alpha_Sharp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationAuction : BaseEntity
    {

        [Key]
        public int AuctionId { get; set; }

        public string UserId {get; set;}

        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public ApplicationItem ItemForBid {get; set;}

        // public TimeSpan TimeLeft {get;set;}
        //SQL error storing timespan

        public string HighestBidder { get; set; }

        public int HightestBidInt {get; set;}

        [NotMapped]
        public TimeSpan timeLeft {get;set;}

    }
}