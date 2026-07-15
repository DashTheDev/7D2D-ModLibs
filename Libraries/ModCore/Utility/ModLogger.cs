using System.Collections.Generic;
using HarmonyLib;

namespace DashTheDev.SDTD.ModCore;

internal class ModLogger(Mod gameMod, IModConfig config) : IModLogger
{
    public void LogLine(object str)
    {
        if (!config.IsDebug)
        {
            return;
        }

        Log.Out($"[{gameMod.Name}](v{gameMod.VersionString}) {str}");
    }

    public void LogTranspilerBefore(string methodName, List<CodeInstruction> instructions)
    {
        LogTranspiler(methodName, true, instructions);
    }

    public void LogTranspilerAfter(string methodName, List<CodeInstruction> instructions)
    {
        LogTranspiler(methodName, false, instructions);
    }

    private void LogTranspiler(string methodName, bool isBefore, List<CodeInstruction> instructions)
    {
        if (!config.IsDebug)
        {
            return;
        }

        string timingDescription = isBefore ? "BEFORE" : "AFTER";
        LogLine($"=== {methodName} Transpiler - {timingDescription} ===");

        for (int i = 0; i < instructions.Count; i++)
        {
            LogLine($" [{i}] {instructions[i].opcode} {instructions[i].operand}");
        }
    }
}

public interface IModLogger
{
    public void LogLine(object str);
    public void LogTranspilerBefore(string methodName, List<CodeInstruction> instructions);
    public void LogTranspilerAfter(string methodName, List<CodeInstruction> instructions);
}