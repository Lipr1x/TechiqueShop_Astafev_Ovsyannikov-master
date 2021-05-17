using Microsoft.EntityFrameworkCore;
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
    public class SupplyStorage: ISupplyStorage
    {
        private readonly int _SupplyNameMaxLength = 50;
        public List<SupplyViewModel> GetFullList()
        {
            using (var context = new TechiqueShopDatabase())
            {
                return context.Supplies
                    .Include(rec => rec.SupplyOrders)
                    .ThenInclude(rec => rec.Order)
                    .ToList()
                    .Select(rec => new SupplyViewModel
                    {
                        Id = rec.Id,
                        SupplyName = rec.SupplyName,
                        Date = rec.Date,
                        SupplyOrders = rec.SupplyOrders
                            .ToDictionary(recSupplyOrders => recSupplyOrders.OrderId,
                            recSupplyOrders => (recSupplyOrders.Order?.OrderName,
                            recSupplyOrders.Sum))
                    })
                    .ToList();
            }
        }
        public List<SupplyViewModel> GetFilteredList(SupplyBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new TechiqueShopDatabase())
            {
                return context.Supplies
                    .Include(rec => rec.SupplyOrders)
                    .ThenInclude(rec => rec.Order)
                    .Where(rec => rec.SupplyName.Contains(model.SupplyName))
                    .ToList()
                    .Select(rec => new SupplyViewModel
                    {
                        Id = rec.Id,
                        SupplyName = rec.SupplyName,
                        Date = rec.Date,
                        SupplyOrders = rec.SupplyOrders
                            .ToDictionary(recSupplyOrders => recSupplyOrders.OrderId,
                            recSupplyOrders => (recSupplyOrders.Order?.OrderName,
                            recSupplyOrders.Sum))
                    })
                    .ToList();
            }
        }
        public SupplyViewModel GetElement(SupplyBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new TechiqueShopDatabase())
            {
                var supply = context.Supplies
                    .Include(rec => rec.SupplyOrders)
                    .ThenInclude(rec => rec.Order)
                    .FirstOrDefault(rec => rec.SupplyName == model.SupplyName ||
                    rec.Id == model.Id);

                return supply != null ?
                    new SupplyViewModel
                    {
                        Id = supply.Id,
                        SupplyName = supply.SupplyName,
                        Date = supply.Date,
                        SupplyOrders = supply.SupplyOrders
                            .ToDictionary(recSupplyOrder => recSupplyOrder.OrderId,
                            recSupplyOrder => (recSupplyOrder.Order?.OrderName,
                            recSupplyOrder.Sum))
                    } :
                    null;
            }
        }
        public void Insert(SupplyBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Supply(), context);
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Update(SupplyBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var supply = context.Supplies.FirstOrDefault(rec => rec.Id == model.Id);

                        if (supply == null)
                        {
                            throw new Exception("Подарок не найден");
                        }

                        CreateModel(model, supply, context);
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Delete(SupplyBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                var Order = context.Supplies.FirstOrDefault(rec => rec.Id == model.Id);

                if (Order == null)
                {
                    throw new Exception("Заказ не найден");
                }

                context.Supplies.Remove(Order);
                context.SaveChanges();
            }
        }
        private Supply CreateModel(SupplyBindingModel model, Supply supply, TechiqueShopDatabase context)
        {
            if (model.SupplyName.Length > _SupplyNameMaxLength)
            {
                throw new Exception($"Название поставки должно быть длиной до { _SupplyNameMaxLength } ");
            }
            supply.SupplyName = model.SupplyName;
            supply.Date = model.Date;
            if (supply.Id == 0)
            {
                context.Supplies.Add(supply);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var supplyOrder = context.SupplyOrders
                    .Where(rec => rec.SupplyId == model.Id.Value)
                    .ToList();

                context.SupplyOrders.RemoveRange(supplyOrder
                    .Where(rec => !model.SupplyOrders.ContainsKey(rec.SupplyId))
                    .ToList());
                context.SaveChanges();

                foreach (var updateComponent in supplyOrder)
                {
                    updateComponent.Sum = model.SupplyOrders[updateComponent.OrderId].Item2;
                    model.SupplyOrders.Remove(updateComponent.SupplyId);
                }
                context.SaveChanges();
            }
            foreach (var supplyOrder in model.SupplyOrders)
            {
                context.SupplyOrders.Add(new SupplyOrder
                {
                    SupplyId = supply.Id,
                    OrderId = supplyOrder.Key,
                    Sum = supplyOrder.Value.Item2
                });
                context.SaveChanges();
            }
            return supply;
        }
    }
}
