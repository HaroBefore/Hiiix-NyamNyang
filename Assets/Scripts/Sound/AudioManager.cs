using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FX_SOUND_TYPE : int {
    TOUCH,
    SHOW_POPUP,
    CLOSE_POPUP,
    GRILL,
    ANGRY,
    HAPPY1,
    HAPPY2,
    MAX
}

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    #region AudioClips
    [Header("AudioClips")]
    public AudioClip background_1;
    public AudioClip background_2;
    public AudioClip background_boss;
    public AudioClip background_minigame;

    public AudioClip background_sand_1;
    public AudioClip background_sand_2;
    public AudioClip background_wave_1;
    public AudioClip background_wave_2;

    public AudioClip button01;
    public AudioClip touch;
    public AudioClip popup_open;
    public AudioClip cancel;
    public AudioClip grill;
    public AudioClip scream;
    public AudioClip meow_1;
    public AudioClip meow_2;

    public AudioClip tipnyang;

    public AudioClip nyang_01;
    public AudioClip nyang_02;
    public AudioClip nyang_angry01;
    public AudioClip nyang_angry02;
    public AudioClip nyang_happy01;
    public AudioClip nyang_happy02;

    public AudioClip box_open;
    public AudioClip box_close;
    public AudioClip select_meat;
    public AudioClip select_powder;
    public AudioClip select_sauce;
    public AudioClip purchase;
    public AudioClip apply_item;

    public AudioClip cook_meat;     // 21
    public AudioClip over_meat;     // 6
    public AudioClip trash;

    public AudioClip close;

    public AudioClip push_button;
    public AudioClip tanning_gauge_up;
    public AudioClip tanning_success;
    public AudioClip tanning_fail;
    public AudioClip roulette;

    public AudioClip brokenRoof;
    #endregion

    private AudioSource bgmChannel;
    private AudioSource sandChannel;
    private AudioSource waveChannel;
    private List<AudioSource> audioSources;

    private AudioSource cook_meat_Clip_Channel;
    private AudioSource tanning_gauge_up_Clip_Channel;

    private IEnumerator Coroutine_BGM;
    private int currentBGMOrder;

    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        audioSources = new List<AudioSource>();
        for (int i = 0; i < 15; i++)
            audioSources.Add(this.transform.GetChild(i).GetComponent<AudioSource>());


        bgmChannel = this.transform.GetChild(15).GetComponent<AudioSource>();
        sandChannel = this.transform.GetChild(16).GetComponent<AudioSource>();
        waveChannel = this.transform.GetChild(17).GetComponent<AudioSource>();



        if ((PlayerPrefs.GetInt("NyamNyangOption") == 1049) && PlayerPrefs.GetInt("Option_Sound") != 1)
            SetMute(true);

    }

    // GetPlayableAudioSource: 놀고먹는 채널 찾기.
    private AudioSource GetPlayableAudioSource() {
        return audioSources.Find(source => (!source.isPlaying && source.clip == null));
    }
    // FindClipSource: 해당 클립을 재생하고 있는 채널 찾기.
    public AudioSource FindClipSource(AudioClip clip) {
        return audioSources.Find(source => source.clip == clip);
    }

    // Play: 해당 클립 재생.
    //public AudioSource Play(AudioClip clip) {
    //    AudioSource source = GetPlayableAudioSource();
    //    if (source == null) return null;
    //    source.volume = 1f;
    //    source.clip = clip;
    //    source.Play();
    //    return source;
    //}
    public AudioSource Play(AudioClip clip, float time = 0) {
        AudioSource source = GetPlayableAudioSource();
        if (source == null) return null;
        if (time == 0) time = clip.length;
        source.volume = 1f;
        StartCoroutine(Play(source, clip, time));
        return source;
    }
    private IEnumerator Play(AudioSource source, AudioClip clip, float time) {
        source.clip = clip;
        source.Play();
        string Name = source.gameObject.name;
        source.gameObject.name += ("_" + clip.name);
        yield return new WaitForSeconds(time);
        source.gameObject.name = Name;
        source.Stop();
        source.clip = null;
    }
    private void Stop(AudioSource source) {
        source.Stop();
        source.clip = null;
        source.gameObject.name = "Channel_XX";
    }
    public void PlayBGM(AudioClip clip) {
        if (bgmChannel.clip == clip) return;
        if (Coroutine_BGM != null) StopCoroutine(Coroutine_BGM);
        bgmChannel.Stop();
        bgmChannel.clip = clip;
        bgmChannel.Play();
        bgmChannel.loop = true;
    }
    public void PlayBGM() {
        if (bgmChannel.isPlaying && (bgmChannel.clip == background_1 || bgmChannel.clip == background_2)) return;
        Coroutine_BGM = PlayBGMOrder();
        StartCoroutine(Coroutine_BGM);
        bgmChannel.loop = false;
    }
    private IEnumerator PlayBGMOrder() {
        while (true) {
            bgmChannel.clip = (currentBGMOrder % 2 == 0) ? background_1 : background_2;
            bgmChannel.Play();
            float offset = (currentBGMOrder % 2 == 0) ? 5 : 10;
            yield return new WaitForSeconds(bgmChannel.clip.length - offset);
            currentBGMOrder++;
        }
    }

    public void PlayNyang() {
        Play((Random.Range(0, 2) == 0 ? nyang_01 : nyang_02), 1f);
    }
    public void PlayNyang_Angry() {
        Play((Random.Range(0, 2) == 0 ? nyang_angry01 : nyang_angry02), 1f);
    }
    public void PlayNyang_Happy() {
        Play((Random.Range(0, 2) == 0 ? nyang_happy01 : nyang_happy02), 1f);
    }
    public void PlayGrill() {
        AudioSource source = GetPlayableAudioSource();
        if (source == null) return;
        source.volume = 0.25f;
        StartCoroutine(Play(source, grill, grill.length));
    }
    public void PlayCookMeat() {
        cook_meat_Clip_Channel = Play(cook_meat, TimeManager.instance.cookTime);
        cook_meat_Clip_Channel.volume *= 0.5f;
    }
    public void PauseCookMeat() {
        cook_meat_Clip_Channel?.Pause();
    }
    public void ResumeCookMeat() {
        cook_meat_Clip_Channel?.Play();
    }
    public void StopCookMeat() {
        Stop(cook_meat_Clip_Channel);
    }
    public void PlayTanningGaugeUp() {
        tanning_gauge_up_Clip_Channel = Play(tanning_gauge_up);
    }
    public void StopTanningGaugeUp() {
        tanning_gauge_up_Clip_Channel.Stop();
    }
    // SetMute: 음소거 설정.
    public void SetMute(bool isMute) {
        foreach (AudioSource source in audioSources)
            source.mute = isMute;
        bgmChannel.mute = isMute;
    }
    public void PlaySand() {
        Play((Random.Range(0, 2) == 0 ? background_sand_1 : background_sand_2), 0.5f);
    }
    public void PlayWave() {
        Play((Random.Range(0, 2) == 0 ? background_wave_1 : background_wave_2), 1.5f);
    }

}