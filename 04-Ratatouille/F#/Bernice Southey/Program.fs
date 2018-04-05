open System
open System.IO

let lines = File.ReadAllLines(@"..\..\..\..\..\Input\B-large-practice.in")
let countline = lines |> Array.take 1
let count = countline.[0] |> int

let getPacketServiceRange (packet:float) (serving:float) =
    let min = packet / serving / 1.1 |> ceil |> int
    let max = packet / serving / 0.9 |> floor |> int
    seq { for i in min .. max do yield i } |> Set

let rec last = function
    | hd :: [] -> hd
    | hd :: tl -> last tl
    | _ -> 0

let getPacketServiceRanges p (packets:list<string>) serving =
        seq { for i in 1 .. p do yield getPacketServiceRange (packets.[i-1] |> float) serving } |>Seq.filter(fun x -> x.Count > 0) |> Seq.sortBy(fun x -> (last (x|> Set.toList), (x |> Set.toList |> List.take 1) )) |> Seq.toList

let getAllPacketServiceRanges n p (packets:list<string>) (recipe:list<string>) =
        seq { for i in 1 .. n do yield getPacketServiceRanges p (packets.[i-1].Split [|' '|] |> Array.toList) (recipe.[i-1] |> float) } |> Seq.toList
        
let rec intersect (ranges:list<Set<int>>) =
    if ranges.Length = 1 then ranges
    else    
        let inter = Set.intersect ranges.[0] ranges.[1]
        if inter.Count = 0 then []
        else intersect (inter::(ranges |> List.skip 2))

let rec abacus n (ranges:list<list<Set<int>>>) (count:int) =
    if Seq.exists (fun (i:list<Set<int>>) -> i.Length = 0) (seq { for i in 1 .. n do yield ranges.[i-1] }) then count
    else
        let sortedRanges = ranges |> List.sortBy(fun x -> last (x.[0] |> Set.toList))
        let column = seq { for i in 1 .. n do yield sortedRanges.[i-1].[0] } |> Seq.toList
        let result = intersect column
        if result.Length > 0 then abacus n ((seq { for i in 1 .. n do yield (sortedRanges.[i-1] |> List.skip 1) }) |> Seq.toList) (count+1)
        else 
            let adjusted = (sortedRanges.[0] |> List.skip 1)::(sortedRanges |> List.skip 1)
            abacus n adjusted count

let calculate2 (lines:list<string>) =
    let line1 = lines.[0].Split [|' '|]
    let n = line1.[0] |> int
    let p = line1.[1] |> int
    let recipe = lines.[1].Split [|' '|] |> Array.toList
    let packets = lines |> List.skip 2 |> List.take n
    let ranges = getAllPacketServiceRanges n p packets recipe
    let result = abacus n ranges 0
    (result, n+2)

let rec calculate (lines:list<string>) output =
    if lines.Length = 0 then List.rev output
    else
        let result = calculate2 lines 
        calculate (lines |> List.skip (snd result)) ((fst result)::output)


let format result i =
    String.Format("CASE #{0}: {1}", i + 1 |> string, result)

let output = calculate (lines |> Array.skip 1 |> Array.toList) [] |> Seq.mapi(fun i x ->format x i) |> Seq.toList

File.WriteAllLines(@"..\..\..\..\..\Output\A-large-practice.out", (output))






