open System
open System.IO

let getSolution (rows: string[]) =
    let pos1 = rows.[0] |> int
    let cards1 = rows.[pos1].Split(" ") |> Seq.map int |> Set
    let pos2 = rows.[5] |> int
    let cards2 = rows.[pos2+5].Split(" ") |> Seq.map int |> Set

    match Set.intersect cards1 cards2 |> Set.toArray with
        | [||] -> "Volunteer cheated!"
        | a when a.Length = 1 -> a.[0] |> string
        | _ -> "Bad magician!"

let processProblems rows =
    let rec processNextBatch rows solutions =
        match rows with
        | [||] -> solutions
        | rs ->  getSolution rs.[0..9] :: solutions |> processNextBatch rs.[10..]

    processNextBatch rows [] |> List.rev |> List.toArray

let printRow index =
    sprintf "CASE #%i: %s" (index+1)

let writeAllLines filePath rows = 
    File.WriteAllLines(filePath, rows)

[<EntryPoint>]
let main argv =
    File.ReadAllLines @"..\..\..\Input\A-small-practice.in"
    |> Array.skip 1
    |> processProblems
    |> Array.mapi printRow
    |> writeAllLines @"..\..\..\Output\A-small-practice.out"

    0 // return an integer exit code