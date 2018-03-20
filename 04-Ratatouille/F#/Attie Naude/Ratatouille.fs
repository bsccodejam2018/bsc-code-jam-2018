module Ratatouille

open System

type KitInput =
    {
        RequiredQuantities : int[]
        IngredientPackages: int[][]
    }

let possibleServingCounts requiredQuantity ingredient =
    let lowerBound = float(ingredient) / float(requiredQuantity) / 1.1 |> ceil |> int
    let upperBound = float(ingredient) / float(requiredQuantity) / 0.9 |> floor |> int
    [|lowerBound .. upperBound|]

let calculateKitCount input =
    let servingCountMatrix = 
        Array.zip input.RequiredQuantities input.IngredientPackages
        |> Array.map (fun a -> Array.map (fun b -> possibleServingCounts (fst a) b) (snd a) |> Array.toList)
        |> Array.map (List.filter (Array.isEmpty >> not))
        |> Array.toList

    let rec calculateKitCountImpl matrix count =
        if matrix |> List.exists (List.isEmpty) then
            count
        else
            let sortedRows = 
                matrix
                |> List.map (fun a -> a |> List.sortBy (fun b -> (b |> Array.min, b |> Array.length)))
    
            let canMakeKit = 
                sortedRows
                |> List.map (List.head >> set)
                |> Set.intersectMany
                |> Set.isEmpty
                |> not

            if canMakeKit then
                let newMatrix =
                    sortedRows
                    |> List.map (fun a -> match a with _ :: rem -> rem)
                calculateKitCountImpl newMatrix count+1
            else match sortedRows with
            | [] -> count
            | h :: rem when h |> List.length = 1 ->
                count
            | h :: rem ->
                let newRow = match h with _ :: r -> r
                calculateKitCountImpl (newRow::rem) count

    calculateKitCountImpl servingCountMatrix 0