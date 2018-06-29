﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace EmergenceGuardian.VapourSynthUI {
	public static class ViewboxExtensions {
		public static readonly DependencyProperty MaxZoomFactorProperty =
			DependencyProperty.RegisterAttached("MaxZoomFactor", typeof(double), typeof(ViewboxExtensions), new PropertyMetadata(1.0, OnMaxZoomFactorChanged));

		private static void OnMaxZoomFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var viewbox = d as Viewbox;
			if (viewbox == null)
				return;
			viewbox.Loaded += Viewbox_OnLoaded;
            viewbox.Unloaded += Viewbox_Unloaded;
		}

        private static void Viewbox_OnLoaded(object sender, RoutedEventArgs e) {
			var viewbox = sender as Viewbox;
			var child = viewbox?.Child as FrameworkElement;
			if (child == null)
				return;

            child.SizeChanged += Child_SizeChanged;
			CalculateMaxSize(viewbox);
		}

        private static void Viewbox_Unloaded(object sender, RoutedEventArgs e) {
            var viewbox = sender as Viewbox;
            var child = viewbox?.Child as FrameworkElement;
            if (child == null)
                return;

            child.SizeChanged -= Child_SizeChanged;
        }

        private static void Child_SizeChanged(object sender, SizeChangedEventArgs e) {
            Viewbox Parent = (sender as Control)?.Parent as Viewbox;
            CalculateMaxSize(Parent);
        }

        private static void CalculateMaxSize(Viewbox viewbox) {
			var child = viewbox.Child as FrameworkElement;
			if (child == null)
				return;
			viewbox.MaxWidth = child.ActualWidth * GetMaxZoomFactor(viewbox);
			viewbox.MaxHeight = child.ActualHeight * GetMaxZoomFactor(viewbox);
		}

		public static void SetMaxZoomFactor(DependencyObject d, double value) {
			d.SetValue(MaxZoomFactorProperty, value);
		}

		public static double GetMaxZoomFactor(DependencyObject d) {
			return (double)d.GetValue(MaxZoomFactorProperty);
		}
	}
}