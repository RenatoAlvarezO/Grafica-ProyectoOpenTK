using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace PrimerProyecto
{
    public class Face : IObject
    {
        public Dictionary<string, Vertex> ListOfVertices { get; set; }
        public int FaceColor { get; set; }
        public Vertex Center { get; set; }
        public Transformation Transformations;
        public int TextureType;
        public Face(Dictionary<string, Vertex> listOfVertices, int faceColor, Vertex center)
        {
            ListOfVertices = listOfVertices;
            FaceColor = faceColor;
            Center = center;
            Transformations = new Transformation(center);

            TextureType = 2;
        }

        public void Draw()
        {
            Color drawingColor = Color.FromArgb(FaceColor);
            GL.Color4(drawingColor);
            GL.Begin((PrimitiveType)TextureType);

            foreach (var vertex in ListOfVertices)
            {
                Vertex vertexToRender = (vertex.Value) * Transformations.TransformationMatrix;
                GL.Vertex3(vertexToRender);
            }

            GL.End();
            GL.Flush();
        }


        public void Rotate(float angleX, float angleY, float angleZ)
        {
            angleX = MathHelper.DegreesToRadians(angleX);
            angleY = MathHelper.DegreesToRadians(angleY);
            angleZ = MathHelper.DegreesToRadians(angleZ);

            Transformations.Rotation *= Matrix4.CreateRotationX(angleX) * Matrix4.CreateRotationY(angleY) *
                        Matrix4.CreateRotationZ(angleZ);

            foreach (var vertex in ListOfVertices)
                vertex.Value.Set(vertex.Value.Get() * Transformations.Rotation);
        }
        public void Rotate(float angleX, float angleY, float angleZ, Vertex refPoint)
        {
            angleX = MathHelper.DegreesToRadians(angleX);
            angleY = MathHelper.DegreesToRadians(angleY);
            angleZ = MathHelper.DegreesToRadians(angleZ);

            Matrix4 rotationMatrix = Matrix4.CreateRotationX(angleX) * Matrix4.CreateRotationY(angleY) * Matrix4.CreateRotationZ(angleZ);

            foreach (var vertex in ListOfVertices)
            {
                Vertex newVertex = (vertex.Value - refPoint) * rotationMatrix + refPoint + Center;
                ListOfVertices[vertex.Key] = newVertex;
            }

        }
        public void Traslate(float x, float y, float z)
        {
            Transformations.Traslation *= Matrix4.CreateTranslation(x, y, z);
        }

        public void Traslate(Vertex position)
        {
            Traslate(position.X, position.Y, position.Z);
        }

        public void Scale(float x, float y, float z)
        {
            Transformations.Scaling *= Matrix4.CreateScale(x, y, z);
        }

        public void Scale(Vertex scale)
        {
            Transformations.Scaling *= Matrix4.CreateScale(scale);
        }

        public void SetRotation(float angleX, float angleY, float angleZ, bool self)
        {
            angleX = MathHelper.DegreesToRadians(angleX);
            angleY = MathHelper.DegreesToRadians(angleY);
            angleZ = MathHelper.DegreesToRadians(angleZ);

            Transformations.Rotation = Matrix4.CreateRotationX(angleX) * Matrix4.CreateRotationY(angleY) *
                       Matrix4.CreateRotationZ(angleZ);

            Transformations.SetTransformation(self);
        }

        public void SetTraslation(float x, float y, float z)
        {
            // Transformations.Traslation = new Vertex(x, y, z);
            Transformations.Traslation = Matrix4.CreateTranslation(x, y, z);
            Transformations.SetTransformation();
        }

        public void SetTraslation(Vertex position)
        {
            SetTraslation(position.X, position.Y, position.Z);
            // Transformations.Traslation = position;
        }

        public void SetTraslation(Matrix4 traslation)
        {
            Transformations.Traslation = traslation;
            Transformations.SetTransformation();
        }

        public void SetScale(float x, float y, float z)
        {
            Transformations.Scaling = Matrix4.CreateScale(x, y, z);
            Transformations.SetScaleTransformation();
        }

        public void SetScale(Vertex scale)
        {
            Transformations.Scaling = Matrix4.CreateScale(scale);
            Transformations.SetTransformation();
        }

        public Matrix4 GetTransformationMatrix()
        {
            return Transformations.TransformationMatrix;
        }

        public void ResetRotation()
        {
            Transformations.Rotation = Matrix4.Identity;
        }

        public void SetTextureType(int value)
        {
            TextureType = value;
        }

        public void SetCenter(Vertex newCenter)
        {
            Center = newCenter;
            Transformations.Center = Matrix4.CreateTranslation(Center);
        }
    }
}