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
        private static readonly Queue<Pawn> IsPawn = new Queue<Pawn>();
        private static void AutoNightOwl(Pawn pawn)
        {
                if ((pawn.story.traits.HasTrait(TraitDefOf.NightOwl) == true) && pawn.timetable != null)
                {
                    pawn.timetable.times = new List<TimeAssignmentDef>(GenDate.HoursPerDay);
                    for (int i = 0; i < GenDate.HoursPerDay; i++)
                    {
                    TimeAssignmentDef setNightOwlHours = i >= 11 && i <= 18 ? TimeAssignmentDefOf.Sleep : TimeAssignmentDefOf.Anything;
                    pawn.timetable.times.Add(setNightOwlHours);
                }
                }
        }
        [HarmonyPatch(typeof(Pawn_TimetableTracker), MethodType.Constructor, new Type[] { typeof(Pawn) })]
        public static class Patch_Pawn_TimetableTracker
        {
            public static void Postfix(Pawn pawn)
            {
                if (Scribe.mode != LoadSaveMode.LoadingVars)
                { IsPawn.Enqueue(pawn); }
            }
        }
        [HarmonyPatch(typeof(GameComponentUtility), nameof(GameComponentUtility.GameComponentUpdate))]
        //[HarmonyPatch(typeof(GameComponentUtility)]
        //[HarmonyPatch(nameof(GameComponentUtility.GameComponentUpdate))]
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
        [DefOf]
        public static class TraitDefOf
        {

            public static TraitDef NightOwl;

        }
    }
}

