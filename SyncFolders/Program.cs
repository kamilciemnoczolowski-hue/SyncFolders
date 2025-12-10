// See https://aka.ms/new-console-template for more information
using SyncFolders.Synchronization;

int sleepInMiliseconds = 60000;
Synchronization synchronization = new();
Console.WriteLine("This is a small program that would synchronize a source folder into the replica folder every minute.");
Console.WriteLine("------------------------------------------------------------------");

while (true)
{
    Console.WriteLine("Synchronization started!");
    synchronization.Synchronize("C:\\tmp\\source", "C:\\tmp\\replica");
    Console.WriteLine($"Synchronization finished! Waiting for the desired time interval = {sleepInMiliseconds / 1000} seconds...");
    Console.WriteLine("------------------------------------------------------------------");
    Thread.Sleep(sleepInMiliseconds);
}