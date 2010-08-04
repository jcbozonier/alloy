using System;
using System.Windows.Input;

namespace Unite.UI.Utilities
{
    public class Do : ICommand
    {
        private Action _Action;
        private Predicate<object> _Pred;

        public Do(Action action)
        {
            _Action = action;
        }

        public Do If(Predicate<object> pred)
        {
            _Pred = pred;
            return this;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _Action();
        }

        public bool CanExecute(object parameter)
        {
            if(_Pred == null)
                return true;

            return _Pred(parameter);
        }
    }
}