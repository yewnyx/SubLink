namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;

internal enum RequestBatchExecutionType : int {
    /**
	 * Not a request batch.
	 * Initial OBS Version: 5.0.0
	 */
    None = -1,
    /**
	 * A request batch which processes all requests serially, as fast as possible.
	 * Note: To introduce artificial delay, use the `Sleep` request and the `sleepMillis` request field.
	 * Initial OBS Version: 5.0.0
	 */
    SerialRealtime = 0,
    /**
	 * A request batch type which processes all requests serially, in sync with the graphics thread. Designed to provide high accuracy for animations.
	 * Note: To introduce artificial delay, use the `Sleep` request and the `sleepFrames` request field.
	 * Initial OBS Version: 5.0.0
	 */
    SerialFrame = 1,
    /**
	 * A request batch type which processes all requests using all available threads in the thread pool.
	 * Note: This is mainly experimental, and only really shows its colors during requests which require lots of
	 * active processing, like `GetSourceScreenshot`.
	 * Initial OBS Version: 5.0.0
	 */
    Parallel = 2,
}
