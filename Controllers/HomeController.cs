using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Alpha_Sharp.Models;
using Alpha_Sharp.Models.HomeViewModels;
using Alpha_Sharp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Alpha_Sharp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [TempData]
        public string ErrorMessage { get; set; }
        public IActionResult Index(HomeIndexViewModel model)
        {
            List<ApplicationAuction> AuctionList = _context.ApplicationAuction.Include(n => n.ApplicationUser).Include(itm => itm.ItemForBid).OrderByDescending(itm => itm.HightestBidInt).ToList();
            foreach(ApplicationAuction auction in AuctionList){
                if(auction.ItemForBid.EndDate < DateTime.Now){
                    var winnerId = auction.HighestBidder;
                    var winnerObj = _userManager.Users.Where(user => user.Id == winnerId).SingleOrDefault();
                    winnerObj.Wallet = winnerObj.Wallet - auction.HightestBidInt;
                    auction.ItemForBid.CreatedBy.Wallet += auction.HightestBidInt;
                    _context.ApplicationItem.Remove(auction.ItemForBid);
                    _context.ApplicationAuction.Remove(auction);
                }
                // else {
                //     auction.TimeLeft = auction.ItemForBid.EndDate - DateTime.Now;
                // }
                _context.SaveChanges();
            };
            var CurrentUserId = _userManager.GetUserId(User);
            var CurrentUserOjb = _userManager.Users.Where(user => user.Id == CurrentUserId).SingleOrDefault();
            model.Auction = AuctionList;
            ViewBag.User = CurrentUserOjb;
            return View(model);
        }

        [HttpGet]
        [Route("/new/")]
        public IActionResult NewAuction()
        {
            return View();
        }

        [HttpPost]
        [Route("/new/")]
        public IActionResult NewAuction(NewAuctionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var CurrentUserId = _userManager.GetUserId(User);
                var CurrentUserOjb = _userManager.Users.Where(user => user.Id == CurrentUserId).SingleOrDefault();
                var item = new ApplicationItem {
                    ItemName = model.ItemName, 
                    ItemDetails = model.ItemDetails, 
                    StartingBid = model.StartingBid, 
                    EndDate = model.EndDate,
                    CreatedBy = CurrentUserOjb
                };
                _context.ApplicationItem.Add(item);
                var newAuction = new ApplicationAuction {
                    ItemForBid = item,
                    ApplicationUser = CurrentUserOjb,
                    HighestBidder = CurrentUserId,
                    HightestBidInt = model.StartingBid
                };
                _context.ApplicationAuction.Add(newAuction);
                _context.SaveChanges();
                return Redirect("/");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [Route("/Item/{itemId}")]
        public IActionResult ItemDetails(ItemDetailsViewModel model, int itemId)
        {
            var CurrentAuction = _context.ApplicationAuction.Where(auction => auction.ItemId == itemId).SingleOrDefault();
            model.AuctionItem = _context.ApplicationItem.Where(itm => itm.ItemId == itemId).Include(n => n.CreatedBy).SingleOrDefault();
            model.AuctionEvent = CurrentAuction;
            model.MonieBags = _userManager.Users.Where(user => user.Id == CurrentAuction.HighestBidder).SingleOrDefault().UserName;

            return View(model);
        }

        [HttpPost]
        [Route("/Bid/{itemId}")]
        public IActionResult NewBid(ItemDetailsViewModel model, int itemId)
        {
            var CurrentUserId = _userManager.GetUserId(User);
            var CurrentUserOjb = _userManager.Users.Where(user => user.Id == CurrentUserId).SingleOrDefault();
            var CurItem = _context.ApplicationItem.Where(itm => itm.ItemId == itemId).SingleOrDefault();
            var Auction = _context.ApplicationAuction.Where(itm => itm.ItemId == itemId).SingleOrDefault();
            bool canBid = true;
            if (CurrentUserOjb.Wallet < model.newBid)
            {
                TempData["Error1"] = "Your Wallet's Ballence is to low to make that Bid.";
                canBid = false;
            }
            if(model.newBid < Auction.HightestBidInt){
                TempData["Error2"] = "Someone is currently out Bidding you. \n Please place a higher bid.";
                canBid = false;
            }
            if(canBid == true)
            {
                Auction.HightestBidInt = model.newBid;
                Auction.HighestBidder = CurrentUserId;
                _context.SaveChanges();
            }

            return Redirect($"/Item/{itemId}");
        }

        [HttpGet]
        [Route("/delete/{ItemId}/")]
        public IActionResult delete(int ItemId, string returnUrl = null)
        {
            var CurrentUserId = _userManager.GetUserId(User);
            var CurrentUserOjb = _userManager.Users.Where(user => user.Id == CurrentUserId).SingleOrDefault();
            var CurItem = _context.ApplicationItem.Where(itm => itm.ItemId == ItemId).SingleOrDefault();
            var Auction = _context.ApplicationAuction.Where(itm => itm.ItemId == ItemId).SingleOrDefault();
            if (CurrentUserOjb != CurItem.CreatedBy)
            {
                return Redirect("/");
            }
            _context.ApplicationItem.Remove(CurItem);
            _context.ApplicationAuction.Remove(Auction);
            _context.SaveChanges();
            return Redirect("/");
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
