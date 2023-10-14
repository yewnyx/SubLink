using Nodify;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SubLink_UI.ViewModel;

public enum ConnectionStyle {
    Default,
    Line,
    Circuit
}

public class EditorSettings : ObservableObject {
    public IReadOnlyCollection<ISetting> Settings { get; }
    public IReadOnlyCollection<ISetting> AdvancedSettings { get; }

    private EditorSettings() {
        Settings = new ObservableCollection<ISetting>() {
            new ProxySetting<bool>(
                () => Instance.EnableRealtimeSelection,
                val => Instance.EnableRealtimeSelection = val,
                "Realtime selection: ",
                "Selects items when finished if disabled."),
            new ProxySetting<bool>(
                () => Instance.EnablePendingConnectionSnapping,
                val => Instance.EnablePendingConnectionSnapping = val,
                "Pending connection snapping: ",
                "Whether to snap the pending connection to connectors"),
            new ProxySetting<bool>(
                () => Instance.EnablePendingConnectionPreview,
                val => Instance.EnablePendingConnectionPreview = val,
                "Pending connection preview: ",
                "Show information about the pending connection."),
            new ProxySetting<bool>(
                () => Instance.AllowConnectingToConnectorsOnly,
                val => Instance.AllowConnectingToConnectorsOnly = val,
                "Disable drop connection on node: ",
                "Can connect directly to nodes if enabled"),
            new ProxySetting<bool>(
                () => Instance.DisableAutoPanning,
                val => Instance.DisableAutoPanning = val,
                "Disable auto panning: "),
            new ProxySetting<bool>(
                () => Instance.DisablePanning,
                val => Instance.DisablePanning = val,
                "Disable panning: "),
            new ProxySetting<bool>(
                () => Instance.DisableZooming,
                val => Instance.DisableZooming = val,
                "Disable zooming: "),
            new ProxySetting<uint>(
                () => Instance.GridSpacing,
                val => Instance.GridSpacing = val,
                "Grid spacing: ",
                "Snapping value in pixels"),
            new ProxySetting<PointEditor>(
                () => Instance.Location,
                val => Instance.Location = val,
                "Location: ",
                "The viewport's location."),
            new ProxySetting<double>(
                () => Instance.Zoom,
                val => Instance.Zoom = val,
                "Zoom: ",
                "The viewport's zoom. Not accurate when trying to zoom outside the MinViewportZoom and MaxViewportZoom because of dependency property coercion not updating the binding with the final result."),
            new ProxySetting<double>(
                () => Instance.MinZoom,
                val => Instance.MinZoom = val,
                "Min zoom: "),
            new ProxySetting<double>(
                () => Instance.MaxZoom,
                val => Instance.MaxZoom = val,
                "Max zoom: "),
            new ProxySetting<double>(
                () => Instance.AutoPanningSpeed,
                val => Instance.AutoPanningSpeed = val,
                "Auto panning speed: ",
                "Speed value in pixels per tick"),
            new ProxySetting<double>(
                () => Instance.AutoPanningEdgeDistance,
                val => Instance.AutoPanningEdgeDistance = val,
                "Auto panning edge distance: ",
                "Distance from edge to trigger auto panning"),
            new ProxySetting<double>(
                () => Instance.CircuitConnectionAngle,
                val => Instance.CircuitConnectionAngle = val,
                "Connection angle: ",
                "Applies to circuit connection style"),
            new ProxySetting<double>(
                () => Instance.ConnectionSpacing,
                val => Instance.ConnectionSpacing = val,
                "Connection spacing: ",
                "The distance between the start point and the where the angle breaks"),
            new ProxySetting<PointEditor>(
                () => Instance.ConnectionArrowSize,
                val => Instance.ConnectionArrowSize = val,
                "Connection arrowhead size: ",
                "The size of the arrowhead."),
            new ProxySetting<ConnectionStyle>(
                () => Instance.ConnectionStyle,
                val => Instance.ConnectionStyle = val,
                "Connection style: "),
            new ProxySetting<ArrowHeadEnds>(
                () => Instance.ArrowHeadEnds,
                val => Instance.ArrowHeadEnds = val,
                "Connection arrowhead end: ",
                "The location of the arrowhead."),
            new ProxySetting<ArrowHeadShape>(
                () => Instance.ArrowHeadShape,
                val => Instance.ArrowHeadShape = val,
                "Connection arrowhead shape: ",
                "The shape of the arrow head."),
            new ProxySetting<ConnectionOffsetMode>(
                () => Instance.ConnectionSourceOffsetMode,
                val => Instance.ConnectionSourceOffsetMode = val,
                "Connection source offset mode: "),
            new ProxySetting<ConnectionOffsetMode>(
                () => Instance.ConnectionTargetOffsetMode,
                val => Instance.ConnectionTargetOffsetMode = val,
                "Connection target offset mode: "),
            new ProxySetting<PointEditor>(
                () => Instance.ConnectionSourceOffset,
                val => Instance.ConnectionSourceOffset = val,
                "Connection source offset: "),
            new ProxySetting<PointEditor>(
                () => Instance.ConnectionTargetOffset,
                val => Instance.ConnectionTargetOffset = val,
                "Connection target offset: "),
            new ProxySetting<bool>(
                () => Instance.DisplayConnectionsOnTop,
                val => Instance.DisplayConnectionsOnTop = val,
                "Display connections on top: "),
            new ProxySetting<double>(
                () => Instance.BringIntoViewSpeed,
                val => Instance.BringIntoViewSpeed = val,
                "Bring into view speed: ",
                "Bring location into view animation speed in pixels per second"),
            new ProxySetting<double>(
                () => Instance.BringIntoViewMaxDuration,
                val => Instance.BringIntoViewMaxDuration = val,
                "Bring into view max duration: ",
                "Bring location into view max animation duration"),
            new ProxySetting<GroupingMovementMode>(
                () => Instance.GroupingNodeMovement,
                val => Instance.GroupingNodeMovement = val,
                "Grouping node movement: ",
                "Whether the grouping node is sticky or not"),
        };

        AdvancedSettings = new ObservableCollection<ISetting>() {
            new ProxySetting<double>(
                () => Instance.HandleRightClickAfterPanningThreshold,
                val => Instance.HandleRightClickAfterPanningThreshold = val,
                "Disable context menu after panning: ",
                "Disable after mouse moved this far"),
            new ProxySetting<double>(
                () => Instance.AutoPanningTickRate,
                val => Instance.AutoPanningTickRate = val,
                "Auto panning tick rate: ",
                "How often is the new position calculated in milliseconds. Disable and enable auto panning for this to have effect."),
            new ProxySetting<bool>(
                () => Instance.AllowDraggingCancellation,
                val => Instance.AllowDraggingCancellation = val,
                "Allow dragging cancellation: ",
                "Right click to cancel dragging."),
            new ProxySetting<bool>(
                () => Instance.AllowPendingConnectionCancellation,
                val => Instance.AllowPendingConnectionCancellation = val,
                "Allow pending connection cancellation: ",
                "Right click to cancel pending connection."),
            new ProxySetting<bool>(
                () => Instance.EnableSnappingCorrection,
                val => Instance.EnableSnappingCorrection = val,
                "Enable snapping correction: ",
                "Correct the final position when moving a selection"),
            new ProxySetting<bool>(
                () => Instance.EnableConnectorOptimizations,
                val => Instance.EnableConnectorOptimizations = val,
                "Enable connector optimizations: ",
                "Enables optimizations for connectors based on viewport distance and minimum selected nodes."),
            new ProxySetting<double>(
                () => Instance.OptimizeSafeZone,
                val => Instance.OptimizeSafeZone = val,
                "Optimize connectors safe zone: ",
                "The minimum distance from the viewport where connectors will start optimizing"),
            new ProxySetting<uint>(
                () => Instance.OptimizeMinimumSelectedItems,
                val => Instance.OptimizeMinimumSelectedItems = val,
                "Optimize connectors minimum selection: ",
                "The minimum selected items needed to start optimizing when outside the safe zone."),
            new ProxySetting<bool>(
                () => Instance.EnableRenderingOptimizations,
                val => Instance.EnableRenderingOptimizations = val,
                "Enable nodes rendering optimization: ",
                "Enables rendering optimizations for nodes based on zoom out percent and nodes count. (zoom in/out to apply optimization)"),
            new ProxySetting<double>(
                () => Instance.OptimizeRenderingZoomOutPercent,
                val => Instance.OptimizeRenderingZoomOutPercent = val,
                "Rendering optimization zoom out percent: ",
                "The zoom out percent that triggers the optimization. (percent of x = 1 - MinViewportZoom)"),
            new ProxySetting<uint>(
                () => Instance.OptimizeRenderingMinimumNodes,
                val => Instance.OptimizeRenderingMinimumNodes = val,
                "Rendering optimization minimum nodes: ",
                "The minimum nodes needed to start optimizing when zoom out percent is met."),
            new ProxySetting<bool>(
                () => Instance.EnableDraggingOptimizations,
                val => Instance.EnableDraggingOptimizations = val,
                "Enable nodes dragging optimizations: ",
                "Simulates dragging visually but only commits the changes at the end."),
            new ProxySetting<double>(
                () => Instance.FitToScreenExtentMargin,
                val => Instance.FitToScreenExtentMargin = val,
                "Fit to screen extent margin: ",
                "Adds some margin to the nodes extent when fit to screen"),
            new ProxySetting<bool>(
                () => Instance.EnableStickyConnectors,
                val => Instance.EnableStickyConnectors = val,
                "Enable sticky connectors: ",
                "The connection can be completed in two steps (e.g. click to create pending connection, click to connect)"),
        };
    }

