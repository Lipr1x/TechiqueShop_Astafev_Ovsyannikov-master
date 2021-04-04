using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;
using TechiqueShopDatabaseImplement.Models;
namespace TechiqueShopDatabaseImplement.Implements
{
    public class DeliveryStorage : IDeliveryStorage
    {
        public List<DeliveryViewModel> GetFullList()
        {
            using (var context = new TechiqueShopDatabase())
            {
                return context.Deliveries
                    .Include(rec => rec.DeliveryComponents)
                    .ThenInclude(rec => rec.Component)
                    .ToList()
                    .Select(rec => new DeliveryViewModel
                    {
                        Id = rec.Id,
                        DeliveryName = rec.DeliveryName,
                        Date = rec.Date,
                        DeliveryComponents = rec.DeliveryComponents
                            .ToDictionary(recDeliveryComponents => recDeliveryComponents.ComponentId,
                            recDeliveryComponents => (recDeliveryComponents.Component?.ComponentName,
                            recDeliveryComponents.Count))
                    })
                    .ToList();
            }
        }
        public List<DeliveryViewModel> GetFilteredList(DeliveryBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new TechiqueShopDatabase())
            {
                return context.Deliveries
                    .Include(rec => rec.DeliveryComponents)
                    .ThenInclude(rec => rec.Component)
                    .Where(rec => rec.DeliveryName.Contains(model.DeliveryName))
                    .ToList()
                    .Select(rec => new DeliveryViewModel
                    {
                        Id = rec.Id,
                        DeliveryName = rec.DeliveryName,
                        Date = rec.Date,
                        DeliveryComponents = rec.DeliveryComponents
                            .ToDictionary(recDeliveryComponents => recDeliveryComponents.ComponentId,
                            recDeliveryComponents => (recDeliveryComponents.Component?.ComponentName,
                            recDeliveryComponents.Count))
                    })
                    .ToList();
            }
        }
        public DeliveryViewModel GetElement(DeliveryBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new TechiqueShopDatabase())
            {
                var delivery = context.Deliveries
                    .Include(rec => rec.DeliveryComponents)
                    .ThenInclude(rec => rec.Component)
                    .FirstOrDefault(rec => rec.DeliveryName == model.DeliveryName ||
                    rec.Id == model.Id);

                return delivery != null ?
                    new DeliveryViewModel
                    {
                        Id = delivery.Id,
                        DeliveryName = delivery.DeliveryName,
                        Date = delivery.Date,
                        DeliveryComponents = delivery.DeliveryComponents
                            .ToDictionary(recDeliveryComponent => recDeliveryComponent.ComponentId,
                            recDeliveryComponent => (recDeliveryComponent.Component?.ComponentName,
                            recDeliveryComponent.Count))
                    } :
                    null;
            }
        }
        public void Insert(DeliveryBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Delivery(), context);
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
        public void Update(DeliveryBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var delivery = context.Deliveries.FirstOrDefault(rec => rec.Id == model.Id);

                        if (delivery == null)
                        {
                            throw new Exception("Закупка не найдена");
                        }

                        CreateModel(model, delivery, context);
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
        public void Delete(DeliveryBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                var Component = context.Deliveries.FirstOrDefault(rec => rec.Id == model.Id);

                if (Component == null)
                {
                    throw new Exception("Материал не найден");
                }

                context.Deliveries.Remove(Component);
                context.SaveChanges();
            }
        }
        private Delivery CreateModel(DeliveryBindingModel model, Delivery delivery, TechiqueShopDatabase context)
        {
            delivery.DeliveryName = model.DeliveryName;
            delivery.Date = model.Date;
            if (delivery.Id == 0)
            {
                context.Deliveries.Add(delivery);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var deliveryComponent = context.DeliveryComponents
                    .Where(rec => rec.DeliveryId == model.Id.Value)
                    .ToList();

                context.DeliveryComponents.RemoveRange(deliveryComponent
                    .Where(rec => !model.DeliveryComponents.ContainsKey(rec.DeliveryId))
                    .ToList());
                context.SaveChanges();

                foreach (var updateComponent in deliveryComponent)
                {
                    updateComponent.Count = model.DeliveryComponents[updateComponent.ComponentId].Item2;
                    model.DeliveryComponents.Remove(updateComponent.DeliveryId);
                }
                context.SaveChanges();
            }
            foreach (var deliveryComponent in model.DeliveryComponents)
            {
                context.DeliveryComponents.Add(new DeliveryComponent
                {
                    DeliveryId = delivery.Id,
                    ComponentId = deliveryComponent.Key,
                    Count = deliveryComponent.Value.Item2
                });
                context.SaveChanges();
            }
            return delivery;
        }
    }
}