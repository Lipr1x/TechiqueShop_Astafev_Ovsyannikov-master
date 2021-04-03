using System;
using System.Collections.Generic;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;

namespace TechiqueShopBusinessLogic.Interfaces
{
    public interface IAssemblyStorage
    {
        List<AssemblyViewModel> GetFullList();
        List<AssemblyViewModel> GetFilteredList(AssemblyBindingModel model);
        AssemblyViewModel GetElement(AssemblyBindingModel model);
        void Insert(AssemblyBindingModel model);
        void Update(AssemblyBindingModel model);
        void Delete(AssemblyBindingModel model);
    }
}
