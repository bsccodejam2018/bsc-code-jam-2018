module Ratatouille

type KitInput =
    {
        RequiredQuantities : int[]
        IngredientPackages: int[][]
    }

let possibleServingCounts requiredQuantity packageQuantity =
    let lowerBound = float(packageQuantity) / float(requiredQuantity) / 1.1 |> ceil |> int
    let upperBound = float(packageQuantity) / float(requiredQuantity) / 0.9 |> floor |> int
    [|lowerBound .. upperBound|]

let calculateKitCount input =
    let servingCountMatrix =
        Array.zip input.RequiredQuantities input.IngredientPackages
        |> Array.map (fun (requiredQuantity, ingredientRow) -> ingredientRow |> Array.map (possibleServingCounts requiredQuantity) |> Array.toList)
        |> Array.map (List.filter (Array.isEmpty >> not))
        |> Array.toList

    let containsEmptyRow = List.exists (List.isEmpty)
    let packageCountThenPackageLength row = (Array.min row, Array.length row)

    let rec calculateKitCountImpl matrix count =
        if matrix |> containsEmptyRow then
            count
        else
            let sortedRows = 
                matrix
                |> List.map (List.sortBy packageCountThenPackageLength)
    
            let canMakeKit = 
                sortedRows
                |> List.map (List.head >> set)
                |> Set.intersectMany
                |> (Set.isEmpty >> not)

            if canMakeKit then
                let newMatrix = sortedRows |> List.map (List.tail)
                calculateKitCountImpl newMatrix count+1
            else 
                match sortedRows with
                | firstRow :: otherRows -> calculateKitCountImpl (List.tail firstRow :: otherRows) count
                | _ -> count

    calculateKitCountImpl servingCountMatrix 0