using BidApp.ModelEF;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BidApp.MVC.Models
{
    /// <summary>
    /// Модель для просмотра страницы редактирования заявки
    /// </summary>
    public class EditBidViewModel
    {
        [Required]
        public Bid bid { get; set; }

        [Display(Name = "Статус")]
        public SelectList status { get; set; }

        [Display(Name = "Комментарий")]
        public Comment newComment { get; set; }
    }
}