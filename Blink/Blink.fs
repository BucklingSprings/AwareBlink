namespace BucklingSprings.Aware.AddOns.Blink

open BlinkStickDotNet

module Blink =

    let allOff : byte array = Array.zeroCreate(3*8)

    let nWithColor n (g, r, b) : byte array = 
        seq { for row in 0 .. n - 1 do
                yield! [| g; r; b |]
        } |> Seq.toArray

    let withOpenDevice fn =
        let device = BlinkStick.FindFirst()
        if device <> null then
            if device.OpenDevice() then
                fn device
        ()

    let turnOff () = 
        withOpenDevice (fun d -> d.SetColors(0uy, nWithColor 0 (255uy,255uy,12uy)))

    let likelyColor (likelyPct : int) =
        if likelyPct > 90 then
            (150uy,0uy,0uy)
        else
            (50uy,20uy,10uy)

    let unlikelyColor (likelyPct : int) =
        if likelyPct < 50 then
            (0uy,250uy,0uy)
        else
            (20uy,100uy,20uy)


    let showProgress  (goal : int) (wordsSoFar : int) (likely : bool) (likelyPct : int) =
        let numOfLedsOn = (int ((float wordsSoFar) * 8.0/ (float goal)))
        let color = if likely then (likelyColor likelyPct) else (unlikelyColor likelyPct)
        withOpenDevice (fun d -> d.SetColors(0uy, nWithColor (max numOfLedsOn 1) color))