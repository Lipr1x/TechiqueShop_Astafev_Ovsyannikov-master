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
        public List<SupplyViewModel> GetFullList()
        {
            using (var context = new TechiqueShopDatabase())
            {
                return context.Supplies
                .Include(rec => rec.Customer)
                .Include(rec => rec.SupplyOrders)
                .ThenInclude(rec => rec.Order)
                .ToList()
                .Select(rec => new SupplyViewModel
                {
                    Id = rec.Id,
                    TotalCost = rec.TotalCost,
                    ComponentId = rec.ComponentId,
                    SupplyName = rec.SupplyName,
                    Date = rec.Date,
                    SupplyOrders = rec.SupplyOrders.ToDictionary(recRC => recRC.OrderId, recRC => (recRC.Order?.OrderName, recRC.Sum)),
                    CustomerId = rec.CustomerId
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
                .Include(rec => rec.Customer)
                .Include(rec => rec.Component)
                .Include(rec => rec.SupplyOrders)
                .ThenInclude(rec => rec.Order)
                .Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue && rec.CustomerId == model.CustomerId) ||
                (model.DateFrom.HasValue && model.DateTo.HasValue && rec.CustomerId == model.CustomerId && rec.Date.Date >= model.DateFrom.Value.Date && rec.Date.Date <= model.DateTo.Value.Date))
                .ToList()
                .Select(rec => new SupplyViewModel
                {
                    Id = rec.Id,
                    SupplyName = rec.SupplyName,
                    ComponentId = rec.ComponentId,
                    TotalCost = rec.TotalCost,
                    Date = rec.Date.Date,
                    SupplyOrders = rec.SupplyOrders.ToDictionary(recRC => recRC.OrderId, recRC => (recRC.Order?.OrderName, recRC.Sum)),
                    CustomerId = rec.CustomerId
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
                Supply supply = context.Supplies
                .Include(rec => rec.Customer)
                .Include(rec => rec.Component)
                .Include(rec => rec.SupplyOrders)
                .ThenInclude(rec => rec.Order)
                .FirstOrDefault(rec => rec.Date == model.Date || rec.Id == model.Id);
                return supply != null ? new SupplyViewModel
                {
                    Id = supply.Id,
                    SupplyName = supply.SupplyName,
                    ComponentId = supply.ComponentId,
                    TotalCost = supply.TotalCost,
                    Date = supply.Date,
                    SupplyOrders = supply.SupplyOrders.ToDictionary(recRC => recRC.OrderId, recRC => (recRC.Order?.OrderName, recRC.Sum)),
                    CustomerId = supply.CustomerId
                } : null;
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
                        Supply element = context.Supplies.FirstOrDefault(rec => rec.Id == model.Id);

                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }

                        CreateModel(model, element, context);
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
                Supply element = context.Supplies.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Supplies.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Supply CreateModel(SupplyBindingModel model, Supply sypply, TechiqueShopDatabase context)
        {
            sypply.SupplyName = model.SupplyName;
            sypply.TotalCost = model.TotalCost;
            sypply.Date = model.Date;
            sypply.ComponentId = model.ComponentId;
            sypply.CustomerId = (int)model.CustomerId;

            if (sypply.Id == 0)
            {
                context.Supplies.Add(sypply);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var supplyOrders = context.SupplyOrders.Where(rec => rec.SupplyId == model.Id.Value).ToList();
                context.SupplyOrders.RemoveRange(supplyOrders.Where(rec => !model.SupplyOrders.ContainsKey(rec.SupplyId)).ToList());
                context.SaveChanges();

                foreach (var updateOrder in supplyOrders)
                {
                    updateOrder.Sum = model.SupplyOrders[updateOrder.OrderId].Item2;
                    model.SupplyOrders.Remove(updateOrder.OrderId);
                }
                context.SaveChanges();
            }
            foreach (var rc in model.SupplyOrders)
            {
                try { 
                context.SupplyOrders.Add(new SupplyOrder
                {
                    SupplyId = sypply.Id,
                    OrderId = rc.Key,
                    Sum = rc.Value.Item2

                });
                }catch(Exception e)
                {
                    Console.WriteLine(e);
                }
                context.SaveChanges();
                
            }
            return sypply;
        }
    }
}
