module FreeformFactory

type Machine = int

type Worker = {
    Id: int
    OperableMachines: Set<Machine>
}

type Cluster = {
    Workers: Set<Worker>
    Machines: Set<Machine>
}

let joinClusters clusters =
    {
        Machines = clusters |> List.map (fun c -> c.Machines) |> Set.unionMany
        Workers = clusters |> List.map (fun c -> c.Workers) |> Set.unionMany
    }

let getInitialClusters (workers: Worker list) =
    let canOperateMachine machineId worker = worker.OperableMachines.Contains(machineId)

    let rec partitionClusters clusters iterWorkers =
        match iterWorkers with
        | [] -> clusters
        | worker::remainingWorkers ->
            let clustersWithThisWorker = clusters |> List.filter (fun c -> c.Workers.Contains worker)
            let remainingClusters = clusters |> List.except clustersWithThisWorker
            let joinedCluster = joinClusters clustersWithThisWorker
            partitionClusters (joinedCluster :: remainingClusters) remainingWorkers

    let machineClusters = 
        [1..workers.Length] 
        |> List.map (fun i ->
            { 
                Machines = [i] |> Set
                Workers = workers |> List.filter (canOperateMachine i) |> Set
            }
        )

    let zeroKnowledgeWorkerClusters =
        workers
        |> List.filter (fun w -> w.OperableMachines.IsEmpty)
        |> List.map (fun w -> 
            {
                Machines = Set.empty
                Workers = [w] |> Set
            }
        )

    let initialClusters = List.concat [machineClusters; zeroKnowledgeWorkerClusters]
    partitionClusters initialClusters workers

let memoize f =
    let cache = System.Collections.Generic.Dictionary()
    fun x ->
        // When caching input values, we can treat clusters as identical if their machine counts and person counts match.
        // This is a major performance enhancement that allows us to use the memoized value for the majority of cases...
        let key = x |> List.map (fun c -> (c.Workers.Count, c.Machines.Count)) |> List.sort
        if cache.ContainsKey(key) then 
            cache.[key]
        else
            let res = f x
            cache.[key] <- res
            res

let isUnbalanced cluster = cluster.Machines.Count <> cluster.Workers.Count
let clusterSize cluster = cluster.Workers.Count * cluster.Machines.Count
let inbalance cluster = cluster.Machines.Count - cluster.Workers.Count

let applyMerge cs (clusterA, clusterB) =
    let mergedCluster = joinClusters [clusterA; clusterB]
    let otherClusters = cs |> List.except [clusterA; clusterB]
    (mergedCluster :: otherClusters)

let canBeMerged a b =
    let inbalanceA = inbalance a
    let inbalanceB = inbalance b
    inbalanceA > 0 && inbalanceB < 0|| inbalanceA < 0 && inbalanceB > 0

let rec calculateMinCardinality = memoize (fun clusters ->
    let unbalancedClusters = clusters |> List.filter isUnbalanced |> List.sortByDescending clusterSize
    let balancedClusters = clusters |> List.filter (isUnbalanced >> not)
    if unbalancedClusters.IsEmpty then
        balancedClusters |> List.sumBy clusterSize
    else
        let leftCluster = unbalancedClusters |> List.head
        let rightClusters = unbalancedClusters |> List.tail |> List.filter (canBeMerged leftCluster)

        List.allPairs [leftCluster] rightClusters
        |> List.map (applyMerge clusters)
        |> List.map calculateMinCardinality
        |> List.min
    )

let calculateMinTrainingCost (workers: Worker list) =
    let currentTraining = workers |> List.sumBy (fun w -> w.OperableMachines.Count)
    let requiredTraining = getInitialClusters workers |> calculateMinCardinality
    requiredTraining - currentTraining