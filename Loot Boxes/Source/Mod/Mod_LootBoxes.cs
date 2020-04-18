using UnityEngine;
using Verse;

namespace LootBoxes
{

    public class Mod_LootBoxes : Mod
    {

        public static ModSettings_LootBoxes settings;

        public Mod_LootBoxes(ModContentPack content) : base(content)
        {
            settings = GetSettings<ModSettings_LootBoxes>();
        }

        public override string SettingsCategory()
        {
            return "LootBoxes_SettingsCategory".Translate();
        }

        public override void DoSettingsWindowContents(Rect rect)
        {
            settings.DoWindowContents(rect);
            settings.Write();
        }

    }

}
