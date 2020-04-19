using RimWorld;

namespace LootBoxes
{

    [DefOf]
    public static class RecordDefOf
    {
        public static RecordDef LootBoxesOpened;
        static RecordDefOf()
		{
            DefOfHelper.EnsureInitializedInCtor(typeof(RecordDefOf));
		}


    }

}
