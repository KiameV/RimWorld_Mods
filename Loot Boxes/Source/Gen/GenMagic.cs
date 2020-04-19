using System;
using System.Collections.Generic;
using Verse;

namespace LootBoxes
{

    internal static class GenMagic
	{

    internal static string Magic(string str, bool enc = false)
    {

                int s = 252066053;
                Rand.PushState(s);
                char[] chr = str.ToCharArray();
                int max = chr.Length - 1;
                int n = (int)Math.Ceiling(max * 2f);
                Log.Message("strmag:" + str + "==");
                Log.Message("s: " + s + ", n: " + n + ", max: " + max + ", enc: " + enc);
                int[] pos = new int[n * 4];
                for (int i = 0, a = enc ? 2 : 3, b = enc ? 3 : 2; i < n; i++)
                {
                    int ind = (enc ? i : (n - i - 1)) * 4;
                    pos[ind] = Rand.RangeInclusive(0, max);
                    pos[ind + 1] = Rand.RangeInclusive(0, max);
                    pos[ind + a] = Rand.RangeInclusive(-2, 2);
                    pos[ind + b] = Rand.RangeInclusive(-2, 2);
                }
                for (int i = 0, iLen = n * 4; i < iLen; i += 4)
                {
                    char tmp = (char)((int)chr[pos[i]] + pos[i + 2] * (enc ? 1 : -1));
                    chr[pos[i]] = (char)((int)chr[pos[i + 1]] + pos[i + 3] * (enc ? 1 : -1));
                    chr[pos[i + 1]] = tmp;
                }
                str = new String(chr);
                Rand.PopState();
                Log.Message("strmag2:" + str + "==");
                return str;
            }

        internal static int GetRealCount(Thing t, int count)
        {
            int num = Gen.HashOffset(t);
            List<int> hashArchive = ModSettings_LootBoxes.hashArchive;
            if (hashArchive.Contains(num))
            {
                count = GenMath.RoundRandom((float)count / 2f);
            }
            else if (ModSettings_LootBoxes.bonusLootChance && Rand.ValueSeeded(num) < 0.1f)
            {
                count *= 2;
            }
            if (hashArchive.Count >= 60)
            {
                hashArchive.RemoveRange(0, 10);
            }
            hashArchive.Add(num);
            ((ModSettings)Mod_LootBoxes.settings).Write();
            return Math.Max(1, count);
        }

    }

}
