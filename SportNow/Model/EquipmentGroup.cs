using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SportNow.Model
{
    public class EquipmentGroup : List<Equipment>
    {
        public string Name { get; private set; }

        public EquipmentGroup(string name, List<Equipment> equipments) : base(equipments)
        {
            this.Name = name;
        }

        public void Print()
        {
            Debug.Print("Equipment Group Name = " + Name);
            foreach (Equipment equipment in this)
            {
                Debug.Print("Equipment Name = " + equipment.name);
            }
        }
    }
}

