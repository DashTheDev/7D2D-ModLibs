using System.Collections.Generic;
using System.Text;

namespace ModTemplate;

public class ModTemplateConsoleCmd : ConsoleCmdAbstract
{
    public ModTemplateConsoleCmd() { }

    public override bool IsExecuteOnClient => true;
    public override bool AllowedInMainMenu => true;

    public override string[] getCommands() =>
    [
        "modtemplate"
    ];

    public override string getDescription()
    {
        return "ModTemplate debugging and config commands.";
    }

    public override string GetHelp()
    {
        StringBuilder sb = new();
        sb.AppendLine("Usage:");
        sb.AppendLine("modtemplate help - Lists commands and their descriptions");
        sb.AppendLine("modtemplate dm - Toggles debug mode");
        sb.AppendLine("modtemplate rconf - Reloads the mod's config");
        return sb.ToString();
    }

    public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
    {
        if (_params.Count < 1)
        {
            OutputConsoleLine("Please provide a subcommand.");
            return;
        }

        switch (_params[0].ToLowerInvariant())
        {
            case "help":
                ExecuteHelp();
                break;

            case "dm":
                ExecuteDebugMode();
                break;

            case "rconf":
                ExecuteReloadConfig();
                break;

            default:
                OutputConsoleLine($"Subcommand not recongised \"{_params[0]}\".");
                break;
        }
    }

    private void ExecuteHelp()
    {
        OutputConsoleLine(GetHelp());
    }

    private void ExecuteDebugMode()
    {
        ModTemplateMod.Instance.Config.IsDebug = !ModTemplateMod.Instance.Config.IsDebug;
        OutputConsoleLine($"Debug mode: {(ModTemplateMod.Instance.Config.IsDebug ? "Enabled" : "Disabled")}");
    }

    private void ExecuteReloadConfig()
    {
        ModTemplateMod.Instance.Config.Load();
        OutputConsoleLine("Config reloaded!");
    }

    private void OutputConsoleLine(string message)
    {
        SingletonMonoBehaviour<SdtdConsole>.Instance.Output(message);
    }
}