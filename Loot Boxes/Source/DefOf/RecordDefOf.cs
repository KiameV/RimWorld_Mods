using RimWorld;

namespace LootBoxes
{

    [DefOf]
    public static class RecordDefOf
    {

        static RecordDefOf()
		{
            DefOfHelper.EnsureInitializedInCtor(typeof(RecordDefOf));
		}

        public static RecordDef LootBoxesOpened;

    }

}
