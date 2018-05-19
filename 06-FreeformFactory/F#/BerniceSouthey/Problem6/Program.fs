open System
open System.IO
open FreeformFactory
       
let lines = File.ReadAllLines(@"..\..\..\..\..\..\Input\D-large-practice.in")
let count = int lines.[0]

let getCluster line currentWorker  =
    let machineKnowledge =  line |> Seq.mapi(fun i x -> if x = '1' then Some i else None) |> Seq.choose id  |> Set.ofSeq
    {Machines=machineKnowledge;Workers=set[currentWorker];Edges=machineKnowledge.Count}

let rec mergeCluster (clusters:list<Cluster>) (merged:Cluster) =
    if clusters.Length = 0 then merged
    else
    mergeCluster (List.skip 1 clusters) (merged.union clusters.[0])

let rec findClusters (lines:string[]) (clusters:list<Cluster>) machineCount currentWorker =
    if lines.Length = 0 then clusters
    else
    let cluster = getCluster lines.[0]  currentWorker 
    let intersectClusters = clusters |> List.filter (fun x-> x.intersect cluster)
    let remainingClusters = clusters |> List.except intersectClusters
    let mergedCluster = mergeCluster intersectClusters cluster
    findClusters (Array.skip 1 lines) (mergedCluster::remainingClusters) machineCount (currentWorker+1)

let rec getZeroSumClustersCost (summedClusters:List<Cluster>) (remainingClusters:List<Cluster>) cost  =
    let sum = summedClusters |> List.sumBy (fun x->x.Balance)
    if sum = 0 then
        let mergedClusters = mergeCluster summedClusters.Tail summedClusters.[0]
        let newCost = cost + mergedClusters.CompleteCost
        if remainingClusters.Length = 0 then newCost
        else getZeroSumClustersCost [remainingClusters.[0]] (remainingClusters.Tail) newCost
    else getZeroSumClustersCost (remainingClusters.[0]::summedClusters) (remainingClusters.Tail) cost

let getMissingMachineClusters clusters machineCount =
    seq { for i in 0..(machineCount - 1) do yield (if clusters |> List.exists (fun x->x.Machines.Contains(i)) then None else Some {Machines=set[i];Workers=set[];Edges=0}) }
        |> Seq.choose id |> Seq.toList
    

let rec getMinOrderingCost (remaining:list<Cluster>) (ordered:list<Cluster>) (minCost:int) = 
    if remaining.Length = 0 then 
        min minCost (getZeroSumClustersCost [ordered.[0]] ordered.Tail 0)
    else 
    //Seq.distinctBy (fun x -> (x.Machines, x.Workers))
        let minCosts = remaining |> Seq.map(fun x -> getMinOrderingCost (remaining |> List.except [x]) (x::ordered) minCost)
        Seq.min minCosts


//let balanceCluster cluster clusters
//    if cluster.Balance = 0 then cluster
 //   else if cluster.Balance > 0 then
  //      clusters. 
   // else 
        

let findMinimalCost (lines:string[]) machineCount =
    let firstCluster = getCluster lines.[0] 0 
    let clusters = (findClusters (Array.skip 1 lines) [firstCluster] machineCount 1) |> Seq.toList |> List.sortByDescending (fun x-> x.Size)
    let allClusters = clusters |> List.append (getMissingMachineClusters clusters machineCount)
    let minCost = allClusters |> List.sumBy (fun x -> x.Balance)
    let maxCost = machineCount*machineCount
    getMinOrderingCost allClusters [] maxCost

let rec processNextTestCase (lines:string[]) results =
    if lines.Length = 0 then List.rev results
    else
    let machineCount = int lines.[0]
    let answer = findMinimalCost (Array.skip 1 lines |> Array.take machineCount) machineCount
    processNextTestCase (Array.skip (machineCount + 1) lines) (answer::results)

let format result i =
    String.Format("CASE #{0}: {1}", i + 1, result)

let output = processNextTestCase (Array.skip 1 lines) [] |> Seq.mapi(fun i x ->format x i) |> Seq.toList


File.WriteAllLines(@"..\..\..\..\..\..\Output\D-large-practice.out", (output))