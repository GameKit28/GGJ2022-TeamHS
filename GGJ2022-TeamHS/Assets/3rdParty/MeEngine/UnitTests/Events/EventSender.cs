using UnityEngine;
using System.Collections;
using MeEngine.Events;

namespace MeEngine.Internal.UnitTests.Events
{
    struct OnelineTestEvent : IEvent { public string message; public int number; }
    struct DirectEvent : IEvent { };

    public class EventSender : MonoBehaviour
    {
        public EventPublisher eventPublisher = new EventPublisher();

        // Use this for initialization
        void Start()
        {
            eventPublisher.Publish(new OnelineTestEvent{ message = "This is the message.", number = 5 });
            eventPublisher.Publish(new DirectEvent { });
            eventPublisher.Send(new DirectEvent { }, GameObject.Find("EventReceiverB").GetComponent<EventReceiver>());
        }

    }
}