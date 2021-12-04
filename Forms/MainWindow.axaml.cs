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
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;


namespace PrimerProyecto
{
    public class MainWindow : Window
    {
        public Scene scene;
        public List<Action> actions;
        public Action currentAction;
        private Game game;
        private Stage stage;
        private Script script;

        ComboBox objectComboBox;
        ComboBox faceComboBox;
        ComboBox modeComboBox;

        Slider XSlider;
        Slider YSlider;
        Slider ZSlider;

        TextBlock fotogramas;

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

            //Elementos de la GUI
            Opened += OnInitialized;

            objectComboBox = this.Find<ComboBox>("ObjectComboBox");
            faceComboBox = this.Find<ComboBox>("FaceComboBox");
            modeComboBox = this.Find<ComboBox>("ModeComboBox");

            XSlider = this.Find<Slider>("XSlider");
            YSlider = this.Find<Slider>("YSlider");
            ZSlider = this.Find<Slider>("ZSlider");

            TextureSwitch = this.Find<ToggleSwitch>("TexutreSwitch");

            fotogramas = this.Find<TextBlock>("Fotogramas");

            //  Objetos 3D
            stage = new Stage(new Vertex(0f, 0f, 0f));

            //  // Hormiga

            // stage.Add("Cabeza", ObjLoader.loadObj("Models/object/Hormiga/Cabeza.obj", new Vertex(2f, 1f, 0), Color.SaddleBrown));
            // stage.Add("Torax", ObjLoader.loadObj("Models/object/Hormiga/Torax.obj", new Vertex(0f,1f,0f), Color.DarkRed));
            // stage.Add("Cola", ObjLoader.loadObj("Models/object/Hormiga/Cola.obj", new Vertex(-2f, 1f, 0), Color.SaddleBrown));

            // stage.Add("Pata1", ObjLoader.loadObj("Models/object/Hormiga/Pata.obj", new Vertex(0, 0, -2.2f), Color.Peru));
            // stage.Add("Pata2", ObjLoader.loadObj("Models/object/Hormiga/Pata.obj", new Vertex(1, 0, -2.2f), Color.Firebrick));
            // stage.Add("Pata3", ObjLoader.loadObj("Models/object/Hormiga/Pata.obj", new Vertex(-1, 0, -2.2f), Color.IndianRed));
            // stage.Add("Pata4", ObjLoader.loadObj("Models/object/Hormiga/Pata.obj", new Vertex(0, 0, 0), Color.Peru));
            // stage.Add("Pata5", ObjLoader.loadObj("Models/object/Hormiga/Pata.obj", new Vertex(1, 0, 0), Color.Firebrick));
            // stage.Add("Pata6", ObjLoader.loadObj("Models/object/Hormiga/Pata.obj", new Vertex(-1, 0, 0), Color.IndianRed));

            // //  Libro
            // stage.Add("Tapa1", ObjLoader.loadObj("Models/object/Libro/Tapa.obj",new Vertex(-3f,0,0), System.Drawing.Color.Aqua));
            // stage.Add("Hoja1", ObjLoader.loadObj("Models/object/Libro/Hoja.obj",new Vertex(-3f,0.1f,0),System.Drawing.Color.Beige));
            // stage.Add("Hoja2", ObjLoader.loadObj("Models/object/Libro/Hoja.obj",new Vertex(-3f,0.2f,0),System.Drawing.Color.Wheat));
            // stage.Add("Hoja3", ObjLoader.loadObj("Models/object/Libro/Hoja.obj",new Vertex(-3f,0.3f,0),System.Drawing.Color.Beige));
            // stage.Add("Tapa2", ObjLoader.loadObj("Models/object/Libro/Tapa.obj",new Vertex(3f,0f,0f), System.Drawing.Color.Aqua));
            // stage.Add("Hoja4", ObjLoader.loadObj("Models/object/Libro/Hoja.obj",new Vertex(3f,0.1f,0),System.Drawing.Color.Wheat));
            // stage.Add("Hoja5", ObjLoader.loadObj("Models/object/Libro/Hoja.obj",new Vertex(3f,0.2f,0),System.Drawing.Color.Beige));
            // stage.Add("Hoja6", ObjLoader.loadObj("Models/object/Libro/Hoja.obj",new Vertex(3f,0.3f,0),System.Drawing.Color.Wheat));


            foreach (var item in stage.GetObjects())
            {
                item.Value.SaveFile("Models/object/" + item.Key +".json");
            }

