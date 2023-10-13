open System.Diagnostics
open System.Threading

printfn "Hello from F#"

let sampleFile = "Synth_Square_C_hi.wav"
let audioPlayerExe = "afplay"
let sampleRootDirectory = @"/Users/josh/Downloads/Metronomes/Favs/"

type Division =
    | Half
    | Quarter
    | Eighth
    | Sixteenth

type MetronomeAction =
    | Beep
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
                           | Half -> [ Beep; Silent; Silent; Silent; Silent; Silent; Silent; Silent; Silent; Silent; Silent; Silent; Silent; Silent; Silent; Silent; ]
                           | Quarter -> [ Beep; Silent; Silent; Silent; Silent; Silent; Silent; Silent; ]
                           | Eighth -> [ Beep; Silent; Silent; Silent; ]
                           | Sixteenth -> [ Beep; ]
    
    metronomeActions |> List.replicate timeSignature.Beats |> List.concat

let convertToMetronomeActions (song: TimeSignature list) =
    song |> List.map convertToBeeps |> List.concat

let song = [ { Beats = 4; Division = Quarter }; { Beats = 6; Division = Eighth } ]
let metronome = convertToMetronomeActions song

let tempo = 120
let clock = ( 60 / tempo ) / 16

let playClick () =
    let audioPlay = ProcessStartInfo(audioPlayerExe, $"{sampleRootDirectory}{sampleFile}") |> Process.Start
    audioPlay.WaitForExit()   

let timer = new Timer
                (
                    (fun _ -> playClick()),
                    metronome,
                    clock,
                    clock
                )

System.Console.ReadKey() |> ignore