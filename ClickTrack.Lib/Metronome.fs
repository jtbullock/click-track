module ClickTrack.Lib.Metronome

open ClickTrack.Lib.Song

type MetronomeAction =
    | BeepHi
    | BeepLo
    | Silent

let convertToMetronomeActions (timeSignature: TimeSignature): MetronomeAction list =
    let firstBeat = match timeSignature.Division with
                           | Half -> [ BeepHi; Silent; Silent; Silent; Silent; Silent; Silent; Silent; ]
                           | Quarter -> [ BeepHi; Silent; Silent; Silent; ]
                           | Eighth -> [ BeepHi; Silent; ]
                           | Sixteenth -> [ BeepHi; ]
    
    firstBeat
    |> List.replicate (timeSignature.Beats - 1)
    |> List.concat
    |> List.map (fun action -> if action = BeepHi then BeepLo else action)
    |> List.append firstBeat

let measuresToMetronomeActions (song: TimeSignature list) =
    song |> List.map convertToMetronomeActions |> List.concat