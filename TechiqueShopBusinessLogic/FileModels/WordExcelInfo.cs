using System;
using System.Collections.Generic;
using System.Text;
using TechiqueShopBusinessLogic.ViewModels;

namespace TechiqueShopBusinessLogic.FileModels
{
    public class WordExcelInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<GetListComponentAssemblyViewModel> ComponentAssembly { get; set; }
        public List<String> ChoosedComponents { get; set; }
    }
}
