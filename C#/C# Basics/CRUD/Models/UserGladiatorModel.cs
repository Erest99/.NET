using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models
{
    public class UserGladiatorModel
    {
        public int link_id { get; set; }
        public int user_id { get; set; }
        public int glad_id { get; set; }

        [ForeignKey("user_id")]
        public UserModel user { get; set; }

        [ForeignKey("glad_id")]
        public GladiatorModel gladiator { get; set; }

    }
}
