using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BidApp.ModelEF
{
    /// <summary>
    /// Модель комментария
    /// </summary>
    public class Comment
    {
        [HiddenInput(DisplayValue = false)]
        public int CommentId { get; set; }

        [Display(Name = "Из какого статуса")]
        [HiddenInput(DisplayValue = false)]
        public string From { get; set; }

        [Display(Name = "В какой статус")]
        [Required(ErrorMessage = "Пожалуйста, укажите в какой статус переносится заявка")]
        public string To { get; set; }

        [Display(Name = "Текст комментария")]
        [Required(ErrorMessage = "Пожалуйста, укажите текст комментария")]
        public string Text { get; set; }

        public int BidId { get; set; }
        public Bid Bid { get; set; }

        public override string ToString()
        {
            return "Статус измененёс с " + From + " на " + To + " комментарий: " + Text;
        }
    }
}
