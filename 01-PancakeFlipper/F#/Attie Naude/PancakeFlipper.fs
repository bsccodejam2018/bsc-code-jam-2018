module PancakeFlipper

type Scenario = 
    {
        Pancakes: bool[]
        FlipperWidth: int
    }

let rec trimEdges input = 
    match input with
    | [||] -> [||]
    | bs when bs.[0] -> trimEdges bs.[1..]
    | bs when bs.[input.Length-1] -> trimEdges bs.[..input.Length-2]
    | bs -> bs

let applyFlip width ps =
    let flipValues = Array.map not
    match Array.splitAt width ps with
    | (pancakesToBeFlipped, remainder) -> Array.concat [flipValues pancakesToBeFlipped; remainder]

let calculateFlipCount scenario = 
    let rec flipPancakesImpl pancakes flipperWidth flipCount =
        match trimEdges pancakes with
        | [||] -> Some(flipCount)
        | ps when ps.Length < flipperWidth -> None
        | ps -> 
            let newState = applyFlip flipperWidth ps
            flipPancakesImpl newState flipperWidth (flipCount+1)

    flipPancakesImpl scenario.Pancakes scenario.FlipperWidth 0

