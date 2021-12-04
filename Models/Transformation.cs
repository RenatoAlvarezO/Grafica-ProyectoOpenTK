using OpenTK;

namespace PrimerProyecto
{
    public class Transformation
    {
        public Matrix4 Rotation { get; set; }
        public Matrix4 Scaling { get; set; }
        public Matrix4 Traslation { get; set; }
        public Matrix4 Center { get; set; }
        public Matrix4 TransformationMatrix { get; set; }

        public Transformation()
        {
            Rotation = Matrix4.Identity;
            Scaling = Matrix4.Identity;
            Traslation = Matrix4.Identity;
            Center = Matrix4.CreateTranslation(Vertex.Origin);
            TransformationMatrix = Matrix4.Identity;
        }

        public Transformation(Vertex center)
        {
            Rotation = Matrix4.Identity;
            Scaling = Matrix4.Identity;
            Traslation = Matrix4.Identity;
            Center = Matrix4.CreateTranslation(center);
            TransformationMatrix = Matrix4.Identity;
        }

        public void SetTransformation(bool self)
        {
            TransformationMatrix = self ? Center * Traslation * Rotation * Scaling
                        : Rotation * Scaling * Center * Traslation;
        }

        public void SetTransformation()
        {
            this.SetTransformation(true);
        }

        public void SetScaleTransformation()
        {
            TransformationMatrix = Traslation * Rotation * Scaling;
        }

    }
}