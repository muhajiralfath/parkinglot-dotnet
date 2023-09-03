using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingLot
{
    internal class Program
    {
        private static int totalLots;
        private static List<Vehicle> parkingLot;

        public static void Main(string[] args)
        {
            Console.WriteLine("==================================");
            Console.WriteLine("Welcome to the parking system!");
            Console.WriteLine("==================================");
            InitializeParkingLot();

            while (true)
            {
                Console.Write("Enter a command: ");
                string command = Console.ReadLine().Trim().ToLower();

                switch (command)
                {
                    case "create_parking_lot":
                        CreateParkingLot();
                        break;
                    case "park":
                        ParkVehicle();
                        break;
                    case "leave":
                        LeaveParkingLot();
                        break;
                    case "status":
                        ShowParkingStatus();
                        break;
                    case "type_of_vehicles":
                        ShowTypeOfVehicles();
                        break;
                    case "registration_numbers_for_vehicles_with_odd_plate":
                        ShowOddPlateNumbers();
                        break;
                    case "registration_numbers_for_vehicles_with_even_plate":
                        ShowEvenPlateNumbers();
                        break;
                    case "registration_numbers_for_vehicles_with_colour":
                        ShowRegistrationNumbersByColor();
                        break;
                    case "slot_numbers_for_vehicles_with_colour":
                        ShowSlotNumbersByColor();
                        break;
                    case "slot_number_for_registration_number":
                        ShowSlotNumberByRegistrationNumber();
                        break;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid command. Please try again.");
                        break;
                }
            }
        }

        private static void InitializeParkingLot()
        {
            totalLots = 0;
            parkingLot = new List<Vehicle>();
        }

        private static void CreateParkingLot()
        {
            Console.Write("Enter the total number of parking lots: ");
            if (int.TryParse(Console.ReadLine(), out int lots) && lots > 0)
            {
                totalLots = lots;
                parkingLot = Enumerable.Repeat<Vehicle>(null, totalLots).ToList();
                Console.WriteLine($"Created a parking lot with {totalLots} slots.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number of parking lots.");
            }
        }

        private static void ParkVehicle()
        {
            if (totalLots == 0)
            {
                Console.WriteLine("You must create a parking lot first.");
                return;
            }

            Console.Write("Enter vehicle details (e.g., Registration No Colour Type): ");
            string[] vehicleDetails = Console.ReadLine().Split(' ');

            if (vehicleDetails.Length != 4)
            {
                Console.WriteLine("Invalid input. Please enter vehicle details as 'Registration No Colour Type'.");
                return;
            }

            string registrationNo = vehicleDetails[0];
            string color = vehicleDetails[1];
            string type = vehicleDetails[2];

            Vehicle vehicle = new Vehicle(registrationNo, color, type);

            int slotNumber = parkingLot.FindIndex(slot => slot == null);
            if (slotNumber >= 0)
            {
                parkingLot[slotNumber] = vehicle;
                Console.WriteLine($"Allocated slot number: {slotNumber + 1}");
            }
            else
            {
                Console.WriteLine("Sorry, parking lot is full.");
            }
        }

        private static void LeaveParkingLot()
        {
            if (totalLots == 0)
            {
                Console.WriteLine("You must create a parking lot first.");
                return;
            }

            Console.Write("Enter the slot number to leave: ");
            if (int.TryParse(Console.ReadLine(), out int slotNumber) && slotNumber > 0 && slotNumber <= totalLots)
            {
                int index = slotNumber - 1;
                if (parkingLot[index] != null)
                {
                    parkingLot[index] = null;
                    Console.WriteLine($"Slot number {slotNumber} is free.");
                }
                else
                {
                    Console.WriteLine($"Slot number {slotNumber} is already empty.");
                }
            }
            else
            {
                Console.WriteLine("Invalid slot number. Please enter a valid slot number.");
            }
        }

        private static void ShowParkingStatus()
        {
            if (totalLots == 0)
            {
                Console.WriteLine("You must create a parking lot first.");
                return;
            }

            Console.WriteLine("Slot No.\tRegistration No\t\tColour\tType");
            for (int i = 0; i < totalLots; i++)
            {
                if (parkingLot[i] != null)
                {
                    Vehicle vehicle = parkingLot[i];
                    Console.WriteLine($"{i + 1}\t\t{vehicle.RegistrationNo}\t\t{vehicle.Color}\t{vehicle.Type}");
                }
            }
        }

        private static void ShowTypeOfVehicles()
        {
            if (totalLots == 0)
            {
                Console.WriteLine("You must create a parking lot first.");
                return;
            }

            Console.Write("Enter vehicle type (e.g., Motor or Mobil): ");
            string type = Console.ReadLine().Trim().ToLower();

            int count = parkingLot.Count(vehicle => vehicle != null && vehicle.Type.ToLower() == type);
            Console.WriteLine(count);
        }

        private static void ShowOddPlateNumbers()
        {
            if (totalLots == 0)
            {
                Console.WriteLine("You must create a parking lot first.");
                return;
            }

            string oddPlateNumbers = string.Join(", ",
                parkingLot.Where(vehicle => vehicle != null && IsOddPlateNumber(vehicle.RegistrationNo))
                    .Select(vehicle => vehicle.RegistrationNo));
            Console.WriteLine(oddPlateNumbers);
        }

        private static void ShowEvenPlateNumbers()
        {
            if (totalLots == 0)
            {
                Console.WriteLine("You must create a parking lot first.");
                return;
            }

            string evenPlateNumbers = string.Join(", ",
                parkingLot.Where(vehicle => vehicle != null && IsEvenPlateNumber(vehicle.RegistrationNo))
                    .Select(vehicle => vehicle.RegistrationNo));
            Console.WriteLine(evenPlateNumbers);
        }

        private static void ShowRegistrationNumbersByColor()
        {
            if (totalLots == 0)
            {
                Console.WriteLine("You must create a parking lot first.");
                return;
            }

            Console.Write("Enter vehicle color: ");
            string color = Console.ReadLine().Trim().ToLower();

            string registrationNumbers = string.Join(", ",
                parkingLot.Where(vehicle => vehicle != null && vehicle.Color.ToLower() == color)
                    .Select(vehicle => vehicle.RegistrationNo));
            Console.WriteLine(registrationNumbers);
        }

        private static void ShowSlotNumbersByColor()
        {
            if (totalLots == 0)
            {
                Console.WriteLine("You must create a parking lot first.");
                return;
            }

            Console.Write("Enter vehicle color: ");
            string color = Console.ReadLine().Trim().ToLower();

            string slotNumbers = string.Join(", ",
                parkingLot.Select((vehicle, index) =>
                        vehicle != null && vehicle.Color.ToLower() == color ? (index + 1).ToString() : "")
                    .Where(s => !string.IsNullOrWhiteSpace(s)));
            Console.WriteLine(slotNumbers);
        }

        private static void ShowSlotNumberByRegistrationNumber()
        {
            if (totalLots == 0)
            {
                Console.WriteLine("You must create a parking lot first.");
                return;
            }

            Console.Write("Enter registration number: ");
            string registrationNo = Console.ReadLine().Trim().ToUpper();

            int slotNumber =
                parkingLot.FindIndex(vehicle => vehicle != null && vehicle.RegistrationNo == registrationNo);
            if (slotNumber >= 0)
            {
                Console.WriteLine($"Slot number: {slotNumber + 1}");
            }
            else
            {
                Console.WriteLine("Not found.");
            }
        }

        private static bool IsOddPlateNumber(string registrationNo)
        {
            char lastDigit = registrationNo.Last();
            return "13579".Contains(lastDigit);
        }

        private static bool IsEvenPlateNumber(string registrationNo)
        {
            char lastDigit = registrationNo.Last();
            return "02468".Contains(lastDigit);
        }
    }

    internal class Vehicle
    {
        public string RegistrationNo { get; }
        public string Color { get; }
        public string Type { get; }

        public Vehicle(string registrationNo, string color, string type)
        {
            RegistrationNo = registrationNo;
            Color = color;
            Type = type;
        }
    }
}