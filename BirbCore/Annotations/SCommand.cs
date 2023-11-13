#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;
using StardewModdingAPI;

namespace BirbCore.Annotations;

/// <summary>
/// A collection of commands in the SMAPI console.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SCommand : ClassHandler
{
    private static readonly Dictionary<string, Dictionary<string, Action<string[]>>> BaseCommands = new();
    public string Name;

    public SCommand(string name)
    {
        this.Name = name;
    }

    private static string GetHelp(string? subcommand = null)
    {
        return "";
    }

    public override void Handle(Type type, object? instance, IMod mod, object[]? args = null)
    {
        instance = Activator.CreateInstance(type);
        base.Handle(type, instance, mod);
    }

    /// <summary>
    /// A single command. This property converts the method into a command if possible. Typed arguments
    /// will be converted from strings as best as possible. Supports optional numbered parameters, and
    /// variadic arguments (params parameter), and will deal with having more or fewer command line arguments
    /// compared to method arguments. Optionally includes help text.
    /// This attribute, combined with the SCommand attribute, creates a two-command structure to seperate mod
    /// commands.
    /// <code>
    /// [SCommand(Name="birb")]
    /// public class BirbCommand {
    ///
    ///     [Command(Subname="one")]
    ///     public static void CommandOne(string arg0, string arg1) {
    ///
    ///     }
    /// }
    /// </code>
    /// The above will create a command like the following
    /// <code>
    /// birb one arg0 arg1
    /// </code>
    /// </summary>
    public class Command : MethodHandler
    {
        public string Subname;
        public string Help;

        public Command(string subname, string help = "")
        {
            this.Subname = subname;
            this.Help = help;
        }

        public override void Handle(MethodInfo method, object? instance, IMod mod, object[]? args = null)
        {
            if (instance is null)
            {
                Log.Error("SCommand class may be static? Cannot parse subcommands.");
                return;
            }
            string? command = instance.GetType().GetCustomAttribute<SCommand>()?.Name.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(command))
            {
                Log.Error("Base command is null or empty, Cannot parse subcommands.");
                return;
            }
            if (!SCommand.BaseCommands.ContainsKey(command))
            {
                mod.Helper.ConsoleCommands.Add(
                    name: command,
                    documentation: GetHelp(),
                    callback: (s, args) => CallCommand(s, args, GetHelp())
                );
                SCommand.BaseCommands.Add(command, new Dictionary<string, Action<string[]>>());
            }
            string subcommand = method.Name.ToLowerInvariant();
            Dictionary<string, Action<string[]>> subcommands = SCommand.BaseCommands[command];

            subcommands.Add(subcommand, (string[] args) =>
            {
                List<object> commandArgs = new();

                for (int i = 0; i < method.GetParameters().Length; i++)
                {
                    ParameterInfo parameter = method.GetParameters()[i];

                    if (parameter.GetCustomAttribute(typeof(ParamArrayAttribute), false) is not null)
                    {
                        for (int j = i; j < (args?.Length ?? 0); j++)
                        {
                            commandArgs.Add(ParseArg(args?[j], parameter));
                        }
                    }
                    else
                    {
                        commandArgs.Add(ParseArg(args?[i], parameter));
                    }
                }

                method.Invoke(instance, commandArgs.ToArray());

            });
        }

        private static object ParseArg(string? arg, ParameterInfo parameter)
        {
            if (arg == null)
            {
                return Type.Missing;
            }
            else if (parameter.ParameterType == typeof(string))
            {
                return arg;
            }
            else if (parameter.ParameterType == typeof(int))
            {
                return int.Parse(arg);
            }
            else if (parameter.ParameterType == typeof(double) || parameter.ParameterType == typeof(float))
            {
                return float.Parse(arg);
            }
            else if (parameter.ParameterType == typeof(bool))
            {
                return bool.Parse(arg);
            }
            else
            {
                return Type.Missing;
            }
        }

        private static void CallCommand(string s, string[] args, string help)
        {
            if (args.Length == 0 || args[0].Equals("help", StringComparison.InvariantCultureIgnoreCase)
                || args[0].Equals("h", StringComparison.InvariantCultureIgnoreCase))
            {
                Log.Info(GetHelp(args?[1]));
                return;
            }
            SCommand.BaseCommands?[s]?[args[0]]?.Invoke(args[1..]);
        }
    }
}
