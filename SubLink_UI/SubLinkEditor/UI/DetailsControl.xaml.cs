﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FlowGraph;

namespace tech.sublink.SubLinkEditor.UI;

/// <summary>
/// Interaction logic for DetailsControl.xaml
/// </summary>
public partial class DetailsControl : UserControl
{
    /// <summary>
    /// 
    /// </summary>
    public DetailsControl()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ButtonAddInput_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is SequenceFunction)
        {
            SequenceFunction func = DataContext as SequenceFunction;
            func.AddInput("input");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ButtonAddOutput_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is SequenceFunction)
        {
            SequenceFunction func = DataContext as SequenceFunction;
            func.AddOutput("output");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnDeleteSlot_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is Image img)
        {
            if (img.Tag is int id)
            {
                if (DataContext is SequenceFunction)
                {
                    SequenceFunction func = DataContext as SequenceFunction;
                    func.RemoveSlotById(id);
                }
            }
        }
    }
}
