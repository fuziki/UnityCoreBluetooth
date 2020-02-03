namespace {{PROJECT_NAME}} {
    public sealed class {{TARGET_NAME}}
    {
#if UNITY_EDITOR_OSX
		public const string IMPORT_TARGET = "{{PRODUCT_NAME}}";
#elif UNITY_IOS
        public const string IMPORT_TARGET = "__Internal";
#endif
    }
}
