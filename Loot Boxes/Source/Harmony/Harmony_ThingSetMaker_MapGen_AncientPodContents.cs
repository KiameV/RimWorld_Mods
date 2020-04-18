using HarmonyLib;
using RimWorld;
using Verse;

namespace LootBoxes
{

    [HarmonyPatch(typeof(ThingSetMaker_MapGen_AncientPodContents))]
    [HarmonyPatch("GiveRandomLootInventoryForTombPawn")]
    public class Harmony_ThingSetMaker_MapGen_AncientPodContents_GiveRandomLootInventoryForTombPawn
    {

        public static void Postfix(Pawn p)
        {
            ThingDef lootToAdd = null;
            float random = Rand.Value;
            if (random < 0.10f)
            {
                lootToAdd = ThingDefOf.LootBoxTreasure;
            }
            else if (random < 0.35f)
            {
                lootToAdd = ThingDefOf.LootBoxSilverSmall;
            }
            else if (random < 0.40f)
            {
                lootToAdd = ThingDefOf.LootBoxGoldSmall;
            }
            else if (random < 0.50f)
            {
                lootToAdd = ThingDefOf.LootBoxPandora;
            }
            // Create and add loot if needed
            if (lootToAdd != null)
            {
                Thing thing = ThingMaker.MakeThing(lootToAdd);
                thing.stackCount = 1;
                p.inventory.innerContainer.TryAdd(thing);
            }
        }

    }

}
