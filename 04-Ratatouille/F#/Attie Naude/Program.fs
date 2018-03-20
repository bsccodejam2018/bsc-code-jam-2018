open System
open System.IO
open Ratatouille

let parseInput lines =
    let rec processNextScenario (lines: string[]) scenarios =
        match lines with
        | [||] -> scenarios
        | ls -> 
            let split = ls.[0].Split " "
            let ingredientCount = split.[0] |> int
            let packageCount = split.[1] |> int

            let scenario = {
                RequiredQuantities = ls.[1].Split " " |> Array.map int
                IngredientPackages = ls.[2..ingredientCount + 1] |> Array.map (fun a -> a.Split " " |> Array.map int)
            }
            let remainder = ls.[ingredientCount+2 ..]
            processNextScenario remainder (scenario :: scenarios)

    processNextScenario lines [] |> List.rev |> List.toArray

let printResult i n =
    sprintf "Case #%i: %i" (i+1) n

let writeAllLines filePath rows = 
    File.WriteAllLines(filePath, rows)

[<EntryPoint>]
let main argv =

    File.ReadAllLines @"..\..\Input\B-small-practice.in"
    |> Array.skip 1
    |> parseInput
    |> Array.map calculateKitCount
    |> Array.mapi printResult
    |> writeAllLines @"..\..\Output\B-small-practice.out"

    File.ReadAllLines @"..\..\Input\B-large-practice.in"
    |> Array.skip 1
    |> parseInput
    |> Array.map calculateKitCount
    |> Array.mapi printResult
    |> writeAllLines @"..\..\Output\B-large-practice.out"

    0 // return an integer exit code