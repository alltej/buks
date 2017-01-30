﻿using System;

namespace PPPDDDChap19.eBidder.Listings.Model.Auctions
{
    public class OutBid
    {
        public OutBid(Guid auctionId, Guid bidderId)
        {
            if (auctionId == Guid.Empty)
                throw new ArgumentNullException("Auction Id cannot be null");

            if (bidderId == Guid.Empty)
                throw new ArgumentNullException("Bidder Id cannot be null");

            AuctionId = auctionId;
            Bidder = bidderId;
        }

        public Guid AuctionId { get; private set; }
        public Guid Bidder { get; private set; }
    }
}
