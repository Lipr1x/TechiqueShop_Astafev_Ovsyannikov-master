using System;
using System.Collections.Generic;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;

namespace TechiqueShopBusinessLogic.Interfaces
{
    public interface IDeliveryStorage
    {
        List<DeliveryViewModel> GetFullList();
        List<DeliveryViewModel> GetFilteredList(DeliveryBindingModel model);
        DeliveryViewModel GetElement(DeliveryBindingModel model);
        void Insert(DeliveryBindingModel model);
        void Update(DeliveryBindingModel model);
        void Delete(DeliveryBindingModel model);
    }
}
