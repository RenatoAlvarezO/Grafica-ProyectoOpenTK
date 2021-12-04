using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK;

namespace PrimerProyecto
{
    public class Object3D : IObject
    {
        public Dictionary<string, Face> ListOfFaces { get; set; }


        public Transformation Transformations { get; set; }
        public Object3D(Dictionary<string, Face> listOfFaces, Vertex center)
        {
            ListOfFaces = listOfFaces;
            Transformations = new Transformation(center);
        }

        public Object3D()
        {
            ListOfFaces = new Dictionary<string, Face>();
            Transformations = new();
        }

        public void SetCenter(Vertex newCenter)
        {
            Matrix4 CenterMatrix = Matrix4.CreateTranslation(newCenter);
            Transformations.Center = CenterMatrix;

            bool isLoaded = false;
            foreach (var face in ListOfFaces)
            {
                face.Value.Transformations.Center = CenterMatrix;
                face.Value.Transformations.SetTransformation(true);
            }
        }

        public Matrix4 GetCenter()
        {
            return Transformations.Center;
        }

        public Face GetFace(string key)
        {
            return ListOfFaces[key];
        }

        public void Draw()
        {
            foreach (var face in ListOfFaces)
                face.Value.Draw();
        }

        public Dictionary<string, Face> getListOfFaces()
        {
            return ListOfFaces;
        }

        public void Rotate(float angleX, float angleY, float angleZ)
        {
            foreach (var face in ListOfFaces)
            {
                face.Value.Rotate(angleX, angleY, angleZ);
            }
        }

        public void SetRotation(float angleX, float angleY, float angleZ)
        {
            bool isLoaded = false;
            foreach (var face in ListOfFaces)
            {
                var formerCenter = face.Value.Transformations.Center.Inverted();
                face.Value.Transformations.TransformationMatrix *= formerCenter;
                face.Value.SetRotation(angleX, angleY, angleZ, true);
                if (!isLoaded)
                {
                    Transformations.Rotation = face.Value.Transformations.Rotation;
                    isLoaded = true;
                }
            }
        }

        public void Traslate(float x, float y, float z)
        {
            foreach (var face in ListOfFaces)
                face.Value.Traslate(x, y, z);
        }

        public void Traslate(Vertex position)
        {
            foreach (var face in ListOfFaces)
                face.Value.Traslate(position);
        }

        public void SetTraslation(float x, float y, float z)
        {
            bool isLoaded = false;
            foreach (var face in ListOfFaces)
            {
                face.Value.SetTraslation(x, y, z);
                face.Value.Transformations.SetTransformation();

                if (!isLoaded)
                {
                    Transformations.Traslation = face.Value.Transformations.Traslation;
                    isLoaded = true;
                }
            }
        }

        public void SetTraslation(Vertex position)
        {
            bool isLoaded = false;
            foreach (var face in ListOfFaces)
            {
                face.Value.SetTraslation(position);
                face.Value.Transformations.SetTransformation();

                if (!isLoaded)
                {
                    Transformations.Traslation = face.Value.Transformations.Traslation;
                    isLoaded = true;
                }
            }
        }

        public void Scale(float x, float y, float z)
        {
            foreach (var face in ListOfFaces)
                face.Value.Scale(x, y, z);
        }

        public void Scale(Vertex position)
        {
            bool isLoaded = false;
            foreach (var face in ListOfFaces)
            {
                face.Value.Scale(position);
                Transformations.SetScaleTransformation();
                if (!isLoaded)
                {
                    Transformations.Scaling = face.Value.Transformations.Scaling;
                    isLoaded = true;
                }
            }
        }

        public void SetScale(float x, float y, float z)
        {
            bool isLoaded = false;
            foreach (var face in ListOfFaces)
            {
                face.Value.SetScale(x, y, z);
                Transformations.SetScaleTransformation();

                if (!isLoaded)
                {
                    Transformations.Scaling = face.Value.Transformations.Scaling;
                    isLoaded = true;
                }

            }
        }

        public void SetScale(Vertex position)
        {
            bool isLoaded = false;
            foreach (var face in ListOfFaces)
            {

                face.Value.SetScale(position);
                if (!isLoaded)
                {
                    Transformations.Traslation = face.Value.Transformations.Traslation;
                    isLoaded = true;
                }
            }
        }


        public void SetTextureType(int value)
        {
            foreach (var face in ListOfFaces)
                face.Value.SetTextureType(value);
        }

        public void Add(string key, Face newFace)
        {
            ListOfFaces.Add(key, newFace);
        }

        public void SaveFile(string path)
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
                Converters = { new MatrixConverter() }
            };

            string jsonOutput = JsonSerializer.Serialize(this, options);

