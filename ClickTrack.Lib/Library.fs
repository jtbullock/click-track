namespace ClickTrack.Lib
        
module Metronome =
    type Division =
        | Half
        | Quarter
        | Eighth
        | Sixteenth
    
    type TimeSignature = {
        Beats: int
        Division: Division
    }
        
open Metronome

module MetronomeUI =
    type Measure = {
        timeSignature: TimeSignature
    }
    
    let public MeasuresExample = [{timeSignature = {Beats = 4; Division = Quarter }}]
    
    let getMeasuresExample () = MeasuresExample