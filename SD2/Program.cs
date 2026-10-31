using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
public class BusStopNode
{
    public string Pavadinimas { get; set; }
    public TimeSpan Laikas { get; set; }
    public BusStopNode Kita { get; set; }
}

public class BusRoute
{ 
    public int ID { get; set; }
    public int Numeris {  get; set; }
    public string Kryptis { get; set; }
    public BusStopNode PirmaStotele { get; set; }
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
    public TimeSpan laikas { get; set; }
}

public class ScheduleManager//skaitom is json pagrindinio, is papildomo json, bei updatinam jsona
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
        var routeDTO = System.Text.Json.JsonSerializer.Deserialize<List<BusRouteDTO>>(data, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,//jeigu jsone is mazosios property name, o busrouteDTO is didziosios, tai programa nekreips i tai demesi ir priskirs attinkamas reiksmes, vietoj null
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
            BusStopNode pirmaStot = null!;
            BusStopNode paskutineStot = null!;

            if (dto.stoteles.Count > 0)
            {
                foreach (var t in dto.stoteles)
                {
                    var stotNode = new BusStopNode {

                        Pavadinimas = t.pavadinimas,
                        Laikas = t.laikas,
                        Kita = null!,
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
    //public static Schedule AddingExtraData(string fileHome)
    //{
    //   var newS = new Schedule();
    //    if (!File.Exists(fileHome))
    //    {
    //        Console.WriteLine("JSON file is not found");
    //        return newS;
    //    }

    //    string duom = File.ReadAllText(fileHome);
    //    var extraDTO = System.Text.Json.JsonSerializer.Deserialize<List<BusRouteDTO>>(duom, new JsonSerializerOptions
    //    {
    //        PropertyNameCaseInsensitive = true,
    //    });
    //    if(extraDTO == null)
    //    {
    //        return newS;
    //    }
        
    //    //foreach()
    //    //{

    //    //}
    //}
}

public class Schedule//trinam, redaguojam, pridedam reisus ir stoteles
{
    public LinkedList<BusRoute> Reisai = new LinkedList<BusRoute>();

    public void PridetiReisa(BusRoute newRoute)
    {
        if(Reisai.First == null)
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

    public void IstrintiStotele(int id, string name)
    {
        foreach (var r in Reisai)
        {
            if (r.ID == id)
            {
                BusStopNode cur = r.PirmaStotele;
                BusStopNode prev = null!;
                while(cur != null)
                {
                    if(cur.Pavadinimas == name)
                    {
                        if(prev == null)
                        {
                            r.PirmaStotele = cur.Kita;
                        }
                        else
                        {
                            prev.Kita = cur.Kita;
                        }
                        return;
                    }
                    prev = cur;
                    cur = cur.Kita;
                }

            }
        }
    }

    public bool RedaguotiReisoLaika(int id, TimeSpan time, TimeSpan newTime, string stop)
    {
        foreach(var r in Reisai)
        {
            if(r.ID == id)
            {
                BusStopNode possibleDub = r.PirmaStotele;
                while (possibleDub != null)
                {
                    if(possibleDub.Pavadinimas.Equals(stop, StringComparison.OrdinalIgnoreCase) && time == newTime)
                    {
                        Console.WriteLine("Can't change time for this stop because it's identical to the one before.");
                        return false;
                    }
                    possibleDub = possibleDub.Kita;
                }

                BusStopNode cur = r.PirmaStotele;
                while (cur != null)
                {
                    if (cur.Laikas == time)
                    {
                        cur.Laikas = newTime;
                        return true;
                    }
                    cur = cur.Kita;
                }
            }
        }
        return false;
    }

    public bool RedauguotiStotelesPavadinima(int id, string oldName, string newName)
    {
        foreach(var r in Reisai)
        {
            if(r.ID == id)
            {
                BusStopNode possibleDub = r.PirmaStotele;
                while (possibleDub != null)
                {
                    if(possibleDub.Pavadinimas.Equals(oldName, StringComparison.OrdinalIgnoreCase))//tikrina stringus, tarsi jie aprasyti is didziosios, ir tikrina po baita su baitu
                    {
                        Console.WriteLine("Your bus stop name already exists in list, can't change.");
                        
                        return false;//aptiktas dublikatas
                    }
                    possibleDub = possibleDub.Kita;
                }

                BusStopNode cur = r.PirmaStotele;
                while(cur != null)
                {
                    if(cur.Pavadinimas == oldName)
                    {
                        cur.Pavadinimas = newName;
                        return true;
                    }
                    cur = cur.Kita;
                }
            }
            
        }
        return false;
    }

}

public class UIManager//rodom reisu ir stoteliu info, greiciausiu reisu paieska
{
    private Schedule _tvarkarastis;
    public UIManager(Schedule tvarkarastis)//konstruktorius
    {
        _tvarkarastis = tvarkarastis;
    }

    public void RodytiVisuReisuID()
    {
        var cur = _tvarkarastis.Reisai.First;
        Console.WriteLine("Visi reisai");
        while (cur != null)
        {
            Console.WriteLine($"Marsruto ID:{cur.Value.ID} - {cur.Value.Numeris} autobusas");
            cur= cur.Next;
        }
    }

    public void RodytiReisoInfo(int id)
    {
        BusRoute found = null!;
        var cur = _tvarkarastis.Reisai.First;
        while (cur != null)
        {
            if (cur.Value.ID == id)
            {
                found = cur.Value;
                break;
            }
            cur = cur.Next;
        }
        Console.WriteLine($"Reisas Nr.{found.Numeris}: {found.Kryptis}");//$ leidzia kintamuosius "" rasyt
        Console.WriteLine("Stoteles:");

        var stop = found.PirmaStotele;
        while (stop != null){ 
            Console.WriteLine($" -> {stop.Pavadinimas} {stop.Laikas}");
            stop = stop.Kita;
        }
    }


}
    class Program//paleidzia visa programa, kazkiek basic ui
    {
        static void Main(string[] args)
        {
            Schedule tvarkarastis = new Schedule();//sukuriam objekta
            tvarkarastis = ScheduleManager.ReadingFromJSON("busdata.json");//i ta tvarkarasti per schedule manageri irasom duomenis is json failo
            UIManager UserUi = new UIManager(tvarkarastis);//sukuriam userui, kuriam per konstruktoriu perduodamas tvarkarastis, ir userui gali vaizduot jo info

            Console.WriteLine("Welcome back to BusTracker! What would you like to do?");
            Console.WriteLine("1. Valdyti tvarkaraštį.");
            Console.WriteLine("2. Perziureti stoteles tvarkarasti.");
            Console.WriteLine("3. Search for fastest bus route.");
            Console.WriteLine("4. Peržiūrėti pasirinkto reiso informaciją. ");
            Console.WriteLine("5. Exit program - Press Escape.");

            ConsoleKeyInfo currentInputKey;
            while (true)
            {
                currentInputKey = Console.ReadKey();
                if (currentInputKey.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine(" - Exiting program...");
                    System.Environment.Exit(0);
                }
                else if (currentInputKey.Key == ConsoleKey.D1)
                {
                    Console.WriteLine(" - You chose first option.");
                    //
                    string file = "RewritingData.json";
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(tvarkarastis);
                    File.WriteAllText(file, jsonString);
                    Console.WriteLine("Įrašyta sėkmingai!");
                    //
                }
                else if (currentInputKey.Key == ConsoleKey.D2)
                {
                    Console.WriteLine(" - You chose second option.");
                }
                else if (currentInputKey.Key == ConsoleKey.D3)
                {
                    Console.WriteLine(" - You chose third option.");
                }
                else if (currentInputKey.Key == ConsoleKey.D4)
                {
                    Console.WriteLine(" - You chose fourth option.");
                    Console.WriteLine("Perziurekite visu reisu ID ir parasykite kurio informacija norite pamatyti");
                    UserUi.RodytiVisuReisuID();
                    int choice = int.Parse(Console.ReadLine());
                    UserUi.RodytiReisoInfo(choice);
                }
                else
                {
                    Console.WriteLine("Choose again.");
                }
                
            }
        }
    }
