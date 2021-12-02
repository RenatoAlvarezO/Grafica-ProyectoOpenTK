using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using OpenTK;

namespace PrimerProyecto
{
    public class Action
    {
        public Dictionary<string, Matrix4> ListOfElements;
        public DateTimeOffset StartTime;
        public DateTimeOffset Duration;

        public Action()
        {
            ListOfElements = new Dictionary<string, Matrix4>();
            StartTime = new DateTimeOffset();
            Duration = new DateTimeOffset();
        }

        public Action(DateTimeOffset startTime, DateTimeOffset duration)
        {
            ListOfElements = new Dictionary<string, Matrix4>();
            StartTime = startTime;
            Duration = duration;
        }

        public void Add(string key, Matrix4 element)
        {
            ListOfElements.Add(key, element);
        }

        public void Transform(string key, Matrix4 transformation)
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