using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.FileModels;
using TechiqueShopBusinessLogic.Interfaces;
using TechiqueShopBusinessLogic.ViewModels;

namespace TechiqueShopBusinessLogic.BusinessLogics
{
    public class GetListLogic
    {
        private readonly IComponentStorage _componentStorage;//medication
        private readonly IAssemblyStorage _assemblyStorage;//medicine
        public GetListLogic(IComponentStorage componentStorage, IAssemblyStorage assemblyStorage)
        {
            _componentStorage = componentStorage;
            _assemblyStorage = assemblyStorage;
        }
        /// <summary>
        /// Получение списка компонент с указанием, в каких изделиях используются
        /// </summary>
        /// <returns></returns>
        public List<GetListComponentAssemblyViewModel> GetComponentAssembly(GetListBindingModel model)
        {
            var components = _componentStorage.GetFullList();
            var assemblies = _assemblyStorage.GetFullList();
            var list = new List<GetListComponentAssemblyViewModel>();
            foreach (var assebmly in assemblies)
            {
                bool have_components = true;
                foreach (String component in model.Components)
                {
                    //if (assebmly.AssemblyComponents.Values.FirstOrDefault(rec => rec == component) == null)
                    //{
                    //    have_components = false;
                    //}
                }
                if (!have_components)
                    continue;

                var record = new GetListComponentAssemblyViewModel
                {
                    AssemblyName = assebmly.AssemblyName,
                    Components = new List<string>()
                };
                foreach (var component in components)
                {
                    if (assebmly.AssemblyComponents.ContainsKey((int)component.Id))
                    {
                        record.Components.Add(component.ComponentName);
                    }
                }
                list.Add(record);
            }
            return list;
        }
        /// <summary>
        /// Сохранение компонент в файл-Word
        /// </summary>
        /// <param name="model"></param>
        public WordExcelInfo SaveComponentsToWordFile(GetListBindingModel model)
        {
            var file = new WordExcelInfo
            {
                FileName = "filename",
                Title = "something",
                ComponentAssembly = GetComponentAssembly(model),
                ChoosedComponents = model.Components
            };
            return file;
        }
        /// <summary>
        /// Сохранение компонент с указаеним продуктов в файл-Excel
        /// </summary>
        /// <param name="model"></param>
        public void SaveDishComponentToExcelFile(GetListBindingModel model)
        {
            SaveToExcel.CreateDocument(new WordExcelInfo
            {
                FileName = model.FileName,
                Title = "Список техники",
                ComponentAssembly = GetComponentAssembly(model),
                ChoosedComponents = model.Components
            });
        }
    }
}
