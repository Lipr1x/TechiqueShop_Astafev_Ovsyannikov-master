using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TechiqueShopBusinessLogic.ViewModels
{
    public class DataGridGetTechniqueItemViewModel
    {
        public int Id { get; set; }

        [DisplayName("Получение техники")]
        public string CosmeticName { get; set; }

        [DisplayName("Количество")]
        public int Count { get; set; }
    }
}
