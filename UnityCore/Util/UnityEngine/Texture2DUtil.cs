using System;
using System.Collections.Generic;
using UnityCore;

namespace UnityEngine
{
    public static class Texture2DUtil
    {
        /// <summary>
        /// Draws a pixel just like SetPixel except 0,0 is the left top corner.
        /// </summary>
        static void DrawPixels(this Texture2D texture, int x, int y,int width,int height, Color color)
        {
            for(int i=0;i<width;i++)
            {
                for(int j=0;j<height;j++)
                {
                    texture.DrawPixel(x+i, y+j, color);
                }
            }
        }

        /// <summary>
        /// Draws a pixel just like SetPixel except 0,0 is the left top corner.
        /// </summary>
        static void DrawPixel(this Texture2D texture, int x, int y, Color color)
        {
            //if (x < 0 || x > texture.width || y < 0 || y > texture.height)
            //{
            //    return;
            //}
            texture.SetPixel(x, y, color);
        }


        /// <summary>
        /// Draws a circle with the midpoint being x0, x1.
        /// Implementation of Bresenham's circle algorithm
        /// </summary>
        public static void DrawCircle(this Texture2D texture, Vector2Int a, int radius, Color color)
        {
            Circle(texture, a.x, a.y, radius, color, false);
        }

        /// <summary>
        /// Draws a filled circle with the midpoint being x0, x1.
        /// Implementation of Bresenham's circle algorithm
        /// </summary>
        public static void DrawFilledCircle(this Texture2D texture, Vector2Int a, int radius, Color color)
        {
            Circle(texture, a.x, a.y, radius, color, true);
        }


        private static void Circle(Texture2D texture, int x, int y, int radius, Color color, bool filled = false)
        {
            int cx = radius;
            int cy = 0;
            int radiusError = 1 - cx;

            while (cx >= cy)
            {
                if (!filled)
                {
                    PlotCircle(texture, cx, x, cy, y, color);
                }
                else
                {
                    ScanLineCircle(texture, cx, x, cy, y, color);
                }

                cy++;

                if (radiusError < 0)
                {
                    radiusError += 2 * cy + 1;
                }
                else
                {
                    cx--;
                    radiusError += 2 * (cy - cx + 1);
                }
            }
        }

        private static void PlotCircle(Texture2D texture, int cx, int x, int cy, int y, Color color)
        {
            texture.DrawPixel(cx + x, cy + y, color); // Point in octant 1...
            texture.DrawPixel(cy + x, cx + y, color);
            texture.DrawPixel(-cx + x, cy + y, color);
            texture.DrawPixel(-cy + x, cx + y, color);
            texture.DrawPixel(-cx + x, -cy + y, color);
            texture.DrawPixel(-cy + x, -cx + y, color);
            texture.DrawPixel(cx + x, -cy + y, color);
            texture.DrawPixel(cy + x, -cx + y, color); // ... point in octant 8
        }

        // Draw scanlines from opposite sides of the circle on y-scanlines instead of just plotting pixels
        // at the right coordinates
        private static void ScanLineCircle(Texture2D texture, int cx, int x, int cy, int y, Color color)
        {
            //texture.DrawPixel(cx + x,  cy + y, color);
            //texture.DrawPixel(-cx + x, cy + y, color);
            texture.DrawLine(cx + x, cy + y, -cx + x, cy + y, color);

            //texture.DrawPixel(cy + x,  cx + y, color);
            //texture.DrawPixel(-cy + x, cx + y, color);
            texture.DrawLine(cy + x, cx + y, -cy + x, cx + y, color);

            //texture.DrawPixel(-cx + x, -cy + y, color);
            //texture.DrawPixel(cx + x,  -cy + y, color);
            texture.DrawLine(-cx + x, -cy + y, cx + x, -cy + y, color);

            //texture.DrawPixel(-cy + x, -cx + y, color);
            //texture.DrawPixel(cy + x,  -cx + y, color);
            texture.DrawLine(-cy + x, -cx + y, cy + x, -cx + y, color);
        }


