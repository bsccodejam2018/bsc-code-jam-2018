module MagicTrick

let getChosenCard (rows: string[]) =
    let pos1 = rows.[0] |> int
    let cards1 = rows.[pos1].Split(" ") |> Seq.map int |> Set
    let pos2 = rows.[5] |> int
    let cards2 = rows.[pos2+5].Split(" ") |> Seq.map int |> Set

    match Set.intersect cards1 cards2 |> Set.toArray with
        | [||] -> "Volunteer cheated!"
        | a when a.Length = 1 -> a.[0] |> string
        | _ -> "Bad magician!"