using System;
using System.Collections.Generic;
using System.Text;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.Interfaces;

namespace TechiqueShopBusinessLogic.BusinessLogics
{
    public class ReportOrderDateLogic
    {
        private readonly IOrderStorage _orderStorage;
        private readonly ISupplyStorage _supplyStorage;
        private readonly IGetTechniqueStorage _getTechniqueStorage;

        public ReportOrderDateLogic(IOrderStorage orderStorage, ISupplyStorage supplyStorage, IGetTechniqueStorage getTechniqueStorage)
        {
            _orderStorage = orderStorage;
            _supplyStorage = supplyStorage;
            _getTechniqueStorage = getTechniqueStorage;
        }


        //public List<ReportOrderSupplyViewModel> GetProcedureReceipt(ReportOrderSupplyBindingModel model)
        //{
        //    var orders = _orderStorage.GetFilteredList(new OrderBindingModel
        //    {
        //        DateFrom = model.DateFrom,
        //        DateTo = model.DateTo
        //    });
        //    var supplies = _supplyStorage.GetFilteredList(new SupplyBindingModel
        //    {
        //        CustomerId = model.CustomerId
        //    });
        //    var getTechniques = _getTechniqueStorage.GetFullList();

        //    var list = new List<ReportOrderSupplyViewModel>();

        //    foreach (var order in orders)
        //    {

        //        foreach (var medicine in medicines)
        //        {
        //            if (getTechnique.getTechniqueSupply.ContainsKey(supply.Id))
        //            {
        //                foreach (var getTechnique in getTechniques)
        //                {
        //                    if (supply.getTechniqueSupply.ContainsKey(supply.Id))
        //                    {
        //                        list.Add(new ReportProcedureReceiptViewModel
        //                        {
        //                            DeliverymanName = receipt.DeliverymanName,
        //                            Date = receipt.Date,
        //                            MedecineName = medicine.Name,
        //                            ProcedureName = procedure.Name
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //        return list;
        //}
    }
}
