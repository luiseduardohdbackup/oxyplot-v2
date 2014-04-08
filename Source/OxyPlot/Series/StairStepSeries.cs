﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StairStepSeries.cs" company="OxyPlot">
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
//   Represents a series for stair step graphs.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a series for stair step graphs.
    /// </summary>
    public class StairStepSeries : LineSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "StairStepSeries" /> class.
        /// </summary>
        public StairStepSeries()
        {
            this.VerticalStrokeThickness = double.NaN;
            this.VerticalLineStyle = this.LineStyle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StairStepSeries" /> class.
        /// </summary>
        /// <param name="title">The title.</param>
        [Obsolete]
        public StairStepSeries(string title)
            : this()
        {
            this.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StairStepSeries" /> class.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="title">The title.</param>
        [Obsolete]
        public StairStepSeries(OxyColor color, double strokeThickness = 1, string title = null)
            : base(color, strokeThickness, title)
        {
            this.VerticalStrokeThickness = double.NaN;
            this.VerticalLineStyle = this.LineStyle;
        }

        /// <summary>
        /// Gets or sets the stroke thickness of the vertical line segments.
        /// </summary>
        /// <value>The vertical stroke thickness.</value>
        /// <remarks>Set the value to NaN to use the StrokeThickness property for both horizontal and vertical segments.
        /// Using the VerticalStrokeThickness property will have a small performance hit.</remarks>
        public double VerticalStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the line style of the vertical line segments.
        /// </summary>
        /// <value>The vertical line style.</value>
        public LineStyle VerticalLineStyle { get; set; }

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">interpolate if set to <c>true</c> .</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (this.XAxis == null || this.YAxis == null)
            {
                return null;
            }

            // http://paulbourke.net/geometry/pointlineplane/
            double minimumDistanceSquared = 16 * 16;

            // snap to the nearest point
            var result = this.GetNearestPointInternal(this.ActualPoints, point);
            if (!interpolate && result != null && result.Position.DistanceToSquared(point) < minimumDistanceSquared)
            {
                return result;
            }

            result = null;

            // find the nearest point on the horizontal line segments
            int n = this.ActualPoints.Count;
            for (int i = 0; i < n; i++)
            {
                var p1 = this.ActualPoints[i];
                var p2 = this.ActualPoints[i + 1 < n ? i + 1 : i];
                var sp1 = this.Transform(p1.X, p1.Y);
                var sp2 = this.Transform(p2.X, p1.Y);

                double spdx = sp2.x - sp1.x;
                double spdy = sp2.y - sp1.y;
                double u1 = ((point.x - sp1.x) * spdx) + ((point.y - sp1.y) * spdy);
                double u2 = (spdx * spdx) + (spdy * spdy);
                double ds = (spdx * spdx) + (spdy * spdy);

                if (ds < 4)
                {
                    // if the points are very close, we can get numerical problems, just use the first point...
                    u1 = 0;
                    u2 = 1;
                }

                if (Math.Abs(u2) < double.Epsilon)
                {
                    continue; // P1 && P2 coincident
                }

                double u = u1 / u2;
                if (u < 0 || u > 1)
                {
                    continue; // outside line
                }

                double sx = sp1.x + (u * spdx);
                double sy = sp1.y + (u * spdy);

                double dx = point.x - sx;
                double dy = point.y - sy;
                double distanceSquared = (dx * dx) + (dy * dy);

                if (distanceSquared < minimumDistanceSquared)
                {
                    double px = p1.X + (u * (p2.X - p1.X));
                    double py = p1.Y;
                    result = new TrackerHitResult(
                        this, new DataPoint(px, py), new ScreenPoint(sx, sy), this.GetItem(i), i);
                    minimumDistanceSquared = distanceSquared;
                }
            }

            return result;
        }

        /// <summary>
        /// Renders the LineSeries on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="model">The owner plot model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (this.ActualPoints.Count == 0)
            {
                return;
            }

            this.VerifyAxes();

            var clippingRect = this.GetClippingRect();
            var dashArray = this.ActualDashArray;
            var verticalLineDashArray = this.VerticalLineStyle.GetDashArray();
            var lineStyle = this.ActualLineStyle;
            var verticalStrokeThickness = double.IsNaN(this.VerticalStrokeThickness)
                                              ? this.StrokeThickness
                                              : this.VerticalStrokeThickness;

            var actualColor = this.GetSelectableColor(this.ActualColor);

            Action<IList<ScreenPoint>, IList<ScreenPoint>> renderPoints = (lpts, mpts) =>
                {
                    // clip the line segments with the clipping rectangle
                    if (this.StrokeThickness > 0 && lineStyle != LineStyle.None)
                    {
                        if (!verticalStrokeThickness.Equals(this.StrokeThickness) || this.VerticalLineStyle != lineStyle)
                        {
                            // TODO: change to array
                            var hlpts = new List<ScreenPoint>();
                            var vlpts = new List<ScreenPoint>();
                            for (int i = 0; i + 2 < lpts.Count; i += 2)
                            {
                                hlpts.Add(lpts[i]);
                                hlpts.Add(lpts[i + 1]);
                                vlpts.Add(lpts[i + 1]);
                                vlpts.Add(lpts[i + 2]);
                            }

                            rc.DrawClippedLineSegments(
                                hlpts,
                                clippingRect,
                                actualColor,
                                this.StrokeThickness,
                                dashArray,
                                this.LineJoin,
                                false);
                            rc.DrawClippedLineSegments(
                                vlpts,
                                clippingRect,
                                actualColor,
                                verticalStrokeThickness,
                                verticalLineDashArray,
                                this.LineJoin,
                                false);
                        }
                        else
                        {
                            rc.DrawClippedLine(
                                lpts,
                                clippingRect,
                                0,
                                actualColor,
                                this.StrokeThickness,
                                dashArray,
                                this.LineJoin,
                                false);
                        }
                    }

                    if (this.MarkerType != MarkerType.None)
                    {
                        rc.DrawMarkers(
                            mpts,
                            clippingRect,
                            this.MarkerType,
                            this.MarkerOutline,
                            new[] { this.MarkerSize },
                            this.MarkerFill,
                            this.MarkerStroke,
                            this.MarkerStrokeThickness);
                    }
                };

            // Transform all points to screen coordinates
            // Render the line when invalid points occur
            var linePoints = new List<ScreenPoint>();
            var markerPoints = new List<ScreenPoint>();
            double previousY = double.NaN;
            foreach (var point in this.ActualPoints)
            {
                if (!this.IsValidPoint(point, this.XAxis, this.YAxis))
                {
                    renderPoints(linePoints, markerPoints);
                    linePoints.Clear();
                    markerPoints.Clear();
                    previousY = double.NaN;
                    continue;
                }

                var transformedPoint = this.Transform(point);
                if (!double.IsNaN(previousY))
                {
                    // Horizontal line from the previous point to the current x-coordinate
                    linePoints.Add(new ScreenPoint(transformedPoint.X, previousY));
                }

                linePoints.Add(transformedPoint);
                markerPoints.Add(transformedPoint);
                previousY = transformedPoint.Y;
            }

            renderPoints(linePoints, markerPoints);
        }
    }
}