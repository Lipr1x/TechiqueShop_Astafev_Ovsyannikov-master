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
        public List<AssemblyViewModel> GetFullList()
        {
            using (var context = new TechiqueShopDatabase())
            {
                return context.Assemblys
                    .Include(rec => rec.AssemblyComponents)
                    .ThenInclude(rec => rec.Component)
                    .ToList()
                    .Select(rec => new AssemblyViewModel
                    {
                        Id = rec.Id,
                        AssemblyName = rec.AssemblyName,
                        Price = rec.Price,
                        AssemblyComponents = rec.AssemblyComponents
                            .ToDictionary(recAssemblyComponents => recAssemblyComponents.ComponentId,
                            recAssemblyComponents => (recAssemblyComponents.Component?.ComponentName,
                            recAssemblyComponents.Count))
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
                    .Include(rec => rec.AssemblyComponents)
                    .ThenInclude(rec => rec.Component)
                    .Where(rec => rec.AssemblyName.Contains(model.AssemblyName))
                    .ToList()
                    .Select(rec => new AssemblyViewModel
                    {
                        Id = rec.Id,
                        AssemblyName = rec.AssemblyName,
                        Price = rec.Price,
                        AssemblyComponents = rec.AssemblyComponents
                            .ToDictionary(recAssemblyComponents => recAssemblyComponents.ComponentId,
                            recAssemblyComponents => (recAssemblyComponents.Component?.ComponentName,
                            recAssemblyComponents.Count))
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
                var assembly = context.Assemblys
                    .Include(rec => rec.AssemblyComponents)
                    .ThenInclude(rec => rec.Component)
                    .FirstOrDefault(rec => rec.AssemblyName == model.AssemblyName ||
                    rec.Id == model.Id);

                return assembly != null ?
                    new AssemblyViewModel
                    {
                        Id = assembly.Id,
                        AssemblyName = assembly.AssemblyName,
                        Price = assembly.Price,
                        AssemblyComponents = assembly.AssemblyComponents
                            .ToDictionary(recAssemblyComponent => recAssemblyComponent.ComponentId,
                            recAssemblyComponent => (recAssemblyComponent.Component?.ComponentName,
                            recAssemblyComponent.Count))
                    } :
                    null;
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
        public void Update(AssemblyBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var assembly = context.Assemblys.FirstOrDefault(rec => rec.Id == model.Id);

                        if (assembly == null)
                        {
                            throw new Exception("Подарок не найден");
                        }

                        CreateModel(model, assembly, context);
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
        public void Delete(AssemblyBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                var Component = context.Assemblys.FirstOrDefault(rec => rec.Id == model.Id);

                if (Component == null)
                {
                    throw new Exception("Материал не найден");
                }

                context.Assemblys.Remove(Component);
                context.SaveChanges();
            }
        }
        private Assembly CreateModel(AssemblyBindingModel model, Assembly assembly, TechiqueShopDatabase context)
        {
            assembly.AssemblyName = model.AssemblyName;
            assembly.Price = model.Price;
            if (assembly.Id == 0)
            {
                context.Assemblys.Add(assembly);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var assemblyComponent = context.AssemblyComponents
                    .Where(rec => rec.AssemblyId == model.Id.Value)
                    .ToList();

                context.AssemblyComponents.RemoveRange(assemblyComponent
                    .Where(rec => !model.AssemblyComponents.ContainsKey(rec.AssemblyId))
                    .ToList());
                context.SaveChanges();

                foreach (var updateComponent in assemblyComponent)
                {
                    updateComponent.Count = model.AssemblyComponents[updateComponent.ComponentId].Item2;
                    model.AssemblyComponents.Remove(updateComponent.AssemblyId);
                }
                context.SaveChanges();
            }
            foreach (var assemblyComponent in model.AssemblyComponents)
            {
                context.AssemblyComponents.Add(new AssemblyComponent
                {
                    AssemblyId = assembly.Id,
                    ComponentId = assemblyComponent.Key,
                    Count = assemblyComponent.Value.Item2
                });
                context.SaveChanges();
            }
            return assembly;
        }
    }
}