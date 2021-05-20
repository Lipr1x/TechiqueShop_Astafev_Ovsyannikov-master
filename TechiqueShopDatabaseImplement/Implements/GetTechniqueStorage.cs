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
                .Include(rec => rec.Customer)
                //.Include(rec => rec.Visit)
                .Include(rec => rec.SupplyGetTechniques)
                .ThenInclude(rec => rec.Supply)
                .ToList()
                .Select(rec => new GetTechniqueViewModel
                {
                    Id = rec.Id,
                    ArrivalTime = rec.ArrivalTime,
                    SupplyGetTechniques = rec.SupplyGetTechniques.ToDictionary(recDC => recDC.SupplyId, recDC => ("($recDC.Supply?.Id)", recDC.Count)),
                    CustomerId = rec.CustomerId,
                    //VisitId = rec.VisitId
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
                .Include(rec => rec.Customer)
                //.Include(rec => rec.Visit)
                .Include(rec => rec.SupplyGetTechniques)
                .ThenInclude(rec => rec.Supply)
                .Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue && rec.CustomerId == model.CustomerId) ||
                (model.DateFrom.HasValue && model.DateTo.HasValue && rec.CustomerId == model.CustomerId && rec.ArrivalTime.Date >= model.DateFrom.Value.Date && rec.ArrivalTime.Date <= model.DateTo.Value.Date))
                .ToList()
                .Select(rec => new GetTechniqueViewModel
                {
                    Id = rec.Id,
                    ArrivalTime = rec.ArrivalTime,
                    SupplyGetTechniques = rec.SupplyGetTechniques.ToDictionary(recDC => recDC.SupplyId, recDC => ("($recDC.Supply?.Id)", recDC.Count)),
                    CustomerId = rec.CustomerId,
                    //VisitId = rec.VisitId
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
                GetTechnique distribution = context.GetTechniquies
                .Include(rec => rec.Customer)
                //.Include(rec => rec.Visit)
                .Include(rec => rec.SupplyGetTechniques)
                .ThenInclude(rec => rec.Supply)
                .FirstOrDefault(rec => rec.ArrivalTime == model.ArrivalTime || rec.Id == model.Id);
                return distribution != null ? new GetTechniqueViewModel
                {
                    Id = distribution.Id,
                    ArrivalTime = distribution.ArrivalTime,
                    SupplyGetTechniques = distribution.SupplyGetTechniques.ToDictionary(recDC => recDC.SupplyId, recDC => ("($recDC.Supply?.Id)", recDC.Count)),
                    CustomerId = distribution.CustomerId,
                    //VisitId = distribution.VisitId
                } : null;
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
                        GetTechnique element = context.GetTechniquies.FirstOrDefault(rec => rec.Id == model.Id);

                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }

                        CreateModel(model, element, context);
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
                GetTechnique element = context.GetTechniquies.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.GetTechniquies.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private GetTechnique CreateModel(GetTechniqueBindingModel model, GetTechnique distribution, TechiqueShopDatabase context)
        {
            distribution.ArrivalTime = model.ArrivalTime;
            distribution.CustomerId = (int)model.CustomerId;
            //distribution.VisitId = model.VisitId;

            if (distribution.Id == 0)
            {
                context.GetTechniquies.Add(distribution);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var distributionCosmetics = context.SupplyGetTechniques.Where(rec => rec.GetTechniqueId == model.Id.Value).ToList();
                context.SupplyGetTechniques.RemoveRange(distributionCosmetics.Where(rec => !model.SupplyGetTechniques.ContainsKey(rec.SupplyId)).ToList());
                context.SaveChanges();

                foreach (var updateCosmetic in distributionCosmetics)
                {
                    updateCosmetic.Count = model.SupplyGetTechniques[updateCosmetic.SupplyId].Item2;
                    model.SupplyGetTechniques.Remove(updateCosmetic.SupplyId);
                }
                context.SaveChanges();
            }
            foreach (var dc in model.SupplyGetTechniques)
            {
                context.SupplyGetTechniques.Add(new SupplyGetTechnique
                {
                    GetTechniqueId = distribution.Id,
                    SupplyId = dc.Key,
                    Count = dc.Value.Item2
                });
                context.SaveChanges();
            }
            return distribution;
        }
    }
}
