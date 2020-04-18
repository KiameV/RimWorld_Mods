using HarmonyLib;
using System.Reflection;
using Verse;

namespace LootBoxes
{

    [StaticConstructorOnStartup]
    public static class HLootBoxes
    {

        static HLootBoxes()
        {
            Harmony harmony = new Harmony("LootBoxesUpdate_Ben");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

    }

}
