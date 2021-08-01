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
    public class AssemblyStorage : IAssemblyStorage
    {
        private readonly int _AssemblyNameMaxLength = 50;
        public List<AssemblyViewModel> GetFullList()
        {
            using (var context = new TechiqueShopDatabase())
            {
                return context.Assemblys
                .Include(rec => rec.Provider)
                .Include(rec => rec.AssemblyComponents)
                .ThenInclude(rec => rec.Component)
                .ToList()
                .Select(rec => new AssemblyViewModel
                {
                    Id = rec.Id,
                    OrderId = rec.OrderId,
                    Price = rec.Price,
                    AssemblyName = rec.AssemblyName,
                    AssemblyComponents = rec.AssemblyComponents.ToDictionary(recRC => recRC.ComponentId, recRC => (recRC.Component?.ComponentName, recRC.Count)),
                    ProviderId = rec.ProviderId
                })
                .ToList();
            }
        }
        public List<AssemblyViewModel> GetFilteredList(AssemblyBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new TechiqueShopDatabase())
            {

                return context.Assemblys
                .Include(rec => rec.Provider)
                .Include(rec => rec.AssemblyComponents)
                .ThenInclude(rec => rec.Component)
                .Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue && rec.ProviderId == model.ProviderId) ||
                (model.DateFrom.HasValue && model.DateTo.HasValue && rec.ProviderId == model.ProviderId))
                .ToList()
                .Select(rec => new AssemblyViewModel
                {
                    Id = rec.Id,
                    OrderId = rec.OrderId,
                    Price = rec.Price,
                    AssemblyName = rec.AssemblyName,
                    AssemblyComponents = rec.AssemblyComponents.ToDictionary(recRC => recRC.ComponentId, recRC => (recRC.Component?.ComponentName, recRC.Count)),
                    ProviderId = rec.ProviderId
                })
                .ToList();
            }
        }
        public AssemblyViewModel GetElement(AssemblyBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new TechiqueShopDatabase())
            {
                Assembly assem = context.Assemblys
                .Include(rec => rec.Provider)
                .Include(rec => rec.AssemblyComponents)
                .ThenInclude(rec => rec.Component)
                .FirstOrDefault(rec => rec.AssemblyName == model.AssemblyName || rec.Id == model.Id);
                return assem != null ? new AssemblyViewModel
                {
                    Id = assem.Id,
                    OrderId = assem.OrderId,
                    Price = assem.Price,
                    AssemblyName = assem.AssemblyName,
                    AssemblyComponents = assem.AssemblyComponents.ToDictionary(recRC => recRC.ComponentId, recRC => (recRC.Component?.ComponentName, recRC.Count)),
                    ProviderId = assem.ProviderId
                } : null;
            }
        }
        public void Insert(AssemblyBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Assembly(), context);
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
        public void Update(AssemblyBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Assembly element = context.Assemblys.FirstOrDefault(rec => rec.Id == model.Id);

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

        public void Delete(AssemblyBindingModel model)
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

        private Assembly CreateModel(AssemblyBindingModel model, Assembly sypply, TechiqueShopDatabase context)
        {
            if (model.AssemblyName.Length > _AssemblyNameMaxLength)
            {
                throw new Exception($"Название сборки должно быть длиной до { _AssemblyNameMaxLength } ");
            }
            sypply.AssemblyName = model.AssemblyName;
            sypply.OrderId = model.OrderId;
            sypply.Price = model.Price;
            sypply.ProviderId = (int)model.ProviderId;

            if (sypply.Id == 0)
            {
                context.Assemblys.Add(sypply);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var assemblyComponents = context.AssemblyComponents.Where(rec => rec.AssemblyId == model.Id.Value).ToList();
                context.AssemblyComponents.RemoveRange(assemblyComponents.Where(rec => !model.AssemblyComponents.ContainsKey(rec.AssemblyId)).ToList());
                context.SaveChanges();

                foreach (var updateOrder in assemblyComponents)
                {
                    updateOrder.Count = model.AssemblyComponents[updateOrder.ComponentId].Item2;
                    model.AssemblyComponents.Remove(updateOrder.ComponentId);
                }
                context.SaveChanges();
            }
            foreach (var rc in model.AssemblyComponents)
            {
                try
                {
                    context.AssemblyComponents.Add(new AssemblyComponent
                    {
                        AssemblyId = sypply.Id,
                        ComponentId = rc.Key,
                        Count = rc.Value.Item2
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                context.SaveChanges();

            }
            return sypply;
        }
    }
}