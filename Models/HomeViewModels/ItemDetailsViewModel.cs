using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha_Sharp.Models.HomeViewModels
{
    public class ItemDetailsViewModel
    {
        public ApplicationItem AuctionItem { get; set; }

        public ApplicationAuction AuctionEvent {get; set;}

        //should add error for log int
        public int newBid {get; set;}

        public string Time {get; set;} = "sample time";

        public string MonieBags {get; set;}
    }
}