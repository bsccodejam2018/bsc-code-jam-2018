module CoinJam

open System
open System.Numerics

// There's a billion (2^30) combinations to try for the big input and thousands (2^14) for the small input.
// Running the non-prime/factor checks until completion is unnecessary when we can get away with just grabbing the 
// low-hanging fruit instead.  So we cap the search space for the first factor at a much lower value in order to 
// complete within a reasonable timeframe...
let FIRST_FACTOR_CAP = 1000I

type JamCoin =
    {
        Coin : int[]
        NonTrivialDivisors: bigint[]
    }

let firstFactor n =
    let searchUpperBound = BigInteger.Min(FIRST_FACTOR_CAP, n/2I)
    let isFactor f = n % f = 0I
    { 2I .. searchUpperBound }
    |> Seq.tryFind isFactor

let coinValueInBase (coinBits: int[]) base' =
    let bitValue pos (b: int) = bigint(b) * (pown base' pos)
    coinBits
    |> Array.mapi bitValue
    |> Array.sum

let buildCoinBits length seed =
    let valueAtPos pos = (seed >>> pos) % 2
    let coin =
        [1] @
        List.map valueAtPos [ 0 .. length - 3 ] @
        [1]
    coin |> List.toArray

let allCoinsOfLength length =
    let seedUpperBound = (pown 2 (length-2)) - 1
    { 0 .. seedUpperBound }
    |> Seq.map (buildCoinBits length)

let buildJamCoin candidateCoin =
    let nonTrivialDivisors = 
        [2I..10I]
        |> List.map (coinValueInBase candidateCoin)
        |> List.map firstFactor
    if nonTrivialDivisors |> Seq.forall Option.isSome then
        Some {
            Coin = candidateCoin
            NonTrivialDivisors = nonTrivialDivisors |> List.map Option.get |> List.toArray
        }
    else
        None

let getCoins (length, count) =
    allCoinsOfLength length
    |> Seq.map buildJamCoin
    |> Seq.filter Option.isSome
    |> Seq.map Option.get
    |> Seq.take count
    |> Seq.toArray