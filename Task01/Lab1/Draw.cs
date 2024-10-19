using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab1.OpenGL;

namespace Lab1
{
    internal class Draw
    {
        double x1, y1, x2, y2;

        public Draw(double x1, double x2, double y1, double y2)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
        }

        public void DrawItem(uint primitiveType, double[] vertices)
        {
            glBegin(primitiveType);
            for (int i = 0; i < vertices.Length; i += 2)
                glVertex2d(vertices[i], vertices[i + 1]);
            glEnd();
        }

        public void DrawBackgroundMarkup()
        {
            glLineWidth(1);
            glColor3d(0.5, 0.5, 0.5);
            glEnable(GL_LINE_STIPPLE);
            glLineStipple(1, 0x00FF);
            glBegin(GL_LINES);
            for (double x = x1 - 1; x < x2 + 1; x += 0.5)
            {
                glVertex2d(x, y1 - 1);
                glVertex2d(x, y2 + 1);
            }

            for (double y = y1 - 1; y < y2 + 1; y += 0.5)
            {
                glVertex2d(x1 - 1, y);
                glVertex2d(x2 + 1, y);
            }
            glEnd();
            glDisable(GL_LINE_STIPPLE);
        }

        public void DrawStreaks()
        {
            glBegin(GL_LINES);
            for (double x = x1; x < x2 + 0.5; x += 0.5)
            {
                glVertex2d(x, -0.95);
                glVertex2d(x, -1.05);
            }

            for (double y = y1; y < y2 + 0.5; y += 0.5)
            {
                glVertex2d(-3.95, y);
                glVertex2d(-4.05, y);
            }
            glEnd();
        }
    }
}
