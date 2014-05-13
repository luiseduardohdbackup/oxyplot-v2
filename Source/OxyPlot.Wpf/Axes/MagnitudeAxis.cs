﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MagnitudeAxis.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.MagnitudeAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    using OxyPlot.Axes;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.MagnitudeAxis.
    /// </summary>
    public class MagnitudeAxis : LinearAxis
    {
        /// <summary>
        /// Initializes static members of the <see cref = "MagnitudeAxis" /> class.
        /// </summary>
        static MagnitudeAxis()
        {
            MajorGridlineStyleProperty.OverrideMetadata(typeof(MagnitudeAxis), new PropertyMetadata(LineStyle.Solid));
            MinorGridlineStyleProperty.OverrideMetadata(typeof(MagnitudeAxis), new PropertyMetadata(LineStyle.Solid));
            PositionProperty.OverrideMetadata(typeof(MagnitudeAxis), new PropertyMetadata(AxisPosition.None, AppearanceChanged));
            IsPanEnabledProperty.OverrideMetadata(typeof(MagnitudeAxis), new PropertyMetadata(false));
            IsZoomEnabledProperty.OverrideMetadata(typeof(MagnitudeAxis), new PropertyMetadata(false));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "MagnitudeAxis" /> class.
        /// </summary>
        public MagnitudeAxis()
        {
            this.InternalAxis = new Axes.MagnitudeAxis();
        }
    }
}