using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidApp.ModelEF
{
    public class BidRepository : IBidRepository
    {
        EFDbContext context = new EFDbContext();
        DateTime minDate = new DateTime(1753, 1, 1);
        public IEnumerable<Bid> Bids
        {
            get { return context.Bids; }
        }

        public IEnumerable<Comment> Comments
        {
            get { return context.Comments; }
        }

        /// <summary>
        /// Сохранить заявку
        /// </summary>
        /// <param name="bid">Экземпляр заявки</param>
        /// <param name="text">Необязательный параметр. Текст комментария к изменению статуса</param>
        public void SaveBid (Bid bid, string text = null)
        {
            //Если заявка новая устанавливаем значения по умолчанию и добавляем её в БД
            if (bid.BidId == 0)
            {
                bid.Returned = false;
                bid.Status = "Открыта";
                bid.Time = DateTime.Now;
                bid.TimeEnd = minDate;
                context.Bids.Add(bid);
            }
            //Если заявка существует, и её изменяют
            else
            {
                //Ещем эту заявку по ID
                Bid dbEntry = context.Bids.Find(bid.BidId);
                if (dbEntry != null)
                {
                    //Меняем значения по логике поставленной задачи
                    dbEntry.Name = bid.Name;
                    dbEntry.Description = bid.Description;
                    //Проверка что заявка получила статус "закрыта" в первый раз
                    if ((bid.Status.Equals("Закрыта")) && (bid.TimeEnd == minDate)) 
                        bid.TimeEnd = DateTime.Now;
                    //Если был изменёнм статуст, создаём новый комментарий для этого изменения и заносимв БД
                    if (dbEntry.Status != bid.Status)
                    {
                        Comment comment = new Comment
                        {
                            From = dbEntry.Status,
                            To = bid.Status,
                            Text = text,
                            Bid = dbEntry
                        };
                        context.Comments.Add(comment);
                    }
                    dbEntry.Status = bid.Status;
                    if (dbEntry.Status == "Возврат")
                        dbEntry.Returned = true;
                    dbEntry.Time = bid.Time;
                }
            }
            //Сохраняем изменения
            context.SaveChanges();
        }
        /// <summary>
        /// Метод считает время выполнения заявки
        /// </summary>
        /// <param name="bid">Заявка в которой считается время выполнения</param>
        /// <returns>Время выполнения заявки</returns>
        public TimeSpan DiffTime(Bid bid)
        {
            DateTime tmp = bid.TimeEnd;
            //Если заявка не закрыта, считаем по данный момент
            if (tmp == DateTime.MinValue)
                tmp = DateTime.Now;
            return tmp.Subtract(bid.Time);
        }

        /// <summary>
        /// Метод удаления заявки из БД
        /// </summary>
        /// <param name="bidId">ID удаляемой заявки</param>
        /// <returns>Удалённая заявка</returns>
        public Bid DeleteBid(int bidId)
        {
            Bid dbEntry = context.Bids.Find(bidId);
            if (dbEntry != null)
            {
                context.Bids.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
