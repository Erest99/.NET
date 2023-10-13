using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUD.Models
{
    public class DuelViewModel
    {
        public string firstFighterID { get; set; }
        public string secondFighterID { get; set; }
        public List<SelectListItem> gladiatorsSelectList { get; set; }
    }
}