            File.WriteAllText(path, jsonOutput);
        }

        public void LoadFromObj(string path, Vertex center)
        {
            List<Vertex> vertices = new List<Vertex>();


            List<Color> colors = new List<Color>();
            colors.Add(Color.Aqua);
            colors.Add(Color.Gold);
            colors.Add(Color.Fuchsia);
            colors.Add(Color.Red);
            colors.Add(Color.Navy);
            colors.Add(Color.Lime);
            colors.Add(Color.White);
            colors.Add(Color.Blue);


            Dictionary<string, Face> faces = new Dictionary<string, Face>();

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Unable to open \"" + path + "\", does not exist.");
            }

            using (StreamReader streamReader = new StreamReader(path))
            {
                int faceCounter = 0;
                while (!streamReader.EndOfStream)
                {
                    List<string> words = new List<string>(streamReader.ReadLine().ToLower().Split(' '));
                    words.RemoveAll(s => s == string.Empty);

                    if (words.Count == 0)
                        continue;

                    string type = words[0];
                    words.RemoveAt(0);

                    switch (type)
                    {
                        // vertex
                        case "v":
                            vertices.Add(new Vertex(float.Parse(words[0], CultureInfo.InvariantCulture),
                                float.Parse(words[1], CultureInfo.InvariantCulture),
                                float.Parse(words[2], CultureInfo.InvariantCulture)));
                            break;


                        // face
                        case "f":
                            Dictionary<string, Vertex> faceVertices = new Dictionary<string, Vertex>();
                            int key = 0;
                            foreach (string w in words)
                            {
                                if (w.Length == 0)
                                    continue;

                                string[] comps = w.Split('/');

                                // subtract 1: indices start from 1, not 0
                                int index = int.Parse(comps[0]) - 1;
                                faceVertices.Add(key.ToString(),
                                    new Vertex(vertices[index].X, vertices[index].Y, vertices[index].Z));

                                key++;
                            }

                            faces.Add(faceCounter.ToString(),
                                new Face(faceVertices, colors[faces.Count % colors.Count].ToArgb(), center));

                            break;
                    }

                    faceCounter++;
                }

                ListOfFaces = faces;
                Matrix4 centerMatrix = Matrix4.CreateTranslation(center);
                Transformations.Center = centerMatrix;
                SetCenter(center);
            }
        }

        public static Object3D LoadFromJson(string path)
        {
            JsonSerializerOptions options = new()
            {
                Converters = { new MatrixConverter() }
            };

            string outputString = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Object3D>(outputString, options);
        }
    }
}

class MatrixConverter : JsonConverter<Matrix4>
{

    public override Matrix4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Dictionary<string, float> jsonDecoded = new Dictionary<string, float>();

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new Matrix4(
                    jsonDecoded["M11"],
                    jsonDecoded["M12"],
                    jsonDecoded["M13"],
                    jsonDecoded["M14"],
                    jsonDecoded["M21"],
                    jsonDecoded["M22"],
                    jsonDecoded["M23"],
                    jsonDecoded["M24"],
                    jsonDecoded["M31"],
                    jsonDecoded["M32"],
                    jsonDecoded["M33"],
                    jsonDecoded["M34"],
                    jsonDecoded["M41"],
                    jsonDecoded["M42"],
                    jsonDecoded["M43"],
                    jsonDecoded["M44"]
                    );
            }


            if (reader.TokenType == JsonTokenType.PropertyName)
            {

                string propertyName = reader.GetString();
                float value;
                reader.Read();

                if (reader.TokenType == JsonTokenType.Number)
                {
                    value = reader.GetSingle();
                    jsonDecoded.Add(propertyName, value);
                }

            }
        }

        throw new JsonException();
    }
    public override void Write(Utf8JsonWriter writer, Matrix4 value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("M11");
        writer.WriteNumberValue(value.M11);
        writer.WritePropertyName("M12");
        writer.WriteNumberValue(value.M12);
        writer.WritePropertyName("M13");
        writer.WriteNumberValue(value.M13);
        writer.WritePropertyName("M14");
        writer.WriteNumberValue(value.M14);

        writer.WritePropertyName("M21");
        writer.WriteNumberValue(value.M21);
        writer.WritePropertyName("M22");
        writer.WriteNumberValue(value.M22);
        writer.WritePropertyName("M23");
        writer.WriteNumberValue(value.M23);
        writer.WritePropertyName("M24");
        writer.WriteNumberValue(value.M24);


        writer.WritePropertyName("M31");
        writer.WriteNumberValue(value.M31);
        writer.WritePropertyName("M32");
        writer.WriteNumberValue(value.M32);
        writer.WritePropertyName("M33");
        writer.WriteNumberValue(value.M33);
        writer.WritePropertyName("M34");
        writer.WriteNumberValue(value.M34);


        writer.WritePropertyName("M41");
        writer.WriteNumberValue(value.M41);
        writer.WritePropertyName("M42");
        writer.WriteNumberValue(value.M42);
        writer.WritePropertyName("M43");
        writer.WriteNumberValue(value.M43);
        writer.WritePropertyName("M44");
        writer.WriteNumberValue(value.M44);

        writer.WriteEndObject();
    }
}