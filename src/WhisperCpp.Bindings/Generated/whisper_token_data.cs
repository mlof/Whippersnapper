namespace WhisperCpp.Bindings
{
    public partial struct whisper_token_data
    {
        [NativeTypeName("whisper_token")]
        public int id;

        [NativeTypeName("whisper_token")]
        public int tid;

        public float p;

        public float plog;

        public float pt;

        public float ptsum;

        [NativeTypeName("int64_t")]
        public long t0;

        [NativeTypeName("int64_t")]
        public long t1;

        public float vlen;
    }
}
