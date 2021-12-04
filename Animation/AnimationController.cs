using System;
using System.Collections.Generic;
using System.Threading;
using OpenTK;

namespace PrimerProyecto
{
    public class AnimationController
    {
        Thread controllerThread;

        Script script;

        float FPS;

        public AnimationController(Script script)
        {

        }

        public void controllerHandler(object? sender, FrameEventArgs e)
        {
            Console.WriteLine("En hilo");
        }
    }
}