    public static EditorSettings Instance { get; } = new();

    #region Default settings

    private ConnectionStyle _connectionStyle;
    public ConnectionStyle ConnectionStyle {
        get => _connectionStyle;
        set => SetProperty(ref _connectionStyle, value);
    }

    private bool _enablePendingConnectionSnapping = true;
    public bool EnablePendingConnectionSnapping {
        get => _enablePendingConnectionSnapping;
        set => SetProperty(ref _enablePendingConnectionSnapping, value);
    }

    private bool _enablePendingConnectionPreview = true;
    public bool EnablePendingConnectionPreview {
        get => _enablePendingConnectionPreview;
        set => SetProperty(ref _enablePendingConnectionPreview, value);
    }

    private bool _allowConnectingToConnectorsOnly;
    public bool AllowConnectingToConnectorsOnly {
        get => _allowConnectingToConnectorsOnly;
        set => SetProperty(ref _allowConnectingToConnectorsOnly, value);
    }

    private bool _realtimeSelection = true;
    public bool EnableRealtimeSelection {
        get => _realtimeSelection;
        set => SetProperty(ref _realtimeSelection, value);
    }

    private bool _disableAutoPanning = false;
    public bool DisableAutoPanning {
        get => _disableAutoPanning;
        set => SetProperty(ref _disableAutoPanning, value);
    }

