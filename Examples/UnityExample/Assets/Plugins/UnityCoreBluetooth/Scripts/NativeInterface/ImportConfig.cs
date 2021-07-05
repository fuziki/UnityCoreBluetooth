#if UNITY_EDITOR_OSX || UNITY_IOS
namespace UnityCoreBluetooth.NativeInterface
{
    public class ImportConfig
    {
#if UNITY_EDITOR_OSX
		public const string TargetName = "mcUnityCoreBluetooth";
#elif UNITY_IOS
        public const string TargetName = "__Internal";
#endif
    }
}
#endif
