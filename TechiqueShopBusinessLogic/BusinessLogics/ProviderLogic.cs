using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;

namespace TechniqueShopBusinessLogic.BusinessLogics
{
    public class ProviderLogic
    {
        private readonly IProviderStorage _providerStorage;
        public ProviderLogic(IProviderStorage providerStorage)
        {
            _providerStorage = providerStorage;
        }
        public List<ProviderViewModel> Read(ProviderBindingModel model)
        {
            if (model == null)
            {
                return _providerStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<ProviderViewModel> { _providerStorage.GetElement(model) };
            }
            return _providerStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(ProviderBindingModel model)
        {
            var element = _providerStorage.GetElement(new ProviderBindingModel
            {
                ProviderName = model.ProviderName,
                ProviderSurname = model.ProviderSurname,
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
                _providerStorage.Update(model);
            }
            else
            {
                _providerStorage.Insert(model);
            }
        }

        public void Delete(ProviderBindingModel model)
        {
            var element = _providerStorage.GetElement(new ProviderBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Поставщик не найден");
            }
            _providerStorage.Delete(model);
        }
    }
}
