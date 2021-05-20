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
    public class CustomerStorage : ICustomerStorage
    {
        private readonly int _passwordMaxLength = 30;
        private readonly int _passwordMinLength = 2;
        private readonly int _emailMaxLength = 50;
        private readonly int _CustomerNameMaxLength = 30;
        private readonly int _CustomerSurnameMaxLength = 50;
        private readonly int _PatronymicMaxLength = 30;
        public List<CustomerViewModel> GetFullList()
        {
            using (var context = new TechiqueShopDatabase())
            {
                return context.Customers
                .Select(rec => new CustomerViewModel
                {
                    Id = rec.Id,
                    CustomerName = rec.CustomerName,
                    CustomerSurname = rec.CustomerSurname,
                    Patronymic = rec.Patronymic,
                    Telephone = rec.Telephone,
                    Email = rec.Email,
                    Password = rec.Password,
                    UserType = rec.UserType
                })
               .ToList();
            }
        }
        public List<CustomerViewModel> GetFilteredList(CustomerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new TechiqueShopDatabase())
            {
                return context.Customers
                 .Where(rec => rec.Telephone == model.Telephone && rec.Password == model.Password)
               .Select(rec => new CustomerViewModel
               {
                   Id = rec.Id,
                   CustomerName = rec.CustomerName,
                   CustomerSurname = rec.CustomerSurname,
                   Patronymic = rec.Patronymic,
                   Telephone = rec.Telephone,
                   Email = rec.Email,
                   Password = rec.Password,
                   UserType = rec.UserType
               })
                .ToList();
            }
        }
        public CustomerViewModel GetElement(CustomerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new TechiqueShopDatabase())
            {
                var customer = context.Customers
                .FirstOrDefault(rec => rec.Telephone == model.Telephone || rec.Id == model.Id);
                return customer != null ?
                new CustomerViewModel
                {
                    Id = customer.Id,
                    CustomerName = customer.CustomerName,
                    CustomerSurname = customer.CustomerSurname,
                    Patronymic = customer.Patronymic,
                    Telephone = customer.Telephone,
                    Email = customer.Email,
                    Password = customer.Password,
                    UserType = customer.UserType
                } :
               null;
            }
        }
        public void Insert(CustomerBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                context.Customers.Add(CreateModel(model, new Customer()));
                context.SaveChanges();
            }
        }
        public void Update(CustomerBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                var element = context.Customers.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element == null)
                {
                    throw new Exception("Заказчик не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }
        public void Delete(CustomerBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                Customer element = context.Customers.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Customers.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Заказчик не найден");
                }
            }
        }
        private Customer CreateModel(CustomerBindingModel model, Customer customer)
        {
            //if (model.CustomerName.Length > _CustomerNameMaxLength || model.CustomerSurname.Length > _CustomerSurnameMaxLength || model.Patronymic.Length > _PatronymicMaxLength)
            //{
            //    throw new Exception($"Имя/Фамилия/отчество должно быть меньше {_CustomerNameMaxLength}/{_CustomerSurnameMaxLength}/{_PatronymicMaxLength} ");
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

            customer.CustomerName = model.CustomerName;
            customer.CustomerSurname = model.CustomerSurname;
            customer.Patronymic = model.Patronymic;
            customer.Telephone = model.Telephone;
            customer.Email = model.Email;
            customer.Password = model.Password;
            customer.UserType = model.UserType;

            return customer;
        }
    }
}