using Serilog;
using SyncFolders.Synchronization;

ArgumentsVerifier argumentsVerifier = new(args);

if (!argumentsVerifier.AreArgumentsValid(out string pathToSource, out string pathToReplica, out int interval, out string pathToLogFile))
    return;

int sleepInMiliseconds = interval;

// logger configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File(pathToLogFile, rollOnFileSizeLimit: true, fileSizeLimitBytes: 10 * 1024 * 1024) // rolling file size - limit set to 10MB
    .CreateLogger();

Synchronization synchronization = new();
Console.WriteLine("This is a small program that would synchronize a source folder into the replica folder every minute.");
Console.WriteLine("------------------------------------------------------------------");

while (true)
{
    Console.WriteLine("Synchronization started!");
    synchronization.Synchronize(pathToSource, pathToReplica);
    Console.WriteLine($"Synchronization finished! Waiting the desired time interval = {sleepInMiliseconds / 1000} seconds for the next sync...");
    Console.WriteLine("------------------------------------------------------------------");
    Thread.Sleep(sleepInMiliseconds);
}