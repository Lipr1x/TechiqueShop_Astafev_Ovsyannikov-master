using System;
using System.Collections.Generic;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;

namespace TechiqueShopBusinessLogic.BusinessLogics
{
    public class GetTechniqueLogic
    {
        private readonly IGetTechniqueStorage _getTechniqueStorage;
        public GetTechniqueLogic(IGetTechniqueStorage getTechniqueStorage)
        {
            _getTechniqueStorage = getTechniqueStorage;
        }
        public List<GetTechniqueViewModel> Read(GetTechniqueBindingModel model)
        {
            if (model == null)
            {
                return _getTechniqueStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<GetTechniqueViewModel> { _getTechniqueStorage.GetElement(model) };
            }
            return _getTechniqueStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(GetTechniqueBindingModel model)
        {
            var element = _getTechniqueStorage.GetElement(new GetTechniqueBindingModel
            {
                ArrivalTime = model.ArrivalTime
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Телефон или Email уже был зарегестрирован!");
            }
            if (model.Id.HasValue)
            {
                _getTechniqueStorage.Update(model);
            }
            else
            {
                _getTechniqueStorage.Insert(model);
            }
        }

        public void Delete(GetTechniqueBindingModel model)
        {
            var element = _getTechniqueStorage.GetElement(new GetTechniqueBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Поставщик не найден");
            }
            _getTechniqueStorage.Delete(model);
        }
    }
}
