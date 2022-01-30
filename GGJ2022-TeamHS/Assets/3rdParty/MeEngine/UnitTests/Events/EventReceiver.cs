using UnityEngine;
using System.Collections;
using MeEngine.Events;

namespace MeEngine.Internal.UnitTests.Events
{

    public class EventReceiver : MonoBehaviour
    {
        EventSender eventSender;

        void Awake()
        {
            eventSender.eventPublisher.SubscribeAll(this);
            eventSender.eventPublisher.Subscribe<DirectEvent>(OnDirectEvent);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [EventListener]
        void OnEvent(OnelineTestEvent @event)
        {
            MeDebug.Info("Event Recieved <OnelineTestEvent>:" + @event.message + ", " + @event.number + " By " + this.gameObject.name);
            MeDebug.Kit.Trace("Testing this feature");
        }

        void OnDirectEvent(DirectEvent @event)
        {
            MeDebug.Info("Event Recieved <DirectEvent>: By " + this.gameObject.name);
        }
    }
}