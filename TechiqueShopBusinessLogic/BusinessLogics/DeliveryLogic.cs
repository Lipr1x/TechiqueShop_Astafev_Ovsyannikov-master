using System;
using System.Collections.Generic;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;

namespace TechiqueShopBusinessLogic.BusinessLogics
{
    public class DeliveryLogic
    {
        private readonly IDeliveryStorage _deliveryStorage;
        public DeliveryLogic(IDeliveryStorage deliveryStorage)
        {
            _deliveryStorage = deliveryStorage;
        }
        public List<DeliveryViewModel> Read(DeliveryBindingModel model)
        {
            if (model == null)
            {
                return _deliveryStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<DeliveryViewModel> { _deliveryStorage.GetElement(model) };
            }
            return _deliveryStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(DeliveryBindingModel model)
        {
            var element = _deliveryStorage.GetElement(new DeliveryBindingModel
            {
                Date = model.Date,
                Price = model.Price
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Телефон или Email уже был зарегестрирован!");
            }
            if (model.Id.HasValue)
            {
                _deliveryStorage.Update(model);
            }
            else
            {
                _deliveryStorage.Insert(model);
            }
        }

        public void Delete(DeliveryBindingModel model)
        {
            var element = _deliveryStorage.GetElement(new DeliveryBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Поставщик не найден");
            }
            _deliveryStorage.Delete(model);
        }
    }
}
