using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;
using TechiqueShopDatabaseImplement.Models;

namespace TechiqueShopDatabaseImplement.Implements
{
    public class ProviderStorage : IProviderStorage
    {
        public List<ProviderViewModel> GetFullList()
        {
            using (var context = new TechiqueShopDatabase())
            {
                return context.Providers
                .Select(rec => new ProviderViewModel
                {
                    Id = rec.Id,
                    ProviderName = rec.ProviderName,
                    ProviderSurname = rec.ProviderSurname,
                    Patronymic = rec.Patronymic,
                    Telephone = rec.Telephone,
                    Email = rec.Email,
                    Password = rec.Password,
                    UserType = rec.UserType
                })
               .ToList();
            }
        }
        public List<ProviderViewModel> GetFilteredList(ProviderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new TechiqueShopDatabase())
            {
                return context.Providers
                .Where(rec => rec.ProviderName.Contains(model.ProviderName))
               .Select(rec => new ProviderViewModel
               {
                   Id = rec.Id,
                   ProviderName = rec.ProviderName,
                   ProviderSurname = rec.ProviderSurname,
                   Patronymic = rec.Patronymic,
                   Telephone = rec.Telephone,
                   Email = rec.Email,
                   Password = rec.Password,
                   UserType = rec.UserType
               })
                .ToList();
            }
        }
        public ProviderViewModel GetElement(ProviderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new TechiqueShopDatabase())
            {
                var provider = context.Providers
                .FirstOrDefault(rec => rec.ProviderName == model.ProviderName ||
               rec.Id == model.Id);
                return provider != null ?
                new ProviderViewModel
                {
                    Id = provider.Id,
                    ProviderName = provider.ProviderName,
                    ProviderSurname = provider.ProviderSurname,
                    Patronymic = provider.Patronymic,
                    Telephone = provider.Telephone,
                    Email = provider.Email,
                    Password = provider.Password,
                    UserType = provider.UserType
                } :
               null;
            }
        }
        public void Insert(ProviderBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                context.Providers.Add(CreateModel(model, new Provider()));
                context.SaveChanges();
            }
        }
        public void Update(ProviderBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                var element = context.Providers.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element == null)
                {
                    throw new Exception("Провайдер не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }
        public void Delete(ProviderBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                Provider element = context.Providers.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Providers.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Провайдер не найден");
                }
            }
        }
        private Provider CreateModel(ProviderBindingModel model, Provider provider)
        {
            provider.ProviderName = model.ProviderName;
            provider.ProviderSurname = model.ProviderSurname;
            provider.Patronymic = model.Patronymic;
            provider.Telephone = model.Telephone;
            provider.Email = model.Email;
            provider.Password = model.Password;
            provider.UserType = model.UserType;

            return provider;
        }
    }
}