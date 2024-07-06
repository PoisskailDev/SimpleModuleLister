using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        string processName = "FiveM_b2189_GTAProcess";

        Console.WriteLine($"En attente du processus {processName}...");

        Process process = WaitForProcess(processName);

        if (process != null)
        {
            Console.WriteLine($"Processus {processName} détecté (PID: {process.Id})");

  
            MonitorProcess(process);
        }
        else
        {
            Console.WriteLine($"Le processus {processName} n'a pas été trouvé.");
        }

        Console.WriteLine("Appuyez sur une touche pour quitter.");
        Console.ReadKey();
    }

    static Process WaitForProcess(string processName)
    {
        int retries = 10; 
        int delayMilliseconds = 2000; 

        for (int i = 0; i < retries; i++)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
            {
                return processes[0];
            }
            Console.WriteLine($"Le processus {processName} n'a pas encore été détecté. Tentative {i + 1}...");
            Thread.Sleep(delayMilliseconds);
        }

        return null;
    }

    static void MonitorProcess(Process process)
    {
        int processId = process.Id;

        while (!process.HasExited)
        {
            try
            {
                foreach (ProcessModule module in process.Modules)
                {
                    if (module.ModuleName.ToLower().Contains(".dll"))
                    {
                        Console.WriteLine($"DLL détectée : {module.ModuleName} (BaseAddress: 0x{module.BaseAddress:X})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la vérification des modules : {ex.Message}");
            }

            Thread.Sleep(1000); 
        }

        Console.WriteLine($"Le processus {process.ProcessName} s'est arrêté.");
    }
}
