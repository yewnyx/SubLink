namespace tech.sublink.SubLinkEditor.Undo;

internal interface IUndoCommand {
    void Redo();
    void Undo();
}
