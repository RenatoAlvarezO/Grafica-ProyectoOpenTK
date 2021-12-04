using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            FPS = fps;
            // ScriptThread = new(AnimationHanlder);
            // ScriptTimer.
        }

        public Script(List<Scene> listOfScenes)
        {
            ListOfScenes = listOfScenes;
            FPS = 30;
            // ScriptThread = new(AnimationHanlder);
        }

        public Script()
        {
            ListOfScenes = new List<Scene>();
            FPS = 30;
            // ScriptThread = new(AnimationHanlder);
        }

        private void AnimationHanlder(object? state)
        {
            ListOfScenes[counter].ExecuteNextAction();
        }

        public Script(int fps)
        {
            ListOfScenes = new List<Scene>();
            FPS = fps;
            // ScriptThread = new(AnimationHanlder);
            // ScriptTimer.
        }

        public void AddScene(string key, Scene newScene)
        {
            ListOfScenes.Add(key, newScene);
        }

        public void Start()
        {
            ScriptTimer = new Timer(AnimationHanlder, new AutoResetEvent(true), FPS, FPS); //1000 / (FPS * 1000)

            // ScriptThread.Start();
            IsPlaying = true;
        }

        public void Stop()
        {
            ScriptThread.Abort();
            IsPlaying = false;
        }

        private void AnimationHanlder()
        {
            DateTimeOffset lastTime = DateTime.UtcNow;
            int counter = 0;
            while (IsPlaying)
            {
                counter++;
                // if ((DateTimeOffset.Now - lastTime).TotalMilliseconds > 1000d)
                    // Console.WriteLine(counter.ToString());
            }
        }

        public void SaveFile(string path)
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
                MaxDepth = 500000,
                Converters = {new ActorJSONConverter()}
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
                Converters = {new ActorJSONConverter()}
            };
            string outputString = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Script>(outputString,options);
        }

        internal void AddScene(Scene scene)
        {
            throw new NotImplementedException();
        }
    }
}