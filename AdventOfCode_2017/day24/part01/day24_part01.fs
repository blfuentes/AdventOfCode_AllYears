module day24_part01

open AdventOfCode_2017.Modules
open System.Collections.Generic

type Port = {
    Name: int
}

type Connection = {
    From: Port
    To: Port
}

let parseContent (lines: string array) =
    let connections = HashSet<Connection>()
    let ports = ResizeArray<Port>()

    lines
    |> Array.iter(fun l ->
        let (pfrom, pto) = 
            (
                { Name = (int)(l.Split("/")[0]) },
                { Name = (int)(l.Split("/")[1]) }
            )
        connections.Add({ From = pfrom; To = pto }) |> ignore

        if not (ports.Contains(pfrom)) then
            ports.Add(pfrom)
        if not (ports.Contains(pto)) then
            ports.Add(pto)
    )
    (ports, connections |> List.ofSeq)

let strength(connections: Connection list) =
    connections
    |> List.sumBy(fun c -> c.From.Name + c.To.Name)

let maxStrength(bridges: Connection list seq) =
    bridges
    |> Seq.map strength
    |> Seq.max

let rec buildBridges (bridge: Connection list) (nextPort: Port) (connections: Connection Set) =
    seq { 
        yield bridge
        let bridgeable = Set.filter (fun c -> c.From = nextPort || c.To = nextPort) connections
        for comp in bridgeable do
            let newNextPort = 
                if comp.To = nextPort then 
                    comp.From
                else comp.To
            yield! buildBridges (comp :: bridge) newNextPort (Set.remove comp connections) 
    }

let findMaxStrength(connections: Connection list) =
    let bridges = buildBridges [] { Name = 0 } (connections |> Set.ofList)
    maxStrength bridges

let execute() =
    let path = "day24/day24_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (_, connections) = parseContent content

    findMaxStrength connections