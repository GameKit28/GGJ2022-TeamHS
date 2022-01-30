using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityEngine.TestTools;
using MeEngine.Events;
using MeEngine;

namespace Tests
{
    public class EventManagerTest
    {
        public struct OnelineTestEvent : IEvent { public string message; public int number; }
        public struct DirectEvent : IEvent { };

        public class EventReceiver
        {
            [EventListener]
            public void OnEvent(OnelineTestEvent @event)
            {

            }

            public void OnDirectEvent(DirectEvent @event)
            {

            }
        }

        public class EventSender {
            public EventPublisher publisher = new EventPublisher();
        }

        [Test]
        public void RegisterSelectEvent()
        {
            EventReceiver receiver = Substitute.For<EventReceiver>();
            EventSender sender = new EventSender();
            sender.publisher.Subscribe<DirectEvent>(receiver.OnDirectEvent);

            sender.publisher.Publish(new DirectEvent { });

            receiver.Received().OnDirectEvent(Arg.Any<DirectEvent>());
        }

        [Test]
        public void RegisterAllEvents()
        {
            EventReceiver receiver = Substitute.For<EventReceiver>();
            EventSender sender = new EventSender();
            
            sender.publisher.SubscribeAll(receiver);

            OnelineTestEvent @event = new OnelineTestEvent { message = "test", number = 5};

            sender.publisher.Publish(@event);

            receiver.Received().OnEvent(@event);
        }
    }
}
