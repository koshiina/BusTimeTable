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

public class BusRouteDTO
{
    public int marsrutoId { get; set; }
    public int numeris { get; set; }
    public string kryptis { get; set; }



}

public class BusStopDTO
{ 
    public string pavadinimas { get; set; }
    public string laikas { get; set; }
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
//File.WriteAllText bla bla json kad perrasyti i jsona atnaujinimus is programos
//var dataFromJSON = File.ReadAllText("data.json");
//skaitydami is json failo, pries tai sukuriam Data Transfer objektus, i kuriuos is pradziu
//bus skaitomi duomenys is json, o poto jau priskiriami og objektams
//nes og turi dvikrypcius, vienkrypciai listai, o jsone viskas kaip paprastas masyvas
//tai tikriausiai bus skitomi is jsono failo su tais objektais ir irasysim taip pat su jais

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
