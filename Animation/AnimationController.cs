using System.Collections.Generic;
using System.Threading;

namespace PrimerProyecto
{
    public class AnimationController
    {
        Thread controllerThread;

        Script script;

        float FPS;

        public AnimationController(Script script)
        {

            controllerThread = new(controllerHandler);
        }

        public void controllerHandler()
        {
            List<Scene> listOfScenes = script.ListOfScenes;
            foreach (var item in listOfScenes)
            {
                
            }
        }
    }
}