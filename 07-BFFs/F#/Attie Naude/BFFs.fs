module BFFs

type Chain = {
    EndsInMutualFriendship: bool
    Items: int list
}

let chainLength chain = chain.Items.Length
let last2ChildrenInChain chain = chain.Items |> List.rev |> List.take 2 |> List.rev
let longestChainInGrouping (_, chains) = chains |> Array.sortByDescending chainLength |> Array.head

let getChain (friendshipMap: Map<int, int>) fromId =
    let extractChainFromPath path loopStart =
        let secondLastNode = path |> List.head
        // Full chain can be used if last 2 children are mutual friends...
        if friendshipMap.[loopStart] = secondLastNode then
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

    let rec getChainImp path childId =
        if path |> List.contains childId then
            extractChainFromPath path childId
        else
            getChainImp (childId :: path) (friendshipMap.[childId])

    getChainImp [] fromId

let getLargestPossibleCircleSize (friendshipMap: Map<int, int>) =
    let longestIndividualChains =
        friendshipMap
        |> Map.toArray
        |> Array.map fst
        |> Array.Parallel.map (getChain friendshipMap)
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