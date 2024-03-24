using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace BlobOA.BlazorClient.Components;

public class RadioItem : ComponentBase, IDisposable, IObserver<RadioItem>
{
    private IDisposable? _disposed;
    private string cssClass = "radio-item";

    [CascadingParameter]
    public RadioGroup? RadioGroup { get; set; }

    [Parameter]
    public RenderFragment<EventCallback<MouseEventArgs>>? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        _disposed = RadioGroup?.Subscribe(this);
    }

    public void OnCompleted() { }

    public void OnError(Exception error) { }

    public void OnNext(RadioItem value)
    {
        var oldState = cssClass;
        // what to do when received a value
        if (this == value)
        {
            Console.WriteLine("This is me!~");
            cssClass = "radio-item selected";
        }
        else
        {
            Console.WriteLine("Nah, none of my business");
            cssClass = "radio-item";
        }

        if (oldState != cssClass) StateHasChanged();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (ChildContent != null)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", cssClass);
            builder.AddContent(3, ChildContent,
                EventCallback.Factory.Create<MouseEventArgs>(this, _ => RadioGroup?.SetSelected(this)));
            builder.CloseElement();
        }
    }

    public void Dispose()
    {
        _disposed?.Dispose();
    }
}