    private double _autoPanningSpeed = 15d;
    public double AutoPanningSpeed {
        get => _autoPanningSpeed;
        set => SetProperty(ref _autoPanningSpeed, value);
    }

    private double _autoPanningEdgeDistance = 15d;
    public double AutoPanningEdgeDistance {
        get => _autoPanningEdgeDistance;
        set => SetProperty(ref _autoPanningEdgeDistance, value);
    }

    private bool _disablePanning = false;
    public bool DisablePanning {
        get => _disablePanning;
        set => SetProperty(ref _disablePanning, value);
    }

    private bool _disableZooming = false;
    public bool DisableZooming {
        get => _disableZooming;
        set => SetProperty(ref _disableZooming, value);
    }

    private uint _gridSpacing = 15u;
    public uint GridSpacing {
        get => _gridSpacing;
        set => SetProperty(ref _gridSpacing, value);
    }

    private double _minZoom = 0.1;
    public double MinZoom {
        get => _minZoom;
        set => SetProperty(ref _minZoom, value);
    }

    private double _maxZoom = 2;
    public double MaxZoom {
        get => _maxZoom;
        set => SetProperty(ref _maxZoom, value);
    }

    private double _zoom = 1;
    public double Zoom {
        get => _zoom;
        set => SetProperty(ref _zoom, value);
    }

    private PointEditor _location = new();
    public PointEditor Location {
        get => _location;
        set => SetProperty(ref _location, value);
    }

    private double _circuitConnectionAngle = 45;
    public double CircuitConnectionAngle {
        get => _circuitConnectionAngle;
        set => SetProperty(ref _circuitConnectionAngle, value);
    }

    private double _connectionSpacing = 20;
    public double ConnectionSpacing {
        get => _connectionSpacing;
        set => SetProperty(ref _connectionSpacing, value);
    }

    private ConnectionOffsetMode _srcConnectionOffsetMode = ConnectionOffsetMode.Static;
    public ConnectionOffsetMode ConnectionSourceOffsetMode {
        get => _srcConnectionOffsetMode;
        set => SetProperty(ref _srcConnectionOffsetMode, value);
    }

    private ConnectionOffsetMode _targetConnectionOffsetMode = ConnectionOffsetMode.Static;
    public ConnectionOffsetMode ConnectionTargetOffsetMode {
        get => _targetConnectionOffsetMode;
        set => SetProperty(ref _targetConnectionOffsetMode, value);
    }

    private ArrowHeadEnds _arrowHeadEnds = ArrowHeadEnds.End;
    public ArrowHeadEnds ArrowHeadEnds {
        get => _arrowHeadEnds;
        set => SetProperty(ref _arrowHeadEnds, value);
    }

