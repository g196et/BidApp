using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidApp.ModelEF
{
    public interface IBidRepository
    {
        IEnumerable<Bid> Bids { get; }
        IEnumerable<Comment> Comments { get; }
        void SaveBid (Bid bid, string text=null);
        Bid DeleteBid (int bidId);
        TimeSpan DiffTime(Bid bid);
    }
}
