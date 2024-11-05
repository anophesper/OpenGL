using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;

namespace Lab2
{
    public partial class RenderControl : OpenGL
    {
        Figure figure;
        // значення за стандартом
        private int horizontalCount = 1;
        private int verticalCount = 1;
        private uint renderMode = GL_TRIANGLES;

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

            int size = 100;
            glOrtho(-size, +size * ((horizontalCount + verticalCount - 1) * 1.5), -size * horizontalCount*1.25, +size * verticalCount*1.25, -1, +1);
            figure.SetFigureCenters(verticalCount, horizontalCount, renderMode);
        }

        //створення об'єкту класу Figure
        private void RenderControl_ContextCreated(object sender, EventArgs e)
        {
            figure = new Figure();
        }

        // метод для оновлення значень в RenderControl та запуску  повторного малювання
        public void SetTileCounts(int hCount, int vCount, uint rMode)
        {
            horizontalCount = hCount;
            verticalCount = vCount;
            renderMode = rMode;
            Invalidate(true);
        }
    }
}
