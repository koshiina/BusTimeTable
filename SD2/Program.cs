using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
public class BusStopNode
{
    public string Pavadinimas { get; set; }
    public string Laikas { get; set; }
    public BusStopNode Next { get; set; }
}

public class BusRoute
{ 
    public int ID { get; set; }
    public int Numeris {  get; set; }
    public string Kryptis { get; set; }
    public BusStopNode PirmaStotele { get; set; }
    public BusRoute Ankstesnis { get; set; }
    public BusRoute Kitas { get; set; }

}
public class Schedule
{
    public BusRoute Pradzia { get; set; }
    public BusRoute Pabaiga { get; set; }


    public void PridetiReisa(BusRoute newRoute)
    {
        if (Pradzia == null)
        {
            Pradzia = newRoute;
            Pabaiga = newRoute;
        }
        else
        {

        }
    }
    
    public void PasalintiReisa()
    {

    }

}


class Manager
{
    static void Main(string[] args)//galima spausdint kazka konsolej tik per si metoda!!!
    {
        Manager Mymanager = new Manager();
        Console.WriteLine("Welcome back to BusTracker! What would you like to do?");
        Console.WriteLine("1. Manage schedule");
        Console.WriteLine("2. Perziureti stoteles tvarkarasti.");
        Console.WriteLine("3. Search for fastest bus route.");
        Console.WriteLine("4. smt else.");
        Console.WriteLine("5. Exit program - Press Escape.");

        ConsoleKeyInfo currentInputKey;
        while(true)
        {
            currentInputKey = Console.ReadKey();
            if(currentInputKey.Key == ConsoleKey.Escape)
            {
                Console.WriteLine(" - Exiting program...");
                System.Environment.Exit(0);
            }
            else if (currentInputKey.Key == ConsoleKey.D1)
            {
                Console.WriteLine(" - You chose first option.");
            }
            else if (currentInputKey.Key == ConsoleKey.D2)
            {
                Console.WriteLine(" - You chose second option.");
            }
            else if(currentInputKey.Key == ConsoleKey.D3)
            {
                Console.WriteLine(" - You chose third option.");
            }
            else if (currentInputKey.Key == ConsoleKey.D4)
            {
                Console.WriteLine(" - You chose fourth option.");
            }
            else
            {
                Console.WriteLine("Choose again.");
            }
        }
    }
}
//#arba antras budas, tai kaip input key padaryt tipo choice kaip per os ir tada su switch tikrinti.



