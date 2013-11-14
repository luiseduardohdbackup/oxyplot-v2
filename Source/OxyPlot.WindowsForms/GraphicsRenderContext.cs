﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicsRenderContext.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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
//   The graphics render context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WindowsForms
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.IO;
    using System.Linq;

    using OxyPlot;

    /// <summary>
    /// The graphics render context.
    /// </summary>
    public class GraphicsRenderContext : RenderContextBase
    {
        /// <summary>
        /// The font size factor.
        /// </summary>
        private const float FontsizeFactor = 0.8f;

        /// <summary>
        /// The images in use
        /// </summary>
        private readonly HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();

        /// <summary>
        /// The image cache
        /// </summary>
        private readonly Dictionary<OxyImage, Image> imageCache = new Dictionary<OxyImage, Image>();

        /// <summary>
        /// The GDI+ drawing surface.
        /// </summary>
        private Graphics g;

        /// <summary>
        /// Sets the graphics target.
        /// </summary>
        /// <param name="graphics">The graphics surface.</param>
        public void SetGraphicsTarget(Graphics graphics)
        {
            this.g = graphics;
            this.g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        }

        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The thickness.</param>
        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            if (fill.IsVisible())
            {
                this.g.FillEllipse(
                    fill.ToBrush(), (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
            }

            if (stroke.IsInvisible() || thickness <= 0)
            {
                return;
            }

            using (var pen = this.GetCachedPen(stroke, thickness))
            {
                this.g.SmoothingMode = SmoothingMode.HighQuality;
                this.g.DrawEllipse(pen, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
            }
        }

        /// <summary>
        /// Draws the polyline from the specified points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">if set to <c>true</c> the shape will be aliased.</param>
        public override void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPenLineJoin lineJoin,
            bool aliased)
        {
            if (stroke.IsInvisible() || thickness <= 0 || points.Count < 2)
            {
                return;
            }

            this.g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;
            using (var pen = this.GetCachedPen(stroke, thickness, dashArray, lineJoin))
            {
                this.g.DrawLines(pen, this.ToPoints(points));
            }
        }

        /// <summary>
        /// Draws the polygon from the specified points. The polygon can have stroke and/or fill.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">if set to <c>true</c> the shape will be aliased.</param>
        public override void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPenLineJoin lineJoin,
            bool aliased)
        {
            if (points.Count < 2)
            {
                return;
            }

            this.g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;

            var pts = this.ToPoints(points);
            if (fill.IsVisible())
            {
                this.g.FillPolygon(fill.ToBrush(), pts);
            }

            if (stroke.IsInvisible() || thickness <= 0)
            {
                return;
            }

            using (var pen = this.GetCachedPen(stroke, thickness))
            {
                if (dashArray != null)
                {
                    pen.DashPattern = this.ToFloatArray(dashArray);
                }

                switch (lineJoin)
                {
                    case OxyPenLineJoin.Round:
                        pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                        break;
                    case OxyPenLineJoin.Bevel:
                        pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                        break;

                    // The default LineJoin is Miter
                }

                this.g.DrawPolygon(pen, pts);
            }
        }

        /// <summary>
        /// Draws the rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            if (fill.IsVisible())
            {
                this.g.FillRectangle(
                    fill.ToBrush(), (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
            }

            if (stroke.IsInvisible() || thickness <= 0)
            {
                return;
            }

            using (var pen = this.GetCachedPen(stroke, thickness))
            {
                this.g.DrawRectangle(pen, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
            }
        }

        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="text">The text.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotate">The rotation angle.</param>
        /// <param name="halign">The horizontal alignment.</param>
        /// <param name="valign">The vertical alignment.</param>
        /// <param name="maxSize">The maximum size of the text.</param>
        public override void DrawText(
            ScreenPoint p,
            string text,
            OxyColor fill,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double rotate,
            HorizontalAlignment halign,
            VerticalAlignment valign,
            OxySize? maxSize)
        {
            var fs = FontStyle.Regular;
            if (fontWeight >= 700)
            {
                fs = FontStyle.Bold;
            }

            using (var font = new Font(fontFamily, (float)fontSize * FontsizeFactor, fs))
            {
                using (var sf = new StringFormat { Alignment = StringAlignment.Near })
                {
                    var size = this.g.MeasureString(text, font);
                    if (maxSize != null)
                    {
                        if (size.Width > maxSize.Value.Width)
                        {
                            size.Width = (float)maxSize.Value.Width;
                        }

                        if (size.Height > maxSize.Value.Height)
                        {
                            size.Height = (float)maxSize.Value.Height;
                        }
                    }

                    float dx = 0;
                    if (halign == HorizontalAlignment.Center)
                    {
                        dx = -size.Width / 2;
                    }

                    if (halign == HorizontalAlignment.Right)
                    {
                        dx = -size.Width;
                    }

                    float dy = 0;
                    sf.LineAlignment = StringAlignment.Near;
                    if (valign == VerticalAlignment.Middle)
                    {
                        dy = -size.Height / 2;
                    }

                    if (valign == VerticalAlignment.Bottom)
                    {
                        dy = -size.Height;
                    }

                    this.g.TranslateTransform((float)p.X, (float)p.Y);
                    if (Math.Abs(rotate) > double.Epsilon)
                    {
                        this.g.RotateTransform((float)rotate);
                    }

                    this.g.TranslateTransform(dx, dy);

                    var layoutRectangle = new RectangleF(0, 0, size.Width + 0.1f, size.Height + 0.1f);
                    this.g.DrawString(text, font, fill.ToBrush(), layoutRectangle, sf);

                    this.g.ResetTransform();
                }
            }
        }

        /// <summary>
        /// Measures the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>
        /// The text size.
        /// </returns>
        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (text == null)
            {
                return OxySize.Empty;
            }

            var fs = FontStyle.Regular;
            if (fontWeight >= 700)
            {
                fs = FontStyle.Bold;
            }

            using (var font = new Font(fontFamily, (float)fontSize * FontsizeFactor, fs))
            {
                var size = this.g.MeasureString(text, font);
                return new OxySize(size.Width, size.Height);
            }
        }

        /// <summary>
        /// Cleans up resources not in use.
        /// </summary>
        /// <remarks>
        /// This method is called at the end of each rendering.
        /// </remarks>
        public override void CleanUp()
        {
            var imagesToRelease = imageCache.Keys.Where(i => !imagesInUse.Contains(i)).ToList();
            foreach (var i in imagesToRelease)
            {
                var image = this.GetImage(i);
                image.Dispose();
                imageCache.Remove(i);
            }

            imagesInUse.Clear();
        }

        /// <summary>
        /// Draws the image.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="srcX">The source executable.</param>
        /// <param name="srcY">The source asynchronous.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="x">The executable.</param>
        /// <param name="y">The asynchronous.</param>
        /// <param name="w">The forward.</param>
        /// <param name="h">The authentication.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="interpolate">if set to <c>true</c> [interpolate].</param>
        public override void DrawImage(OxyImage source, double srcX, double srcY, double srcWidth, double srcHeight, double x, double y, double w, double h, double opacity, bool interpolate)
        {
            var image = this.GetImage(source);
            if (image != null)
            {
                ImageAttributes ia = null;
                if (opacity < 1)
                {
                    var cm = new ColorMatrix
                                 {
                                     Matrix00 = 1f,
                                     Matrix11 = 1f,
                                     Matrix22 = 1f,
                                     Matrix33 = 1f,
                                     Matrix44 = (float)opacity
                                 };

                    ia = new ImageAttributes();
                    ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                }

                g.InterpolationMode = interpolate ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
                int sx = (int)Math.Floor(x);
                int sy = (int)Math.Floor(y);
                int sw = (int)Math.Ceiling(x + w) - sx;
                int sh = (int)Math.Ceiling(y + h) - sy;
                var destRect = new Rectangle(sx, sy, sw, sh);
                g.DrawImage(image, destRect, (float)srcX - 0.5f, (float)srcY - 0.5f, (float)srcWidth, (float)srcHeight, GraphicsUnit.Pixel, ia);
            }
        }

        /// <summary>
        /// Sets the clip rectangle.
        /// </summary>
        /// <param name="rect">The clip rectangle.</param>
        /// <returns>
        /// True if the clip rectangle was set.
        /// </returns>
        public override bool SetClip(OxyRect rect)
        {
            this.g.SetClip(rect.ToRect(false));
            return true;
        }

        /// <summary>
        /// Resets the clip rectangle.
        /// </summary>
        public override void ResetClip()
        {
            this.g.ResetClip();
        }

        private Image GetImage(OxyImage source)
        {
            if (source == null)
            {
                return null;
            }

            if (!this.imagesInUse.Contains(source))
            {
                this.imagesInUse.Add(source);
            }

            Image src;
            if (this.imageCache.TryGetValue(source, out src))
            {
                return src;
            }

            Image btm;
            using (var ms = new MemoryStream(source.GetData()))
            {
                btm = Image.FromStream(ms);
            }

            this.imageCache.Add(source, btm);
            return btm;
        }

        private Pen GetCachedPen(OxyColor stroke, double thickness, double[] dashArray = null, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter)
        {
            // TODO: cache
            var pen = new Pen(stroke.ToColor(), (float)thickness);
            if (dashArray != null)
            {
                pen.DashPattern = this.ToFloatArray(dashArray);
            }

            switch (lineJoin)
            {
                case OxyPenLineJoin.Round:
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    break;
                case OxyPenLineJoin.Bevel:
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                    break;
                // The default LineJoin is Miter
            }

            return pen;
        }
        
        /// <summary>
        /// Converts a double array to a float array.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <returns>
        /// The float array.
        /// </returns>
        private float[] ToFloatArray(double[] a)
        {
            if (a == null)
            {
                return null;
            }

            var r = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                r[i] = (float)a[i];
            }

            return r;
        }

        /// <summary>
        /// Converts a list of point to an array of PointF.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <returns>
        /// An array of points.
        /// </returns>
        private PointF[] ToPoints(IList<ScreenPoint> points)
        {
            if (points == null)
            {
                return null;
            }

            var r = new PointF[points.Count()];
            int i = 0;
            foreach (ScreenPoint p in points)
            {
                r[i++] = new PointF((float)p.X, (float)p.Y);
            }

            return r;
        }
    }
}