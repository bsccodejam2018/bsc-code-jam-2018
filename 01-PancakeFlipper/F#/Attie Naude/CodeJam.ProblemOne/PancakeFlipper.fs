module PancakeFlipper

type Position = Left|Right
type Scenario = 
    {
        Pancakes: bool[]
        FlipperWidth: int
    }

let (|TrimLeadingPositive|_|) (bs: bool[]) =
    if bs.[0]
        then Some((Array.splitAt 1 bs) |> snd)
        else None

let (|TrimTrailingPositive|_|) (bs: bool[]) =
    if bs.[bs.Length-1]
        then Some((Array.splitAt (bs.Length-1) bs) |> fst)
        else None

let rec trimEdges input = 
    match input with
    | [||] -> [||]
    | TrimLeadingPositive bs -> trimEdges bs
    | TrimTrailingPositive bs -> trimEdges bs
    | bs -> bs

let getOptimalFlipPosition width pancakes =
    let left = (Array.splitAt width pancakes) |> fst
    let right = (Array.splitAt (pancakes.Length - width) pancakes) |> snd
    let invertedCount = Array.filter not >> Array.length

    if (invertedCount left >= invertedCount right) then
        Position.Left
    else
        Position.Right

let applyFlip width pos arr =
    let flipValues = Array.map not
    match pos with
    | Position.Left ->
        let sections = arr |> Array.splitAt width
        Array.concat [fst sections |> flipValues; snd sections]
    | Position.Right ->
        let sections = arr |> Array.splitAt (arr.Length-width)
        Array.concat [fst sections; snd sections |> flipValues]

let calculateFlipCount scenario = 
    let rec flipPancakesImpl pancakes flipperWidth flipCount =
        match trimEdges pancakes with
        | [||] -> Some(flipCount)
        | ps when ps.Length < flipperWidth -> None
        | ps -> 
            let optimalFlipPosition = getOptimalFlipPosition flipperWidth ps
            let newState = applyFlip flipperWidth optimalFlipPosition ps
            flipPancakesImpl newState flipperWidth (flipCount+1)

    flipPancakesImpl scenario.Pancakes scenario.FlipperWidth 0

