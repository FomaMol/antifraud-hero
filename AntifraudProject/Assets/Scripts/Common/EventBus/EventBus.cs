using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.EventBus
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<Action<IEvent>>> _subscriptions = new();

        public IDisposable Subscribe<TEvent>(Action<TEvent> action) where TEvent : class, IEvent
        {
            var type = typeof(TEvent);

            if (!_subscriptions.ContainsKey(type))
            {
                _subscriptions.Add(type, new());
            }

            Action<IEvent> subscription = ev => action((TEvent) ev);
            _subscriptions[type].Add(subscription);

            return new ActionDisposable(() => Unsubscribe<TEvent>(subscription));
        }

        private void Unsubscribe<TEvent>(Action<IEvent> action) where TEvent : class, IEvent
        {
            var type = typeof(TEvent);
            if (_subscriptions.TryGetValue(type, out var list))
            {
                if (!list.Contains(action))
                    return;

                list.Remove(action);

                if (list.Count == 0)
                {
                    _subscriptions.Remove(type);
                }
            }
        }

        public void SendEvent(IEvent eEvent)
        {
            var type = eEvent.GetType();
            //
            if (!_subscriptions.TryGetValue(type, out var subscribersList)) return;
            //
            foreach (var action in subscribersList.ToArray())
            {
                try
                {
                    action?.Invoke(eEvent);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public UniTask<TEvent> WaitUntilEvent<TEvent>() where TEvent : class, IEvent
        {
            var tcs = new UniTaskCompletionSource<TEvent>();
            IDisposable subscription = null;

            subscription = Subscribe<TEvent>(e =>
            {
                tcs.TrySetResult(e);
                subscription.Dispose();
            });

            return tcs.Task;
        }
    }
}