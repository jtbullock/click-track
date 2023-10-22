open System.Diagnostics
open System.Threading
open ClickTrack.Lib.Song
open ClickTrack.Lib.Metronome

let sampleFileHi = "Synth_Square_C_hi.wav"
let sampleFileLo = "Synth_Square_C_lo.wav"
let audioPlayerExe = "afplay"
let sampleRootDirectory = @"/Users/josh/Downloads/Metronomes/Favs/"

let song = [ { Beats = 4; Division = Quarter }; { Beats = 6; Division = Eighth } ]
let metronome = measuresToMetronomeActions song

let tempo = 120 //bpm
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
        actionToSound currentAction
        state.MetronomeActions <- remainingActions
    | [] -> state.CompletedEvent.Set() |> ignore

let timer = new Timer(onTimer, metronomeState, clock, clock)

threadSync.WaitOne() |> ignore
timer.Dispose()