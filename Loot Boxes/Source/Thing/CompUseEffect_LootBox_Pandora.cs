using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace LootBoxes
{

    public class CompUseEffect_LootBox_Pandora : CompUseEffect_LootBox
    {

        private static readonly Dictionary<List<IncidentCategoryDef>, IncidentGroupData> incidentGroups = new Dictionary<List<IncidentCategoryDef>, IncidentGroupData>()
            {
                { new List<IncidentCategoryDef>() { IncidentCategoryDefOf.ShipChunkDrop, IncidentCategoryDefOf.OrbitalVisitor, IncidentCategoryDefOf.FactionArrival, IncidentCategoryDefOf.Misc }, new IncidentGroupData(0.30f, 1, 3, 6  ) }, // Good and neutral
                { new List<IncidentCategoryDef>() { IncidentCategoryDefOf.ShipChunkDrop, IncidentCategoryDefOf.OrbitalVisitor, IncidentCategoryDefOf.ThreatSmall, IncidentCategoryDefOf.Misc, IncidentCategoryDefOf.DiseaseHuman, IncidentCategoryDefOf.DiseaseAnimal }, new IncidentGroupData(0.20f, 2, 5, 9 ) }, // Neutral to slightly bad
                { new List<IncidentCategoryDef>() { IncidentCategoryDefOf.ShipChunkDrop, IncidentCategoryDefOf.ThreatBig, IncidentCategoryDefOf.ThreatSmall, IncidentCategoryDefOf.Misc, IncidentCategoryDefOf.DiseaseHuman, IncidentCategoryDefOf.DiseaseAnimal }, new IncidentGroupData(0.50f, 1, 3, 7 ) } // Bad and neutral
            };

        protected override void OpenBox(Pawn usedBy)
        {
            Map map = usedBy.Map;
            Rand.PushState(parent.HashOffset());
            bool hostileActivity = GenHostility.AnyHostileActiveThreatToPlayer(map);
            KeyValuePair<List<IncidentCategoryDef>, IncidentGroupData> selectedGroup = incidentGroups.RandomElementByWeight(kvp => kvp.Value.chance);
            while (hostileActivity && selectedGroup.Key.Contains(IncidentCategoryDefOf.FactionArrival))
            {
                selectedGroup = incidentGroups.RandomElementByWeight(kvp => kvp.Value.chance);
            }
            int count = Rand.RangeInclusive(selectedGroup.Value.countMin, Rand.RangeInclusive(selectedGroup.Value.countStep, selectedGroup.Value.countMax));
            for (int i = 0; i < count; i++)
            {
                List<IncidentDef> validIncidents = new List<IncidentDef>();
                foreach (IncidentDef incident in from def in DefDatabase<IncidentDef>.AllDefs where selectedGroup.Key.Contains(def.category) && def.TargetAllowed(map) select def)
                {
                    IncidentParms parms = StorytellerUtility.DefaultParmsNow(incident.category, map);
                    if (incident.Worker.CanFireNow(parms, false))
                    {
                        validIncidents.Add(incident);
                    }
                }
                IncidentDef selectedIncident = validIncidents.RandomElementByWeight(new Func<IncidentDef, float>(IncidentChanceFinal));
                if (selectedIncident != null)
                {
                    IncidentParms parms = StorytellerUtility.DefaultParmsNow(selectedIncident.category, map);
                    if (selectedIncident.pointsScaleable)
                    {
                        StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
                        if (storytellerComp != null)
                        {
                            parms = storytellerComp.GenerateParms(selectedIncident.category, parms.target);
                        }
                    }
                    selectedIncident.Worker.TryExecute(parms);
                }
            }
            Rand.PopState();
        }

        private float IncidentChanceFinal(IncidentDef def)
        {
            float chance = def.Worker.BaseChanceThisGame;
            chance *= IncidentChanceFactor_CurrentPopulation(def);
            return Math.Max(0f, chance);
        }

        private float IncidentChanceFactor_CurrentPopulation(IncidentDef def)
        {
            if (def.chanceFactorByPopulationCurve == null)
            {
                return 1f;
            }
            int pawnCount = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count();
            return def.chanceFactorByPopulationCurve.Evaluate(pawnCount);
        }

        private struct IncidentGroupData
        {
            public float chance;
            public int countMin;
            public int countStep;
            public int countMax;

            public IncidentGroupData(float chance, int countMin, int countStep, int countMax)
            {
                this.chance = chance;
                this.countMin = countMin;
                this.countStep = countStep;
                this.countMax = countMax;
            }
        }

    }

}