        /// <summary>
        /// Starts a flood fill at point startX, startY.
		/// This is a pretty slow flood fill, biggest bottle neck is comparing two colors which happens
		/// a lot. Should be a way to make it much faster.
        /// O(n) space.  n = width*height - makes a copy of the bitmap temporarily in the memory
        /// </summary>
        public static void FloodFill(this Texture2D texture, Vector2Int start, Color newColor)
        {

            Flat2DArray copyBmp = new Flat2DArray(texture.height, texture.width, texture.GetPixels());

            Color originalColor = texture.GetPixel(start.x, start.y);
            int width = texture.width;
            int height = texture.height;


            if (originalColor == newColor)
            {
                return;
            }

            copyBmp[start.x, start.y] = newColor;

            Queue<Vector2Int> openNodes = new Queue<Vector2Int>();
            openNodes.Enqueue(start);

            int i = 0;

            // TODO: remove this
            // emergency switch so it doesn't hang if something goes wrong
            int emergency = width * height;

            while (openNodes.Count > 0)
            {
                i++;

                if (i > emergency)
                {
                    return;
                }

                Vector2Int current = openNodes.Dequeue();
                int x = current.x;
                int y = current.y;

                if (x > 0)
                {
                    if (copyBmp[x - 1, y] == originalColor)
                    {
                        copyBmp[x - 1, y] = newColor;
                        openNodes.Enqueue(new Vector2Int(x - 1, y));
                    }
                }
                if (x < width - 1)
                {
                    if (copyBmp[x + 1, y] == originalColor)
                    {
                        copyBmp[x + 1, y] = newColor;
                        openNodes.Enqueue(new Vector2Int(x + 1, y));
                    }
                }
                if (y > 0)
                {
                    if (copyBmp[x, y - 1] == originalColor)
                    {
                        copyBmp[x, y - 1] = newColor;
                        openNodes.Enqueue(new Vector2Int(x, y - 1));
                    }
                }
                if (y < height - 1)
                {
                    if (copyBmp[x, y + 1] == originalColor)
                    {
                        copyBmp[x, y + 1] = newColor;
                        openNodes.Enqueue(new Vector2Int(x, y + 1));
                    }
                }
            }

            texture.SetPixels(copyBmp.data);
        }

        // Could be its own file
        private class Flat2DArray
        {
            public Color[] data;
            private readonly int height;
            private readonly int width;

            public Flat2DArray(int height, int width, Color[] data)
            {
                this.height = height;
                this.width = width;

                this.data = data;
            }

            public Color this[int x, int y]
            {
                get
                {
                    return data[x + y * width];
                }
                set
                {
                    data[x + y * width] = value;
                }
            }
        }

        /// <summary>
        /// Draws a rectangle
        /// </summary>
        public static void DrawRectangle(this Texture2D texture, RectInt rectangle, Color color)
        {
            int x = rectangle.x;
            int y = rectangle.y;
            int height = rectangle.height;
            int width = rectangle.width;
            

            // top left to bottom left
            texture.DrawLine(x, y, x, y + height, color);

            // bottom left to bottom right
            texture.DrawLine(x, y + height, x + width, y + height, color);

            // bottom right to top right
            texture.DrawLine(x + width, y + height, x + width, y, color);

            // top right to top left
            texture.DrawLine(x + width, y, x, y, color);

        }

        /// <summary>
        /// Fills the given rectangle area with a solid color.
        /// </summary>
        public static void DrawFilledRectangle(this Texture2D texture, RectInt rectangle, Color color)
        {
            Color[] colorsArray = new Color[rectangle.width * rectangle.height];
            for (int i = 0; i < colorsArray.Length; i++)
            {
                colorsArray[i] = color;
            }

            texture.SetPixels(rectangle.x, rectangle.y,rectangle.width, rectangle.height, colorsArray);
        }

        public static void DrawLine(this Texture2D texture, Vector2Int start, Vector2Int end, Color color)
        {
            texture.DrawLine(start.x, start.y, end.x, end.y, color);
        }
        public static void DrawLine(this Texture2D texture, Vector2Int start, Vector2Int end, int lineWidth, Color color)
        {
            texture.DrawLine(start.x, start.y, end.x, end.y, lineWidth, color);
        }
        /// <summary>
        /// Draws a line between two points. Implementation of Bresenham's line algorithm.
        /// </summary>
        static void DrawLine(this Texture2D texture, int x0, int y0, int x1, int y1, Color color)
        {
            DrawCalc.DrawLine(x0, y0, x1, y1, (x, y) =>
            {
                texture.DrawPixel(x, y, color);
            });
        }
        /// <summary>
        /// Draws a line between two points. Implementation of Bresenham's line algorithm.
        /// </summary>
        static void DrawLine(this Texture2D texture, int x0, int y0, int x1, int y1,int lineWidth, Color color)
        {
            int delta = lineWidth / 2;
            
            DrawCalc.DrawLine(x0, y0, x1, y1, (x, y) =>
            {
                texture.DrawPixels(x - delta, y - delta, lineWidth, lineWidth, color);
            });
        }

        
    }
}
