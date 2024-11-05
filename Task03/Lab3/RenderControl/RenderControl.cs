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
        // значення за стандартом
        Function function;
        double xMin = -1;
        double xMax = 1;
        int points = 100;
        int selectedFunc = 1;

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

            // обчислюємо точки для обраної функції
            var pointsData = selectedFunc == 1
                ? function.FirstFunction(xMin, xMax, points)
                : function.SecondFunction(xMin, xMax, points);

            // отримуємо yMin та yMax
            var (yMin, yMax) = function.GetYRange(pointsData.pointsY);

            glOrtho(xMin, xMax, yMin, yMax, -1, +1);

            // малюємо осі та розмітку
            function.DrawCoordinateAxis(xMin, xMax, yMin, yMax);
            function.DrawBackgroundMarkup(xMin, xMax, yMin, yMax);

            // малюємо графік
            function.DrawGraph(pointsData);
        }

        //метод для створення об'єкту класу Function
        private void RenderControl_ContextCreated(object sender, EventArgs e)
        {
            function = new Function();
        }

        // метод для оновлення значень в RenderControl та запуску  повторного малювання
        public void SetValues(double minX, double maxX, int point, int func)
        {
            xMin = minX;
            xMax = maxX;
            points = point;
            selectedFunc = func;
            Invalidate(true);
        }
    }
}
