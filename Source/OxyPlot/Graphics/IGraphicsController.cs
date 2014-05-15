﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphicsController.cs" company="OxyPlot">
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
//   Specifies functionality to interact with a graphics view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies functionality to interact with a graphics view.
    /// </summary>
    public interface IGraphicsController
    {
        /// <summary>
        /// Handles mouse down events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleMouseDown(IGraphicsView view, OxyMouseDownEventArgs args);

        /// <summary>
        /// Handles mouse move events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleMouseMove(IGraphicsView view, OxyMouseEventArgs args);

        /// <summary>
        /// Handles mouse up events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleMouseUp(IGraphicsView view, OxyMouseEventArgs args);

        /// <summary>
        /// Handles mouse enter events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleMouseEnter(IGraphicsView view, OxyMouseEventArgs args);

        /// <summary>
        /// Handles mouse leave events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleMouseLeave(IGraphicsView view, OxyMouseEventArgs args);

        /// <summary>
        /// Handles mouse wheel events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseWheelEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleMouseWheel(IGraphicsView view, OxyMouseWheelEventArgs args);

        /// <summary>
        /// Handles touch started events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyTouchEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleTouchStarted(IGraphicsView view, OxyTouchEventArgs args);

        /// <summary>
        /// Handles touch delta events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyTouchEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleTouchDelta(IGraphicsView view, OxyTouchEventArgs args);

        /// <summary>
        /// Handles touch completed events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyTouchEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleTouchCompleted(IGraphicsView view, OxyTouchEventArgs args);

        /// <summary>
        /// Handles key down events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyKeyEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleKeyDown(IGraphicsView view, OxyKeyEventArgs args);

        /// <summary>
        /// Handles the specified gesture.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="gesture">The gesture.</param>
        /// <param name="args">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleGesture(IGraphicsView view, OxyInputGesture gesture, OxyInputEventArgs args);

        /// <summary>
        /// Adds the specified mouse manipulator and invokes the <see cref="MouseManipulator.Started" /> method with the specified mouse event arguments.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="manipulator">The manipulator to add.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        void AddMouseManipulator(IGraphicsView view, MouseManipulator manipulator, OxyMouseDownEventArgs args);

        /// <summary>
        /// Adds the specified mouse hover manipulator and invokes the <see cref="MouseManipulator.Started" /> method with the specified mouse event arguments.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="manipulator">The manipulator.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        void AddHoverManipulator(IGraphicsView view, MouseManipulator manipulator, OxyMouseEventArgs args);

        /// <summary>
        /// Adds the specified touch manipulator and invokes the <see cref="MouseManipulator.Started" /> method with the specified mouse event arguments.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="manipulator">The manipulator.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        void AddTouchManipulator(IGraphicsView view, TouchManipulator manipulator, OxyTouchEventArgs args);

        /// <summary>
        /// Binds the specified command to the specified mouse down gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The mouse down gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        void Bind(OxyMouseDownGesture gesture, IGraphicsControllerCommand<OxyMouseDownEventArgs> command);

        /// <summary>
        /// Binds the specified command to the specified mouse enter gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The mouse enter gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        void Bind(OxyMouseEnterGesture gesture, IGraphicsControllerCommand<OxyMouseEventArgs> command);

        /// <summary>
        /// Binds the specified command to the specified mouse wheel gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The mouse wheel gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        void Bind(OxyMouseWheelGesture gesture, IGraphicsControllerCommand<OxyMouseWheelEventArgs> command);

        /// <summary>
        /// Binds the specified command to the specified touch gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The touch gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        void Bind(OxyTouchGesture gesture, IGraphicsControllerCommand<OxyTouchEventArgs> command);

        /// <summary>
        /// Binds the specified command to the specified key gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The key gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        void Bind(OxyKeyGesture gesture, IGraphicsControllerCommand<OxyKeyEventArgs> command);

        /// <summary>
        /// Unbinds the specified gesture.
        /// </summary>
        /// <param name="gesture">The gesture to unbind.</param>
        void Unbind(OxyInputGesture gesture);

        /// <summary>
        /// Unbinds the specified command from all gestures.
        /// </summary>
        /// <param name="command">The command to unbind.</param>
        void Unbind(IGraphicsControllerCommand command);

        /// <summary>
        /// Unbinds all commands.
        /// </summary>
        void UnbindAll();
    }
}