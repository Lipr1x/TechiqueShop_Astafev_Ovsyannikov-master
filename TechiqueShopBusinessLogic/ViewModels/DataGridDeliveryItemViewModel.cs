using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TechiqueShopBusinessLogic.ViewModels
{
    public class DataGridDeliveryItemViewModel
    {
        public int Id { get; set; }

        [DisplayName("Закупка")]
        public string DeliveryName { get; set; }

        [DisplayName("Дата")]
        public decimal? Date { get; set; }
        [DisplayName("Количество")]
        public int Count { get; set; }
    }
}
