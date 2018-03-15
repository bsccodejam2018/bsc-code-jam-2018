open System
open System.IO
open System.Numerics

let lines = File.ReadAllLines(@"..\..\Input\C-small-practice.in")
let countline = lines |> Array.take 1
let count = countline.[0] |> int

let rec dectobase b (i:uint64)  =
    match i with
    | 0UL | 1UL -> string i
    | _ ->
        let bit = string (i % b)
        (dectobase b (i / b)) + bit 

let rec basetodec b p n s =
    if p = -1 then s
    else
        let pow = pown b p
        let bit = dectobase b pow |> uint64
        let div = n / bit
        if div = 0UL then basetodec b (p-1) n (div*pow+s)
        else basetodec b (p-1) (n - bit) (div*pow+s)

let rec binarytodec p n s =
    if p = -1 then s
    else
        let pow = pown 2UL p
        let bit = dectobase 2UL pow |> uint64
        let div = n / bit
        if div = 0UL then binarytodec (p-1) n (div*pow+s)
        else binarytodec (p-1) (n - bit) (div*pow+s)

let isPrime (n:uint64) =
    match n with
    | _ when n > 3UL && (n % 2UL = 0UL)  -> 2UL
    | _ when n > 3UL && (n % 3UL = 0UL) -> 3UL
    | _ ->
        let maxDiv = uint64(System.Math.Sqrt(float n)) + 1UL
        let rec f d i = 
            if d > maxDiv then 
                0UL
            else
                if n % d = 0UL then 
                    d
                else
                    f (d + i) (6UL - i)  
        f 5UL 2UL

let convertojamcoin index n =
    String.Format("{0}1", dectobase 2UL (index + pown 2UL (n - 2)))

let checkjamcoin n jamcoin i =
    let rebased = basetodec i (n-1) jamcoin 0UL
    isPrime (rebased |> uint64)

let rec checkjamcoinall n jamcoin i factors =
    let factor = checkjamcoin n jamcoin i 
    if factor = 0UL then []
    else
    match i with
            | 1UL -> factors
            | _ -> checkjamcoinall n jamcoin (i - 1UL) (factor::factors)

let converttostring l = l |> List.map (fun i -> i.ToString()) |> String.concat " "

let rec getnextjamcoin j n index found results =
    if j = found then results
    else
        let jamcoin = convertojamcoin index n
        let factors = checkjamcoinall n (jamcoin |> uint64) 10UL []
        if  factors = [] then getnextjamcoin j n (index+1UL) found results 
        else getnextjamcoin j n (index+1UL) (found + 1) ((converttostring ((jamcoin |> uint64)::factors))::results)


let processline (line:string) =
    let split = line.Split [|' '|]
    let n = split.[0] |> int
    let j = split.[1] |> int
    getnextjamcoin j n 0UL 0 []
    
let results = seq { for i in 1 .. count  do yield processline (lines.[i]) }
  

let format result i =
    String.Format("CASE #{0}:", i + 1 |> string)::result 

let output = results |> Seq.mapi(fun i x ->format x i) |> Seq.concat


File.WriteAllLines(@"..\..\Output\A-small-practice.out", output)

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    0 // return an integer exit code




