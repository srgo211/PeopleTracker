using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleTracker.MauiApp.Commands.Base;

public abstract class Command : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public abstract bool CanExecute(object? parameter);
    public abstract void Execute(object? parameter);

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}




public abstract class CommandAsync : ICommand
{
    private bool _isExecuting;
    private CancellationTokenSource? _cts;

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => !_isExecuting;

    public void Execute(object? parameter)
    {
        _ = ExecuteSafeAsync(parameter);
    }

    public abstract Task ExecuteAsync(object? parameter, CancellationToken cancellationToken = default);

    protected void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    private async Task ExecuteSafeAsync(object? parameter)
    {
        if (_isExecuting) return; // Блокируем повторный вызов

        _isExecuting = true;
        RaiseCanExecuteChanged();

        _cts = new CancellationTokenSource();

        try
        {
            await ExecuteAsync(parameter, _cts.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            _isExecuting = false;
            RaiseCanExecuteChanged();
            _cts.Dispose();
            _cts = null;
        }
    }

    public void Cancel()
    {
        if (_isExecuting)
        {
            _cts?.Cancel();
        }
    }
}

