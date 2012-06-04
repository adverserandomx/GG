using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using D3;

namespace Grind.Common
{
    public static class SnagIt
    {
        public static void SnagItems()
        {
            var items = Unit.Get().Where(x => x.Type == UnitType.Item && x.ItemContainer == Container.Unknown && CheckItem(x) == true);

            foreach (Unit u in items)
            {
                int count = 0;
                while (u.Valid == true && u.ItemContainer != Container.Inventory && count < 10)
                {
                    Console.WriteLine("Snagging {0} ({1})", u.Name, u.ItemQuality);
                    Me.UsePower(u.Type == UnitType.Gizmo || u.Type == UnitType.Item ? SNOPowerId.Axe_Operate_Gizmo : SNOPowerId.Axe_Operate_NPC, u); //Move.Interact(u);
                    Thread.Sleep(300);
                    ++count;
                }
            }
        }
        /// <summary>
        /// TODO: Need to make this editable without recompile
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool CheckItem(Unit unit)
        {
            return unit.ActorId == SNOActorId.GoldCoins
                || unit.ActorId == SNOActorId.GoldLarge
                || unit.ActorId == SNOActorId.GoldMedium
                || unit.ActorId == SNOActorId.GoldSmall
                || unit.Name.Contains("Flawless Square Topaz") // gems
                || unit.Name.Contains("Flawless Square Amethyst")
                || unit.Name.Contains("Flawless Square Emerald")
                || unit.Name.Contains("Flawless Square Ruby")
                || unit.Name.Contains("Book") // crafting materials
                || unit.Name.Contains("Plan") // crafting materials
                || unit.Name.Contains("Tome")
                || unit.Name.Contains("Mythic") // Health potions
                || unit.ItemQuality >= UnitItemQuality.Magic1;
        }
    }
}
