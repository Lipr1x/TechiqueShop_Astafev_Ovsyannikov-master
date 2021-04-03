using System;
using System.Collections.Generic;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;

namespace TechiqueShopBusinessLogic.BusinessLogics
{
    public class AssemblyLogic
    {
        private readonly IAssemblyStorage _assemblyStorage;
        public AssemblyLogic(IAssemblyStorage assemblyStorage)
        {
            _assemblyStorage = assemblyStorage;
        }
        public List<AssemblyViewModel> Read(AssemblyBindingModel model)
        {
            if (model == null)
            {
                return _assemblyStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<AssemblyViewModel> { _assemblyStorage.GetElement(model) };
            }
            return _assemblyStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(AssemblyBindingModel model)
        {
            var element = _assemblyStorage.GetElement(new AssemblyBindingModel
            {
                AssemblyName = model.AssemblyName,
                Price = model.Price
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Телефон или Email уже был зарегестрирован!");
            }
            if (model.Id.HasValue)
            {
                _assemblyStorage.Update(model);
            }
            else
            {
                _assemblyStorage.Insert(model);
            }
        }

        public void Delete(AssemblyBindingModel model)
        {
            var element = _assemblyStorage.GetElement(new AssemblyBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Поставщик не найден");
            }
            _assemblyStorage.Delete(model);
        }
    }
}
