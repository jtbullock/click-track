open System.Diagnostics
open System.Threading
// open System.Timers

printfn "Hello from F#"

let sampleFileHi = "Synth_Square_C_hi.wav"
let sampleFileLo = "Synth_Square_C_lo.wav"
let audioPlayerExe = "afplay"
let sampleRootDirectory = @"/Users/josh/Downloads/Metronomes/Favs/"

type Division =
    | Half
    | Quarter
    | Eighth
    | Sixteenth

type MetronomeAction =
    | BeepHi
    | BeepLo
    | Silent

type TimeSignature = {
    Beats: int
    Division: Division
}

// type Song = {
//     TimeSignatures: TimeSignature list
// }

let convertToBeeps (timeSignature: TimeSignature): MetronomeAction list =
    let metronomeActions = match timeSignature.Division with
                           | Half -> [ BeepHi; Silent; Silent; Silent; Silent; Silent; Silent; Silent; ]
                           | Quarter -> [ BeepHi; Silent; Silent; Silent; ]
                           | Eighth -> [ BeepHi; Silent; ]
                           | Sixteenth -> [ BeepHi; ]
    
    metronomeActions
    |> List.replicate (timeSignature.Beats - 1)
    |> List.concat
    |> List.map (fun action -> if action = BeepHi then BeepLo else action)
    |> List.append metronomeActions

let convertToMetronomeActions (song: TimeSignature list) =
    song |> List.map convertToBeeps |> List.concat

let song = [ { Beats = 4; Division = Quarter }; { Beats = 6; Division = Eighth } ]
let metronome = convertToMetronomeActions song

let tempo = 120
let clock = ( 60000 / tempo ) / 4

let playClickHiAsync () = 
    let audioPlay = ProcessStartInfo(audioPlayerExe, $"{sampleRootDirectory}{sampleFileHi}") |> Process.Start
    audioPlay.WaitForExitAsync()
    
let playClickLoAsync () =
    let audioPlay = ProcessStartInfo(audioPlayerExe, $"{sampleRootDirectory}{sampleFileLo}") |> Process.Start
    audioPlay.WaitForExitAsync()
    
let printAction (action: MetronomeAction) =
    match action with
    | Silent -> printf "s-"
    | BeepHi -> printf "H-"
    | BeepLo -> printf "L-"
    
metronome |> List.iter printAction

printfn "Starting Metronome..."

type MetronomeState(metronomeActions: MetronomeAction list) =
    member val MetronomeActions = metronomeActions with get, set

printfn "Metronome State:"
let testState = MetronomeState(metronome)
testState.MetronomeActions |> List.iter printAction

let actionToSound (action: MetronomeAction) =
    match action with
    | BeepHi -> async { playClickHiAsync() |> ignore } |> Async.Start
    | BeepLo -> async { playClickLoAsync() |> ignore } |> Async.Start
    | Silent -> ()
    
let actionDebug (action: MetronomeAction) =
    printf "Executing Action: " 
    printAction action

let onTimer (state:obj) =
    let metronomeState = state :?> MetronomeState
                        
    printfn "Received State:"
    metronomeState.MetronomeActions |> List.iter printAction
    
    match metronomeState.MetronomeActions with
    | currentAction :: remainingActions ->
        actionToSound currentAction
        actionDebug currentAction
        metronomeState.MetronomeActions <- remainingActions
    | [] -> ()

let timer = new Timer(onTimer, MetronomeState(metronome), clock, clock)

printfn $"Clock speed: {clock}"

System.Console.ReadKey() |> ignore