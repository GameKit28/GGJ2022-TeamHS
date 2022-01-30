using System;

namespace MeEngine.Events
{
	// Custom attribute that allows the registration of a function to an event with a SubscribeAll call.
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class EventListenerAttribute : Attribute
	{}
}