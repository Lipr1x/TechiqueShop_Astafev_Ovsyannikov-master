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
        private readonly IOrderStorage _orderStorage;
        public AssemblyLogic(IAssemblyStorage assemblyStorage, IOrderStorage orderStorage)
        {
            _assemblyStorage = assemblyStorage;
            _orderStorage = orderStorage;
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
                throw new Exception("Такая сборка уже есть!");
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

        public void Linking(AssemblyLinkingBindingModel model)
        {
            var assembly = _assemblyStorage.GetElement(new AssemblyBindingModel
            {
                Id = model.AssemblyId
            });

            var order = _orderStorage.GetElement(new OrderBindingModel
            {
                Id = model.OrderId
            });

            if (assembly == null)
            {
                throw new Exception("Не найдена сборка");
            }

            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }

            if (assembly.OrderId.HasValue)
            {
                throw new Exception("Данная cборка уже привязана");
            }

            _assemblyStorage.Update(new AssemblyBindingModel
            {
                Id = assembly.Id,
                Price = assembly.Price,
                AssemblyName = assembly.AssemblyName,
                AssemblyComponents = assembly.AssemblyComponents,
                ProviderId = assembly.ProviderId,
                OrderId = model.OrderId
            });
        }
    }
}
