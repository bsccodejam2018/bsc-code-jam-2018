module PonyExpress

open System

[<Measure>]
type km
[<Measure>]
type hour

type Horse =
    {
        Speed: float<km/hour>
        Endurance: float<km>
        VisitedCities: int list
    }

type CityConnections = Map<int, Map<int, float<km>>>

type ScenarioInput =
    {
        CityConnections: CityConnections
        Horses: Map<int, Horse>
        SearchPairs: Tuple<int, int>[]
    }

type State =
    {
        CurrentCity: int
        HorseMap: Map<int, Horse list>
        TotalTime: float<hour>
    }

// Small optimization - if 1 horse is both faster and has more endurance then it is the clear choice...
let availableHorses horses =
    let fastestHorse = horses |> Seq.sortByDescending (fun a -> a.Speed) |> Seq.head
    let enduranceChamp = horses |> Seq.sortByDescending (fun a -> a.Endurance) |> Seq.head
    [fastestHorse; enduranceChamp] |> Set

let citiesReachableByHorse cityConnections horse =
    cityConnections
    |> Map.filter (fun cityId distance -> horse.Endurance >= distance && not (List.contains cityId horse.VisitedCities))
    |> Map.toList

let calculateNewState currentState horse cityId distance =
    let updatedHorse = { horse with Endurance = horse.Endurance - distance; VisitedCities = cityId :: horse.VisitedCities }
    let currentCityHorses = currentState.HorseMap.[currentState.CurrentCity] |> List.except [horse]
    let nextCityHorses = updatedHorse :: currentState.HorseMap.[cityId]
    let updatedHorseMap = 
        currentState.HorseMap
            .Remove(currentState.CurrentCity)
            .Remove(cityId)
            .Add(currentState.CurrentCity, currentCityHorses)
            .Add(cityId, nextCityHorses)
    { currentState with CurrentCity = cityId; HorseMap = updatedHorseMap; TotalTime = currentState.TotalTime + (distance / horse.Speed) }


let yieldBestJourneyTimes (cityConnections: CityConnections) horses startCity endCity =
    // Mutable values cause me great sadness, but at least it's constrained to 1 function and it's purely for optimization purposes... :|
    let mutable currentBest = infinity * 1.0<hour>

    let rec yieldBestJourneyTimesImpl (state: State) =
        seq {
            if endCity = state.CurrentCity then
                if state.TotalTime < currentBest then
                    currentBest <- state.TotalTime
                    yield state.TotalTime
            else
                for horse in availableHorses state.HorseMap.[state.CurrentCity] do
                    for (cityId, distance) in citiesReachableByHorse cityConnections.[state.CurrentCity] horse do
                        let newState = calculateNewState state horse cityId distance
                        if newState.TotalTime < currentBest then
                            yield! yieldBestJourneyTimesImpl newState
        }

    yieldBestJourneyTimesImpl { CurrentCity = startCity; HorseMap = horses |> Map.map (fun k v -> [v]); TotalTime = 0.0<hour> }

let calculateMinRideTimes (scenario: ScenarioInput) =
    let getBestJourneyTime cityPair =
        yieldBestJourneyTimes scenario.CityConnections scenario.Horses (fst cityPair) (snd cityPair)
        |> Seq.min
        |> float

    scenario.SearchPairs
    |> Array.Parallel.map getBestJourneyTime
