﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorColumnSeriesExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;

    [Examples("ErrorColumnSeries")]
    public class ErrorColumnSeriesExamples
    {
        [Example("ErrorColumnSeries")]
        public static PlotModel GetErrorColumnSeries()
        {
            var model = new PlotModel("ErrorColumnSeries")
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            var s1 = new ErrorColumnSeries { Title = "Series 1", IsStacked = false, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Items.Add(new ErrorColumnItem { Value = 25, Error = 2 });
            s1.Items.Add(new ErrorColumnItem { Value = 137, Error = 25 });
            s1.Items.Add(new ErrorColumnItem { Value = 18, Error = 4 });
            s1.Items.Add(new ErrorColumnItem { Value = 40, Error = 29 });

            var s2 = new ErrorColumnSeries { Title = "Series 2", IsStacked = false, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Items.Add(new ErrorColumnItem { Value = 35, Error = 20 });
            s2.Items.Add(new ErrorColumnItem { Value = 17, Error = 7 });
            s2.Items.Add(new ErrorColumnItem { Value = 118, Error = 44 });
            s2.Items.Add(new ErrorColumnItem { Value = 49, Error = 29 });

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");

            var valueAxis = new LinearAxis(AxisPosition.Left) { MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);

            return model;
        }

        //[Example("ErrorColumnSeries (stacked)")]
        //public static PlotModel GetErrorColumnSeriesStacked()
        //{
        //    var model = GetErrorColumnSeries();
        //    foreach (ErrorColumnSeries s in model.Series)
        //        s.IsStacked = true;
        //    return model;
        //}
    }
}
