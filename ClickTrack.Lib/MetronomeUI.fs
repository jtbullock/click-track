module ClickTrack.Lib.MetronomeUI

open ClickTrack.Lib.Song

type Measure = {
    timeSignature: TimeSignature
}

let public MeasuresExample = [{timeSignature = {Beats = 4; Division = Quarter }}]

let getMeasuresExample () = MeasuresExample