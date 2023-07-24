namespace WhisperCpp.Bindings
{
    public unsafe partial struct whisper_model_loader
    {
        public void* context;

        [NativeTypeName("size_t (*)(void *, void *, size_t)")]
        public delegate* unmanaged[Cdecl]<void*, void*, nuint, nuint> read;

        [NativeTypeName("bool (*)(void *)")]
        public delegate* unmanaged[Cdecl]<void*, byte> eof;

        [NativeTypeName("void (*)(void *)")]
        public delegate* unmanaged[Cdecl]<void*, void> close;
    }
}
