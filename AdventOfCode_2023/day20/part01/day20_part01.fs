module day20_part01

open AdventOfCode_2023.Modules
open FSharpx.Collections
open FParsec

let flip f = fun x y -> f y x

let rec multirepeat f =
    seq {
        yield f ()
        yield! multirepeat f
    }

let repeat x = multirepeat (fun () -> x)

type FlipFlopGate =
    { 
        state: bool
        outputs: list<string> 
    }

    member this.Toggle = { this with state = not this.state }

type NandGate =
    { 
        inputStates: Map<string, bool>
        outputStates: list<string> 
    }

    member this.Update k v = { this with inputStates = Map.add k v this.inputStates }

type Broadcaster = { destinations: list<string> }
type Button = { destinations: list<string> }

type Pulse = { 
    inputs: string;
    outputs: string;
    value: bool;
}

let pulseButton = { inputs = "button"; outputs = "broadcaster"; value = false }

type Component =
    | FlipFlop of FlipFlopGate
    | Inverter of NandGate
    | Broadcaster of Broadcaster
    | Button of Button
    | NoOp

    member this.Destinations =
        match this with
        | FlipFlop { outputs = destinations } -> destinations
        | Inverter { outputStates = destinations } -> destinations
        | Broadcaster { destinations = destinations } -> destinations
        | Button { destinations = destinations } -> destinations
        | NoOp -> []

    member this.PulseState =
        match this with
        | FlipFlop { state = state } -> state
        | Inverter { inputStates = sourceStates } -> Map.values sourceStates |> Seq.forall id |> not
        | Broadcaster _ -> false
        | Button _ -> false
        | NoOp -> failwith "Upexpected"

    member this.SendPulse source destination =
        { inputs = source
          outputs = destination
          value = this.PulseState }

    member this.ProcessPulse(p: Pulse) =
        let newComp, sendPulses =
            match this with
            | FlipFlop ff when not p.value -> ff.Toggle |> FlipFlop, true
            | FlipFlop _ -> this, false
            | Inverter inv -> inv.Update p.inputs p.value |> Inverter, true
            | NoOp -> this, false
            | _ -> this, true

        newComp,
        if sendPulses then
            List.map (fun d -> newComp.SendPulse p.outputs d) (this.Destinations)
        else
            []

type PulseQueue = Queue<Pulse>

let parseLabel: Parser<string, string> = many1Chars asciiLetter

let parseDestinations: Parser<list<string>, string> =
    sepBy1 parseLabel (skipString ", ")

let parseArrow = skipString " -> "

let parseBroadcaster: Parser<string * Component, string> =
    parse {
        let! destLabels = skipString "broadcaster" >>. parseArrow >>. parseDestinations
        return "broadcaster", Broadcaster { destinations = destLabels }
    }

let parseFlipFlop: Parser<string * Component, string> =
    parse {
        let! compName = pchar '%' >>. parseLabel
        let! destLabels = parseArrow >>. parseDestinations

        return
            compName,
            FlipFlop
                { state = false
                  outputs = destLabels }
    }

let parseInverter: Parser<string * Component, string> =
    parse {
        let! compName = pchar '&' >>. parseLabel
        let! destLabels = parseArrow >>. parseDestinations

        return
            compName,
            Inverter
                { inputStates = Map.empty
                  outputStates = destLabels }
    }

let parseComponent: Parser<string * Component, string> =
    List.map attempt [ parseFlipFlop; parseInverter; parseBroadcaster ] |> choice

let runParser p s =
    match runParserOnString p "" "" s with
    | Success(res, _, _) -> res
    | _ -> failwith "parse error"

let connectInverters (messages: Map<string, Component>) =
    seq {
        for label, comp in Map.toSeq messages do
            let comp' =
                match comp with
                | Inverter inv ->
                    let sources =
                        seq {
                            for k, v in Map.toSeq messages do
                                if List.contains label (v.Destinations) then
                                    yield k
                        }

                    Inverter
                        { inv with inputStates = Seq.zip sources (repeat false) |> Map }
                | _ -> comp

            yield label, comp'
    }
    |> Map

let nextPush (messages: Map<string, Component>) (queue: Queue<Pulse>) =
    match Queue.tryUncons queue with
    | Some(p, queue') ->
        let comp', pulses =
            (Map.tryFind p.outputs messages |> Option.defaultValue NoOp).ProcessPulse p

        Some(p, (Map.add p.outputs comp' messages, List.fold (flip Queue.conj) queue' pulses))
    | None -> None

let continuePushing (messages: Map<string, Component>) =
    let rec push (timepushed: int) (msgs: Map<string, Component>) (queue: Queue<Pulse>) =
        match nextPush msgs queue with
        | Some(p, (m', q')) ->
            seq {
                yield (timepushed, p)
                yield! push timepushed m' q'
            }
        | None -> push (timepushed + 1) msgs (Queue.conj pulseButton queue)

    push 0 messages Queue.empty

let findDestinationButtons (messages: Map<string, Component>) dest =
    continuePushing messages
    |> Seq.filter (snd >> fun p -> p.outputs = dest && (not p.value))
    |> Seq.head
    |> fst

let runComponents (messages: Map<string, Component>) =
    continuePushing messages
    |> Seq.takeWhile (fst >> (>=) 1000)
    |> Seq.groupBy (snd >> _.value)
    |> Seq.map (snd >> Seq.length)
    |> Seq.fold (*) 1

let parseInput (lines: string seq) =
    Seq.map (runParser parseComponent) lines 
    |> Map 
    |> connectInverters

let execute =
    let path = "day20/day20_input.txt"
    let msg = LocalHelper.GetLinesFromFile path |> parseInput
    runComponents msg
    