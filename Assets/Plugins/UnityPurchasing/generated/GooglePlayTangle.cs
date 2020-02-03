#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("t7y7Ijbi5iTEmg5HTYHnVF6n8NxKUs+aDUnupCMLHLvY6TQMssgHBJEjoIORrKeoiyfpJ1asoKCgpKGivx20IsccVlwzRS+DI2C4iRv1FDNuaAs/RWbO6OtJzvbdnaroSivtc/YEQDv6yAAfrczLV3IIIt4HHDeGI6CuoZEjoKujI6CgoQayM5gQDqUNl2+6ZAT/H7nSUPUva9gCKuEfxaxI/zO5KFIw1CarrpIp7dVY6eo+qfaTjiHxJyld1DtXI5H8F1uQUDWGn+edRYtFo/6OnC84hIj4SBXUpQVLiJF1hi+qubzdu8GzxRAeyzXrWceqz7cA+8quvnVWHqyBdSJavyWRfdWANO6Ea8UNdXfw/TaN1EEkeGpMwoEMSEgTuqOioKGg");
        private static int[] order = new int[] { 4,12,4,11,9,9,12,9,13,10,13,11,12,13,14 };
        private static int key = 161;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
