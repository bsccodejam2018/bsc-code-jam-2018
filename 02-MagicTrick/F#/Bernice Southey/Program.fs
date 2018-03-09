// Learn more about F# at http://fsharp.org

open System
 open System.IO

let lines = File.ReadAllLines(@"C:\Codejam\Problem2\A-small-practice.in")
let countline = lines |> Array.take 1
let count = countline.[0] |> int

let getintersect (list1:string) (list2:string) =
    Set.intersect (list1.Split [|' '|] |> Set) (list2.Split [|' '|] |> Set)

let processlist list =
    let row1 = list |> Array.take 1
    let index1 = row1.[0] |> int
    let list1 = list |> Array.skip (index1) |> Array.take 1
    let row2 = list |> Array.skip 5 |> Array.take 1
    let index2 = row2.[0] |> int
    let list2 = list |> Array.skip (5 + index2) |> Array.take 1
    getintersect list1.[0] list2.[0]

let getresult (intersect:list<string>) =
    let length = intersect.Length
    match length with  
        | 1 -> intersect.[0] |> string
        | _ when length >1 -> "Bad magician!"
        | _ -> "Volunteer cheated!"


let results = seq { for i in 1 .. count  do yield getresult (processlist (lines |> Array.skip (1+ (i-1)*10) |> Array.take 10) |> Set.toList)}
    

let format result i =
    String.Format("CASE #{0}: {1}", i + 1 |> string, result |> string)

let output = results |> Seq.mapi(fun i x ->format x i) |> Seq.toList


File.WriteAllLines("C:\Codejam\Problem2\A-large-practice.out", output)

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    0 // return an integer exit code




