using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.Sound;

namespace LootBoxes
{

    public abstract class CompUseEffect_LootBox : CompUseEffect
    {

        public virtual string LootTable => string.Empty;

        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(usedBy.MapHeld);
            usedBy.records.Increment(RecordDefOf.LootBoxesOpened);
            OpenBox(usedBy);
        }

        protected virtual void OpenBox(Pawn usedBy)
        {
            IntVec3 c = parent.Position;
            Map map = parent.Map;
            map.wealthWatcher.ForceRecount();
            //Log.Message("current wealth: " + map.wealthWatcher.WealthItems, true);
            //for (int i = 0; i < 100; i++)
            //{
            List<Thing> lootList = CreateLoot();
            foreach (Thing item in lootList)
            {
                //GenPlace.TryPlaceThing(t, c, map, ThingPlaceMode.Near);
                GenPlace.TryPlaceThing(item, c, map, (ThingPlaceMode.Near), (Action<Thing, int>)null, (Predicate<IntVec3>)null);
            }
            //}
            //map.wealthWatcher.ForceRecount();
            //Log.Message("current wealth: " + map.wealthWatcher.WealthItems, true);
        }

        private List<Thing> CreateLoot()
        {
            Dictionary<string, float> lootEntries = new Dictionary<string, float>();
            string[] arr = GenMagic.Magic(LootTable).Split('|');
            int setsMin = int.Parse(arr[0]);
            int setsMax = int.Parse(arr[1]);
            int setsCount = GenMagic.GetRealCount(parent, Rand.RangeInclusive(setsMin, Rand.RangeInclusive(setsMin, setsMax)));
            //Log.Message("min: " + setsMin + ", max: " + setsMax + ", final: " + setsCount);
            for (int i = 2, iLen = arr.Length; i < iLen; i++)
            {
                string str = arr[i];
                int pos = str.FirstIndexOf(c => c == ';');
                lootEntries.Add(str.Substring(pos + 1), float.Parse(str.Substring(0, pos)));
                //Log.Message("key : " + str.Substring(pos + 1) + ", val: " + float.Parse(str.Substring(0, pos)));
            }
            List<Thing> lootList = new List<Thing>();
            for (int i = 0; i < setsCount; i++)
            {
                string chosenSet = lootEntries.RandomElementByWeight(kvp => kvp.Value * kvp.Value).Key;
                //Log.Message("chosen set: " + chosenSet);
                arr = chosenSet.Split(';');
                for (int j = 0, jLen = arr.Length; j < jLen; j++)
                {
                    string[] inner = arr[j].Split(',');
                    int min = (inner.Length <= 1) ? 1 : int.Parse(inner[1]);
                    int max = (inner.Length <= 2) ? min : int.Parse(inner[2]);
                    if (inner.Length >= 4)
                    {
                        max = Rand.RangeInclusive(max, int.Parse(inner[3]));
                    }
                    ThingDef def = DefDatabase<ThingDef>.GetNamed(inner[0]);
                    //Log.Message("def: " + def.defName + ", min: " + min + ", max: " + max);
                    if (def != null)
                    {
                        int countToDo = Rand.RangeInclusive(min, max);
                        while (countToDo > 0)
                        {
                            int count = Math.Min(countToDo, def.stackLimit);
                            countToDo -= count;
                            Thing thing = ThingMaker.MakeThing(def, GenStuff.RandomStuffFor(def));
                            thing.stackCount = count;
                            //Log.Message("thing: " + thing + ", stuff: " + thing.Stuff + ", count: " + count);
                            CompQuality compQuality = thing.TryGetComp<CompQuality>();
                            if (compQuality != null)
                            {
                                QualityCategory q = QualityUtility.GenerateQualityCreatedByPawn(Rand.RangeInclusive(0, 19), false);
                                compQuality.SetQuality(q, ArtGenerationContext.Outsider);
                            }
                            CompArt compArt = thing.TryGetComp<CompArt>();
                            if (compArt != null)
                            {
                                compArt.InitializeArt(ArtGenerationContext.Outsider);
                            }
                            lootList.Add(thing);
                        }
                    }
                }
            }
            return lootList;
        }

        public override string CompInspectStringExtra()
        {
            return " \n" + parent.DescriptionFlavor;
        }
       
    }
}

