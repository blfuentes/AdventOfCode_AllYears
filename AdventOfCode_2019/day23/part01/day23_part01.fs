﻿module day23_part01

open Intcode

open AdventOfCode_2019.Modules

let rec provideInput inputs =
    match inputs with
    | x :: xs ->
        function
        | Input f -> f x |> provideInput xs
        | Output (o1, s) -> Output (o1, provideInput inputs s)
        | Halted -> failwith "Halted while still providing input"
    | [] -> id

type Packet = { Dest : int; X : int64; Y : int64 }

let rec readPackets =
    function
    | Output (dest :: x :: y :: xs, s) ->
        let ps, p = readPackets (if xs = [] then s else Output (xs, s))
        { Dest = int dest; X = x; Y = y } :: ps, p
    | p -> [], p

[<NoComparison>]
[<NoEquality>]
type Network =
    { Computers : Map<int, ProgramState> 
      NatPacket : Packet option 
      PrevSentNat : Packet option }

    static member sendPacket packet network =
        if packet.Dest = 255 then { network with NatPacket = Some packet }
        else
            match Map.tryFind packet.Dest network.Computers with
            | Some comp ->
                let comp' = provideInput [packet.X; packet.Y] comp
                { network with Computers = Map.add packet.Dest comp' network.Computers }
            | None -> network

    static member sendReceiveAllPackets network =
        let afterReading = Map.map (fun _ -> readPackets) network.Computers
        let packets = Map.toList afterReading |> List.collect (snd >> fst)
        let comps = Map.map (fun _ -> snd) afterReading

        let comps' =
            if packets.Length = 0 then
                Map.map (fun _ -> provideInput [-1L]) comps
            else comps

        ({ network with Computers = comps' }, packets)
        ||> List.fold (fun n p -> Network.sendPacket p n)

    static member isIdle network =
        network.Computers
        |> Map.forall (fun _ v -> match v with Input _ -> true | _ -> false)

    static member create intcode =
        let comp = Computer.create intcode |> run
        let comps = Array.init 50 (fun i -> i, comp |> provideInput [int64 i]) |> Map.ofArray
        { Computers = comps; NatPacket = None; PrevSentNat = None }

let rec searchUntilFirstNat network =
    match network.NatPacket with
    | Some pkt -> pkt.Y
    | None -> Network.sendReceiveAllPackets network |> searchUntilFirstNat

let execute =
    let path = "day23/day23_input.txt"
    let content = (LocalHelper.GetContentFromFile path).Split(",") |> Array.map int64
    Network.create content |> searchUntilFirstNat