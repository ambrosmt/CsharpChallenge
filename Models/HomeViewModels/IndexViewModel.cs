using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha_Sharp.Models.HomeViewModels
{
    public class HomeIndexViewModel
    {
        public List<ApplicationAuction> Auction {get; set;}
    }
}