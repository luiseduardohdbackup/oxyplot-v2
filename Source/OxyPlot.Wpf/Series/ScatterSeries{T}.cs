// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterSeries{T}.cs" company="OxyPlot">
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
//   Provides a base class for scatter series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    using OxyPlot.Series;

    /// <summary>
    /// Provides a base class for scatter series.
    /// </summary>
    /// <typeparam name="T">The type of the points.</typeparam>
    public abstract class ScatterSeries<T> : XYAxisSeries where T : ScatterPoint, new()
    {
        /// <summary>
        /// Identifies the <see cref="BinSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BinSizeProperty = DependencyProperty.Register(
            "BinSize", typeof(int), typeof(ScatterSeries<T>), new PropertyMetadata(0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldSizeProperty = DependencyProperty.Register(
            "DataFieldSize", typeof(string), typeof(ScatterSeries<T>), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldTag"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldTagProperty = DependencyProperty.Register(
            "DataFieldTag", typeof(string), typeof(ScatterSeries<T>), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldValueProperty = DependencyProperty.Register(
            "DataFieldValue", typeof(string), typeof(ScatterSeries<T>), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldXProperty = DependencyProperty.Register(
            "DataFieldX", typeof(string), typeof(ScatterSeries<T>), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldY"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldYProperty = DependencyProperty.Register(
            "DataFieldY", typeof(string), typeof(ScatterSeries<T>), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="Mapping"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MappingProperty = DependencyProperty.Register(
            "Mapping", typeof(Func<object, ScatterPoint>), typeof(ScatterSeries<T>), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerFill"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerFillProperty = DependencyProperty.Register(
            "MarkerFill", typeof(Color), typeof(ScatterSeries<T>), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerOutline"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerOutlineProperty = DependencyProperty.Register(
            "MarkerOutline", typeof(ScreenPoint[]), typeof(ScatterSeries<T>), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerSizeProperty = DependencyProperty.Register(
            "MarkerSize", typeof(double), typeof(ScatterSeries<T>), new PropertyMetadata(5.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerStroke"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerStrokeProperty = DependencyProperty.Register(
            "MarkerStroke", typeof(Color), typeof(ScatterSeries<T>), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerStrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerStrokeThicknessProperty =
            DependencyProperty.Register(
                "MarkerStrokeThickness",
                typeof(double),
                typeof(ScatterSeries<T>),
                new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerTypeProperty = DependencyProperty.Register(
            "MarkerType",
            typeof(MarkerType),
            typeof(ScatterSeries<T>),
            new PropertyMetadata(MarkerType.Square, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "ScatterSeries{T}" /> class.
        /// </summary>
        protected ScatterSeries()
        {
            this.InternalSeries = new OxyPlot.Series.ScatterSeries();
        }

        /// <summary>
        /// Gets or sets BinSize.
        /// </summary>
        public int BinSize
        {
            get
            {
                return (int)this.GetValue(BinSizeProperty);
            }

            set
            {
                this.SetValue(BinSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldSize.
        /// </summary>
        public string DataFieldSize
        {
            get
            {
                return (string)this.GetValue(DataFieldSizeProperty);
            }

            set
            {
                this.SetValue(DataFieldSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldTag.
        /// </summary>
        public string DataFieldTag
        {
            get
            {
                return (string)this.GetValue(DataFieldTagProperty);
            }

            set
            {
                this.SetValue(DataFieldTagProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldValue.
        /// </summary>
        public string DataFieldValue
        {
            get
            {
                return (string)this.GetValue(DataFieldValueProperty);
            }

            set
            {
                this.SetValue(DataFieldValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldX.
        /// </summary>
        public string DataFieldX
        {
            get
            {
                return (string)this.GetValue(DataFieldXProperty);
            }

            set
            {
                this.SetValue(DataFieldXProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldY.
        /// </summary>
        public string DataFieldY
        {
            get
            {
                return (string)this.GetValue(DataFieldYProperty);
            }

            set
            {
                this.SetValue(DataFieldYProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Mapping.
        /// </summary>
        public Func<object, T> Mapping
        {
            get
            {
                return (Func<object, T>)this.GetValue(MappingProperty);
            }

            set
            {
                this.SetValue(MappingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerFill.
        /// </summary>
        public Color MarkerFill
        {
            get
            {
                return (Color)this.GetValue(MarkerFillProperty);
            }

            set
            {
                this.SetValue(MarkerFillProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerOutline.
        /// </summary>
        public ScreenPoint[] MarkerOutline
        {
            get
            {
                return (ScreenPoint[])this.GetValue(MarkerOutlineProperty);
            }

            set
            {
                this.SetValue(MarkerOutlineProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerSize.
        /// </summary>
        public double MarkerSize
        {
            get
            {
                return (double)this.GetValue(MarkerSizeProperty);
            }

            set
            {
                this.SetValue(MarkerSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerStroke.
        /// </summary>
        public Color MarkerStroke
        {
            get
            {
                return (Color)this.GetValue(MarkerStrokeProperty);
            }

            set
            {
                this.SetValue(MarkerStrokeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerStrokeThickness.
        /// </summary>
        public double MarkerStrokeThickness
        {
            get
            {
                return (double)this.GetValue(MarkerStrokeThicknessProperty);
            }

            set
            {
                this.SetValue(MarkerStrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerType.
        /// </summary>
        public MarkerType MarkerType
        {
            get
            {
                return (MarkerType)this.GetValue(MarkerTypeProperty);
            }

            set
            {
                this.SetValue(MarkerTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color axis key.
        /// </summary>
        /// <value>The color axis key.</value>
        public string ColorAxisKey { get; set; }

        /// <summary>
        /// Creates the internal series.
        /// </summary>
        /// <returns>The series.</returns>
        public override OxyPlot.Series.Series CreateModel()
        {
            this.SynchronizeProperties(this.InternalSeries);
            return this.InternalSeries;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.ScatterSeries<T>)series;
            s.MarkerFill = this.MarkerFill.ToOxyColor();
            s.MarkerStroke = this.MarkerStroke.ToOxyColor();
            s.MarkerStrokeThickness = this.MarkerStrokeThickness;
            s.MarkerType = this.MarkerType;
            s.MarkerSize = this.MarkerSize;
            s.DataFieldX = this.DataFieldX;
            s.DataFieldY = this.DataFieldY;
            s.DataFieldSize = this.DataFieldSize;
            s.DataFieldValue = this.DataFieldValue;
            s.DataFieldTag = this.DataFieldTag;
            s.ItemsSource = this.ItemsSource;
            s.BinSize = this.BinSize;
            s.Mapping = this.Mapping;
            s.MarkerOutline = this.MarkerOutline;
            s.ColorAxisKey = this.ColorAxisKey;
        }
    }
}