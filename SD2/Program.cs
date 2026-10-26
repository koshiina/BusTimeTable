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
    public string puk = "Hello";
    static void Main(string[] args)//galima spausdint kazka konsolej tik per si metoda!!!
    {
        Manager Mymanager = new Manager();
        Console.WriteLine("Sveiki atvyke i Bus Trackeri! Ka noretumete daryti?");
        Console.WriteLine("1. Valdyti tvarkarasti.");
        Console.WriteLine("2. Perziureti stoteles tvarkarasti.");
        Console.WriteLine("3. Ieskoti greiciausio reiso.");
        Console.WriteLine("4. Iseiti is programos.");

        Console.ReadLine();
    }
}
//#budas kaip useris galetu iseiti is programos, t.y. paspaudzius ant escape mygtuko ant keyboardo

//ConsoleKeyInfo CurrentInputKey; 
//while (true)
//{
//    // Your Other Codes
//    Console.WriteLine("\rHello C# Developer.");
//    CurrentInputKey = Console.ReadKey();

//    if (CurrentInputKey != null && CurrentInputKey.Key == ConsoleKey.Escape)
//    {
//        Console.WriteLine("Exit ...");
//        System.Environment.Exit(0);
//    }
//}

//#arba antras budas, tai kaip input key padaryt tipo choice kaip per os ir tada su switch tikrinti.



