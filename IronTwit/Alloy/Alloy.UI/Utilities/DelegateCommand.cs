using System;
using System.Windows.Input;

namespace Unite.UI.Utilities
{
    public class Do<T> : ICommand
    {
        private Action<T> _Action;
        private Predicate<T> _Pred;

        public Do(Action<T> action)
        {
            _Action = action;
        }

        public void If(Predicate<T> pred)
        {
            _Pred = pred;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if(!(parameter is T) && parameter != null) 
                throw new ArgumentException("parameter doesn't match the type.");
            _Action((T) parameter);
        }

        public bool CanExecute(object parameter)
        {
            if(_Pred == null)
                return true;
            return _Pred((T) parameter);
        }
    }
}