using System.Threading.Tasks;

namespace xyz.yewnyx.SubLink;

internal sealed partial class FanslyService {
    private void WireCallbacks() {
        _fansly.FanslyDisconnected += OnFanslyDisconnected;
        _fansly.FanslyError += OnFanslyError;
        _fansly.ChatMessageEvent += OnChatMessageEvent;
        _fansly.TipEvent += OnTipEvent;
        _fansly.GoalUpdatedEvent += OnGoalUpdatedEvent;
    }

    private void OnFanslyDisconnected(object? sender, System.EventArgs e) =>
        _logger.Warning("[{TAG}] Disconnected from socket", "Fansly");

    private void OnFanslyError(object? sender, Fansly.FanslyErrorArgs e) =>
        _logger.Error("[{TAG}] Error ocured with the socket\r\n{Exception}", "Fansly", e.Exception);

    private void OnChatMessageEvent(object? sender, Fansly.ChatMessageEventArgs e) {
        Task.Run(async () => {
            if (_rules is FanslyRules { OnChatMessageEvent: { } callback })
                await callback(e.Data);
        });
    }

    private void OnTipEvent(object? sender, Fansly.TipEventArgs e) {
        Task.Run(async () => {
            if (_rules is FanslyRules { OnTipEvent: { } callback })
                await callback(e.Data);
        });
    }

    private void OnGoalUpdatedEvent(object? sender, Fansly.GoalUpdatedEventArgs e) {
        Task.Run(async () => {
            if (_rules is FanslyRules { OnGoalUpdatedEvent: { } callback })
                await callback(e.Data);
        });
    }
}
