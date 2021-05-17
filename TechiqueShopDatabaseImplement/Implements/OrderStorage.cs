using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;
using TechiqueShopDatabaseImplement.Models;

namespace TechiqueShopDatabaseImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        private readonly int _OrderMaxLength = 50;
        public List<OrderViewModel> GetFullList()
        {
            using (var context = new TechiqueShopDatabase())
            {
                return context.Orders
                .Select(rec => new OrderViewModel
                {
                    Id = rec.Id,
                    OrderName = rec.OrderName,
                    Price = rec.Price
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
                .Where(rec => rec.OrderName.Contains(model.OrderName))
               .Select(rec => new OrderViewModel
               {
                   Id = rec.Id,
                   OrderName = rec.OrderName,
                   Price = rec.Price
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
                var order = context.Orders
                .FirstOrDefault(rec => rec.OrderName == model.OrderName || rec.Id == model.Id);
                return order != null ?
                new OrderViewModel
                {
                    Id = order.Id,
                    OrderName = order.OrderName,
                    Price = order.Price
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
                var element = context.Orders.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element == null)
                {
                    throw new Exception("Провайдер не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }
        public void Delete(OrderBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                Order element = context.Orders.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Orders.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Провайдер не найден");
                }
            }
        }
        private Order CreateModel(OrderBindingModel model, Order order)
        {
            if (model.OrderName.Length > _OrderMaxLength)
            {
                throw new Exception($"Название заказа должно быть длиной до { _OrderMaxLength } ");
            }
            if (!Regex.IsMatch(model.Price.ToString(), @"^\d{1,50}$"))
            {
                throw new Exception($"Нельзя назначить настолько высокую цену заказу, также должен состоять из цифр");
            }
            order.OrderName = model.OrderName;
            order.Price = model.Price;

            return order;
        }
    }
}
