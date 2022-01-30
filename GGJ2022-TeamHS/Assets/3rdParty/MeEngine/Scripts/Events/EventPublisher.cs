using System;
using System.Reflection;
using System.Collections.Generic;

namespace MeEngine.Events
{
    public class EventPublisher : IEventPublisher
	{
		// A cache of system types and all methods tagged with the [EventListener] attribute. For quick retrieval with less reflection.
		private static readonly Dictionary<System.Type, LinkedList<MethodAttributePair>> SubscriptionTypeDict =
			new Dictionary<Type, LinkedList<MethodAttributePair>>();
        
		// A mapping of all event types and all listeners to that event type.
		private readonly Dictionary<Type, Delegate> _delegates = new Dictionary<Type, Delegate>();
        
		// A mapping of all subscribers and their listeners.
		private readonly Dictionary<object, HashSet<DelegateTypePair>> _targetListenerDict = 
            new Dictionary<object,  HashSet<DelegateTypePair>>();
        
        
		/// <summary>
		/// Specifies a method that will listen for events of type EventT. Will be called when an event of type EventT is published by this EventPublisher.
		/// <param name="listener">An instance method matching the 'void Foo(EventT)' signature.</param>.
		/// </summary>
		public void Subscribe<EventT>(EventDelegate<EventT> listener) where EventT : IEvent
		{
			Subscribe(typeof(EventT), listener);
		}
        
		/// <summary>
		/// The specified listener will no longer receive EventT events when Published or Sent.
		/// <param name="listener">A subscribed instance method matching the 'void Foo(EventT)' signature.</param>.
		/// </summary>
		public void Unsubscribe<EventT>(EventDelegate<EventT> listener) where EventT : IEvent
		{
			Unsubscribe(typeof(EventT), listener);
		}
        
		/// <summary>
		/// Subscribes all methods denoted by the [EventListener] attribute to the appropriate events. All listeners will be called when an event of type EventT is published by this EventPublisher.
		/// Note: This will not subscribe static methods.
		/// <param name="classInstance">An instance of a class with listener methods that we wish to subscribe.</param>
		/// </summary>
		public void SubscribeAll(object classInstance)
		{
			if (classInstance == null)
			{
				throw new ArgumentNullException(nameof(
					classInstance), $"A null instance was passed to {nameof(EventPublisher)}.{nameof(SubscribeAll)}.");
			}
            
			HashSet<DelegateTypePair> listeners = new HashSet<DelegateTypePair>();
            
			// Loop through each method derived in this type and each attribute on that method
			foreach (MethodAttributePair subscription in GetSubscriptionsForType(classInstance.GetType()))
			{
				try
				{
					// Subscribe all methods with the EventListener attribute
					Delegate listener = Delegate.CreateDelegate(subscription.delegateType, classInstance,
						subscription.method);
					Subscribe(subscription.eventType, listener);
					listeners.Add(new DelegateTypePair(listener, subscription.eventType));
				}
				catch (Exception e)
				{
					if (subscription.method.ReturnType != typeof(void))
					{
						throw new Exception(
							$"The method {subscription.method.DeclaringType.Name}.{subscription.method.Name} is tagged as an [{nameof(EventListenerAttribute)}] and thus must have a return type of void.");
					}
					else if (subscription.method.GetParameters().Length != 1 ||
					         !subscription.method.GetParameters()[0].ParameterType.IsSubclassOf(typeof(IEvent)))
					{
						throw new ArgumentException(
							$"The method {subscription.method.DeclaringType.Name}.{subscription.method.Name} is tagged as an [{nameof(EventListenerAttribute)}] and thus must have a single parameter deriving from {nameof(IEvent)}.");
					}
					else
					{
						throw e;
					}
				}
			}
			_targetListenerDict.Add(classInstance, listeners);
		}
        
		/// <summary>
		/// Removes the subscription of all listeners that were previously added with SubscribeAll.
		/// <param name="classInstance">A subscribed instance of a class with listener methods that we wish to unsubscribe.</param>
		/// </summary>
		public void UnsubscribeAll(object classInstance)
		{
			HashSet<DelegateTypePair> listenerSet;
            
			// Loop through each listener found with SubscribeAll
			if (_targetListenerDict.TryGetValue(classInstance, out listenerSet))
			{
				foreach (DelegateTypePair dtPair in listenerSet)
				{
					Unsubscribe(dtPair.eventType, dtPair.listener);
				}
			}
			_targetListenerDict.Remove(classInstance);
		}
        
		/// <summary>
		/// Broadcasts the specified event to all subscribers/listeners.
		/// Try 'Publish(new EventT() { paramName = paramValue; })' as an easy to read syntax.
		/// <param name="@event">An instance of the event to be published. Will be accessible to all listeners.</param>
		/// </summary>
		public void Publish<EventT>(EventT @event) where EventT : IEvent
		{
			if (@event == null)
			{
				throw new ArgumentNullException(
					$"A null event was passed to {nameof(EventPublisher)}.{nameof(Publish)}.");
			}
            
			Delegate d;
			if (_delegates.TryGetValue(typeof(EventT), out d))
			{
				EventDelegate<EventT> callback = d as EventDelegate<EventT>;
				if (callback != null)
				{
					callback(@event);
				}
			}
		}
        
