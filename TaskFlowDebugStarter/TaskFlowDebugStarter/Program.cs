using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TaskFlowDebugStarter
{
    public class Program
    {
        private static Timer _timer;
        private static bool _firstCheckDone = false;

        private static readonly object _lock = new();
        private static List<Process> _processesWorking = new List<Process>();
        private static List<string> _paths = new List<string>();

        static async Task Main(string[] args)
        {
            bool isFound = await FindProgramPathAsync();

            if (!isFound)
            {
                Console.ReadLine();
                return;
            }

            bool isStarted = await StartAsync();

            if (isStarted)
            {
                SetTimer();
                Console.ReadLine();
            }
            else
            {
                Console.ReadLine();
                return;
            }
        }

        public static async Task<bool> StartAsync()
        {
            lock (_lock)
            {
                _processesWorking.Clear();
            }

            foreach (string path in _paths)
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine($"Path not found in 'JsonSettings'. Please add your program paths and restart the application");
                    return false;
                }

                try
                {
                    var newProc = await Task.Run(() => Process.Start(path));
                    if (newProc != null)
                    {
                        lock (_lock)
                        {
                            _processesWorking.Add(newProc);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to start process from {path}: {ex.Message}");
                }
            }

            foreach (string path in _paths)
            {
                string processName = Path.GetFileNameWithoutExtension(path);
                var runningProcs = Process.GetProcessesByName(processName);
                lock (_lock)
                {
                    foreach (var proc in runningProcs)
                    {
                        if (!_processesWorking.Exists(p => p.Id == proc.Id))
                        {
                            _processesWorking.Add(proc);
                        }
                    }
                }
            }

            return true;
        }

        private static void SetTimer()
        {
            _timer = new Timer(CheckProcessIsWorking, null, 0, 7500);
        }

        public static void CheckProcessIsWorking(object? state)
        {
            if (!_firstCheckDone)
            {
                _firstCheckDone = true;
                return;
            }

            ShowMessage("green", "Monitoring processes...\n");

            List<Process> processesCopy;
            lock (_lock)
            {
                processesCopy = new List<Process>(_processesWorking);
            }

            foreach (var proc in processesCopy)
            {
                string procName;
                try
                {
                    procName = proc.ProcessName;
                }
                catch
                {
                    procName = "Unknown Process";
                }

                bool hasExited = false;
                try
                {
                    hasExited = proc.HasExited;
                }
                catch
                {
                    hasExited = true;
                }

                if (hasExited)
                {
                    ShowMessage("red", $"{procName} has closed. Trying to restart...\n");

                    lock (_lock)
                    {
                        _processesWorking.RemoveAll(p => p.Id == proc.Id);
                    }

                    var runningProcesses = Process.GetProcessesByName(procName);
                    if (runningProcesses.Length == 0)
                    {
                        string? pathToStart = null;
                        lock (_lock)
                        {
                            foreach (var path in _paths)
                            {
                                if (string.Equals(Path.GetFileNameWithoutExtension(path), procName, StringComparison.OrdinalIgnoreCase))
                                {
                                    pathToStart = path;
                                    break;
                                }
                            }
                        }

                        if (pathToStart != null)
                        {
                            try
                            {
                                var newProc = Process.Start(pathToStart);
                                if (newProc != null)
                                {
                                    lock (_lock)
                                    {
                                        _processesWorking.Add(newProc);
                                    }
                                    ShowMessage("green", $"{procName} restarted successfully!\n");
                                }
                            }
                            catch (Exception ex)
                            {
                                ShowMessage("red", $"Failed to restart {procName}: {ex.Message}\n");
                            }
                        }
                        else
                        {
                            ShowMessage("red", $"Path to restart {procName} not found\n");
                        }
                    }
                    else
                    {
                        ShowMessage("green", $"{procName} is already running\n");
                    }
                }
            }
        }

        public static async Task<bool> FindProgramPathAsync()
        {
            var defaultPaths = new
            {
                TaskFlowAuthServer = "Your path here",
                TaskFlowTaskServer = "Your path here",
            };

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };

            string pathsFile = "JsonSettings.json";

            if (File.Exists(pathsFile))
            {
                await using var stream = File.OpenRead(pathsFile);
                var paths = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream);

                if (paths == null || paths.Count == 0)
                {
                    Console.WriteLine("'JsonSettings' is empty. Please add your program paths.");
                    return false;
                }

                lock (_lock)
                {
                    _paths.Clear();
                    foreach (var path in paths.Values)
                    {
                        _paths.Add(path);
                    }
                }
                return true;
            }
            else
            {
                string json = JsonSerializer.Serialize(defaultPaths, options);
                await File.WriteAllTextAsync(pathsFile, json);

                Console.WriteLine("'JsonSettings' created. Please add your program paths and restart the application");
                return false;
            }
        }

        public static void ShowMessage(string color, string message)
        {
            if (color == "red")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Info: ");
            }
            Console.ResetColor();

            Console.Write(message);
        }
    }
}
