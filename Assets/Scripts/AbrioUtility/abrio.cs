using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Abrio
{
    public partial class EventWrapper
    {
        public enum EventType
        {
            BasicEvent = 1,
            Response = 2,
            RequestEvent = 3,
        }

        /// <summary> Identifies which field is filled in.</summary>
        public Abrio.EventWrapper.EventType eventType { get; set; }

        /// <summary> One of the following will be filled in.</summary>
        public Abrio.BasicEvent BasicEvent { get; set; }

        public Abrio.Response Response { get; set; }

        public Abrio.RequestEvent RequestEvent { get; set; }

    }

    public partial class BasicEvent
    {
        public string Title { get; set; }

        public string Body { get; set; }

    }

    public partial class AuthEvent
    {
        public string UserId { get; set; }

        public string DeviceId { get; set; }

        public string PrivateKey { get; set; }

    }

    public partial class Response
    {
        public string Type { get; set; }

    }

    public partial class RequestEvent
    {
        public List<string> RequestData { get; set; }

    }

}