		/// <summary>
		/// Sends the specified event to a single target subscriber.
		/// Note: Nothing will happen if the target has not subscribed to this event.
		/// Note: I'm not sure if I want to include Send, as it can encourage close object coupling. -Kit
		/// </summary>
		public void Send<EventT>(EventT @event, object listenerInstance) where EventT : IEvent
		{
			if (@event == null)
				throw new ArgumentNullException($"A null event was passed to {nameof(EventPublisher)}.{nameof(Send)}.");
			if (listenerInstance == null)
			{
				throw new ArgumentNullException(
					$"A null {nameof(listenerInstance)} was passed to {nameof(EventPublisher)}.{nameof(Send)}.");
			}
            
			Delegate multicastDelegate;
			if (_delegates.TryGetValue(typeof(EventT), out multicastDelegate))
			{
				Delegate[] singleDelegateArray = multicastDelegate.GetInvocationList();
				foreach (Delegate @delegate in singleDelegateArray)
				{
					if (@delegate.Target == listenerInstance)
					{
						EventDelegate<EventT> callback = @delegate as EventDelegate<EventT>;
						if (callback != null)
						{
							callback(@event);
						}
						break; // Presumably only one method in each class is subscribed to a single event type
					}
				}
			}
		}
        
		// A non-generic implementation of Subscribe<EventType>
		private void Subscribe(System.Type eventType, Delegate listener)
		{
			Delegate d;
            
			if (_delegates.TryGetValue(eventType, out d))
			{
				_delegates[eventType] = Delegate.Combine(d, listener);
			}
			else
			{
				_delegates[eventType] = listener;
			}
		}
        
		// A non-generic implementation of Unsubscribe<EventType>
		private void Unsubscribe(System.Type eventType, Delegate listener)
		{
			Delegate d;
            
			if (_delegates.TryGetValue(eventType, out d))
			{
				Delegate currentDel = Delegate.Remove(d, listener);
                
				if (currentDel == null)
				{
					_delegates.Remove(eventType);
				}
				else
				{
					_delegates[eventType] = currentDel;
				}
			}
		}
        
		// Get all methods of a specific system type that are tagged with the EventListener attribute. Uses reflection to find them if this is the first time we are checking this system type.
		private LinkedList<MethodAttributePair> GetSubscriptionsForType(System.Type type)
		{
			LinkedList<MethodAttributePair> methods;
            
			// See if we already have an entry for this system type
			if (!SubscriptionTypeDict.TryGetValue(type, out methods))
			{
				// No. Let's loop through the methods in this type.
				methods = new LinkedList<MethodAttributePair>();
				foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.InvokeMethod
					| BindingFlags.Public | BindingFlags.NonPublic))
				{
					// And through the attributes on those methods.
					foreach (EventListenerAttribute subscription in method.GetCustomAttributes(typeof(
						EventListenerAttribute), true))
					{
						// To find the methods that have the EventListener attribute.
						// Add them to our list once found.
						methods.AddLast(new MethodAttributePair(method, subscription));
					}
				}
                
				// Add all the ones we've found to our dictionary (even if none were found)
				SubscriptionTypeDict.Add(type, methods);
			}
			return methods;
		}
        
		// Used for caching the results of our reflection-based search for event subscribers
		struct MethodAttributePair
		{
			public MethodAttributePair(MethodInfo method, EventListenerAttribute attribute)
			{
				this.method = method;
				this.attribute = attribute;
				this.eventType = method.GetParameters()[0].ParameterType;
				if (!typeof(IEvent).IsAssignableFrom(this.eventType))
				{
					throw new ArgumentException(
						$"The method {method.DeclaringType.Name}.{method.Name} is tagged as an [{nameof(EventListenerAttribute)}] and thus must have a single parameter deriving from {nameof(IEvent)}.");
				}
				this.delegateType = typeof(EventDelegate<>).MakeGenericType(eventType);
			}
            
			public MethodInfo             method {get; private set;}
			public EventListenerAttribute attribute {get; private set;}
			public System.Type            delegateType {get; private set;}
			public System.Type            eventType {get; private set;}
		}
        
		// Used for caching all the events that were subscribed to with SubscribeAll and will be removed with UnsubscribeAll
		struct DelegateTypePair
		{
			public DelegateTypePair(Delegate listener, System.Type eventType)
			{
				this.listener = listener;
				this.eventType = eventType;
			}
            
			public Delegate    listener {get; private set;}
			public System.Type eventType {get; private set;}
		}
	}
}