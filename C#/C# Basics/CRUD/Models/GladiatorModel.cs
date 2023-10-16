﻿namespace CRUD.Models
{
    public class GladiatorModel{
        public int id { get; set; }
        public String name { get; set; }
        public String weapon { get; set; }
        public int attack { get; set; }
        public int speed { get; set; }
        public int defence { get; set; }
        public int health { get; set; }
        public int maxhealth { get; set; }
        public bool hasShield { get; set; }

        public GladiatorModel()
        {
            
        }
    }
}
