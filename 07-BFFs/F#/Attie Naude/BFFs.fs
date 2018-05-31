module BFFs

type Chain = {
    EndsInMutualFriendship: bool
    Items: int list
}

let chainLength chain = chain.Items.Length
let last2ChildrenInChain chain = chain.Items |> List.rev |> List.take 2 |> List.rev
let longestChainInGrouping (_, chains) = chains |> Array.sortByDescending chainLength |> Array.head

let getChain (children: Map<int, int>) fromId =
    let extractChainFromPath path loopStart =
        let secondLastNode = path |> List.head
        // Full chain can be used if last 2 children are mutual friends...
        if children.[loopStart] = secondLastNode then
            {
                EndsInMutualFriendship = true
                Items = path |> List.rev
            }
        // Otherwise, only the cycle inside this path is a valid circle...
        else
            let loopStartIndex = path |> List.findIndex (fun p -> p = loopStart)
            {
                EndsInMutualFriendship = false
                Items = path.[0..loopStartIndex] |> List.rev
            }

    let rec getChainImp path nodeId =
        if path |> List.contains nodeId then
            extractChainFromPath path nodeId
        else
            getChainImp (nodeId :: path) (children.[nodeId])

    getChainImp [] fromId

let getLargestPossibleCircleSize children =
    let longestIndividualChains =
        children
        |> Map.toArray
        |> Array.map fst
        |> Array.Parallel.map (getChain children)
        |> Array.groupBy last2ChildrenInChain
        |> Array.map longestChainInGrouping

    let combinedChain =
        longestIndividualChains
        |> Array.filter (fun c -> c.EndsInMutualFriendship)
        |> Array.map (fun c -> c.Items)
        |> List.concat
        |> List.distinct

    max
        (longestIndividualChains |> Array.map chainLength |> Array.max)
        (combinedChain |> List.length)