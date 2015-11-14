namespace BucklingSprings.Aware.AddOns.Blink

open Nessos.Argu

open System
open System.Threading


module Program = 

    type Arguments =
        | Goal_Likely of string // Likely - Unlikely
        | Goal_Words of int // What is the goal
        | Goal_Words_Done of int // Number of words towards the goal
        | Goal_Likely_Percent of int // What is the chance the goal will be reached
    with
        interface IArgParserTemplate with
            member s.Usage = 
                match s with
                    | Arguments.Goal_Likely _ -> "Is the goal likely to be met - Likely\Unlikely"
                    | Arguments.Goal_Words _ -> "Goal Words - Can be zero"
                    | Arguments.Goal_Words_Done _ -> "Words typed towards the goal"
                    | Arguments.Goal_Likely_Percent _ -> "What is the chance the goal will be reached - percentage - 0-100"


    [<EntryPoint>]
    let main argv = 
        
        let createdNew = ref false
        use mutex = new Mutex(true, "9411F9EF-05DF-401C-8EF3-96C2374F5082", createdNew)
        if !createdNew then
            let parser = ArgumentParser.Create<Arguments>()
            let results = parser.Parse(argv)
            let goal = results.GetResult(<@ Arguments.Goal_Words @>, 0)
            if goal > 0 then
                let likely = "Likely".Equals(results.GetResult(<@ Arguments.Goal_Likely @>, "Likely"), StringComparison.CurrentCultureIgnoreCase)
                let wordsSoFar = results.GetResult(<@ Arguments.Goal_Words_Done @>, 0)
                let likelyPct =  results.GetResult(<@ Arguments.Goal_Likely_Percent @>, 50)
                Blink.showProgress goal wordsSoFar likely likelyPct
                0
            else
                Blink.turnOff()
                -1
        else
            -2
