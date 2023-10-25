using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models
{
    public class GladiatorModel{

        [Key]
        [Display(Name = "Gladiator id")]
        public int id { get; set; }

        [Display(Name = "Gladiator name")]
        public String name { get; set; }

        [Display(Name = "Weapon")]
        public String weapon { get; set; }

        [Display(Name = "Attack")]
        public int attack { get; set; }

        [Display(Name = "Speed")]
        public int speed { get; set; }

        [Display(Name = "Armor")]
        public int defence { get; set; }

        [Display(Name = "Current health")]
        public int health { get; set; }

        [Display(Name = "Maximum health")]
        public int maxhealth { get; set; }

        [Display(Name = "Holds a shield")]
        public bool hasShield { get; set; }

        [Display(Name = "Level")]
        public int level { get; set; }

        [Display(Name = "XP")]
        public int xp { get; set; }

        [Display(Name = "XP for next level")]
        public int xptolevel { get; set; }

        [Display(Name = "Time this gladiator will stop resting and can fight")]
        public DateTime restingTill { get; set; }

        //[ForeignKey("User")]
        public int? user_id { get; set; } // Foreign key
        public virtual UserModel? User { get; set; } // Navigation property

        public GladiatorModel()
        {
            
        }
    }
}
