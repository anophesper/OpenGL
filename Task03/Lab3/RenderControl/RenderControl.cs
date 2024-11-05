using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;

namespace Lab3
{
    public partial class RenderControl : OpenGL
    {
        Function function;

        public RenderControl()
        {
            InitializeComponent();
        }

        private void RenderControl_Render(object sender, EventArgs e)
        {
            glClear(GL_COLOR_BUFFER_BIT);
            glLoadIdentity();

            if (Width > Height)
                glViewport((Width - Height) / 2, 0, Height, Height);
            else
                glViewport(0, (Height - Width) / 2, Width, Width);

            glOrtho(function.GetXMin(), function.GetXMax(), function.GetYMin(), function.GetYMax(), -1, +1);

            // малюємо осі та розмітку
            function.DrawBackgroundMarkup();
            function.DrawCoordinateAxis();

            // малюємо графік
            function.DrawGraph();
        }

        //метод для створення об'єкту класу Function
        private void RenderControl_ContextCreated(object sender, EventArgs e)
        {
            function = new Function(-1, 1, 100, 1);
        }

        // метод для оновлення значень в RenderControl та запуску  повторного малювання
        public void SetValues(double minX, double maxX, int points, int func)
        {
            function.SetX(minX, maxX);
            function.SetPoints(points);
            function.SetCurrentSelectedFunc(func);
            Invalidate(true);
        }
    }
}
