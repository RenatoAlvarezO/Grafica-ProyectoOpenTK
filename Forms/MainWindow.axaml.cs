using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using OpenTK;
using System.Drawing;
// using Color = Avalonia.Media.Color;
using Point = System.Drawing.Point;

namespace PrimerProyecto
{
    public class MainWindow : Window
    {
        private Game game;
        private Stage stage;
        private Script script;

        ComboBox objectComboBox;
        ComboBox faceComboBox;
        ComboBox modeComboBox;

        Slider XSlider;
        Slider YSlider;
        Slider ZSlider;

        ToggleSwitch TextureSwitch;

        private float minRotate = -180f;
        private float maxRotate = 180f;

        private float minTraslate = -5f;
        private float maxTraslate = 5f;
        private float minScale = 0f;
        private float maxScale = 2f;

        public MainWindow()
        {
            InitializeComponent();


            Opened += OnInitialized;

            objectComboBox = this.Find<ComboBox>("ObjectComboBox");
            faceComboBox = this.Find<ComboBox>("FaceComboBox");
            modeComboBox = this.Find<ComboBox>("ModeComboBox");

            XSlider = this.Find<Slider>("XSlider");
            YSlider = this.Find<Slider>("YSlider");
            ZSlider = this.Find<Slider>("ZSlider");

            TextureSwitch = this.Find<ToggleSwitch>("TexutreSwitch");

            stage = new Stage(new Vertex(0f, 0f, 0f));

            Object3D cuboTrucho = new Object3D();
            cuboTrucho.SetCenter(Vertex.Origin);
            Dictionary<string, Vertex> vertices = new Dictionary<string, Vertex>();

            Matrix4 rotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90));


            vertices.Add("vert1", new Vertex(-2, 2, 0));
            vertices.Add("vert2", new Vertex(2, 2, 0));
            vertices.Add("vert3", new Vertex(2, -2, 0));
            vertices.Add("vert4", new Vertex(-2, -2, 0));

            cuboTrucho.Add("cara1", new Face(vertices, System.Drawing.Color.Red.ToArgb(), Vertex.Origin));

            Dictionary<string, Vertex> vertices2 = new Dictionary<string, Vertex>();

            vertices2.Add("vert1", new Vertex(-2, 0, 2));
            vertices2.Add("vert2", new Vertex(2, 0, 2));
            vertices2.Add("vert3", new Vertex(2, 0, -2));
            vertices2.Add("vert4", new Vertex(-2, 0, -2));

            cuboTrucho.Add("cara2", new Face(vertices2, System.Drawing.Color.Aqua.ToArgb(), new Vertex(0f, 2f, -2f)));


            // stage.Add("cubo", ObjLoader.loadObj("../../../Models/object/Casa.obj", Vertex.Origin));
            // stage.Add("techo", ObjLoader.loadObj("../../../Models/object/Techo.json", Vertex.Origin));
            // stage.Add("cono", ObjLoader.loadObj("../../../Models/object/Cono.json", Vertex.Origin));


            cuboTrucho.SaveFile("Models/object/cubotrucho.json");

            // stage.Add("cubo", Object3D.LoadFromJson("Models/object/Casa.json"));
            // stage.Add("techo", Object3D.LoadFromJson("Models/object/Techo.json"));
            // stage.Add("cono", Object3D.LoadFromJson("Models/object/Cono.json"));
            stage.Add("cubo Trucho", Object3D.LoadFromJson("Models/object/cubotrucho.json"));



            objectComboBox.Items = stage.GetObjects().Keys.Prepend("Escenario");
            objectComboBox.SelectedIndex = 0;

            modeComboBox.Items = new List<string> { "Rotación", "Traslación", "Escalado" };
            modeComboBox.SelectedIndex = 0;

            //Prueba 
            Action actionTest = new Action();
            // actionTest.Add("stage", stage);
            // actionTest.Add("cubo", cuboTrucho);
            // actionTest.Add("cara2", cuboTrucho.ListOfFaces["cara2"]);

            foreach (var item in actionTest.ListOfElements)
            {
                Console.WriteLine(item);
            }

            //Fin de prueba
            updateFaceItems();

