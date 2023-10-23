using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
    public class UsersGladiatorModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string user_id { get; set; }
        public string? user_name { get; set; }
        public List<int>? owned_gladiator_ids { get; set; }
    }
}
