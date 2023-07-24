namespace WhisperCpp.Bindings
{
    public unsafe partial struct whisper_full_params
    {
        [NativeTypeName("enum whisper_sampling_strategy")]
        public whisper_sampling_strategy strategy;

        public int n_threads;

        public int n_max_text_ctx;

        public int offset_ms;

        public int duration_ms;

        [NativeTypeName("bool")]
        public byte translate;

        [NativeTypeName("bool")]
        public byte no_context;

        [NativeTypeName("bool")]
        public byte single_segment;

        [NativeTypeName("bool")]
        public byte print_special;

        [NativeTypeName("bool")]
        public byte print_progress;

        [NativeTypeName("bool")]
        public byte print_realtime;

        [NativeTypeName("bool")]
        public byte print_timestamps;

        [NativeTypeName("bool")]
        public byte token_timestamps;

        public float thold_pt;

        public float thold_ptsum;

        public int max_len;

        [NativeTypeName("bool")]
        public byte split_on_word;

        public int max_tokens;

        [NativeTypeName("bool")]
        public byte speed_up;

        public int audio_ctx;

        [NativeTypeName("bool")]
        public byte tdrz_enable;

        [NativeTypeName("const char *")]
        public sbyte* initial_prompt;

        [NativeTypeName("const whisper_token *")]
        public int* prompt_tokens;

        public int prompt_n_tokens;

        [NativeTypeName("const char *")]
        public sbyte* language;

        [NativeTypeName("bool")]
        public byte detect_language;

        [NativeTypeName("bool")]
        public byte suppress_blank;

        [NativeTypeName("bool")]
        public byte suppress_non_speech_tokens;

        public float temperature;

        public float max_initial_ts;

        public float length_penalty;

        public float temperature_inc;

        public float entropy_thold;

        public float logprob_thold;

        public float no_speech_thold;

        [NativeTypeName("struct (anonymous struct at D:/source/repos/testrepo/lib/whisper.cpp/whisper.h:407:9)")]
        public _greedy_e__Struct greedy;

        [NativeTypeName("struct (anonymous struct at D:/source/repos/testrepo/lib/whisper.cpp/whisper.h:411:9)")]
        public _beam_search_e__Struct beam_search;

        [NativeTypeName("whisper_new_segment_callback")]
        public delegate* unmanaged[Cdecl]<whisper_context*, whisper_state*, int, void*, void> new_segment_callback;

        public void* new_segment_callback_user_data;

        [NativeTypeName("whisper_progress_callback")]
        public delegate* unmanaged[Cdecl]<whisper_context*, whisper_state*, int, void*, void> progress_callback;

        public void* progress_callback_user_data;

        [NativeTypeName("whisper_encoder_begin_callback")]
        public delegate* unmanaged[Cdecl]<whisper_context*, whisper_state*, void*, byte> encoder_begin_callback;

        public void* encoder_begin_callback_user_data;

        [NativeTypeName("whisper_logits_filter_callback")]
        public delegate* unmanaged[Cdecl]<whisper_context*, whisper_state*, whisper_token_data*, int, float*, void*, void> logits_filter_callback;

        public void* logits_filter_callback_user_data;

        public partial struct _greedy_e__Struct
        {
            public int best_of;
        }

        public partial struct _beam_search_e__Struct
        {
            public int beam_size;

            public float patience;
        }
    }
}
