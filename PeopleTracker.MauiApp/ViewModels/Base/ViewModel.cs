using System.ComponentModel.DataAnnotations;

namespace PeopleTracker.MauiApp.ViewModels.Base;
public abstract class ViewModelOld : INotifyPropertyChanged, IDisposable, INotifyDataErrorInfo
{
    private bool _disposed;
    private WeakReference? _rootRef;
    private WeakReference? _targetRef;

    private readonly Dictionary<string, List<string>> _errors = new();

    public object? TargetObject => _targetRef?.Target;
    public object? RootObject => _rootRef?.Target;

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool HasErrors => _errors.Count > 0;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
            return Array.Empty<object>();

        return _errors[propertyName];
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        var handlers = PropertyChanged;
        if (handlers is null) return;

        var args = new PropertyChangedEventArgs(propertyName);

        foreach (Delegate handler in handlers.GetInvocationList())
        {
            if (handler.Target is not null)
            {
                MainThread.BeginInvokeOnMainThread(() => handler.DynamicInvoke(this, args));
            }
            else
            {
                handler.DynamicInvoke(this, args);
            }
        }
    }

    protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected virtual void OnInitialized(object? target, object? root)
    {
        _targetRef = new WeakReference(target);
        _rootRef = new WeakReference(root);
    }

    protected virtual void AddError(string propertyName, string errorMessage)
    {
        if (!_errors.ContainsKey(propertyName))
            _errors[propertyName] = new List<string>();

        if (!_errors[propertyName].Contains(errorMessage))
        {
            _errors[propertyName].Add(errorMessage);
            OnDataErrorsChanged(propertyName);
        }
    }

    protected virtual void RemoveError(string propertyName, string errorMessage)
    {
        if (_errors.TryGetValue(propertyName, out var errorList) &&
            errorList.Contains(errorMessage))
        {
            errorList.Remove(errorMessage);
            if (errorList.Count == 0)
                _errors.Remove(propertyName);

            OnDataErrorsChanged(propertyName);
        }
    }

    private void OnDataErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || _disposed) return;

        _disposed = true;
        // Здесь можно освободить управляемые ресурсы, если они есть
    }
}

public abstract class ViewModel : INotifyPropertyChanged, INotifyDataErrorInfo, IDisposable
{
    private bool _disposed;
    private readonly Dictionary<string, List<string>> _errors = new();

    private bool _isBusy;
    private bool _isDirty;

    public bool IsBusy
    {
        get => _isBusy;
        protected set => Set(ref _isBusy, value);
    }

    public bool IsDirty
    {
        get => _isDirty;
        protected set => Set(ref _isDirty, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool HasErrors => _errors.Any();

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
            return Array.Empty<object>();

        return _errors[propertyName];
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (propertyName is null) return;

        var args = new PropertyChangedEventArgs(propertyName);
        var handlers = PropertyChanged;

        if (handlers != null)
        {
            foreach (PropertyChangedEventHandler handler in handlers.GetInvocationList())
            {
                MainThread.BeginInvokeOnMainThread(() => handler(this, args));
            }
        }
    }

    protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null, bool validate = true)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;

        field = value;
        OnPropertyChanged(propertyName);

        if (validate && propertyName is not null)
            ValidateProperty(value, propertyName);

        IsDirty = true;
        return true;
    }

    protected virtual void ValidateProperty(object? value, string propertyName)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(this)
        {
            MemberName = propertyName
        };

        if (!Validator.TryValidateProperty(value, context, results))
        {
            ReplaceErrors(propertyName, results.Select(r => r.ErrorMessage!).ToList());
        }
        else
        {
            ClearErrors(propertyName);
        }
    }

    protected virtual void ValidateAll()
    {
        var context = new ValidationContext(this);
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(this, context, results, true);

        var grouped = results.GroupBy(r => r.MemberNames.FirstOrDefault() ?? string.Empty);

        _errors.Clear();

        foreach (var group in grouped)
        {
            _errors[group.Key] = group.Select(r => r.ErrorMessage!).ToList();
            OnErrorsChanged(group.Key);
        }
    }

    protected void AddError(string propertyName, string errorMessage)
    {
        if (!_errors.ContainsKey(propertyName))
            _errors[propertyName] = new List<string>();

        if (!_errors[propertyName].Contains(errorMessage))
        {
            _errors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }
    }

    protected void ClearErrors(string propertyName)
    {
        if (_errors.Remove(propertyName))
        {
            OnErrorsChanged(propertyName);
        }
    }

    protected void ReplaceErrors(string propertyName, List<string> newErrors)
    {
        _errors[propertyName] = newErrors;
        OnErrorsChanged(propertyName);
    }

    protected void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || _disposed) return;

        _disposed = true;
        // Освобождение управляемых ресурсов (если нужно)
    }
}