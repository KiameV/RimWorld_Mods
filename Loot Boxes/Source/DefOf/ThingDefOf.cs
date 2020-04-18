using RimWorld;
using Verse;

namespace LootBoxes
{

    [DefOf]
    public static class ThingDefOf
    {

        static ThingDefOf()
		{
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
		}

        public static ThingDef LootBoxTreasure;
        public static ThingDef LootBoxSilverSmall;
        public static ThingDef LootBoxSilverLarge;
        public static ThingDef LootBoxGoldSmall;
        public static ThingDef LootBoxGoldLarge;
        public static ThingDef LootBoxPandora;

    }

}
