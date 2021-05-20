using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using TechiqueShopDatabaseImplement.Models;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace TechiqueShopDatabaseImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        public List<OrderViewModel> GetFullList()
        {
            using (var context = new TechiqueShopDatabase())
            {
                return context.Orders
                .Include(rec => rec.Customer)
                .Select(rec => new OrderViewModel
                {
                    Id = rec.Id,
                    OrderName = rec.OrderName,
                    Price = rec.Price,
                    CustomerId = rec.CustomerId
                })
                .ToList();
            }
        }

        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new TechiqueShopDatabase())
            {
                return context.Orders
                .Include(rec => rec.Customer)
                .Where(rec => rec.CustomerId == model.CustomerId || rec.OrderName.Contains(model.OrderName))
                .Select(rec => new OrderViewModel
                {
                    Id = rec.Id,
                    OrderName = rec.OrderName,
                    Price = rec.Price,
                    CustomerId = rec.CustomerId
                })
                .ToList();
            }
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new TechiqueShopDatabase())
            {
                var tmp = model.CustomerId;
                var cosmetic = context.Orders
                .Include(rec => rec.Customer)
                .FirstOrDefault(rec => rec.OrderName == model.OrderName || rec.Id == model.Id);
                return cosmetic != null ?
                new OrderViewModel
                {
                    Id = cosmetic.Id,
                    OrderName = cosmetic.OrderName,
                    Price = cosmetic.Price,
                    CustomerId = cosmetic.CustomerId
                } :
                null;
            }
        }

        public void Insert(OrderBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                context.Orders.Add(CreateModel(model, new Order()));
                context.SaveChanges();
            }
        }

        public void Update(OrderBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                var element = context.Orders.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }

        public void Delete(OrderBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                Order element = context.Orders.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Orders.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.OrderName = model.OrderName;
            order.Price = model.Price;
            order.CustomerId = (int)model.CustomerId;
            return order;
        }
    }
}
