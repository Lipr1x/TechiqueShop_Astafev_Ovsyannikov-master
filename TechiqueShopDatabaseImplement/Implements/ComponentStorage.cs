using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;
using TechiqueShopDatabaseImplement.Models;

namespace TechiqueShopDatabaseImplement.Implements
{
    public class ComponentStorage : IComponentStorage
    {
        public List<ComponentViewModel> GetFullList()
        {
            using (var context = new TechiqueShopDatabase())
            {
                return context.Components
                .Select(rec => new ComponentViewModel
                {
                    Id = rec.Id,
                    ComponentName = rec.ComponentName,
                    Price = rec.Price
                })
               .ToList();
            }
        }
        public List<ComponentViewModel> GetFilteredList(ComponentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new TechiqueShopDatabase())
            {
                return context.Components
                .Where(rec => rec.ComponentName.Contains(model.ComponentName))
               .Select(rec => new ComponentViewModel
               {
                   Id = rec.Id,
                   ComponentName = rec.ComponentName,
                   Price = rec.Price
               })
                .ToList();
            }
        }
        public ComponentViewModel GetElement(ComponentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new TechiqueShopDatabase())
            {
                var component = context.Components
                .FirstOrDefault(rec => rec.ComponentName == model.ComponentName ||
               rec.Id == model.Id);
                return component != null ?
                new ComponentViewModel
                {
                    Id = component.Id,
                    ComponentName = component.ComponentName,
                    Price = component.Price
                } :
               null;
            }
        }
        public void Insert(ComponentBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                context.Components.Add(CreateModel(model, new Component()));
                context.SaveChanges();
            }
        }
        public void Update(ComponentBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                var element = context.Components.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }
        public void Delete(ComponentBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                Component element = context.Components.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Components.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        private Component CreateModel(ComponentBindingModel model, Component component)
        {
            component.ComponentName = model.ComponentName;
            component.Price = model.Price;
            component.ProviderId = model.UserId;
            return component;
        }
    }
}
