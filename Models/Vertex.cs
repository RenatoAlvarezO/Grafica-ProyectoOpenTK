using System.Text;
using OpenTK;

namespace PrimerProyecto
{
    public class Vertex
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }


        public Vertex(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;

        }

        public Vertex()
        {
            X = 0f;
            Y = 0f;
            Z = 0f;
        }


        public override string ToString()
        {
            return "[" + X + "|" + Y + "|" + Z + "]";
        }

        public void Set(Vertex newVertex)
        {
            X = newVertex.X;
            Y = newVertex.Y;
            Z = newVertex.Z;
        }

        public Vertex Get()
        {
            return this;
        }

        public static Vertex operator +(Vertex a, Vertex b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vertex operator -(Vertex a, Vertex b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vertex operator -(Vertex a) => new Vertex(-a.X, -a.Y, -a.Z);
        public static Vertex operator *(Vertex a, Matrix4 b) => new(
            a.X * b.M11 + a.Y * b.M21 + a.Z * b.M31 + 1f * b.M41,
            a.X * b.M12 + a.Y * b.M22 + a.Z * b.M32 + 1f * b.M42,
            a.X * b.M13 + a.Y * b.M23 + a.Z * b.M33 + 1f * b.M43
        );

        public static Vertex operator *(Vertex a, float b) => new(b * a.X, b * a.Y, b * a.Z);

        public static readonly Vertex Origin = new Vertex();

        public static bool operator ==(Vertex a, Vertex b) => (a.X == b.X && a.Y == b.Y && a.Z == b.Z);

        public static bool operator !=(Vertex a, Vertex b) => !(a == b);


        public static implicit operator Vector3(Vertex convert)
        {
            return new Vector3(convert.X, convert.Y, convert.Z);
        }
    }
}