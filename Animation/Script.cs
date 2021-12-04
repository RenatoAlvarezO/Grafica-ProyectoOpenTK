using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace PrimerProyecto
{
    public class Script
    {
        public List<Scene> ListOfScenes { get; set; }

        public string currentScene { get; set; }

        public Thread ScriptThread { get; set; }

        public Timer ScriptTimer { get; set; }

        public int FPS { get; set; }

        public bool IsPlaying { get; set; }

        public int counter;

        public Script(List<Scene> listOfScenes, int fps)
        {
            ListOfScenes = listOfScenes;
        }

        public Script(List<Scene> listOfScenes)
        {
            ListOfScenes = listOfScenes;
        }

        public Script(int fps)
        {
            ListOfScenes = new List<Scene>();
            FPS = fps;
        }

        public void AddScene(Scene newScene)
        {
            ListOfScenes.Add(newScene);
        }

        public void SaveFile(string path)
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
                MaxDepth = 500000,
                Converters = { new ActorJSONConverter() }
            };

            string jsonOutput = JsonSerializer.Serialize(this, options);

            File.WriteAllText(path, jsonOutput);
        }

        public static Script LoadFile(string path)
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
                MaxDepth = 500000,
                Converters = { new ActorJSONConverter() }
            };
            string outputString = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Script>(outputString, options);
        }

        public void Play()
        {
            while (true)
            {

            }
        }
    }
}