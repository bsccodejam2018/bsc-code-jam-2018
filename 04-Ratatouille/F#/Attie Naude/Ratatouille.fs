module Ratatouille

type ScenarioInput =
    {
        RequiredQuantities : int[]
        IngredientPackages: int[][]
    }

let possibleServingCounts requiredQuantity packageQuantity =
    let lowerBound = float(packageQuantity) / float(requiredQuantity) / 1.1 |> ceil |> int
    let upperBound = float(packageQuantity) / float(requiredQuantity) / 0.9 |> floor |> int
    [|lowerBound .. upperBound|]

let getServingCountMatrix scenario =
    let calculateRow (requiredQuantity, ingredientRow) = 
        ingredientRow 
        |> Array.map (possibleServingCounts requiredQuantity) 
        |> Array.toList
    let packageCountThenPackageLength row = (Array.min row, Array.length row)

    Array.zip scenario.RequiredQuantities scenario.IngredientPackages
    |> Array.map calculateRow
    |> Array.map (List.filter (Array.isEmpty >> not))
    |> Array.map (List.sortBy packageCountThenPackageLength)
    |> Array.toList

let calculateKitCount scenario =
    let containsEmptyRow = List.exists (List.isEmpty)

    let rec calculateKitCountImpl count matrix =
        if matrix |> containsEmptyRow then
            count
        else
            let canMakeKit = 
                matrix
                |> List.map (List.head >> set)
                |> Set.intersectMany
                |> (Set.isEmpty >> not)

            if canMakeKit then
                matrix |> List.map List.tail |> calculateKitCountImpl (count+1)
            else 
                match matrix with
                | firstRow :: otherRows -> (List.tail firstRow) :: otherRows |> calculateKitCountImpl count
                | _ -> count

    getServingCountMatrix scenario |> calculateKitCountImpl 0