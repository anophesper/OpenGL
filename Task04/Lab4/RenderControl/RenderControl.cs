using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Lab4
{
    public partial class RenderControl : OpenGL
    {
        Function function;
        bool is_paint = false;
        Point point;
        private double startXNormalized, startYNormalized, endXNormalized, endYNormalized;
        int offsetX, offsetY, effectiveWidth, effectiveHeight;

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


            // вирахування зміщення при нерівних Width і Height (інакше лінія буде малюватись зі зміщенням а не чітко по координатам положення курсора)
            offsetX = Width > Height ? (Width - Height) / 2 : 0;
            effectiveWidth = Width > Height ? Height : Width;
            offsetY = Height > Width ? (Height - Width) / 2 : 0;
            effectiveHeight = Height > Width ? Width : Height;

            glOrtho(function.GetXMin(), function.GetXMax(), function.GetYMin(), function.GetYMax(), -1, +1);

            // малюємо осі та розмітку
            function.DrawBackgroundMarkup();
            function.DrawCoordinateAxis();

            // малюємо графік
            function.DrawGraph();

            // малюємо лінію
            DrawLine(startXNormalized, startYNormalized, endXNormalized, endYNormalized);

            // Знаходимо точки перетину
            //List<PointF> intersectionPoints = FindIntersectionPoints();
            //function.DrawIntersectionPoints(intersectionPoints);
        }

        private void RenderControl_ContextCreated(object sender, EventArgs e)
        {
            function = new Function(1);
        }

        // метод для оновлення значень в RenderControl та запуску  повторного малювання
        public void SetValues(int func)
        {
            function.SetCurrentSelectedFunc(func);
            startXNormalized = 0; 
            startYNormalized = 0; 
            endXNormalized = 0; 
            endYNormalized = 0;
            Invalidate(true);
        }

        // методи для вираховування координат для малювання лінії при натисканні кнопки миші
        private void RenderControl_MouseDown(object sender, MouseEventArgs e)
        {
            is_paint = e.Button == MouseButtons.Left;
            point = e.Location;

            // Зберігаємо початкові координати в нормалізованому вигляді
            startXNormalized = ((double)point.X - offsetX) / effectiveWidth;
            startYNormalized = 1 - ((double)point.Y - offsetY) / effectiveHeight; // Інвертуємо Y
        }

        private void RenderControl_MouseUp(object sender, MouseEventArgs e)
        {
            is_paint = e.Button != MouseButtons.Left;
            point = e.Location;

            // Зберігаємо кінцеві координати в нормалізованому вигляді
            endXNormalized = ((double)point.X - offsetX) / effectiveWidth;
            endYNormalized = 1 - ((double)point.Y - offsetY) / effectiveHeight; // Інвертуємо Y

            Invalidate();
        }

        private void RenderControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (is_paint)
            {
                point = e.Location;

                // Зберігаємо поточні координати в нормалізованому вигляді
                endXNormalized = ((double)point.X - offsetX) / effectiveWidth;
                endYNormalized = 1 - ((double)point.Y - offsetY) / effectiveHeight; // Інвертуємо Y

                Invalidate();
            }
        }

        // метод для малювання лінії по отриманим координатам
        private void DrawLine(double startXNorm, double startYNorm, double endXNorm, double endYNorm)
        {
            glBegin(GL_LINES);

            // Перетворення нормалізованих координат в координати в межах осей X та Y
            double startX = function.GetXMin() + (function.GetXMax() - function.GetXMin()) * startXNorm;
            double startY = function.GetYMin() + (function.GetYMax() - function.GetYMin()) * startYNorm;
            double endX = function.GetXMin() + (function.GetXMax() - function.GetXMin()) * endXNorm;
            double endY = function.GetYMin() + (function.GetYMax() - function.GetYMin()) * endYNorm;

            glVertex2d(startX, startY);
            glVertex2d(endX, endY);

            glEnd();
        }

        //ЩОСЬ НЕ ТАК ПРАЦЮЄ
        private List<PointF> FindIntersectionPoints()
        {
            List<PointF> intersectionPoints = new List<PointF>();

            // Якщо лінія вертикальна
            bool isVertical = Math.Abs(endXNormalized - startXNormalized) < 1e-6;
            double m = 0, b = 0;
            if (!isVertical)
            {
                m = (endYNormalized - startYNormalized) / (endXNormalized - startXNormalized);
                b = startYNormalized - m * startXNormalized;
            }

            // Отримуємо точки графіка
            double[] pointsX = function.GetPointsX();
            double[] pointsY = function.GetPointsY();
            int pointsCount = function.GetPoints();

            for (int i = 0; i < pointsCount - 1; i++)
            {
                // Поточний сегмент графіка
                double x1 = pointsX[i], y1 = pointsY[i];
                double x2 = pointsX[i + 1], y2 = pointsY[i + 1];

                double xIntersect = 0, yIntersect = 0;

                if (isVertical)
                {
                    // Перетин з вертикальною лінією
                    xIntersect = startXNormalized * (function.GetXMax() - function.GetXMin()) + function.GetXMin();
                    if (Math.Abs(x2 - x1) < 1e-6) // Графік також вертикальний
                        continue;

                    if (xIntersect < Math.Min(x1, x2) || xIntersect > Math.Max(x1, x2))
                        continue;

                    yIntersect = y1 + (xIntersect - x1) * (y2 - y1) / (x2 - x1);
                }
                else
                {
                    // Перетин двох прямих
                    double denom = (x2 - x1) - m * (y2 - y1);
                    if (Math.Abs(denom) < 1e-6) // Лінії паралельні
                        continue;

                    double ua = ((x1 - startXNormalized) + m * (startYNormalized - y1)) / denom;
                    if (ua < 0 || ua > 1) // Перетин поза сегментом
                        continue;

                    xIntersect = x1 + ua * (x2 - x1);
                    yIntersect = y1 + ua * (y2 - y1);
                }

                // Перевірка меж Y
                if (yIntersect >= Math.Min(y1, y2) && yIntersect <= Math.Max(y1, y2))
                {
                    intersectionPoints.Add(new PointF((float)xIntersect, (float)yIntersect));
                }
            }

            return intersectionPoints;
        }
    }
}
