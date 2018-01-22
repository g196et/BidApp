using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BidApp.ModelEF
{
    /// <summary>
    /// Модель заявки
    /// </summary>
    public class Bid
    {
        [HiddenInput(DisplayValue = false)]
        public int BidId { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название заявки")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, укажите Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }

        [Display(Name = "Время создания")]
        [HiddenInput(DisplayValue = false)]
        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }

        [Display(Name = "Время выполнения")]
        [HiddenInput(DisplayValue = false)]
        [DataType(DataType.DateTime)]
        public DateTime TimeEnd { get; set; }

        [Display(Name = "Была возвращена?")]
        [HiddenInput(DisplayValue = false)]
        public bool Returned { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
