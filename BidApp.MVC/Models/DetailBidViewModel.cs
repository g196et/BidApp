using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BidApp.ModelEF;

namespace BidApp.MVC.Models
{
    /// <summary>
    /// Модель для просмотра страницы информации о заявке
    /// </summary>
    public class DetailBidViewModel
    {
        public Bid bid { get; set; }
        public IEnumerable<Comment> comments { get; set; }
    }
}