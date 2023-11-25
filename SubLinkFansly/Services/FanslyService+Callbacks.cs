using System.Threading.Tasks;

namespace xyz.yewnyx.SubLink;

internal sealed partial class FanslyService {
    private void WireCallbacks() {
        //_fansly.TipEvent += OnTipEvent;
    }

    /*
    private void OnTipEvent(object? sender, Fansly.TipEventArgs e) {
        Task.Run(async () => {
            if (_rules is FanslyRules { OnTipEvent: { } callback })
                await callback(e);
        });
    }
    */
}
