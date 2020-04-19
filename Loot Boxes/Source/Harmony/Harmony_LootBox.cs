using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

namespace LootBoxes
{

	[HarmonyPatch(typeof(GenMagic))]
	[HarmonyPatch("Magic")]
	public class Harmony_GenMagic_Magic
	{
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
		{
			int i = 0;
			for (int iLen = instructions.Count(); i < iLen; i++)
			{
				CodeInstruction ci = instructions.ElementAt(i);
				if ((ci.opcode == OpCodes.Stloc_0 || ci.opcode == OpCodes.Stloc_1) && instructions.ElementAt(i - 1).opcode == OpCodes.Ldc_I4)
				{
					yield return new CodeInstruction(OpCodes.Ldc_I4, 3);
					yield return new CodeInstruction(OpCodes.Mul);
					yield return new CodeInstruction(OpCodes.Ldc_I4, 104);
					yield return new CodeInstruction(OpCodes.Add);
				}
				else if (ci.opcode == OpCodes.Conv_R8 && instructions.ElementAt(i + 1).opcode == OpCodes.Call && instructions.ElementAt(i + 2).opcode == OpCodes.Conv_I4 && instructions.ElementAt(i - 1).opcode == OpCodes.Mul && instructions.ElementAt(i - 2).opcode == OpCodes.Ldc_R4 && instructions.ElementAt(i - 3).opcode == OpCodes.Conv_R4)
				{
					yield return new CodeInstruction(OpCodes.Ldc_R4, 0.325f);
					yield return new CodeInstruction(OpCodes.Mul);
				}
				else if (ci.opcode == OpCodes.Conv_R8 && instructions.ElementAt(i - 1).opcode == OpCodes.Mul && instructions.ElementAt(i - 2).opcode == OpCodes.Ldc_I4 && instructions.ElementAt(i - 3).opcode == OpCodes.Conv_R4 && instructions.ElementAt(i + 1).opcode == OpCodes.Call && instructions.ElementAt(i + 2).opcode == OpCodes.Conv_R4)
				{
					yield return new CodeInstruction(OpCodes.Ldc_R4, 0.7f);
					yield return new CodeInstruction(OpCodes.Mul);
					yield return new CodeInstruction(OpCodes.Ldc_R4, 2.65f);
					yield return new CodeInstruction(OpCodes.Pop);
				}
				yield return ci;
			}
		}
	}
}
