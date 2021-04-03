using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechiqueShopBusinessLogic.BindingModels;

namespace TechiqueShopBusinessLogic.Interfaces
{
    public interface IProviderStorage
    {
        List<ProviderViewModel> GetFullList();
        List<ProviderViewModel> GetFilteredList(ProviderBindingModel model);
        ProviderViewModel GetElement(ProviderBindingModel model);
        void Insert(ProviderBindingModel model);
        void Update(ProviderBindingModel model);
        void Delete(ProviderBindingModel model);
    }
}
