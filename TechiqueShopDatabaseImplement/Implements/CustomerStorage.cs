using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;
using TechiqueShopDatabaseImplement.Models;

namespace TechiqueShopDatabaseImplement.Implements
{
    public class CustomerStorage : ICustomerStorage
    {
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
                .Where(rec => rec.CustomerName.Contains(model.CustomerName))
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
                .FirstOrDefault(rec => rec.CustomerName == model.CustomerName ||
               rec.Id == model.Id);
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