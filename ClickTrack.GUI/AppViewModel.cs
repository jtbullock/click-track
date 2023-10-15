using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClickTrack.Lib;

namespace ClickTrack.GUI;

public class AppViewModel : INotifyPropertyChanged
{
    public List<MetronomeUI.Measure> Measures => MetronomeUI.MeasuresExample.ToList();
    public int MeasuresCount => MetronomeUI.MeasuresExample.Length;
    public event PropertyChangedEventHandler PropertyChanged;

    public List<string> TextList
    {
        get { return new List<string> {"1", "2", "3"}; }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}