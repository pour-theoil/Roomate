using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {

            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository chorRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository mateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Find unassigned chores"):
                        List<Chore> unchore = chorRepo.GetUnassignedChores();
                        foreach (Chore c in unchore)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        List<Chore> chores = chorRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;
                    case ("Get chore counts"):
                        List<RoommateChoreCount> counts = chorRepo.GetChoreCount();
                        foreach(RoommateChoreCount rc in counts)
                        {
                            Console.WriteLine($"{rc.FirstName}: {rc.NumberChores}");
                        }

                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        int mateid = int.Parse(Console.ReadLine());

                        Roommate roommate = mateRepo.GetById(mateid);
                        Console.WriteLine($"{roommate.Id} - RentPortion: {roommate.RentPortion}%  - Room Name: {roommate.Room.Name}");


                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        Console.Write("Chore Id: ");
                        int chorid = int.Parse(Console.ReadLine());
                        Chore chore = chorRepo.GetById(chorid);
                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign chores"):
                        List<Roommate> roommatess = mateRepo.GetAll();
                        foreach (Roommate r in roommatess)
                        {
                            Console.WriteLine($"{r.Id} - {r.FirstName} {r.LastName} ");
                        }
                        Console.Write("Choose Roommate Id: ");
                        int mate = int.Parse(Console.ReadLine());
                        List<Chore> chorelist = chorRepo.GetAll();
                        foreach (Chore c in chorelist)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.Write("Choose Chore Id: ");
                        int chorechoice = int.Parse(Console.ReadLine());

                        chorRepo.AssignChore(chorechoice, mate);




                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string chorename = Console.ReadLine();

                        Chore choretoadd = new Chore()
                        {
                            Name = chorename
                        };

                        chorRepo.Insert(choretoadd);

                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Update a chore"):
                        List<Chore> choroptions = chorRepo.GetAll();
                        foreach (Chore c in choroptions)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.Write("Which chore would you like to update? ");
                        int selectedchoreid = int.Parse(Console.ReadLine());
                        Chore selectedchore = choroptions.FirstOrDefault(c => c.Id == selectedchoreid);

                        Console.Write("New Name: ");
                        selectedchore.Name = Console.ReadLine();

                        chorRepo.Update(selectedchore);
                 
                        Console.WriteLine("Chore has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Delete room"):
                        List<Room> delroom = roomRepo.GetAll();
                        foreach (Room r in delroom)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name}");
                        }
                        Console.Write("Which room would you like to delete? ");
                        int roomid = int.Parse(Console.ReadLine());
                        try
                        {
                            roomRepo.Delete(roomid);
                        }
                        catch 
                        {
                            Console.WriteLine("Room cannot be deleted, there is someone living in it...");
                            Console.Write("Press any key to continue");
                            Console.ReadKey();
                            break;
                        }
                        Console.WriteLine($"You have deleted room with {roomid}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete chore"):
                        List<Chore> delchore = chorRepo.GetAll();
                        foreach(Chore c in delchore)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore would you like to delete? ");
                        int choreid = int.Parse(Console.ReadLine());

                        try
                        {
                            chorRepo.Delete(choreid);
                        }
                        catch
                        {
                            Console.WriteLine("Chore cannot be deleted, someone is likely assigned it...");
                            Console.Write("Press any key to continue");
                            Console.ReadKey();
                            break;
                        }
                        Console.WriteLine("Chore Successfully deleted");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;


                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Update a room",
                "Delete room",
                "Add a room",
                "Show all chores",
                "Search for chore",
                "Add a chore",
                "Search for roommate",
                "Find unassigned chores",
                "Assign chores",
                "Get chore counts",
                "Delete chore",
                
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}