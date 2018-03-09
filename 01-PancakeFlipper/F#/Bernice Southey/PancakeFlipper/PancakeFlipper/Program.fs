// Learn more about F# at http://fsharp.org

open System
 open System.IO

let lines = File.ReadAllLines(@"C:\Codejam\Problem1\A-large-practice.in") |> Array.skip 1

// Convert file lines into a list.
let input = Seq.toList

let flip pancake = 
    if pancake = '+' then '-'
    else '+'

let bigflip list k = 
    let first = List.take k list  
    let flippedFirst = first |> List.map(fun(x) ->flip x)
    let last = List.skip k list
    List.append flippedFirst last

let lastflip  (list:list<char>) count k =
    match list with
        | _ when list.Length < k -> -1
        | _ when list |> List.forall ((=) '+') -> count
        | _ when list |> List.forall ((=) '-') -> count + 1
        | _ -> -1

let rec calculate (list:list<char>) k count =
    match list with
        | _ when list.Length <= k ->  lastflip list count k
        | '+'::tail -> calculate tail k count
        | '-'::_ -> calculate (bigflip list k) k (count + 1)

let explode (s:string) =
    [for c in s -> c]

let getlist (line:string) = 
    let split = line.Split [|' '|]
    if split.Length = 1 then []
    else split.[0] |> explode
    
let getk (line:string) = 
    let split = line.Split [|' '|]
    if split.Length = 1 then split.[0] |> int
    else split.[1] |> int

let resultstring count =
    match count with
        | -1 -> "IMPOSSIBLE"
        | _ -> count |> string

let format result i =
    String.Format("CASE #{0}: {1}", i + 1 |> string,  resultstring result |> string)

let output = lines |> Seq.mapi(fun i x ->format(calculate (getlist x) (getk x) 0) i) |> Seq.toList

File.WriteAllLines("C:\Codejam\Problem1\A-large-practice.out", output)

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    0 // return an integer exit code




