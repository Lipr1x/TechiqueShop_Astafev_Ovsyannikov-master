using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TechiqueShopBusinessLogic.ViewModels
{
    public class DataGridSupplyItemViewModel
    {
        public int Id { get; set; }

        [DisplayName("Заказ")]
        public string CosmeticName { get; set; }

        [DisplayName("Стоимость")]
        public decimal? Price { get; set; }
        [DisplayName("Количество")]
        public int Count { get; set; }
    }
}
