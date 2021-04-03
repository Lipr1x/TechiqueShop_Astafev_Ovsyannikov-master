using System;
using System.Collections.Generic;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;

namespace TechiqueShopBusinessLogic.BusinessLogics
{
    public class SupplyLogic
    {
        private readonly ISupplyStorage _supplyStorage;
        public SupplyLogic(ISupplyStorage supplyStorage)
        {
            _supplyStorage = supplyStorage;
        }
        public List<SupplyViewModel> Read(SupplyBindingModel model)
        {
            if (model == null)
            {
                return _supplyStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<SupplyViewModel> { _supplyStorage.GetElement(model) };
            }
            return _supplyStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(SupplyBindingModel model)
        {
            var element = _supplyStorage.GetElement(new SupplyBindingModel
            {
                SupplyName = model.SupplyName,
                Date = model.Date
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Такая поставка уже есть!");
            }
            if (model.Id.HasValue)
            {
                _supplyStorage.Update(model);
            }
            else
            {
                _supplyStorage.Insert(model);
            }
        }

        public void Delete(SupplyBindingModel model)
        {
            var element = _supplyStorage.GetElement(new SupplyBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Поставка не найдена");
            }
            _supplyStorage.Delete(model);
        }
    }
}
