using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models.Factory
{
    class ActionFactory
    {
        private Queue<Func<Task<bool>>> _Actions;
        private Action<double> _ProgressChangedAction;

        private int _startActionsNum;

        public ActionFactory()
        {
            _Actions = new Queue<Func<Task<bool>>>();
        }

        public ActionFactory ContinueWith(Func<Task<bool>> action)
        {
            _Actions.Enqueue(action);
            return this;
        }

        public ActionFactory ProgressChanged(Action<double> action)
        {
            _ProgressChangedAction = action;
            return this;
        }

        public async Task<(bool, int)> StartAsync()
        {
            _startActionsNum = _Actions.Count();
            int performedActions = 0;

            do
            {
                bool isCancel = await _Actions.Dequeue().Invoke();

                if (isCancel)
                    return (true, ++performedActions);

                if (_ProgressChangedAction != null)
                {
                    performedActions++;
                    double percentage = performedActions * 100.0 / _startActionsNum;
                    _ProgressChangedAction(percentage);
                }
            }
            while (_Actions.Count() > 0);

            return (false, performedActions);
        }
    }
}
