using System;
using System.Collections.Generic;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;

namespace TechiqueShopBusinessLogic.BusinessLogics
{
    public class CustomerLogic
    {
        private readonly ICustomerStorage _customerStorage;
        public CustomerLogic(ICustomerStorage customerStorage)
        {
            _customerStorage = customerStorage;
        }
        public List<CustomerViewModel> Read(CustomerBindingModel model)
        {
            if (model == null)
            {
                return _customerStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<CustomerViewModel> { _customerStorage.GetElement(model) };
            }
            return _customerStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(CustomerBindingModel model)
        {
            var element = _customerStorage.GetElement(new CustomerBindingModel
            {
                CustomerName = model.CustomerName,
                CustomerSurname = model.CustomerSurname,
                Patronymic = model.Patronymic,
                Telephone = model.Telephone,
                Email = model.Email,
                Password = model.Password,
                UserType = model.UserType
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Телефон или Email уже был зарегестрирован!");
            }
            if (model.Id.HasValue)
            {
                _customerStorage.Update(model);
            }
            else
            {
                _customerStorage.Insert(model);
            }
        }

        public void Delete(CustomerBindingModel model)
        {
            var element = _customerStorage.GetElement(new CustomerBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Поставщик не найден");
            }
            _customerStorage.Delete(model);
        }
    }
}
