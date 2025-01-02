using System.Windows.Input;

namespace hotel_reservation_desktop_app.ViewModels;

public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute.Invoke();
    }

    public void Execute(object parameter)
    {
        _execute.Invoke();
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public void RaiseCanExecuteChanged()
    {
        throw new NotImplementedException();
    }
}