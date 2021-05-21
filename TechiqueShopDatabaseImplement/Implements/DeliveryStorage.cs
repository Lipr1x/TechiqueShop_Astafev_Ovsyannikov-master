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
    public class DeliveryStorage : IDeliveryStorage
    {
        public List<DeliveryViewModel> GetFullList()
        {
            using (var context = new TechiqueShopDatabase())
            {
                return context.Deliveries
                .Include(rec => rec.Provider)
                .Include(rec => rec.DeliveryComponents)
                .ThenInclude(rec => rec.Component)
                .ToList()
                .Select(rec => new DeliveryViewModel
                {
                    Id = rec.Id,
                    Date = rec.Date,
                    DeliveryComponents = rec.DeliveryComponents.ToDictionary(recDC => recDC.ComponentId, recDC => (recDC.Component?.ComponentName, recDC.Count)),
                    ProviderId = rec.ProviderId
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
                .Include(rec => rec.Provider)
                .Include(rec => rec.DeliveryComponents)
                .ThenInclude(rec => rec.Component)
                .Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue && rec.ProviderId == model.ProviderId) ||
                (model.DateFrom.HasValue && model.DateTo.HasValue && rec.ProviderId == model.ProviderId && rec.Date >= model.DateFrom.Value.Date && rec.Date <= model.DateTo.Value.Date))
                .ToList()
                .Select(rec => new DeliveryViewModel
                {
                    Id = rec.Id,
                    DeliveryName = rec.DeliveryName,
                    Date = rec.Date,
                    DeliveryComponents = rec.DeliveryComponents.ToDictionary(recDC => recDC.ComponentId, recDC => (recDC.Component?.ComponentName, recDC.Count)),
                    ProviderId = rec.ProviderId
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
                Delivery del = context.Deliveries
                .Include(rec => rec.Provider)
                .Include(rec => rec.DeliveryComponents)
                .ThenInclude(rec => rec.Component)
                .FirstOrDefault(rec => rec.Date == model.Date || rec.Id == model.Id);
                return del != null ? new DeliveryViewModel
                {
                    Id = del.Id,
                    Date = del.Date,
                    DeliveryComponents = del.DeliveryComponents.ToDictionary(recDC => recDC.ComponentId, recDC => (recDC.Component?.ComponentName, recDC.Count)),
                    ProviderId = del.ProviderId
                } : null;
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
                        Delivery element = context.Deliveries.FirstOrDefault(rec => rec.Id == model.Id);

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

        public void Delete(DeliveryBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                Delivery element = context.Deliveries.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Deliveries.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Delivery CreateModel(DeliveryBindingModel model, Delivery delivery, TechiqueShopDatabase context)
        {
            delivery.Date = model.Date;
            delivery.ProviderId = (int)model.ProviderId;
            delivery.DeliveryName = model.DeliveryName;

            if (delivery.Id == 0)
            {
                context.Deliveries.Add(delivery);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var distributionCosmetics = context.DeliveryComponents.Where(rec => rec.DeliveryId == model.Id.Value).ToList();
                context.DeliveryComponents.RemoveRange(distributionCosmetics.Where(rec => !model.DeliveryComponents.ContainsKey(rec.ComponentId)).ToList());
                context.SaveChanges();

                foreach (var updateCosmetic in distributionCosmetics)
                {
                    updateCosmetic.Count = model.DeliveryComponents[updateCosmetic.ComponentId].Item2;
                    model.DeliveryComponents.Remove(updateCosmetic.ComponentId);
                }
                context.SaveChanges();
            }
            foreach (var dc in model.DeliveryComponents)
            {
                context.DeliveryComponents.Add(new DeliveryComponent
                {
                    DeliveryId = delivery.Id,
                    ComponentId = dc.Key,
                    Count = dc.Value.Item2
                });
                context.SaveChanges();
            }
            return delivery;
        }
    }
}