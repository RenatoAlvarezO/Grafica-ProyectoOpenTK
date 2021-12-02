using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using OpenTK;

namespace PrimerProyecto
{
    public class Action
    {
        public Dictionary<string, Transformation> ListOfElements;
        public DateTimeOffset StartTime;
        public DateTimeOffset Duration;

        public Action()
        {
            ListOfElements = new Dictionary<string, Transformation>();
            StartTime = new DateTimeOffset();
            Duration = new DateTimeOffset();
        }

        public Action(DateTimeOffset startTime, DateTimeOffset duration)
        {
            ListOfElements = new Dictionary<string, Transformation>();
            StartTime = startTime;
            Duration = duration;
        }

        public void Add(string key, Transformation element)
        {
            if (ListOfElements.ContainsKey(key))
            {
                ListOfElements[key] = element;
                return;
            }
            ListOfElements.Add(key, element);
        }

        public void Transform(string key, Transformation transformation)
        {
            ListOfElements.Add(key, transformation);
        }

        public void SetStartTime(DateTimeOffset startTime)
        {
            StartTime = startTime;
        }

        public void SetDuration(DateTimeOffset duration)
        {
            Duration = duration;
        }
        public void Apply() { }
    }
}