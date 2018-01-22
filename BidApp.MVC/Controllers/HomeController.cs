using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BidApp.ModelEF;

namespace BidApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        private IBidRepository repository;

        public HomeController(IBidRepository repo)
        {
            repository = repo;
        }

        /// <summary>
        /// Отображение домашней страницы, с возможностью фильтрации
        /// </summary>
        /// <param name="status">Фильтр статуса</param>
        /// <param name="minTime">Фильтр минимального времени</param>
        /// <param name="maxTime">Фильтр максимального времени</param>
        /// <param name="returned">Фильтр условия возврата</param>
        /// <param name="sortType">Сортировка</param>
        /// <returns>Отфильтрованный список заявок</returns>
        [HttpGet]
        public ActionResult Index(string status, string minTime, string maxTime
            , bool? returned, string sortType="Без сортировки")
        {
            IEnumerable<Bid> bids = repository.Bids;
            if (!String.IsNullOrEmpty(status) && !status.Equals("Все"))
                bids = bids.Where(p => p.Status == status);
            //Валидайия через jquery and regex
            DateTime? time1, time2;
            if (string.IsNullOrEmpty(minTime))
                time1 = DateTime.MinValue;
            else
                time1 = DateTime.Parse(minTime);
            if (string.IsNullOrEmpty(maxTime))
                time2 = DateTime.Now;
            else
                time2 = DateTime.Parse(maxTime);
            bids = bids.Where(p => p.Time.CompareTo(time1) > 0 && p.Time.CompareTo(time2) < 0);
            if (returned != null)
                bids = bids.Where(p => p.Returned == returned);
            switch (sortType)
            {
                case "По времени выполнения (быстрые сначала)": bids = bids.OrderBy(p => repository.DiffTime(p)); break;
                case "По времени выполнения (долгие сначала)": bids = bids.OrderByDescending(p => repository.DiffTime(p)); break;
                case "По дате создания (свежие)":
                case "Без сортировки":
                    bids = bids.OrderByDescending(p => p.Time); break;
                case "По дате создания (старые)": bids = bids.OrderBy(p => p.Time); break;
            }
            Models.ListBidViewModel lbvm = new Models.ListBidViewModel
            {
                bids = bids.ToList(),
                status = new SelectList(new List<string>()
                {
                    "Все",
                    "Открыта",
                    "Решена",
                    "Возврат",
                    "Закрыта"
                }),
                minTime = DateTime.MinValue,
                maxTime = DateTime.Now,
                returned = false,
                sortType = new SelectList(new List<string>
                {
                    "Без сортировки",
                    "По времени выполнения (быстрые сначала)",
                    "По времени выполнения (долгие сначала)",
                    "По дате создания (свежие)",
                    "По дате создания (старые)"
                })
            };
            return View(lbvm);
        }

        /// <summary>
        /// Страница изменения заявки
        /// </summary>
        /// <param name="bidId">ID изменяемой заявки</param>
        /// <returns>View редактирования найденной заявки</returns>
        public ViewResult Edit(int bidId)
        {
            Bid bid = repository.Bids
                .FirstOrDefault(g => g.BidId == bidId);
            return View(bid);
        }

        /// <summary>
        /// Перегруженная версия Edit() для сохранения изменений
        /// </summary>
        /// <param name="bid">Редактируемая заявка</param>
        /// <param name="text">Необязательный параметр. Текст комментария к изменению статуса</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Bid bid, string text=null)
        {
            //Проверка на валидность модели
            if (ModelState.IsValid)
            {
                //Ищем старый статус завки
                string oldStatus = repository.Bids
                    .FirstOrDefault(g => g.BidId == bid.BidId).Status;
                //Если текущий статус null, то заявка уже закрыта
                if (bid.Status == null)
                    bid.Status = "Закрыта";
                //Если статус изменился и комментарий к изменению пустой
                if (!bid.Status.Equals(oldStatus) && (string.IsNullOrEmpty(text)))
                {
                    TempData["message"] = string.Format("Необходимо заполнить комментарий");
                    //Возвращаем заявке старый статус и требуем комментарий
                    bid.Status = repository.Bids
                        .FirstOrDefault(g => g.BidId == bid.BidId).Status;
                    return View(bid);
                }
                //Сохраняем изменения
                repository.SaveBid(bid, text);
                //говорим что всё хорошо
                TempData["message"] = string.Format("Изменения в заявке \"{0}\" были сохранены", bid.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(bid);
            }
        }

        /// <summary>
        /// Страница создания заявки
        /// </summary>
        /// <returns>View для создания новой заявки</returns>
        public ViewResult Create()
        {
            return View(new Bid());
        }

        /// <summary>
        /// Перегруженная версия Create() для сохранения новой заявки
        /// </summary>
        /// <param name="bid">Созданная заявка</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Bid bid)
        {
            //Проверка на валидность модели
            if (ModelState.IsValid)
            {
                //Если всё хорошо, сохраняем и переходим на домашнюю страницу
                repository.SaveBid(bid);
                TempData["message"] = string.Format("Новая заявка \"{0}\" была сохранена", bid.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(bid);
            }
        }

        /// <summary>
        /// Удаление заявки
        /// </summary>
        /// <param name="bidId">ID удаляемой заявки</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int bidId)
        {
            Bid deletedBid = repository.DeleteBid(bidId);
            if (deletedBid != null)
            {
                TempData["message"] = string.Format("Заявка \"{0}\" была удалена",
                    deletedBid.Name);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Просмотр полной информации о заявке
        /// </summary>
        /// <param name="bidId">ID просматриваемой заявки</param>
        /// <returns>View информации о заявке</returns>
        public ViewResult Detail(int bidId)
        {
            Models.DetailBidViewModel model = new Models.DetailBidViewModel
            {
                //Ищем заявку по ID
                bid = repository.Bids
               .FirstOrDefault(g => g.BidId == bidId),
                comments = repository.Comments.Where(p => p.BidId == bidId)
            };
            return View(model);
        }
    }
}