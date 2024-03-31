using System;

namespace SP.UI.Utils
{
    public class Debounce
    {
        private int _milliseconds;
        private Action _action;
        long _lastCall;
        System.Timers.Timer _timer;

        public Debounce(int milliseconds, Action action)
        {
            _milliseconds = milliseconds;
            _action = action;
            _lastCall = 0;
            _timer = null;
        }

        public void Call()
        {
            lock (this)
            {
                var currentCall = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var period = currentCall - _lastCall;
                if (period >= _milliseconds)
                {
                    _lastCall = currentCall;
                    _action.Invoke();
                    return;
                }
                if (_timer != null)
                {
                    _timer.Stop();
                    _timer.Dispose();
                }
                _timer = new System.Timers.Timer(_milliseconds - period);
                _timer.AutoReset = false;
                _timer.Elapsed += (s, e) => Call();
                _timer.Start();
            }
        }
    }
}