    private ArrowHeadShape _arrowHeadShape = ArrowHeadShape.Arrowhead;
    public ArrowHeadShape ArrowHeadShape {
        get => _arrowHeadShape;
        set => SetProperty(ref _arrowHeadShape, value);
    }

    private PointEditor _connectionSourceOffset = new() { X = 14, Y = 0 };
    public PointEditor ConnectionSourceOffset {
        get => _connectionSourceOffset;
        set => SetProperty(ref _connectionSourceOffset, value);
    }

    private PointEditor _connectionTargetOffset = new() { X = 14, Y = 0 };
    public PointEditor ConnectionTargetOffset {
        get => _connectionTargetOffset;
        set => SetProperty(ref _connectionTargetOffset, value);
    }

    private PointEditor _connectionArrowSize = new() { X = 8, Y = 8 };
    public PointEditor ConnectionArrowSize {
        get => _connectionArrowSize;
        set => SetProperty(ref _connectionArrowSize, value);
    }

    private bool _displayConnectionsOnTop;
    public bool DisplayConnectionsOnTop {
        get => _displayConnectionsOnTop;
        set => SetProperty(ref _displayConnectionsOnTop, value);
    }

    private double _bringIntoViewSpeed = 1000;
    public double BringIntoViewSpeed {
        get => _bringIntoViewSpeed;
        set => SetProperty(ref _bringIntoViewSpeed, value);
    }

    private double _bringIntoViewMaxDuration = 1;
    public double BringIntoViewMaxDuration {
        get => _bringIntoViewMaxDuration;
        set => SetProperty(ref _bringIntoViewMaxDuration, value);
    }

    private GroupingMovementMode _groupingNodeMovement;
    public GroupingMovementMode GroupingNodeMovement {
        get => _groupingNodeMovement;
        set => SetProperty(ref _groupingNodeMovement, value);
    }

    #endregion

    #region Advanced settings

    public double HandleRightClickAfterPanningThreshold {
        get => Nodify.NodifyEditor.HandleRightClickAfterPanningThreshold;
        set => Nodify.NodifyEditor.HandleRightClickAfterPanningThreshold = value;
    }

    public double AutoPanningTickRate {
        get => Nodify.NodifyEditor.AutoPanningTickRate;
        set => Nodify.NodifyEditor.AutoPanningTickRate = value;
    }

    public bool AllowDraggingCancellation {
        get => ItemContainer.AllowDraggingCancellation;
        set => ItemContainer.AllowDraggingCancellation = value;
    }

    public bool AllowPendingConnectionCancellation {
        get => Nodify.Connector.AllowPendingConnectionCancellation;
        set => Nodify.Connector.AllowPendingConnectionCancellation = value;
    }

    public bool EnableSnappingCorrection {
        get => Nodify.NodifyEditor.EnableSnappingCorrection;
        set => Nodify.NodifyEditor.EnableSnappingCorrection = value;
    }

    public bool EnableConnectorOptimizations {
        get => Nodify.Connector.EnableOptimizations;
        set => Nodify.Connector.EnableOptimizations = value;
    }

    public double OptimizeSafeZone {
        get => Nodify.Connector.OptimizeSafeZone;
        set => Nodify.Connector.OptimizeSafeZone = value;
    }

    public uint OptimizeMinimumSelectedItems {
        get => Nodify.Connector.OptimizeMinimumSelectedItems;
        set => Nodify.Connector.OptimizeMinimumSelectedItems = value;
    }

    public bool EnableRenderingOptimizations {
        get => Nodify.NodifyEditor.EnableRenderingContainersOptimizations;
        set => Nodify.NodifyEditor.EnableRenderingContainersOptimizations = value;
    }

    public uint OptimizeRenderingMinimumNodes {
        get => Nodify.NodifyEditor.OptimizeRenderingMinimumContainers;
        set => Nodify.NodifyEditor.OptimizeRenderingMinimumContainers = value;
    }

    public double OptimizeRenderingZoomOutPercent {
        get => Nodify.NodifyEditor.OptimizeRenderingZoomOutPercent;
        set => Nodify.NodifyEditor.OptimizeRenderingZoomOutPercent = value;
    }

    public double FitToScreenExtentMargin {
        get => Nodify.NodifyEditor.FitToScreenExtentMargin;
        set => Nodify.NodifyEditor.FitToScreenExtentMargin = value;
    }

    public bool EnableDraggingOptimizations {
        get => Nodify.NodifyEditor.EnableDraggingContainersOptimizations;
        set => Nodify.NodifyEditor.EnableDraggingContainersOptimizations = value;
    }

    public bool EnableStickyConnectors {
        get => Nodify.Connector.EnableStickyConnections;
        set => Nodify.Connector.EnableStickyConnections = value;
    }

    #endregion
}
