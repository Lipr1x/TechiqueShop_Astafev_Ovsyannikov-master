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
    public class ProviderStorage : IProviderStorage
    {
        private readonly int _passwordMaxLength = 30;
        private readonly int _passwordMinLength = 2;
        private readonly int _emailMaxLength = 50;
        private readonly int _ProviderNameMaxLength = 30;
        private readonly int _ProviderSurnameMaxLength = 50;
        private readonly int _PatronymicMaxLength = 30;
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
                .Where(rec => rec.Telephone == model.Telephone && rec.Password == model.Password)
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
                .FirstOrDefault(rec => rec.Telephone == model.Telephone ||
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
            //if (model.ProviderName.Length > _ProviderNameMaxLength || model.ProviderSurname.Length > _ProviderSurnameMaxLength || model.Patronymic.Length > _PatronymicMaxLength)
            //{
            //    throw new Exception($"Имя/Фамилия/отчество должно быть меньше {_ProviderNameMaxLength}/{_ProviderSurnameMaxLength}/{_PatronymicMaxLength} ");
            //}
            //if (model.Password.Length > _passwordMaxLength || model.Password.Length < _passwordMinLength)
            //{
            //    throw new Exception($"Пароль должен быть длиной от {_passwordMinLength} до { _passwordMaxLength } ");
            //}
            //if (model.Email.Length > _emailMaxLength || !Regex.IsMatch(model.Email, @"^[A-Za-z0-9]+(?:[._%+-])?[A-Za-z0-9._-]+[A-Za-z0-9]@[A-Za-z0-9]+(?:[.-])?[A-Za-z0-9._-]+\.[A-Za-z]{2,6}$"))
            //{
            //    throw new Exception($"Мэйл должен быть длиной до { _emailMaxLength } ");
            //}
            //if (!Regex.IsMatch(model.Telephone, @"^\d{2,11}$"))
            //{
            //    throw new Exception($"Телефон должен быть длиной до 11 цифр");
            //}

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