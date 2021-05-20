using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;
using TechiqueShopDatabaseImplement.Models;

namespace TechiqueShopDatabaseImplement.Implements
{
    public class GetTechniqueStorage: IGetTechniqueStorage
    {
        public List<GetTechniqueViewModel> GetFullList()
        {
            using (var context = new TechiqueShopDatabase())
            {
                return context.GetTechniquies
                    .Include(rec => rec.SupplyGetTechniques)
                    .ThenInclude(rec => rec.Supply)
                    .ToList()
                    .Select(rec => new GetTechniqueViewModel
                    {
                        //Id = rec.Id,
                        //GetTechniqueName = rec.GetTechniqueName,
                        //ArrivalTime = rec.ArrivalTime,
                        //SupplyGetTechniques = rec.SupplyGetTechniques
                        //    .ToDictionary(recSupplyGetTechniques => recSupplyGetTechniques.SupplyId,
                        //    recSupplyGetTechniques => (recSupplyGetTechniques.Supply?.,
                        //    recSupplyGetTechniques.Count))
                    })
                    .ToList();
            }
        }
        public List<GetTechniqueViewModel> GetFilteredList(GetTechniqueBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new TechiqueShopDatabase())
            {
                return context.GetTechniquies
                    .Include(rec => rec.SupplyGetTechniques)
                    .ThenInclude(rec => rec.Supply)
                    .Where(rec => rec.GetTechniqueName.Contains(model.GetTechniqueName))
                    .ToList()
                    .Select(rec => new GetTechniqueViewModel
                    {
                        //Id = rec.Id,
                        //GetTechniqueName = rec.GetTechniqueName,
                        //ArrivalTime = rec.ArrivalTime,
                        //SupplyGetTechniques = rec.SupplyGetTechniques
                        //    .ToDictionary(recSupplyGetTechniques => recSupplyGetTechniques.SupplyId,
                        //    recSupplyGetTechniques => (recSupplyGetTechniques.Supply?.SupplyName,
                        //    recSupplyGetTechniques.Count))
                    })
                    .ToList();
            }
        }
        public GetTechniqueViewModel GetElement(GetTechniqueBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new TechiqueShopDatabase())
            {
                var getTechnique = context.GetTechniquies
                    .Include(rec => rec.SupplyGetTechniques)
                    .ThenInclude(rec => rec.Supply)
                    .FirstOrDefault(rec => rec.GetTechniqueName == model.GetTechniqueName ||
                    rec.Id == model.Id);

                return getTechnique != null ?
                    new GetTechniqueViewModel
                    {
                        //Id = getTechnique.Id,
                        //GetTechniqueName = getTechnique.GetTechniqueName,
                        //ArrivalTime = getTechnique.ArrivalTime,
                        //SupplyGetTechniques = getTechnique.SupplyGetTechniques
                        //    .ToDictionary(recSupplyGetTechniques => recSupplyGetTechniques.SupplyId,
                        //    recSupplyGetTechniques => (recSupplyGetTechniques.Supply?.SupplyName,
                        //    recSupplyGetTechniques.Count))
                    } :
                    null;
            }
        }
        public void Insert(GetTechniqueBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new GetTechnique(), context);
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Update(GetTechniqueBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var getTechnique = context.GetTechniquies.FirstOrDefault(rec => rec.Id == model.Id);

                        if (getTechnique == null)
                        {
                            throw new Exception("Поставка не найдена");
                        }

                        CreateModel(model, getTechnique, context);
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Delete(GetTechniqueBindingModel model)
        {
            using (var context = new TechiqueShopDatabase())
            {
                var Supply = context.GetTechniquies.FirstOrDefault(rec => rec.Id == model.Id);

                if (Supply == null)
                {
                    throw new Exception("Материал не найден");
                }

                context.GetTechniquies.Remove(Supply);
                context.SaveChanges();
            }
        }
        private GetTechnique CreateModel(GetTechniqueBindingModel model, GetTechnique getTechnique, TechiqueShopDatabase context)
        {
            getTechnique.GetTechniqueName = model.GetTechniqueName;
            getTechnique.ArrivalTime = model.ArrivalTime;
            if (getTechnique.Id == 0)
            {
                context.GetTechniquies.Add(getTechnique);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var supplyGetTechnique = context.SupplyGetTechniques
                    .Where(rec => rec.Id == model.Id.Value)
                    .ToList();

                context.SupplyGetTechniques.RemoveRange(supplyGetTechnique
                    .Where(rec => !model.SupplyGetTechniques.ContainsKey(rec.Id))
                    .ToList());
                context.SaveChanges();

                foreach (var updateSupply in supplyGetTechnique)
                {
                    updateSupply.Count = model.SupplyGetTechniques[updateSupply.SupplyId].Item2;
                    model.SupplyGetTechniques.Remove(updateSupply.Id);
                }
                context.SaveChanges();
            }
            foreach (var supplyGetTechnique in model.SupplyGetTechniques)
            {
                context.SupplyGetTechniques.Add(new SupplyGetTechnique
                {
                    Id = getTechnique.Id,
                    SupplyId = supplyGetTechnique.Key,
                    Count = supplyGetTechnique.Value.Item2
                });
                context.SaveChanges();
            }
            return getTechnique;
        }
    }
}
