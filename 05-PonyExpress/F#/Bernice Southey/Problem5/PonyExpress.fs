module PonyExpress

type Route = 
    {
        City: int
        HorseDistance: int
        HorseSpeed: int
        Cost: Option<float>
    }

type Horse =
    {
        Distance: int
        Speed: int
    }

