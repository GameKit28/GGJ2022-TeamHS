using System;
using System.Reflection;
using System.Collections.Generic;

namespace MeEngine.Events
{
    /// <summary>
	/// All event classes/structs should derive from this type.
	/// </summary>
	public interface IEvent {}
    
	// All event handlers must follow the 'void Foo(EventType bar)' format.
	public delegate void EventDelegate<in T>(T e) where T : IEvent;
    
	public interface IEventPublisher
	{
		void Subscribe<EventT>(EventDelegate<EventT> listener) where EventT : IEvent;
		void Unsubscribe<EventT>(EventDelegate<EventT> listener) where EventT : IEvent;
		void SubscribeAll(object classInstance);
		void UnsubscribeAll(object classInstance);
		void Publish<EventT>(EventT eventInstance) where EventT : IEvent;
	}
}