using System.Collections;

namespace tech.sublink.SubLinkEditor.Controls.Utils;

/// <summary>
/// Arguments to the ItemsAdded and ItemsRemoved events.
/// </summary>
internal class CollectionItemsChangedEventArgs : EventArgs {
    /// <summary>
    /// The collection of items that changed.
    /// </summary>
    private readonly ICollection _items;

    public CollectionItemsChangedEventArgs(ICollection items) {
        _items = items;
    }

    /// <summary>
    /// The collection of items that changed.
    /// </summary>
    public ICollection Items => _items;
}
