using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
    public class UserModel
    {
        [Key]
        public int user_id { get; set; }
        public required string user_email { get; set; }
        public virtual ICollection<GladiatorModel>? gladiators { get; set; }
    }
}
