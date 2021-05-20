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
        private readonly IComponentStorage _componentStorage;
        public SupplyLogic(ISupplyStorage supplyStorage, IComponentStorage componentStorage)
        {
            _supplyStorage = supplyStorage;
            _componentStorage = componentStorage;
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
        public void Linking(SupplyBindingModel model)
        {
            var supply = _supplyStorage.GetElement(new SupplyBindingModel
            {
                Id = model.Id
            });

            var components = _componentStorage.GetElement(new ComponentBindingModel
            {
                Id = model.Id
            });

            if (components == null)
            {
                throw new Exception("Не найдены компоненты");
            }

            _supplyStorage.Update(new SupplyBindingModel
            {
                Id = supply.Id,
                Date = supply.Date,
                SupplyOrders = supply.SupplyOrders,
                CustomerId = supply.CustomerId,
                SupplyComponents = model.SupplyComponents,
                TotalCost = supply.TotalCost
            });
        }
    }
}
