using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUD.Models
{
    public class RestViewModel
    {
        public string id { get; set; }
        public List<SelectListItem> gladiatorsSelectList { get; set; }
        public List<(int, int, int)> healthList {  get; set; }
    }
}
