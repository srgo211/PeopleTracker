using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PeopleTracker.MauiApp.Commands.Base;

internal class LambdaCommand : Command
{
    private readonly Func<object, bool> _CanExecute;
    private readonly Action<object> _Execute;

    public LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
    {
        _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
        _CanExecute = CanExecute;
    }

    /// <summary>Можно ли выполнить команду</summary>
    public override bool CanExecute(object parameter)
    {
        return _CanExecute?.Invoke(parameter) ?? true;
    }

    /// <summary>Основная логика программы </summary>
    /// <param name="parameter"></param>
    public override void Execute(object parameter)
    {
        if (!CanExecute(parameter)) return;
        _Execute(parameter);
    }
}
