using System.Runtime.InteropServices;

namespace WhisperCpp.Bindings
{
    public static unsafe partial class WhisperCppMethods
    {
        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("struct whisper_context *")]
        public static extern whisper_context* whisper_init_from_file([NativeTypeName("const char *")] sbyte* path_model);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("struct whisper_context *")]
        public static extern whisper_context* whisper_init_from_buffer(void* buffer, [NativeTypeName("size_t")] nuint buffer_size);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("struct whisper_context *")]
        public static extern whisper_context* whisper_init([NativeTypeName("struct whisper_model_loader *")] whisper_model_loader* loader);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("struct whisper_context *")]
        public static extern whisper_context* whisper_init_from_file_no_state([NativeTypeName("const char *")] sbyte* path_model);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("struct whisper_context *")]
        public static extern whisper_context* whisper_init_from_buffer_no_state(void* buffer, [NativeTypeName("size_t")] nuint buffer_size);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("struct whisper_context *")]
        public static extern whisper_context* whisper_init_no_state([NativeTypeName("struct whisper_model_loader *")] whisper_model_loader* loader);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("struct whisper_state *")]
        public static extern whisper_state* whisper_init_state([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_ctx_init_openvino_encoder([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("const char *")] sbyte* model_path, [NativeTypeName("const char *")] sbyte* device, [NativeTypeName("const char *")] sbyte* cache_dir);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void whisper_free([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void whisper_free_state([NativeTypeName("struct whisper_state *")] whisper_state* state);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void whisper_free_params([NativeTypeName("struct whisper_full_params *")] whisper_full_params* @params);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_pcm_to_mel([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("const float *")] float* samples, int n_samples, int n_threads);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_pcm_to_mel_with_state([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("struct whisper_state *")] whisper_state* state, [NativeTypeName("const float *")] float* samples, int n_samples, int n_threads);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_pcm_to_mel_phase_vocoder([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("const float *")] float* samples, int n_samples, int n_threads);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_pcm_to_mel_phase_vocoder_with_state([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("struct whisper_state *")] whisper_state* state, [NativeTypeName("const float *")] float* samples, int n_samples, int n_threads);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_set_mel([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("const float *")] float* data, int n_len, int n_mel);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_set_mel_with_state([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("struct whisper_state *")] whisper_state* state, [NativeTypeName("const float *")] float* data, int n_len, int n_mel);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_encode([NativeTypeName("struct whisper_context *")] whisper_context* ctx, int offset, int n_threads);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_encode_with_state([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("struct whisper_state *")] whisper_state* state, int offset, int n_threads);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_decode([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("const whisper_token *")] int* tokens, int n_tokens, int n_past, int n_threads);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_decode_with_state([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("struct whisper_state *")] whisper_state* state, [NativeTypeName("const whisper_token *")] int* tokens, int n_tokens, int n_past, int n_threads);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_tokenize([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("const char *")] sbyte* text, [NativeTypeName("whisper_token *")] int* tokens, int n_max_tokens);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_lang_max_id();

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_lang_id([NativeTypeName("const char *")] sbyte* lang);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* whisper_lang_str(int id);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_lang_auto_detect([NativeTypeName("struct whisper_context *")] whisper_context* ctx, int offset_ms, int n_threads, float* lang_probs);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_lang_auto_detect_with_state([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("struct whisper_state *")] whisper_state* state, int offset_ms, int n_threads, float* lang_probs);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_n_len([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_n_len_from_state([NativeTypeName("struct whisper_state *")] whisper_state* state);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_n_vocab([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_n_text_ctx([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_n_audio_ctx([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_is_multilingual([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_model_n_vocab([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_model_n_audio_ctx([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_model_n_audio_state([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_model_n_audio_head([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_model_n_audio_layer([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_model_n_text_ctx([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_model_n_text_state([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_model_n_text_head([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_model_n_text_layer([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_model_n_mels([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_model_ftype([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_model_type([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern float* whisper_get_logits([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern float* whisper_get_logits_from_state([NativeTypeName("struct whisper_state *")] whisper_state* state);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* whisper_token_to_str([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("whisper_token")] int token);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* whisper_model_type_readable([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("whisper_token")]
        public static extern int whisper_token_eot([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("whisper_token")]
        public static extern int whisper_token_sot([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("whisper_token")]
        public static extern int whisper_token_solm([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("whisper_token")]
        public static extern int whisper_token_prev([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("whisper_token")]
        public static extern int whisper_token_nosp([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("whisper_token")]
        public static extern int whisper_token_not([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("whisper_token")]
        public static extern int whisper_token_beg([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("whisper_token")]
        public static extern int whisper_token_lang([NativeTypeName("struct whisper_context *")] whisper_context* ctx, int lang_id);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("whisper_token")]
        public static extern int whisper_token_translate([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("whisper_token")]
        public static extern int whisper_token_transcribe([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void whisper_print_timings([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void whisper_reset_timings([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* whisper_print_system_info();

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("struct whisper_full_params *")]
        public static extern whisper_full_params* whisper_full_default_params_by_ref([NativeTypeName("enum whisper_sampling_strategy")] whisper_sampling_strategy strategy);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("struct whisper_full_params")]
        public static extern whisper_full_params whisper_full_default_params([NativeTypeName("enum whisper_sampling_strategy")] whisper_sampling_strategy strategy);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_full([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("struct whisper_full_params")] whisper_full_params @params, [NativeTypeName("const float *")] float* samples, int n_samples);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_full_with_state([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("struct whisper_state *")] whisper_state* state, [NativeTypeName("struct whisper_full_params")] whisper_full_params @params, [NativeTypeName("const float *")] float* samples, int n_samples);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_full_parallel([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("struct whisper_full_params")] whisper_full_params @params, [NativeTypeName("const float *")] float* samples, int n_samples, int n_processors);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_full_n_segments([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_full_n_segments_from_state([NativeTypeName("struct whisper_state *")] whisper_state* state);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_full_lang_id([NativeTypeName("struct whisper_context *")] whisper_context* ctx);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_full_lang_id_from_state([NativeTypeName("struct whisper_state *")] whisper_state* state);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int64_t")]
        public static extern long whisper_full_get_segment_t0([NativeTypeName("struct whisper_context *")] whisper_context* ctx, int i_segment);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int64_t")]
        public static extern long whisper_full_get_segment_t0_from_state([NativeTypeName("struct whisper_state *")] whisper_state* state, int i_segment);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int64_t")]
        public static extern long whisper_full_get_segment_t1([NativeTypeName("struct whisper_context *")] whisper_context* ctx, int i_segment);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int64_t")]
        public static extern long whisper_full_get_segment_t1_from_state([NativeTypeName("struct whisper_state *")] whisper_state* state, int i_segment);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("bool")]
        public static extern byte whisper_full_get_segment_speaker_turn_next([NativeTypeName("struct whisper_context *")] whisper_context* ctx, int i_segment);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* whisper_full_get_segment_text([NativeTypeName("struct whisper_context *")] whisper_context* ctx, int i_segment);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* whisper_full_get_segment_text_from_state([NativeTypeName("struct whisper_state *")] whisper_state* state, int i_segment);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_full_n_tokens([NativeTypeName("struct whisper_context *")] whisper_context* ctx, int i_segment);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_full_n_tokens_from_state([NativeTypeName("struct whisper_state *")] whisper_state* state, int i_segment);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* whisper_full_get_token_text([NativeTypeName("struct whisper_context *")] whisper_context* ctx, int i_segment, int i_token);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* whisper_full_get_token_text_from_state([NativeTypeName("struct whisper_context *")] whisper_context* ctx, [NativeTypeName("struct whisper_state *")] whisper_state* state, int i_segment, int i_token);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("whisper_token")]
        public static extern int whisper_full_get_token_id([NativeTypeName("struct whisper_context *")] whisper_context* ctx, int i_segment, int i_token);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("whisper_token")]
        public static extern int whisper_full_get_token_id_from_state([NativeTypeName("struct whisper_state *")] whisper_state* state, int i_segment, int i_token);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern whisper_token_data whisper_full_get_token_data([NativeTypeName("struct whisper_context *")] whisper_context* ctx, int i_segment, int i_token);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern whisper_token_data whisper_full_get_token_data_from_state([NativeTypeName("struct whisper_state *")] whisper_state* state, int i_segment, int i_token);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern float whisper_full_get_token_p([NativeTypeName("struct whisper_context *")] whisper_context* ctx, int i_segment, int i_token);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern float whisper_full_get_token_p_from_state([NativeTypeName("struct whisper_state *")] whisper_state* state, int i_segment, int i_token);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_bench_memcpy(int n_threads);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* whisper_bench_memcpy_str(int n_threads);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int whisper_bench_ggml_mul_mat(int n_threads);

        [DllImport("whisper", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* whisper_bench_ggml_mul_mat_str(int n_threads);
    }
}
