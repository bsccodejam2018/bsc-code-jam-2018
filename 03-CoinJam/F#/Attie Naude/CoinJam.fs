module CoinJam

open System
open System.IO
open System.Numerics

// There's literally trillions of combinations to try for the big input and billions for the small input.
// Running the non-prime/factor checks until completion is unnecessary when we can get away with just grabbing the low-hanging fruit instead,
// so we cap the search space for the first factor at a much lower value in order to complete within a reasonable timeframe...
let FIRST_FACTOR_CAP = 1000I

type JamCoin =
    {
        Coin : int[]
        NonTrivialDivisors: bigint[]
    }

let firstFactor n =
    { 2I .. BigInteger.Min(FIRST_FACTOR_CAP, n/2I) }
    |> Seq.tryFind (fun a -> n % a = 0I)

let valueInBase (coinBits: int seq) base' =
    coinBits 
    |> Seq.toArray
    |> Array.rev
    |> Array.mapi (fun i a -> (a |> bigint) * (pown base' i))
    |> Array.sum

let buildCoinBits length seed =
    seq {
        yield 1
        for i in 0 .. (length - 3) do
            yield (seed >>> i) % 2
        yield 1
    }

let allPossibleCoins length =
    seq {
        for i in 0 .. (pown 2 (length-2)) - 1 do
            yield buildCoinBits length i |> Seq.toArray |> Array.rev
    }

let verifyJamCoin candidateCoin =
    let nonTrivialDivisors = 
        [2I..10I]
        |> List.map (valueInBase candidateCoin)
        |> List.map firstFactor

    if nonTrivialDivisors |> Seq.forall Option.isSome then
        Some {
            Coin = candidateCoin
            NonTrivialDivisors = nonTrivialDivisors |> List.map Option.get |> List.toArray
        }
    else
        None

let getCoins (length, count) =
    allPossibleCoins length
    |> Seq.map verifyJamCoin
    |> Seq.filter Option.isSome
    |> Seq.map Option.get
    |> Seq.take count
    |> Seq.toArray