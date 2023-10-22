using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTrack.GUI;

public partial class TimeSignatureView : ContentView
{
    public static readonly BindableProperty BeatsProperty =
        BindableProperty.Create(
            nameof(Beats), 
            typeof(string), 
            typeof(TimeSignatureView), 
            string.Empty);
    
    public static readonly BindableProperty SubdivisionProperty =
        BindableProperty.Create(
            nameof(Subdivision), 
            typeof(string), 
            typeof(TimeSignatureView), 
            string.Empty);
    
    public string Beats
    {
        get => (string)GetValue(TimeSignatureView.BeatsProperty);
        set => SetValue(TimeSignatureView.BeatsProperty, value);
    }
    
    public string Subdivision
    {
        get => (string)GetValue(TimeSignatureView.SubdivisionProperty);
        set => SetValue(TimeSignatureView.SubdivisionProperty, value);
    }
    
    public TimeSignatureView()
    {
        InitializeComponent();
    }
}