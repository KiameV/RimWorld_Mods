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
			ThingDef val = null;
			float value = Rand.Value;
			if (value < 0.1f)
			{
				val = ThingDefOf.LootBoxTreasure;
			}
			else if (value < 0.35f)
			{
				val = ThingDefOf.LootBoxSilverSmall;
			}
			else if (value < 0.4f)
			{
				val = ThingDefOf.LootBoxGoldSmall;
			}
			else if (value < 0.5f)
			{
				val = ThingDefOf.LootBoxPandora;
			}
			if (val != null)
			{
				Thing val2 = ThingMaker.MakeThing(val, (ThingDef)null);
				val2.stackCount = 1;
				((ThingOwner)p.inventory.innerContainer).TryAdd(val2, true);
			}
		}

    }

}
