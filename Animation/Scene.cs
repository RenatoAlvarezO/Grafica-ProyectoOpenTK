using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrimerProyecto
{
    public class Scene
    {
        public List<Action> ListOfActions{ get; set; }

        public int lastIndex { get; set; }

        public Scene()
        {
            ListOfActions = new List<Action>();
            lastIndex = 0;
        }

        public Scene(List<Action> listOfActions)
        {
            ListOfActions = listOfActions;
            lastIndex = 0;
        }

        public void AddAction(Action newAction)
        {
            ListOfActions.Add(newAction);
        }

        public void InsertAction(int index, Action newAction)
        {
            ListOfActions.Insert(index, newAction);
        }

    }
}