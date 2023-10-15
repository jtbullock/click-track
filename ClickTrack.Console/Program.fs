open System.Diagnostics
open System.Threading
open ClickTrack.Lib.Metronome

let sampleFileHi = "Synth_Square_C_hi.wav"
let sampleFileLo = "Synth_Square_C_lo.wav"
let audioPlayerExe = "afplay"
let sampleRootDirectory = @"/Users/josh/Downloads/Metronomes/Favs/"

type MetronomeAction =
    | BeepHi
    | BeepLo
    | Silent

// type Song = {
//     TimeSignatures: TimeSignature list
// }

let convertToBeeps (timeSignature: TimeSignature): MetronomeAction list =
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

let convertToMetronomeActions (song: TimeSignature list) =
    song |> List.map convertToBeeps |> List.concat

let song = [ { Beats = 4; Division = Quarter }; { Beats = 6; Division = Eighth } ]
let metronome = convertToMetronomeActions song

let tempo = 150
let clock = ( 60000 / tempo ) / 4

printfn $"Clock speed: {clock}"

let playClickHiAsync () = 
    let audioPlay = ProcessStartInfo(audioPlayerExe, $"{sampleRootDirectory}{sampleFileHi}") |> Process.Start
    audioPlay.WaitForExitAsync()
    
let playClickLoAsync () =
    let audioPlay = ProcessStartInfo(audioPlayerExe, $"{sampleRootDirectory}{sampleFileLo}") |> Process.Start
    audioPlay.WaitForExitAsync()
    
let actionToSound (action: MetronomeAction) =
    match action with
    | BeepHi -> async { playClickHiAsync() |> ignore } |> Async.Start
    | BeepLo -> async { playClickLoAsync() |> ignore } |> Async.Start
    | Silent -> ()
    
let printAction (action: MetronomeAction) =
    match action with
    | Silent -> printf "s-"
    | BeepHi -> printf "H-"
    | BeepLo -> printf "L-"
    
let actionDebug (action: MetronomeAction) =
    printf "Executing Action: " 
    printAction action
    
metronome |> List.iter printAction

printfn "Starting Metronome..."

type MetronomeState(metronomeActions: MetronomeAction list, threadSync: AutoResetEvent) =
    member val MetronomeActions = metronomeActions with get, set
    member this.CompletedEvent = threadSync

printfn "Metronome State:"
let threadSync = new AutoResetEvent(false)
let metronomeState = MetronomeState(metronome, threadSync)
metronomeState.MetronomeActions |> List.iter printAction
    
let onTimer (state:obj) =
    let state = state :?> MetronomeState
                        
    printfn "Received State:"
    state.MetronomeActions |> List.iter printAction
    
    match state.MetronomeActions with
    | currentAction :: remainingActions ->
        state.MetronomeActions <- remainingActions
        actionToSound currentAction
    | [] -> state.CompletedEvent.Set() |> ignore

let timer = new Timer(onTimer, metronomeState, clock, clock)

threadSync.WaitOne() |> ignore
timer.Dispose()

//System.Console.ReadKey() |> ignore