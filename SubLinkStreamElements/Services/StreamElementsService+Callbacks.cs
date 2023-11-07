using System.Threading.Tasks;

namespace xyz.yewnyx.SubLink;

internal sealed partial class StreamElementsService {
    private void WireCallbacks() {
        _streamElements.TipEvent += OnTipEvent;
    }

    private void OnTipEvent(object? sender, StreamElements.TipEventArgs e) {
        Task.Run(async () => {
            if (_rules is StreamElementsRules { OnTipEvent: { } callback })
                await callback(e);
        });
    }
}