            Thread openGL = new(openGLHandler);
            openGL.Start();


#if DEBUG
            this.AttachDevTools();
#endif
        }

        protected override void OnClosed(EventArgs e)
        {
            // script.ScriptThread.Abort();
            // game.Close();
            base.OnClosed(e);
        }


        private void openGLHandler(object? obj)
        {
            game = new Game(800, 800, "Tee");
            game.Location = new Point(1000, 0);

            game.stage = stage;

            game.UpdateFrame += onUpdateFrameHandler;

            script = new Script(50);

            Scene scene = new Scene();

            // for (int i = 0; i < 10; i++)
            // {
            //     Action action = new(game.stage.GetObject3D("cubo"));
            //     action.yRotation = 10f;
            //     scene.AddAction(action);
            // }

            // for (int i = 0; i < 15; i++)
            // {
            //     Action action = new(game.stage.GetObject3D("cono"));
            //     action.traslation = new Vertex(0.1f, 0.1f, 0);
            //     scene.AddAction(action);
            // }
            //
            // for (int i = 0; i < 15; i++)
            // {
            //     Action action = new(game.stage.GetObject3D("cono"));
            //     action.traslation = new Vertex(-0.1f, -0.1f, 0);
            //     scene.AddAction(action);
            // }
            //
            // script.AddScene("Cono", scene);
            // script.currentScene = "Cono";
            //
            // script.SaveFile("Scripts/Cono.json");

            //
            // script = Script.LoadFile("../../../Scripts/Cono.json");
            // script.Start();

            game.Run(200);
            Dispatcher.UIThread.InvokeAsync(() => { Close(); });
        }

        private void onUpdateFrameHandler(object? sender, FrameEventArgs e)
        {
        }

        private void OnInitialized(object? sender, EventArgs e)
        {
            Position = new PixelPoint(200, 0);
            Width = 400;
            Height = 600;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            Position = new PixelPoint(0, 0);
        }

        private void ObjectSelected(object? sender, SelectionChangedEventArgs e)
        {
            updateFaceItems();
        }

        private void updateFaceItems()

        {
            string key = (string)objectComboBox.SelectedItem;
            if (key == "Escenario")
            {
                faceComboBox.IsEnabled = false;
                faceComboBox.Items = null;
            }
            else
            {
                faceComboBox.IsEnabled = true;
                faceComboBox.Items = stage.GetObject3D(key).getListOfFaces().Keys.Prepend("Objeto");
                faceComboBox.SelectedIndex = 0;
            }
        }

        private void SliderHandler(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (XSlider != null && YSlider != null && ZSlider != null)
            {
                string mode = (string)modeComboBox.SelectedItem;
                string objectString = (string)objectComboBox.SelectedItem;
                string faceString = (string)faceComboBox.SelectedItem;


                if (mode == "Rotación")
                {
                    if (objectString == "Escenario")
                    {
                        game.stage.SetRotation((float)XSlider.Value, (float)YSlider.Value,
                                (float)ZSlider.Value);
                    }
                    else
                    {
                        Object3D objectToProcess = game.stage.GetObject3D(objectString);
                        if (faceString == "Objeto")
                        {
                            objectToProcess.SetRotation((float)XSlider.Value, (float)YSlider.Value,
                                    (float)ZSlider.Value);
                            return;
                        }

                        Face faceToProcess = objectToProcess.GetFace(faceString);
                        faceToProcess.SetRotation((float)XSlider.Value, (float)YSlider.Value,
                                (float)ZSlider.Value, false);
                    }

                    return;
                }

                Vertex coordinates = new Vertex((float)XSlider.Value, (float)YSlider.Value,
                        (float)ZSlider.Value);

                if (mode == "Traslación")
                {
                    if (objectString == "Escenario")
                    {
                        game.stage.SetTraslation(coordinates);
                    }
                    else
                    {
                        Object3D objectToProcess = game.stage.GetObject3D(objectString);
                        if (faceString == "Objeto")
                        {
                            objectToProcess.SetTraslation(coordinates);
                            return;
                        }

                        Face faceToProcess = objectToProcess.GetFace(faceString);
                        faceToProcess.SetTraslation(coordinates);
                    }

                    return;
                }

                if (mode == "Escalado")
                {
                    if (objectString == "Escenario")
                    {
                        game.stage.SetScale(coordinates);
                    }
                    else
                    {
                        Object3D objectToProcess = game.stage.GetObject3D(objectString);
                        if (faceString == "Objeto")
                        {
                            objectToProcess.SetScale(coordinates);
                            return;
                        }

                        Face faceToProcess = objectToProcess.GetFace(faceString);
                        faceToProcess.SetScale(coordinates);
                    }
                }
            }
        }

        private void ModeSelected(object? sender, SelectionChangedEventArgs e)
        {
            switch (modeComboBox.SelectedItem)
            {
                case "Rotación":
                    setSlidersRange(minRotate, maxRotate);
                    break;
                case "Traslación":
                    setSlidersRange(minTraslate, maxTraslate);
                    break;
                case "Escalado":
                    setSlidersRange(minScale, maxScale);
                    break;
            }
        }

        private void setSlidersRange(float minValue, float maxValue)
        {
            XSlider.Minimum = minValue;
            XSlider.Maximum = maxValue;
            YSlider.Minimum = minValue;
            YSlider.Maximum = maxValue;
            ZSlider.Minimum = minValue;
            ZSlider.Maximum = maxValue;

            if (modeComboBox.SelectedItem == "Escalado")
            {
                XSlider.Value = 1f;
                YSlider.Value = 1f;
                ZSlider.Value = 1f;
                return;
            }

            XSlider.Value = 0;
            YSlider.Value = 0;
            ZSlider.Value = 0;
        }

        private void SwitchHandler(object? sender, RoutedEventArgs e)
        {
            game.stage.SetTextureType((bool)TextureSwitch.IsChecked ? 9 : 2);
        }

        private void applyAction(object? sender, RoutedEventArgs e)
        {

            string mode = (string)modeComboBox.SelectedItem;
            string objectString = (string)objectComboBox.SelectedItem;
            string faceString = (string)faceComboBox.SelectedItem;

            if (mode == "Rotación")
            {
                if (objectString == "Escenario")
                {
                    game.stage.SetRotation((float)XSlider.Value, (float)YSlider.Value, (float)ZSlider.Value);
                }
                else
                {
                    Object3D objectToProcess = game.stage.GetObject3D(objectString);
                    if (faceString == "Objeto")
                    {

                        // objectToProcess.SetRotation((float)XSlider.Value, (float)YSlider.Value, (float)ZSlider.Value);
                        return;
                    }

                    Face faceToProcess = objectToProcess.GetFace(faceString);
                    faceToProcess.SetRotation((float)XSlider.Value, (float)YSlider.Value, (float)ZSlider.Value, false);
                }

                return;
            }

            Vertex coordinates = new Vertex((float)XSlider.Value, (float)YSlider.Value, (float)ZSlider.Value);

            if (mode == "Traslación")
            {
                if (objectString == "Escenario")
                {
                    game.stage.SetTraslation(coordinates);
                }
                else
                {
                    Object3D objectToProcess = game.stage.GetObject3D(objectString);
                    if (faceString == "Objeto")
                    {
                        objectToProcess.SetTraslation(coordinates);
                        return;
                    }

                    Face faceToProcess = objectToProcess.GetFace(faceString);
                    faceToProcess.SetTraslation(coordinates);
                }

                return;
            }

            if (mode == "Escalado")
            {
                if (objectString == "Escenario")
                {
                    game.stage.SetScale(coordinates);
                }
                else
                {
                    Object3D objectToProcess = game.stage.GetObject3D(objectString);
                    if (faceString == "Objeto")
                    {
                        objectToProcess.SetScale(coordinates);
                        return;
                    }

                    Face faceToProcess = objectToProcess.GetFace(faceString);
                    faceToProcess.SetScale(coordinates);
                }
            }

            // Object3D objectToAction = game.stage.GetObject3D("cubo");
            // Action action = new Action(objectToAction);


            // Action action = new(game.stage.GetObject3D("cubo").GetFace("60"));
            // action.yRotation = 10;


            // action.Apply();
        }
    }
}
