open System
open System.IO
open PonyExpress


let lines = File.ReadAllLines(@"..\..\..\..\..\..\Input\C-large-practice.in")
let countline = lines |> Array.take 1
let count = countline.[0] |> int


let checkForCheaperRoute (target:Route) (replacement:Route) =
    if target.Cost = None && replacement.Cost = None then target
    else if target.Cost = None then replacement
    else if replacement.Cost = None then target
    else if target.Cost < replacement.Cost then target
    else if target.Cost > replacement.Cost then replacement
    else if target.HorseDistance > replacement.HorseDistance then target
    else replacement

let calculateCost (horses:list<list<int>>) (cities:list<list<int>>) (routes:list<Route>) startCity currentCity (currentHorse:list<int>) nextCity =
    let target = routes.[nextCity]
    let currentCost = cities.[startCity].[currentCity]
    let distance = cities.[currentCity].[nextCity]
    if distance = -1 then target
    else
    let newHorse = horses.[currentCity]
    if currentHorse.[1] < distance && newHorse.[1] < distance then target
    else
    let currentHorseCost = (distance  |> float) / (currentHorse.[3] |> float) + currentCost
    let newHorseCost = (distance |> float) / (newHorse.[3] |> float) + currentCost
    if currentHorseCost > newHorseCost then checkForCheaperRoute target {Cost=Some newHorseCost; City=nextCity; HorseSpeed=newHorse.[0]; HorseDistance=newHorse.[1]} 
    else if currentHorseCost < newHorseCost then checkForCheaperRoute target (currentHorseCost::currentHorse) 
    else if currentHorse.[1] > newHorse.[1] then checkForCheaperRoute target (currentHorseCost::currentHorse)
    else checkForCheaperRoute target (newHorseCost::newHorse)

let  rec moveNext horses cities destination  (routes:list<Route>) startCity currentCity currentHorse  =
    let newRoutes = routes |> List.filter(fun x-> x.[0] <> currentCity) |> List.map(fun x-> calculateCost horses cities routes startCity currentCity currentHorse x.[0])
    moveNext horses cities destination newRoutes startCity currentCity currentHorse


let solveRoute cityCount (horses:list<list<int>>) cities start destination =
    let initialRoutes  =  seq { for i in 1 .. cityCount  do yield {Cost = (if i=start then Some 0.0 else None); City=i; HorseSpeed=horses.[i].[0]; HorseDistance=horses.[i].[1]} } |> Seq.toList
    moveNext horses cities destination initialRoutes start start horses.[start]

let rec processDelivery cityCount (horses:list<list<int>>) (cities:list<list<int>>) (lines:list<string>) results =
    if lines.Length = 0 then List.rev results
    else
    let delivery = lines.[0].Split [|' '|]
    let start = delivery.[0] |> int
    let destination = delivery.[1] |> int
    let deliveryTime = solveRoute cityCount horses cities start destination
    processDelivery cityCount horses cities (lines |> List.skip 1) (deliveryTime::results)

let splitLine (line:string) =
    line.Split [|' '|]

let getMatrix cityCount lines =
    lines |> List.take cityCount |> Seq.map(fun x -> (splitLine x) |> Seq.map(fun y -> y|> int) |> Seq.toList) |> Seq.toList

let rec solveNextConfiguration (lines:list<string>) results =
    if lines.Length = 0 then List.rev results
    else
    let problemSize = lines.[0].Split [|' '|]
    let cityCount = problemSize.[0] |> int
    let deliveryCount = problemSize.[1] |> int
    let horses = getMatrix cityCount (lines |> List.skip 1)
    let cities = getMatrix cityCount (lines |> List.skip (1+cityCount))
    let deliveries = lines |> List.skip (1+cityCount*2) |> List.take deliveryCount
    let result = processDelivery cityCount horses cities deliveries []
    solveNextConfiguration (lines |> List.skip (1+cityCount*2+deliveryCount)) (result::results)

let format (result:list<int>) i =
    String.Format("CASE #{0}: {1}", i + 1 |> string, (result |> Seq.map(fun x -> x |> string) |> String.concat " "))

let output = solveNextConfiguration (lines |> Array.skip 1 |> Array.toList) [] |> Seq.mapi(fun i x ->format x i) |> Seq.toList

File.WriteAllLines(@"..\..\..\..\..\..\Output\A-small-practice.out", (output))