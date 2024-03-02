using System.Threading.Tasks;
using xyz.yewnyx.SubLink.StreamElements.SEClient;

namespace xyz.yewnyx.SubLink.StreamElements.Services;

internal sealed partial class StreamElementsService {
    private void WireCallbacks() {
        _streamElements.TipEvent += OnTipEvent;
    }

    private void OnTipEvent(object? sender, TipEventArgs e) {
        Task.Run(async () => {
            if (_rules is StreamElementsRules { OnTipEvent: { } callback })
                await callback(e);
        });
    }
}
