using System;
using System.Collections.Generic;
using BidApp.ModelEF;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BidApp.MVC.Models
{
    /// <summary>
    /// Модель для просмотра домашней страницы
    /// </summary>
    public class ListBidViewModel
    {
        public IEnumerable<Bid> bids { get; set; }

        [Display(Name = "Статус")]
        public SelectList status {get;set;}

        [Display(Name = "Минимальное время")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Пожалуйста, введите коррекное время: dd.mm.yyyy hh:mm:ss")]
        public DateTime minTime { get; set; }

        [Display(Name = "Максимальное время")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Пожалуйста, введите коррекное время: dd.mm.yyyy hh:mm:ss")]
        public DateTime maxTime { get; set; }

        [Display(Name = "Возврат")]
        public bool returned { get; set; }

        [Display(Name = "Сортировка")]
        public SelectList sortType { get; set; }
    }
}