﻿using System.Windows;
using System.Windows.Media.Animation;

namespace tech.sublink.SubLinkEditor.Controls.ZoomAndPan;

/// <summary>
/// A helper class to simplify animation.
/// </summary>
internal static class AnimationHelper {
    /// <summary>
    /// Starts an animation to a particular value on the specified dependency property.
    /// </summary>
    public static void StartAnimation(UIElement animatableElement, DependencyProperty dependencyProperty, double toValue, double animationDurationSeconds) =>
        StartAnimation(animatableElement, dependencyProperty, toValue, animationDurationSeconds, null);

    /// <summary>
    /// Starts an animation to a particular value on the specified dependency property.
    /// You can pass in an event handler to call when the animation has completed.
    /// </summary>
    public static void StartAnimation(UIElement animatableElement, DependencyProperty dependencyProperty, double toValue, double animationDurationSeconds, EventHandler? completedEvent) {
        double fromValue = (double)animatableElement.GetValue(dependencyProperty);

        DoubleAnimation animation = new() {
            From = fromValue,
            To = toValue,
            Duration = TimeSpan.FromSeconds(animationDurationSeconds)
        };

        animation.Completed += delegate (object? sender, EventArgs e) {
            //
            // When the animation has completed bake final value of the animation
            // into the property.
            //
            animatableElement.SetValue(dependencyProperty, animatableElement.GetValue(dependencyProperty));
            CancelAnimation(animatableElement, dependencyProperty);

            completedEvent?.Invoke(sender, e);
        };

        animation.Freeze();
        animatableElement.BeginAnimation(dependencyProperty, animation);
    }

    /// <summary>
    /// Cancel any animations that are running on the specified dependency property.
    /// </summary>
    public static void CancelAnimation(UIElement animatableElement, DependencyProperty dependencyProperty) =>
        animatableElement.BeginAnimation(dependencyProperty, null);
}
