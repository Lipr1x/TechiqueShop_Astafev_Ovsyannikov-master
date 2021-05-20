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
    public class ComponentStorage : IComponentStorage
    {
        private readonly int _ComponentNameMaxLength = 50;
        public List<ComponentViewModel> GetFullList()
        {
            using (var context = new TechiqueShopDatabase())
            {
                return context.Components
                .Include(rec => rec.Provider)
                .Select(rec => new ComponentViewModel
                {
                    Id = rec.Id,
                    ComponentName = rec.ComponentName,
                    Price = rec.Price,
                    ProviderId = rec.ProviderId
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
                .Include(rec => rec.Provider)
                .Where(rec => rec.ProviderId == model.ProviderId || rec.ComponentName.Contains(model.ComponentName))
               .Select(rec => new ComponentViewModel
               {
                   Id = rec.Id,
                   ComponentName = rec.ComponentName,
                   Price = rec.Price,
                   ProviderId = rec.ProviderId
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
                var tmp = model.ProviderId;
                var component = context.Components
                .Include(rec => rec.Provider)
                .FirstOrDefault(rec => rec.ComponentName == model.ComponentName ||
               rec.Id == model.Id);
                return component != null ?
                new ComponentViewModel
                {
                    Id = component.Id,
                    ComponentName = component.ComponentName,
                    Price = component.Price,
                    ProviderId = component.ProviderId
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
                Component element = context.Components.FirstOrDefault(rec => rec.Id == model.Id);
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
            if (model.ComponentName.Length > _ComponentNameMaxLength)
            {
                throw new Exception($"Название комплектующей должно быть длиной до { _ComponentNameMaxLength } ");
            }
            if (!Regex.IsMatch(model.Price.ToString(), @"^\d{1,10}$"))
            {
                throw new Exception($"Нельзя назначить настолько высокую цену за комплектующую, и должна состоять из цифр");
            }
            component.ComponentName = model.ComponentName;
            component.Price = model.Price;
            component.ProviderId = (int)model.ProviderId;
            return component;
        }
    }
}
