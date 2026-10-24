using System.Collections.Generic;
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
    public BusRoute Ankstesne { get; set; }
    public BusRoute Sekanti { get; set; }

}
//using (StreamReader r = new StreamReader("data.json"))
//{
//    string json = r.ReadToEnd();
    
//}




