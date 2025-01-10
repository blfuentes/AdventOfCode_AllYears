module day19_part02

open AdventOfCode_2021.Modules
open System

type Axis = XPos | XNeg | YPos | YNeg | ZPos | ZNeg
type Rotation = Rot0 | Rot90 | Rot180 | Rot270

let transform up rotation (x, y, z) =
    let (ox, oy, oz) =
        match up with
        | YPos -> x, y, z
        | YNeg -> x, -y, -z
        | XPos -> y, x, -z
        | XNeg -> y, -x, z
        | ZPos -> y, z, x
        | ZNeg -> y, -z, -x

    match rotation with
    | Rot0 -> ox, oy, oz
    | Rot90 -> oz, oy, -ox
    | Rot180 -> -ox, oy, -oz
    | Rot270 -> -oz, oy, ox

let rots = [ Rot0; Rot90; Rot180; Rot270 ]
let axes = [ XPos; XNeg; YPos; YNeg; ZPos; ZNeg ]
let axesRots = List.allPairs axes rots

let sum (x1, y1, z1) (x2, y2, z2) = (x1 + x2, y1 + y2, z1 + z2)
let difference (x1, y1, z1) (x2, y2, z2) = (x1 - x2, y1 - y2, z1 - z2)

let align s1 s2 =
    axesRots
    |> Seq.tryPick (fun (axis, rot) ->
        let rotatedS2 = Seq.map (transform axis rot) s2

        Seq.allPairs s1 rotatedS2
        |> Seq.tryPick (fun (b1, b2) ->
            let delta = difference b1 b2
            let alignedS2 = Seq.map (sum delta) rotatedS2
            let intersection = Seq.filter (fun b -> Set.contains b s1) alignedS2

            if (Seq.truncate 12 intersection |> Seq.length) = 12 then
                Some(Set.ofSeq alignedS2, delta, axis, rot)
            else
                None))

let rec reduce (scannersBeacons: (Set<int * int * int> * Set<int * int * int>) list) =
    let rec merge (s1, b1) (accScanners, accBeacons) rem unmerged =
        match rem with
        | [] -> accScanners, accBeacons, unmerged
        | (s2, b2) :: rest ->
            match align b1 b2 with
            | None -> merge (s1, b1) (accScanners, accBeacons) rest ((s2, b2) :: unmerged)
            | Some (alignedBeacons, delta, axis, rot) ->
                let alignedScanners = Set.map (transform axis rot >> sum delta) s2
                let newScanners = Set.union alignedScanners accScanners
                let newBeacons = Set.union alignedBeacons accBeacons
                merge (s1, b1) (newScanners, newBeacons) rest unmerged

    let scanners, beacons, unmerged =
        merge (List.head scannersBeacons) (List.head scannersBeacons) (List.tail scannersBeacons) []

    match unmerged with
    | [] -> scanners, beacons
    | _ ->
        reduce ((scanners, beacons) :: unmerged)

let ScannersAndBeacons (scans: Set<int*int*int> list) =
    let (scanners, beacons) =
        scans
        |> List.map (fun scans -> Set.singleton (0, 0, 0), scans)
        |> reduce
    (scanners, beacons)

let parseContent(lines: string array) =
    lines
    |> Seq.filter (fun line -> not (String.IsNullOrWhiteSpace(line)))
    |> Seq.fold
        (fun acc line ->
            if line.StartsWith("---") then
                Set.empty :: acc
            else
                let pos =
                    line.Split(',')
                    |> fun arr -> int arr[0], int arr[1], int arr[2]

                (Set.add pos (List.head acc)) :: (List.tail acc))
        []
    |> List.rev

let manhattan a b =
    difference a b
    |> fun (dx, dy, dz) -> abs dx + abs dy + abs dz

let execute =
    let path = "day19/day19_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let scanInfo = parseContent content

    let (scanners, _) = ScannersAndBeacons scanInfo
    Seq.allPairs scanners scanners
    |> Seq.map (fun (a, b) -> manhattan a b)
    |> Seq.max