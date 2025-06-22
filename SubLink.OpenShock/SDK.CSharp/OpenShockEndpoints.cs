using System;

namespace OpenShock.SDK.CSharp;

public static class OpenShockEndpoints
{
    
    public static class V1
    {
        public const string Root = "1";
        
        public static class Shockers
        {
            public const string OwnShockers = "1/shockers/own";
            public static string Pause(Guid shockerId) => $"1/shockers/{shockerId}/pause";
        }
        
        public static class Devices
        {
            public static string Get(Guid deviceId) => $"1/devices/{deviceId}";
            public static string GetGateway(Guid deviceId) => $"1/devices/{deviceId}/lcg";
        }

        public static class Users
        {
            public const string Self = "1/users/self";
        }
    }

    public static class V2
    {
        public static class Shockers
        {
            public const string Control = "2/shockers/control";
        }
    }
}