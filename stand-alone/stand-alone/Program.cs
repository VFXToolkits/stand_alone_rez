// See https://aka.ms/new-console-template for more information
using Sharprompt;
using System.Text;
using stand_alone;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (args.Length != 0) {
            if (args[0].StartsWith("-"))
            {
                stand_tool_command(args);
            }
            else {
                if (rez_command_validate(args[0]))
                {
                    new REZTool(string.Join(" ", args)).run_env();
                }
                else {
                    Prompt.Symbols.Error = new Symbol("😱", ">>");
                    throw new ArgumentOutOfRangeException();
                }

            }
            return;
        }
        Console.WriteLine($"Welcome {Environment.UserName} to stand alone rez tool CLI V{SAConfig.sa_version}!");
        Console.WriteLine($"Curent operating system:{Environment.OSVersion.VersionString}");
        Console.WriteLine("-help for more infomation \n");
        stand_tool_select();
    }

    private static void stand_tool_command(string[] args) {

        if (args[0] == "-help")
        {
            Prompt.ColorSchema.Answer = ConsoleColor.DarkRed;
            Console.WriteLine("command:\n");
            Console.WriteLine("env rez env\n");
        } else
        {
            Console.WriteLine("Command error \n");
            Console.WriteLine("-help for more infomation \n");
        }
    }

    private static void stand_tool_select() {
        var select_type = Prompt.Select<InputCommandType>("Select the command you want to execute");
        var select_command = new SelectCommand();
        switch (select_type)
        {
            case InputCommandType.rez_command:
                select_command.rez_command();
                break;
            case InputCommandType.setup_project:
                select_command.setup_project();
                break;
            case InputCommandType.quit:
                Console.WriteLine("Bye");
                Environment.Exit(0);
                break;
            case InputCommandType.local_build:
                select_command.local_build();
                break;
            case InputCommandType.project_info:
                select_command.project_info();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static bool rez_command_validate(string rez_command) {

        RezCommandType flag;
        if (Enum.TryParse<RezCommandType>(rez_command, true, out flag))
        {

            return true;

        }
        return false;
    }


}

