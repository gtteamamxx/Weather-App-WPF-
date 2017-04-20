using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models.Application
{
    public class MVVMMessagerService
    {
        private static Dictionary<Type, object> _registeredReceivers;

        public static void RegisterReceiver(Type sourcePageType, Action action)
            => _RegisterReceiver(sourcePageType, action);
        public static void RegisterReceiver<T>(Type sourcePageType, Action<T> action)
            => _RegisterReceiver(sourcePageType, action);
        public static void RegisterReceiver<T1, T2>(Type sourcePageType, Action<T1, T2> action)
            => _RegisterReceiver(sourcePageType, action);

        private static void _RegisterReceiver(Type sourcePageType, object action)
        {
            if (_registeredReceivers == null)
                _registeredReceivers = new Dictionary<Type, object>();
            if (_registeredReceivers.Any(p => p.Key == sourcePageType))
                return;
            _registeredReceivers.Add(sourcePageType, action);
        }

        public static void SendMessage(Type pageToReceiveType, object one = null, object two = null)
        {
            foreach (KeyValuePair<Type, object> receiver in _registeredReceivers.Where(p => p.Key == pageToReceiveType))
            {
                object action = receiver.Value;

                MethodInfo methodInfo = action.GetType().GetMethod("Invoke");

                if (methodInfo.GetParameters().Count() == 0)
                    methodInfo.Invoke(action, null);
                else if (methodInfo.GetParameters().Count() == 1)
                    methodInfo.Invoke(action, new[] { one });
                else
                    methodInfo.Invoke(action, new[] { one, two });
            }
        }

        public static bool ReceiverExist(Type receiverType)
        {
            return _registeredReceivers.Any(p => p.Key == receiverType);
        }

        public static int GetReceiversNum(Type receiverType)
        {
            return _registeredReceivers.Count(p => p.Key == receiverType);
        }

        public static void UnregisterReceiver(Type sourcePageType)
        {
            if (!_registeredReceivers.Any(p => p.Key == sourcePageType))
                throw new Exception("Can't unregister `" + sourcePageType + "` because it doesnt exist.");
            _registeredReceivers.Remove(sourcePageType);
        }
    }
}