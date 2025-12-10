// See https://aka.ms/new-console-template for more information
using SyncFolders.Synchronization;

Synchronization synchronization = new();
Console.WriteLine("This is a small program that would synchronize a source folder into the replica folder.");
Console.WriteLine("Click any button to start the synchronization process....");
Console.WriteLine("---------------------------------------------------------");
Console.ReadKey();
Console.WriteLine("Synchronization started!");
Console.WriteLine("---------------------------------------------------------");

synchronization.Synchronize("TestingSetup\\Source", "TestingSetup\\Replica");

Console.WriteLine("Synchronization process is now finished!");
