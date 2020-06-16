﻿using System;
using System.Reflection;
using RimWorld;
using Verse;
using UnityEngine;

namespace NoRandomConstructionQuality
{
    class Nrcq_mod : Mod
    {
        public static Nrcq_settings settings;

        public Nrcq_mod(ModContentPack content) : base(content)
        {
            Nrcq_mod.settings = GetSettings<Nrcq_settings>();
            Log.Message($"NoRandomConstructionQuality :: Initialized");
        }

        public override string SettingsCategory() => "No Random Construction Quality";

        public override void DoSettingsWindowContents(Rect canvas) { settings.DoWindowContents(canvas); }
    }

    [StaticConstructorOnStartup]
    public static class Initializer
    {
        static Initializer()
        {
            MethodInfo m1 = typeof(QualityUtility).GetMethod("GenerateQualityCreatedByPawn", BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Standard, new Type[] { typeof(int), typeof(bool) }, null);
            MethodInfo m2 = typeof(_QualityUtility).GetMethod("GenerateQualityCreatedByPawn", BindingFlags.Static | BindingFlags.Public);
            
            if (Detours.TryDetourFromTo(m1, m2))
            {
                Log.Message("NoRandomConstructionQuality: QualityUtility.RandomCreationQuality overridden successfully!");
            }

            MethodInfo m3 = typeof(QualityUtility).GetMethod("SendCraftNotification", BindingFlags.Static | BindingFlags.Public);
            MethodInfo m4 = typeof(_QualityUtility).GetMethod("_SendCraftNotification", BindingFlags.Static | BindingFlags.Public);

            if (Detours.TryDetourFromTo(m3, m4))
            {
                Log.Message("NoRandomConstructionQuality: QualityUtility._SendCraftNotification overridden successfully!");
            }
        }
    }
}
