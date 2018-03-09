// Learn more about F# at http://fsharp.org

open System
open System.IO
open PancakeFlipper

let parseRow (s: string) =
    let sections = s.Split " "
    {
        Pancakes = sections.[0].ToCharArray() |> Array.map ((=) '+')
        FlipperWidth = sections.[1] |> int
    }

let printRow i = function
    | Some a -> sprintf "CASE #%i: %i" (i+1) a
    | None -> sprintf "CASE #%i: IMPOSSIBLE" (i+1)

let writeAllLines filePath rows = 
    File.WriteAllLines(filePath, rows)

[<EntryPoint>]
let main argv =
    File.ReadAllLines @"..\..\..\Input\A-small-practice.in"
    |> Array.skip 1
    |> Array.map (parseRow >> calculateFlipCount)
    |> Array.mapi printRow
    |> writeAllLines @"..\..\..\Output\A-small-practice.out"

    File.ReadAllLines @"..\..\..\Input\A-large-practice.in"
    |> Array.skip 1
    |> Array.map (parseRow >> calculateFlipCount)
    |> Array.mapi printRow
    |> writeAllLines @"..\..\..\Output\A-large-practice.out"

    0 // return an integer exit code