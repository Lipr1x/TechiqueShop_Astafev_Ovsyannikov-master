using System;
using System.Collections.Generic;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;

namespace TechiqueShopBusinessLogic.BusinessLogics
{
    public class ComponentLogic
    {
        private readonly IComponentStorage _componentStorage;
        public ComponentLogic(IComponentStorage componentStorage)
        {
            _componentStorage = componentStorage;
        }
        public List<ComponentViewModel> Read(ComponentBindingModel model)
        {
            if (model == null)
            {
                return _componentStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<ComponentViewModel> { _componentStorage.GetElement(model) };
            }
            return _componentStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(ComponentBindingModel model)
        {
            var element = _componentStorage.GetElement(new ComponentBindingModel
            {
                ComponentName = model.ComponentName,
                Price = model.Price
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Телефон или Email уже был зарегестрирован!");
            }
            if (model.Id.HasValue)
            {
                _componentStorage.Update(model);
            }
            else
            {
                _componentStorage.Insert(model);
            }
        }

        public void Delete(ComponentBindingModel model)
        {
            var element = _componentStorage.GetElement(new ComponentBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Поставщик не найден");
            }
            _componentStorage.Delete(model);
        }
    }
}
