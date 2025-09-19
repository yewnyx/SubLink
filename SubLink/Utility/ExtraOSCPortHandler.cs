using BuildSoft.OscCore;
using BuildSoft.OscCore.UnityObjects;

namespace xyz.yewnyx.SubLink.Utility;

public class ExtraOSCPortHandler {
    public class ExtraPort {
        private readonly uint _id;
        private readonly ExtraOSCPortHandler _handler;
        private readonly OscClient _client;

        internal ExtraPort(string ipAddr, int port, uint id, ExtraOSCPortHandler handler) {
            _id = id;
            _handler = handler;
            _client = new OscClient(ipAddr, port);
        }

        public void Close() {
            _client.Dispose();
            _handler.Remove(_id);
        }

        /// <summary>
        /// Sends an OSC message with the specified <see cref="float"/> value to the specified OSC address.
        /// </summary>
        /// <param name="address">The OSC address to send the message to.</param>
        /// <param name="value">The value to send in the OSC message.</param>
        public void SendValue(string address, float value) =>
            _client.Send(address, value);

        /// <summary>
        /// Sends an OSC message with the specified <see cref="int"/> value to the specified OSC address.
        /// </summary>
        /// <param name="address">The OSC address to send the message to.</param>
        /// <param name="value">The value to send in the OSC message.</param>
        public void SendValue(string address, int value) =>
            _client.Send(address, value);

        /// <summary>
        /// Sends an OSC message with the specified <see cref="bool"/> value to the specified OSC address.
        /// </summary>
        /// <param name="address">The OSC address to send the message to.</param>
        /// <param name="value">The value to send in the OSC message.</param>
        public void SendValue(string address, bool value) =>
            _client.Send(address, value);

        /// <summary>
        /// Sends an OSC message with the specified <see cref="string"/> value to the specified OSC address.
        /// </summary>
        /// <param name="address">The OSC address to send the message to.</param>
        /// <param name="value">The value to send in the OSC message.</param>
        public void SendValue(string address, string value) =>
            _client.Send(address, value);

        /// <summary>
        /// Sends an OSC message with the specified <see cref="double"/> value to the specified OSC address.
        /// </summary>
        /// <param name="address">The OSC address to send the message to.</param>
        /// <param name="value">The value to send in the OSC message.</param>
        public void SendValue(string address, double value) =>
            _client.Send(address, value);

        /// <summary>
        /// Sends an OSC message with the specified <see cref="long"/> value to the specified OSC address.
        /// </summary>
        /// <param name="address">The OSC address to send the message to.</param>
        /// <param name="value">The value to send in the OSC message.</param>
        public void SendValue(string address, long value) =>
            _client.Send(address, value);

        /// <summary>
        /// Sends an OSC message with the specified <see cref="Vector2"/> value to the specified OSC address.
        /// </summary>
        /// <param name="address">The OSC address to send the message to.</param>
        /// <param name="value">The value to send in the OSC message.</param>
        public void SendValue(string address, Vector2 value) =>
            _client.Send(address, value);

        /// <summary>
        /// Sends an OSC message with the specified <see cref="Vector3"/> value to the specified OSC address.
        /// </summary>
        /// <param name="address">The OSC address to send the message to.</param>
        /// <param name="value">The value to send in the OSC message.</param>
        public void SendValue(string address, Vector3 value) =>
            _client.Send(address, value);

        /// <summary>
        /// Sends an OSC message with the specified <see cref="Color32"/> value to the specified OSC address.
        /// </summary>
        /// <param name="address">The OSC address to send the message to.</param>
        /// <param name="value">The value to send in the OSC message.</param>
        public void SendValue(string address, Color32 value) =>
            _client.Send(address, value);

        /// <summary>
        /// Sends an OSC message with the specified <see cref="MidiMessage"/> value to the specified OSC address.
        /// </summary>
        /// <param name="address">The OSC address to send the message to.</param>
        /// <param name="value">The value to send in the OSC message.</param>
        public void SendValue(string address, MidiMessage value) =>
            _client.Send(address, value);

        /// <summary>
        /// Sends an OSC message with the specified <see cref="byte"/>[] value to the specified OSC address.
        /// </summary>
        /// <param name="address">The OSC address to send the message to.</param>
        /// <param name="value">The value to send in the OSC message.</param>
        public void SendValue(string address, byte[] value) =>
            _client.Send(address, value, value.Length);

        /// <summary>
        /// Sends an OSC message with the specified <see cref="char"/> value to the specified OSC address.
        /// </summary>
        /// <param name="address">The OSC address to send the message to.</param>
        /// <param name="value">The value to send in the OSC message.</param>
        public void SendValue(string address, char value) =>
            _client.Send(address, value);
    }

    private uint curIdx = 0;
    private Dictionary<uint, ExtraPort> _extraPorts = [];

    public ExtraPort? Open(int port, string ipAddr = "127.0.0.1") {
        try {
            var newIdx = curIdx++;
            var newPort = new ExtraPort(ipAddr, port, newIdx, this);
            _extraPorts.Add(newIdx, newPort);
            return newPort;
        } catch (Exception) {
            return null;
        }
    }


    internal void Remove(uint id) =>
        _extraPorts.Remove(id);

    ~ExtraOSCPortHandler() {
        var items = _extraPorts.ToArray();

        for (int i = 0; i < items.Length; i++) {
            items[i].Value.Close();
        }
    }
}
