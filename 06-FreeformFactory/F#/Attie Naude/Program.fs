open System
open System.IO
open FreeformFactory

let parseOperableMachines (machineCompatibilityString: string) =
    machineCompatibilityString.ToCharArray()
    |> Array.mapi (fun i v -> if v = '1' then Some(i+1) else None)
    |> Array.choose id
    |> Set

let parseInput lines =
    let rec processNextScenario (lines: string[]) scenarios =
        match lines with
        | [||] -> scenarios
        | ls -> 
            let machineCount = ls.[0] |> int
            let workers = 
                ls.[1..machineCount] 
                |> Array.mapi (fun workerIndex machineCompatibilityString -> { Id = workerIndex + 1; OperableMachines = parseOperableMachines machineCompatibilityString })
                |> Array.toList

            let remainder = ls.[machineCount+1..]
            processNextScenario remainder (workers :: scenarios)

    processNextScenario lines [] |> List.rev |> List.toArray

let printResult i r =
    sprintf "Case #%i: %i" (i+1) r

let writeAllLines filePath rows = 
    File.WriteAllLines(filePath, rows)

[<EntryPoint>]
let main argv =

    File.ReadAllLines @"..\..\Input\D-small-practice.in"
    |> Array.skip 1
    |> parseInput
    |> Array.map calculateMinTrainingCost
    |> Array.mapi printResult
    |> writeAllLines @"..\..\Output\D-small-practice.out"

    File.ReadAllLines @"..\..\Input\D-large-practice.in"
    |> Array.skip 1
    |> parseInput
    |> Array.map calculateMinTrainingCost
    |> Array.mapi printResult
    |> writeAllLines @"..\..\Output\D-large-practice.out"

    0 // return an integer exit code