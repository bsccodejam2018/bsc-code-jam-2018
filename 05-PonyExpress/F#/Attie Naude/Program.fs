open System
open System.IO
open PonyExpress
open System.Globalization

let parseInput lines =
    let rec processNextScenario (lines: string[]) scenarios =
        match lines with
        | [||] -> scenarios
        | ls -> 
            let scenarioDimensions = ls.[0].Split " " |> Array.map int
            let townCount = scenarioDimensions.[0]
            let caseCount = scenarioDimensions.[1]

            let horses = 
                ls.[1..townCount]
                |> Array.mapi (fun townIndex row -> row.Split " " |> Array.map float |> (fun horseValues -> (townIndex + 1, { Endurance = horseValues.[0] * 1.0<km>; Speed = horseValues.[1] * 1.0<km/hour>; VisitedCities = [] })))
                |> Map.ofArray

            let cityConnections =
                ls.[1+townCount..townCount*2]
                |> Array.mapi (fun sourceIndex row -> 
                    (
                        sourceIndex+1, 
                        row.Split " " 
                        |> Array.mapi (fun destinationIndex value -> ( destinationIndex+1, value |> (fun v -> (float)v * 1.0<km>)) )
                        |> Array.filter (fun (_, value) -> value > 0.0<km>)
                        |> Map.ofArray
                    )
                )
                |> Map.ofArray

            let scenarioRows = 
                ls.[ (1+townCount*2) .. (caseCount+townCount*2) ]
                |> Array.map (fun row -> row.Split " " |> Array.map int |> (fun vals -> (vals.[0], vals.[1]) ))
             
            let currentScenario =
                    {
                        CityConnections = cityConnections
                        Horses = horses
                        SearchPairs = scenarioRows
                    }
            
            let remainder = ls.[1+caseCount+townCount*2..]
            processNextScenario remainder (currentScenario :: scenarios)

    processNextScenario lines [] |> List.rev |> List.toArray

let printResult i (r: float[]) =
    sprintf "Case #%i: %s" (i+1) (String.Join(" ", r |> Seq.map (fun f -> f.ToString(CultureInfo.CreateSpecificCulture("en-GB")))))

let writeAllLines filePath rows = 
    File.WriteAllLines(filePath, rows)

[<EntryPoint>]
let main argv =

    File.ReadAllLines @"..\..\Input\C-small-practice.in"
    |> Array.skip 1
    |> parseInput
    |> Array.Parallel.map calculateMinRideTimes
    |> Array.mapi printResult
    |> writeAllLines @"..\..\Output\C-small-practice.out"

    File.ReadAllLines @"..\..\Input\C-large-practice.in"
    |> Array.skip 1
    |> parseInput
    |> Array.Parallel.map calculateMinRideTimes
    |> Array.mapi printResult
    |> writeAllLines @"..\..\Output\C-large-practice.out"

    0 // return an integer exit code