open System
open System.IO
open MagicTrick

let processScenarios rows =
    let rec processNextScenario solutions rows =
        match rows with
        | [||] -> solutions
        | rs ->  getChosenCard rs.[0..9] :: processNextScenario solutions rs.[10..]

    rows |> processNextScenario [] |> List.rev |> List.toArray

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