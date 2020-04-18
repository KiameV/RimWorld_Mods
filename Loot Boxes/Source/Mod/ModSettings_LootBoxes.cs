using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace LootBoxes
{

    public class ModSettings_LootBoxes : ModSettings
    {

        public const float GapHeight = 10f;

        public static List<int> hashArchive = new List<int>();

        private static bool DefaultBonusLootChance = true;

        public static bool bonusLootChance = DefaultBonusLootChance;

        public ModSettings_LootBoxes() : base()
        {
        }

        public void DoWindowContents(Rect rect)
        {
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(rect);
            ls.Gap(GapHeight);

            ls.CheckboxLabeled("LootBoxes_SettingsBonusLootChance".Translate(), ref bonusLootChance, "LootBoxes_SettingsBonusLootChanceDesc".Translate());
            ls.Gap(GapHeight);

            ls.End();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref hashArchive, "hashArchive", LookMode.Value);
            Scribe_Values.Look(ref bonusLootChance, "bonusLootChance", DefaultBonusLootChance);
        }

    }

}
