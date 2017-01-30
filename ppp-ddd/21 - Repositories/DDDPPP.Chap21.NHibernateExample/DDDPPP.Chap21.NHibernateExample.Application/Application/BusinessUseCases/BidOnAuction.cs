﻿using System;
using DDDPPP.Chap21.NHibernateExample.Application.Model.Auction;
using DDDPPP.Chap21.NHibernateExample.Application.Model.BidHistory;
using NHibernate;
using DDDPPP.Chap21.NHibernateExample.Application.Infrastructure;

namespace DDDPPP.Chap21.NHibernateExample.Application.Application.BusinessUseCases
{
    public class BidOnAuction
    {
        private IAuctionRepository _auctionRepository;
        private IBidHistoryRepository _bidHistoryRepository;
        private ISession _unitOfWork;
        private IClock _clock;

        public BidOnAuction(IAuctionRepository auctionRepository, 
                            IBidHistoryRepository bidHistoryRepository, 
                            ISession unitOfWork, IClock clock)
        {
            _auctionRepository = auctionRepository;
            _bidHistoryRepository = bidHistoryRepository;
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        public void Bid(Guid auctionId, Guid memberId, decimal amount)
        {
            try
            {
                using (ITransaction transaction = _unitOfWork.BeginTransaction())
                {
                    using (DomainEvents.Register(OutBid()))
                    using (DomainEvents.Register(BidPlaced()))
                    {
                        var auction = _auctionRepository.FindBy(auctionId);

                        var bidAmount = new Money(amount);

                        auction.PlaceBidFor(new Offer(memberId, bidAmount, _clock.Time()), _clock.Time());
                    }

                    transaction.Commit();
                }
            }
            catch (StaleObjectStateException ex)
            {
                _unitOfWork.Clear();

                Bid(auctionId, memberId, amount);
            } 
        }

        private Action<BidPlaced> BidPlaced()
        {
            return (BidPlaced e) =>
            {               
                var bidEvent = new Bid(e.AuctionId, e.Bidder, e.AmountBid, e.TimeOfBid);
              
                _bidHistoryRepository.Add(bidEvent);
            };
        }

        private Action<OutBid> OutBid()
        {
            return (OutBid e) => 
            { 
                // Email customer to say that he has been out bid                
            };
        }
    }
}
