namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;

public enum EventSubscription : uint {
    /**
	 * Subcription value used to disable all events.
	 * Initial OBS Version: 5.0.0
	 */
    None = 0,
    /**
	 * Subscription value to receive events in the `General` category.
	 * Initial OBS Version: 5.0.0
	 */
    General = (1 << 0),
    /**
	 * Subscription value to receive events in the `Config` category.
	 * Initial OBS Version: 5.0.0
	 */
    Config = (1 << 1),
    /**
	 * Subscription value to receive events in the `Scenes` category.
	 * Initial OBS Version: 5.0.0
	 */
    Scenes = (1 << 2),
    /**
	 * Subscription value to receive events in the `Inputs` category.
	 * Initial OBS Version: 5.0.0
	 */
    Inputs = (1 << 3),
    /**
	 * Subscription value to receive events in the `Transitions` category.
	 * Initial OBS Version: 5.0.0
	 */
    Transitions = (1 << 4),
    /**
	 * Subscription value to receive events in the `Filters` category.
	 * Initial OBS Version: 5.0.0
	 */
    Filters = (1 << 5),
    /**
	 * Subscription value to receive events in the `Outputs` category.
	 * Initial OBS Version: 5.0.0
	 */
    Outputs = (1 << 6),
    /**
	 * Subscription value to receive events in the `SceneItems` category.
	 * Initial OBS Version: 5.0.0
	 */
    SceneItems = (1 << 7),
    /**
	 * Subscription value to receive events in the `MediaInputs` category.
	 * Initial OBS Version: 5.0.0
	 */
    MediaInputs = (1 << 8),
    /**
	 * Subscription value to receive the `VendorEvent` event.
	 * Initial OBS Version: 5.0.0
	 */
    Vendors = (1 << 9),
    /**
	 * Subscription value to receive events in the `Ui` category.
	 * Initial OBS Version: 5.0.0
	 */
    Ui = (1 << 10),
    /**
	 * Helper to receive all non-high-volume events.
	 * Initial OBS Version: 5.0.0
	 */
    All = (General | Config | Scenes | Inputs | Transitions | Filters | Outputs | SceneItems | MediaInputs | Vendors | Ui),
    /**
	 * Subscription value to receive the `InputVolumeMeters` high-volume event.
	 * Initial OBS Version: 5.0.0
	 */
    InputVolumeMeters = (1 << 16),
    /**
	 * Subscription value to receive the `InputActiveStateChanged` high-volume event.
	 * Initial OBS Version: 5.0.0
	 */
    InputActiveStateChanged = (1 << 17),
    /**
	 * Subscription value to receive the `InputShowStateChanged` high-volume event.
	 * Initial OBS Version: 5.0.0
	 */
    InputShowStateChanged = (1 << 18),
    /**
	 * Subscription value to receive the `SceneItemTransformChanged` high-volume event.
	 * Initial OBS Version: 5.0.0
	 */
    SceneItemTransformChanged = (1 << 19),
}
