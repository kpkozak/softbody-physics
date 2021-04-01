using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Geometry;
using Geometry.Shapes;
using Geometry.Vector;

namespace PostScriptImage
{
    public class Image
    {
        private readonly bool _finalized;
        private readonly double _lineMaxX;
        private readonly double _lineMinX;
        private readonly StringBuilder _stringBuilder;

        public Image(Vector2 boundingBoxStart, Vector2 boundingBoxEnd)
        {
            _lineMinX = boundingBoxStart.X;
            _lineMaxX = boundingBoxEnd.X;

            _stringBuilder = new StringBuilder();
            _finalized = false;

            AddHeader(boundingBoxStart, boundingBoxEnd);
        }

        private void AddHeader(Vector2 boundingBoxStart, Vector2 boundingBoxEnd)
        {
            _stringBuilder.AppendFormat("%! PS_Adobe-3.0 EPSF-3.0\n");
            _stringBuilder.AppendFormat("%%BoundingBox: {0} {1} {2} {3}\n", (int) boundingBoxStart.X,
                (int) boundingBoxStart.Y,
                (int) boundingBoxEnd.X, (int) boundingBoxEnd.Y);
            _stringBuilder.AppendFormat(
                "%%BeginSetup\n% A4, unrotated\n<< /PageSize [{0} {1}] /Orientation 0 >> setpagedevice\n%%EndSetup",
                (int) (boundingBoxEnd.X - boundingBoxStart.X), (int) (boundingBoxEnd.Y - boundingBoxStart.Y));
            _stringBuilder.AppendFormat("% \n");
            _stringBuilder.AppendFormat("/bd {{bind def}} bind def \n");
            _stringBuilder.AppendFormat("/ld {{load def}} bd \n");
            _stringBuilder.AppendFormat("/m  /moveto ld \n");
            _stringBuilder.AppendFormat("/l  /lineto ld \n");
            _stringBuilder.AppendFormat("/vl {{aload pop lineto}} bd \n");
            _stringBuilder.AppendFormat("/vm {{aload pop moveto}} bd \n");
            _stringBuilder.AppendFormat("/tx {{/Times-Roman findfont 16 scalefont setfont}} bd \n");
            _stringBuilder.AppendFormat("/pt {{aload pop 3 0 360 arc 1 setlinewidth fill stroke}} bd \n"); //punkt
            _stringBuilder.AppendFormat("/arc0 {{0 360 arc}} bd \n");
        }

        public void AddPoint(Vector2 point, string name)
        {
            _stringBuilder.AppendFormat("/{0} [ {1} {2} ] pt \n", name, point.X.ToString(CultureInfo.InvariantCulture),
                point.Y.ToString(CultureInfo.InvariantCulture));
        }

        public void AddSegment(Vector2 point1, Vector2 point2)
        {
            _stringBuilder.AppendFormat("{0} {1} moveto\n{2} {3} lineto\nstroke\n",
                point1.X.ToString(CultureInfo.InvariantCulture), point1.Y.ToString(CultureInfo.InvariantCulture),
                point2.X.ToString(CultureInfo.InvariantCulture), point2.Y.ToString(CultureInfo.InvariantCulture));
        }

        public void AddSegment(Segment segment)
        {
            AddSegment(segment.A, segment.B);
        }

        public void AddCircle(Vector3 center, double radius)
        {
            _stringBuilder.AppendFormat("stroke {0} {1} {2} 0 360 arc closepath stroke \n",
                center.X.ToString(CultureInfo.InvariantCulture), center.Y.ToString(CultureInfo.InvariantCulture),
                radius.ToString(CultureInfo.InvariantCulture));
        }

        public void AddDescription(Vector2 descriptionCoords, string description)
        {
            _stringBuilder.AppendFormat("tx [ {0} {1} ] vm ({2}) show\nstroke\n",
                descriptionCoords.X.ToString(CultureInfo.InvariantCulture),
                descriptionCoords.Y.ToString(CultureInfo.InvariantCulture), description);
        }

        public void SetLineHeight(float height)
        {
            _stringBuilder.AppendFormat("{0} setlinewidth stroke \n", height.ToString(CultureInfo.InvariantCulture));
        }

        public void SetLineColor(float r, float g, float b)
        {
            _stringBuilder.AppendFormat("{0} {1} {2} setrgbcolor \n", r.ToString(CultureInfo.InvariantCulture),
                g.ToString(CultureInfo.InvariantCulture), b.ToString(CultureInfo.InvariantCulture));
        }

        public void FinalizeFile()
        {
            _stringBuilder.Append("showpage \n");
        }

        public void AddTriangle(Vector2[] points, string[] names)
        {
            AddPoint(points[0], names[0]);
            AddPoint(points[1], names[1]);
            AddPoint(points[2], names[2]);

            AddDescription(points[0], names[0]);
            AddDescription(points[1], names[1]);
            AddDescription(points[2], names[2]);

            AddSegment(points[0], points[1]);
            AddSegment(points[1], points[2]);
            AddSegment(points[2], points[0]);
        }

        /// <summary>
        ///     Actually cheating and haxing
        /// </summary>
        /// <param name="line"></param>
        public void AddLine(Line line)
        {
            if (line.IsVertical)
                AddSegment(new Vector2(_lineMinX, line.B), new Vector2(_lineMaxX, line.B));
            AddSegment(new Vector2(_lineMinX, line.A*_lineMinX + line.B),
                new Vector2(_lineMaxX, line.A*_lineMaxX + line.B));
        }

        public void Save(string filePath)
        {
            if (!_finalized)
            {
                FinalizeFile();
            }

            File.WriteAllText(filePath, _stringBuilder.ToString());
        }

        public void AddLabelledPoint(Vector2 point, string name)
        {
            AddPoint(point, name);
            AddDescription(point, name);
        }

        public void AddPolygon(Polygon polygon)
        {
            AddPoint(polygon.Points.First(), string.Empty);
            AddSegment(polygon.Points.First(), polygon.Points.Last());
            for (var i = 1; i < polygon.Points.Length; ++i)
            {
                AddSegment(polygon.Points[i - 1], polygon.Points[i]);
                AddPoint(polygon.Points[i], string.Empty);
            }
        }

        // TODO labels 
        public void AddPolygon(Polygon polygon, params string[] labels)
        {
            for (var i = 0; i < polygon.Points.Length; ++i)
            {
                AddDescription(polygon.Points[i], labels[i]);
            }
            AddPolygon(polygon);
        }
    }
}