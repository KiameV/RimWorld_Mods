using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace AutomaticNightOwl
{
    [StaticConstructorOnStartup]
    public static class AutomaticNightOwl
    {
        private static readonly TraitDef NightOwl = DefDatabase<TraitDef>.GetNamed("NightOwl");
        private static readonly Queue<Pawn> IsPawn = new Queue<Pawn>();
        private static void AutoNightOwl(Pawn pawn)
        {
            if (pawn != null && (pawn.IsFreeColonist || pawn.IsPrisonerOfColony))
            {
                if ((pawn.story.traits.HasTrait(NightOwl) == true) && pawn.timetable != null)
                {
                    pawn.timetable.times = new List<TimeAssignmentDef>(GenDate.HoursPerDay);
                    for (int i = 0; i < GenDate.HoursPerDay; i++)
                    {
                        TimeAssignmentDef setNightOwlHours;
                        if (i >= 11 && i <= 18) { setNightOwlHours = TimeAssignmentDefOf.Sleep; }
                        else { setNightOwlHours = TimeAssignmentDefOf.Anything; }
                        pawn.timetable.times.Add(setNightOwlHours);
                    }
                }
            }
        }
        [HarmonyPatch(new Type[] { typeof(Pawn) })]
        [HarmonyPatch(MethodType.Constructor)]
        [HarmonyPatch(typeof(Pawn_TimetableTracker))]
        public static class Patch_Pawn_TimetableTracker
        {
            public static void Postfix(Pawn pawn)
            {
                if (Scribe.mode != LoadSaveMode.LoadingVars)
                { IsPawn.Enqueue(pawn); }
            }
        }
        [HarmonyPatch(typeof(GameComponentUtility))]
        [HarmonyPatch(nameof(GameComponentUtility.GameComponentUpdate))]
        public static class Patch_GameComponentUtility
        {
            public static void Postfix()
            { if (IsPawn.Count > 0) { AutoNightOwl(IsPawn.Dequeue()); } }
        }
        static AutomaticNightOwl()
        {
            Harmony harmony = new Harmony("AutomaticNightOwl_Ben");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}

