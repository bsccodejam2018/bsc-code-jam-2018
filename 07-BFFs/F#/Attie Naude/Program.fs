open System
open System.IO
open BFFs

let parseInput lines =
    let rec processNextScenario (lines: string[]) scenarios =
        match lines with
        | [||] -> scenarios
        | ls -> 
            let bffMap = ls.[1].Split(" ") |> Array.mapi (fun i bI -> (i+1, bI |> int)) |> Map
            let remainder = ls.[2..]

            processNextScenario remainder (bffMap :: scenarios)

    processNextScenario lines [] |> List.rev |> List.toArray

let printResult i r =
    sprintf "Case #%i: %i" (i+1) r

let writeAllLines filePath rows = 
    File.WriteAllLines(filePath, rows)

[<EntryPoint>]
let main argv =
    File.ReadAllLines @"..\..\Input\C-small-practice.in"
    |> Array.skip 1
    |> parseInput
    |> Array.map getLargestPossibleCircleSize
    |> Array.mapi printResult
    |> writeAllLines @"..\..\Output\C-small-practice.out"

    File.ReadAllLines @"..\..\Input\C-large-practice.in"
    |> Array.skip 1
    |> parseInput
    |> Array.map getLargestPossibleCircleSize
    |> Array.mapi printResult
    |> writeAllLines @"..\..\Output\C-large-practice.out"

    0 // return an integer exit code