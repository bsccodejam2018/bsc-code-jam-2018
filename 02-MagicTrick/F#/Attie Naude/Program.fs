open System
open System.IO
open MagicTrick

let processScenarios rows =
    let rec processNextScenario rows solutions =
        match rows with
        | [||] -> solutions
        | rs ->  getChosenCard rs.[0..9] :: solutions |> processNextScenario rs.[10..]

    processNextScenario rows [] |> List.rev |> List.toArray

let printRow index =
    sprintf "CASE #%i: %s" (index+1)

let writeAllLines filePath rows = 
    File.WriteAllLines(filePath, rows)

[<EntryPoint>]
let main argv =
    File.ReadAllLines @"..\..\Input\A-small-practice.in"
    |> Array.skip 1
    |> processScenarios
    |> Array.mapi printRow
    |> writeAllLines @"..\..\Output\A-small-practice.out"

    0 // return an integer exit code