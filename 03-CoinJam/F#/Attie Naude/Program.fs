open System
open System.IO
open CoinJam

let printCoins i coins =
    let printCoin coin =
        sprintf "%s %s" 
            (coin.Coin |> Array.rev |> Array.map string |> String.concat "") 
            (coin.NonTrivialDivisors |> Array.map string |> String.concat " ")
    sprintf "Case #%i:\n%s" 
        (i+1)
        (coins |> Array.map printCoin |> String.concat "\n")

let parseLine (s: string) =
    let splitString = s.Split " "
    (splitString.[0] |> int, splitString.[1] |> int)

let writeAllLines filePath rows = 
    File.WriteAllLines(filePath, rows)

[<EntryPoint>]
let main argv =

    File.ReadAllLines @"..\..\Input\C-small-practice.in"
    |> Array.skip 1
    |> Array.map parseLine
    |> Array.map getCoins
    |> Array.mapi printCoins
    |> writeAllLines @"..\..\Output\C-small-practice.out"

    File.ReadAllLines @"..\..\Input\C-large-practice.in"
    |> Array.skip 1
    |> Array.map parseLine
    |> Array.map getCoins
    |> Array.mapi printCoins
    |> writeAllLines @"..\..\Output\C-large-practice.out"

    0 // return an integer exit code