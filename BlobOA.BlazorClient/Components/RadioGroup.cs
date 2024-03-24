using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Reactive.Disposables;

namespace BlobOA.BlazorClient.Components;

public class RadioGroup : ComponentBase, IDisposable, IObservable<RadioItem>
{
    private RadioItem? _selected;
    private readonly List<IDisposable> _disposables = new List<IDisposable>();
    private readonly HashSet<IObserver<RadioItem>> _observers = new HashSet<IObserver<RadioItem>>();

    [Parameter, EditorRequired]
    public RenderFragment? ChildContent { get; set; }

    public void SetSelected(RadioItem selected)
    {
        _selected = selected;
        foreach (var observer in _observers)
        {
            observer.OnNext(selected);
        }
    }

    public void Dispose()
    {
        _selected = null;
        foreach (var observer in _observers)
        {
            observer.OnCompleted();
        }
        _observers.Clear();

        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
        _disposables.Clear();
    }

    public IDisposable Subscribe(IObserver<RadioItem> observer)
    {
        if (_observers.Add(observer) && _selected != null)
        {
            observer.OnNext(_selected);
        }

        var disposable = Disposable.Create(() => _observers.Remove(observer));
        _disposables.Add(disposable);
        return disposable;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (ChildContent != null)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "radio-group");
            builder.OpenComponent<CascadingValue<RadioGroup>>(2);
            builder.AddAttribute(3, "Value", this);
            builder.AddAttribute(4, "ChildContent", ChildContent);
            builder.CloseComponent();
            builder.CloseElement();
        }
    }
}
