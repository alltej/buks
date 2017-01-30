﻿using System;

namespace DDDPPP.Chap21.EFExample.Application.Model.Auction
{
    public class AuctionSnapshot
    {
        public Guid Id { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime EndsAt { get; set; }
        public WinningBidSnapshot WinningBid { get; set; }
        public int Version { get; set; }
    }
}
