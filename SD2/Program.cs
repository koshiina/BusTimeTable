using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
public class BusStopNode
{
    public string Pavadinimas { get; set; }
    public string Laikas { get; set; }
    public BusStopNode Kita { get; set; }
}

public class BusRoute
{ 
    public int ID { get; set; }
    public int Numeris {  get; set; }
    public string Kryptis { get; set; }
    public BusStopNode PirmaStotele { get; set; }
}

public class ScheduleManager
{
    //static metodas leidzia kitoms klasems naudot sios metodus, ne kuriant sios klases objekta(?)
    public static Schedule ReadingFromJSON(string fileHome)
    {
        var schedule = new Schedule();

        if (!File.Exists(fileHome))
        {
            Console.WriteLine("JSON file is not found.");
            return schedule;
        }

        //skaitydami is json failo, pries tai sukuriam Data Transfer objektus, i kuriuos is pradziu
        //bus skaitomi duomenys is json, o poto jau priskiriami og objektams
        //nes og turi dvikrypcius, vienkrypciai listai, o jsone viskas kaip paprastas masyvas
        //tai tikriausiai bus skitomi is jsono failo su tais objektais ir irasysim taip pat su jais
        string data = File.ReadAllText(fileHome);
        var routeDTO = JsonSerializer.Deserialize<List<BusRouteDTO>>(fileHome, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,//jeigu jsone is mazosios property name, o busrouteDTO is didziosios, tai 
            //programa nekreips i tai demesi ir priskirs attinkamas reiksmes, vietoj null
        });

        if (routeDTO == null)
        {
            return schedule;
        }

        foreach (var dto in routeDTO)
        {
            var rNode = new BusRoute
            {
                ID = dto.marsrutoId,
                Numeris = dto.numeris,
                Kryptis = dto.kryptis,

            };
            BusStopNode pirmaStot = null;
            BusStopNode paskutineStot = null;

            if (dto.stoteles.Count > 0)
            {
                foreach (var t in dto.stoteles)
                {
                    var stotNode = new BusStopNode {

                        Pavadinimas = t.pavadinimas,
                        Laikas = t.laikas,
                        Kita = null,
                    };

                    if (pirmaStot == null)
                    {
                        pirmaStot = stotNode;
                    }
                    else
                    {
                        paskutineStot.Kita = stotNode;
                    }
                    paskutineStot = stotNode;
                }
                rNode.PirmaStotele = pirmaStot;
            }
            schedule.PridetiReisa(rNode);
        }
        return schedule;
    }
    public void UpdateJSON()
    {

    }
}


public class BusRouteDTO
{
    public int marsrutoId { get; set; }
    public int numeris { get; set; }
    public string kryptis { get; set; }
    public List<BusStopDTO> stoteles { get; set; }
}

public class BusStopDTO
{ 
    public string pavadinimas { get; set; }
    public string laikas { get; set; }
}

public class Schedule
{
    public LinkedList<BusRoute> Reisai = new LinkedList<BusRoute>();

    public void PridetiReisa(BusRoute newRoute)
    {
        if(Reisai.First() == null)
        {
            Reisai.AddFirst(newRoute);
        }
        else
        {
            Reisai.AddLast(newRoute);
        }
    }    
    public void PasalintiReisa(int id)
    {
        var cur = Reisai.First;
        while (cur != null)
        {
            if (cur.Value.ID == id)
            {
                Reisai.Remove(cur);
                break;
            }
            cur = cur.Next;
        }
    }

}


class UIManager
{
    static void Main(string[] args)//galima spausdint kazka konsolej tik per si metoda!!!
    {
        UIManager MyManager = new UIManager();
        Console.WriteLine("Welcome back to BusTracker! What would you like to do?");
        Console.WriteLine("1. Manage schedule");
        Console.WriteLine("2. Perziureti stoteles tvarkarasti.");
        Console.WriteLine("3. Search for fastest bus route.");
        Console.WriteLine("4. Peržiūrėti pasirinkto reiso informaciją. ");
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
