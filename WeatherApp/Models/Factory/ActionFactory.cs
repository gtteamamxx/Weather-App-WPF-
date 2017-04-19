using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models.Factory
{
    public class ActionFactory
    {
        private Queue<Func<Task<bool>>> _actions;
        private Action<double> _progressChangedAction;

        private int _startActionsNum;

        public ActionFactory()
        {
            _actions = new Queue<Func<Task<bool>>>();
        }

        public ActionFactory ContinueWith(Func<Task<bool>> action)
        {
            _actions.Enqueue(action);
            return this;
        }

        public ActionFactory ProgressChanged(Action<double> action)
        {
            _progressChangedAction = action;
            return this;
        }

        public async Task<(bool, int)> StartAsync()
        {
            _startActionsNum = _actions.Count();
            int performedActions = 0;

            do
            {
                performedActions++;
                bool isCancel = await _actions.Dequeue().Invoke();

                if (isCancel)
                    return (true, performedActions);

                if (_progressChangedAction != null)
                {
                    double percentage = performedActions * 100.0 / _startActionsNum;
                    _progressChangedAction(percentage);
                }
            }
            while (_actions.Count() > 0);

            return (false, performedActions);
        }

        ~ActionFactory()
        {
            _actions = null;
            _progressChangedAction = null;
            _startActionsNum = 0;
        }
    }
}
