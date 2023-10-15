namespace ClickTrack.Lib
        
module Song =
    // type Song = {
    //     TimeSignatures: TimeSignature list
    // }
    
    type Division =
        | Half
        | Quarter
        | Eighth
        | Sixteenth
    
    type TimeSignature = {
        Beats: int
        Division: Division
    }