            // Hormiga

            stage.Add("Cabeza", Object3D.LoadFromJson("Models/object/Cabeza.json"));
            stage.Add("Torax", Object3D.LoadFromJson("Models/object/Torax.json"));
            stage.Add("Cola", Object3D.LoadFromJson("Models/object/Cola.json"));

            stage.Add("Pata1", Object3D.LoadFromJson("Models/object/Pata1.json"));
            stage.Add("Pata2", Object3D.LoadFromJson("Models/object/Pata2.json"));
            stage.Add("Pata3", Object3D.LoadFromJson("Models/object/Pata3.json"));
            stage.Add("Pata4", Object3D.LoadFromJson("Models/object/Pata4.json"));
            stage.Add("Pata5", Object3D.LoadFromJson("Models/object/Pata5.json"));
            stage.Add("Pata6", Object3D.LoadFromJson("Models/object/Pata6.json"));

            //  Libro
            stage.Add("Tapa1", Object3D.LoadFromJson("Models/object/Tapa1.json"));
            stage.Add("Hoja1", Object3D.LoadFromJson("Models/object/Hoja1.json"));
            stage.Add("Hoja2", Object3D.LoadFromJson("Models/object/Hoja2.json"));
            stage.Add("Hoja3", Object3D.LoadFromJson("Models/object/Hoja3.json"));
            stage.Add("Tapa2", Object3D.LoadFromJson("Models/object/Tapa2.json"));
            stage.Add("Hoja4", Object3D.LoadFromJson("Models/object/Hoja4.json"));
            stage.Add("Hoja5", Object3D.LoadFromJson("Models/object/Hoja5.json"));
            stage.Add("Hoja6", Object3D.LoadFromJson("Models/object/Hoja6.json"));

            // cuboTrucho.SaveFile("Models/object/cubotrucho.json");

            // stage.Add("cubo", Object3D.LoadFromJson("Models/object/Casa.json"));
            // stage.Add("techo", Object3D.LoadFromJson("Models/object/Techo.json"));
            // stage.Add("cono", Object3D.LoadFromJson("Models/object/Cono.json"));
            // stage.Add("cubo Trucho", Object3D.LoadFromJson("Models/object/cubotrucho.json"));


            objectComboBox.Items = stage.GetObjects().Keys.Prepend("Escenario");
            objectComboBox.SelectedIndex = 0;

            modeComboBox.Items = new List<string> { "Rotación", "Traslación", "Escalado" };
            modeComboBox.SelectedIndex = 0;

            //Prueba 
            actions = new List<Action>();
            scene = new Scene();
            currentAction = new Action();
            currentAction.SetStage(stage);

            script = new Script(50);
            script.AddScene(scene);

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
            base.OnClosed(e);
        }


        private void openGLHandler(object? obj)
        {
            game = new Game(800, 800, "Tee");
            game.Location = new Point(1000, 0);

            game.stage = stage;

            game.UpdateFrame += onUpdateFrameHandler;

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
                                (float)ZSlider.Value, true);
                            return;
                        }

                        Face faceToProcess = objectToProcess.GetFace(faceString);
                        faceToProcess.SetRotation((float)XSlider.Value, (float)YSlider.Value,
                            (float)ZSlider.Value, true);
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
            string objectString = (string)objectComboBox.SelectedItem;
            string faceString = (string)faceComboBox.SelectedItem;

            if (objectString == "Escenario")
            {
                currentAction.Add("stage", game.stage.Transformations);
                fotogramas.Text = currentAction.ListOfElements.Count.ToString();
                return;
            }

            if (faceString == "Objeto")
            {
                Object3D objectToAdd = game.stage.GetObject3D(objectString);
                currentAction.Add(objectString, objectToAdd.Transformations);
                fotogramas.Text = currentAction.ListOfElements.Count.ToString();
                return;
            }

            Face faceToAdd = game.stage.GetObject3D(objectString).ListOfFaces[faceString];
            currentAction.Add(faceString, faceToAdd.Transformations);
        }

        private void addAction(object? sender, RoutedEventArgs e)
        {
            scene.AddAction(currentAction);

            currentAction = new Action();
            currentAction.SetStage(stage);
            fotogramas.Text = scene.ListOfActions.Count.ToString();
        }

        private void playAnimation(object? sender, RoutedEventArgs e)
        {
            // script.Start();
        }
    }
}