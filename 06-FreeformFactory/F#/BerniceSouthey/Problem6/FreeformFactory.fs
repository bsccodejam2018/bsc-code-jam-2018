module FreeformFactory
    type Cluster =
        {
            Machines:Set<int>
            Workers:Set<int>
            Edges:int
        } with

    member x.intersect (y:Cluster) =
        if (Set.intersect x.Machines y.Machines).Count > 0  || (Set.intersect x.Workers y.Workers).Count > 0 then true
        else false

    member x.union (y:Cluster) =
        {Machines=Set.union x.Machines y.Machines; Workers=Set.union x.Workers y.Workers; Edges=x.Edges+y.Edges}

    member x.Equivalent (y:Cluster) = x.Machines.Count =y.Machines.Count && x.Workers.Count = y.Workers.Count

    member x.Size = max x.Machines.Count x.Workers.Count

    member x.Cost = x.Machines.Count*x.Workers.Count - x.Edges

    member x.Balance = x.Machines.Count-x.Workers.Count 

    member x.BalanceCost = abs (x.Balance*x.Size)

    member x.CompleteCost = x.Size*x.Size - x.Edges


