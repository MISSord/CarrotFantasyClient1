using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class MusicInfo
    {
        public String path;
        public int prority;
        public int uid;
        public MusicInfo(String upath, int uprority, int uuid)
        {
            path = upath;
            prority = uprority;
            uid = uuid;
        }
    }


    public class AudioManager
    {
        public GameObject nodeObject;
        private GameObject audio_music;
        private GameObject audio_sound_effect;

        private AudioSource audioSourceMusic;
        private AudioSource audioSourceEffect;

        private float timeCurrent;

        public bool musicEnable { get; private set; }
        public float musicVolume { get; private set; }
        public bool effectEnable { get; private set; }
        public float effectVolume { get; private set; }

        private int uid = 0;

        private const float RESET_PLAYING_EFFECT_INTERVAL = 0.1f;

        Dictionary<int, MusicInfo> uid2MusicInfo = new Dictionary<int, MusicInfo>();
        Dictionary<int, List<MusicInfo>> musicProrityGroupMap = new Dictionary<int, List<MusicInfo>>();
        List<int> orderOfPriority = new List<int>();

        int currentUid = 0;

        public void init()
        {
            this.nodeObject = new GameObject("node_object");

            GameObject audio_music = new GameObject("audio_musice");
            audio_music.transform.SetParent(nodeObject.transform, false);
            this.audioSourceMusic = audio_music.AddComponent<AudioSource>();

            GameObject audio_sound_effect = new GameObject("audio_sound_effect");
            audio_sound_effect.transform.SetParent(nodeObject.transform, false);
            this.audioSourceEffect = audio_sound_effect.AddComponent<AudioSource>();

            this.timeCurrent = Time.realtimeSinceStartup;

            this.audioSourceMusic.loop = true;

            
            this.musicEnable = LocalStorageManager.getInstance().getDataFromLocal<bool>
                (LocalStorageType.CUR_USER_MUSIC_ENABLE, (System.Object)true, LocalStorageSaveType.BoolType);
            this.musicVolume = LocalStorageManager.getInstance().getDataFromLocal<float>
                (LocalStorageType.CUR_USER_MUSIC_VOLUME, (System.Object)1f, LocalStorageSaveType.FloatType);
            this.effectEnable = LocalStorageManager.getInstance().getDataFromLocal<bool>
                (LocalStorageType.CUR_USER_MUSIC_EFFECT_ENABLE, (System.Object)true, LocalStorageSaveType.BoolType);
            this.effectVolume = LocalStorageManager.getInstance().getDataFromLocal<float>
                (LocalStorageType.CUR_USER_MUSIC_EFFECT_VOLUME, (System.Object)1f, LocalStorageSaveType.FloatType);

            this.refreshMusicActiveState();
            this.refreshMusicVolume();
            this.refreshEffectActiveState();
            this.refreshEffectVolume();
            
        }

        public void refreshMusicActiveState()
        {
            this.audioSourceMusic.enabled = this.musicEnable;
        }

        public void refreshMusicVolume()
        {
            this.audioSourceMusic.volume = this.musicVolume;
        }

        public void refreshEffectActiveState()
        {
            this.audioSourceEffect.enabled = this.effectEnable;
        }

        public void refreshEffectVolume()
        {
            this.audioSourceEffect.volume = this.effectVolume;
        }

        public int playMusic(String path, int priority = 1)
        {
            uid = uid + 1;
            MusicInfo musicInfo = new MusicInfo(path, priority, uid);
            this.uid2MusicInfo.Add(uid, musicInfo);
            if(this.musicProrityGroupMap.ContainsKey(priority) == false)
            {
                bool insertSuc = false;
                for(int i = 0; i <= this.orderOfPriority.Count - 1; i++)
                {
                    if(priority > orderOfPriority[i])
                    {
                        orderOfPriority.Add(priority);
                        insertSuc = true;
                        break;
                    }
                }
                if (!insertSuc)
                    orderOfPriority.Add(priority);
                this.musicProrityGroupMap[priority] = new List<MusicInfo>();
            }
            this.musicProrityGroupMap[priority].Add(musicInfo);
            this.checkMusic();
            return uid;
        }

        private void checkMusic()
        {
            for(int i = 0; i <= this.orderOfPriority.Count - 1; i++)
            {
                if(this.musicProrityGroupMap[orderOfPriority[i]].Count > 0)
                {
                    MusicInfo musicInfo = this.musicProrityGroupMap[orderOfPriority[i]][0];
                    if(musicInfo.uid != this.currentUid)
                    {
                        AudioClip clip = ResourceLoader.getInstance().loadRes<AudioClip>(musicInfo.path);
                        this.audioSourceMusic.clip = clip;
                        this.audioSourceMusic.Play();
                    }
                    break;
                }
            }
        }

        public void stopMusic()
        {
            this.audioSourceMusic.Stop();
        }

        public void stopMusic(int uuid)
        {
            MusicInfo musicInfo;
            if(this.uid2MusicInfo.TryGetValue(uuid, out musicInfo))
            {
                this.uid2MusicInfo.Remove(uuid);
                this.musicProrityGroupMap[musicInfo.prority].Remove(musicInfo);
                this.checkMusic();
            }
        }

        public void playEffect(String path, int volumeScale = 1)
        {
            if(this.effectEnable == false) { return; }
            float curTime = Time.realtimeSinceStartup;
            if(this.timeCurrent + RESET_PLAYING_EFFECT_INTERVAL < curTime)
            {
                this.timeCurrent = curTime;
                AudioClip clip = ResourceLoader.getInstance().loadRes<AudioClip>(path);
                this.audioSourceEffect.PlayOneShot(clip, volumeScale);
            }
        }

        public void stopEffectClip()
        {
            this.audioSourceEffect.Stop();
        }

        public bool getMusicEnable()
        {
            return this.musicEnable;
        }

        public bool getEffectEnable()
        {
            return this.effectEnable;
        }

        public float getMusicVolume()
        {
            return this.musicVolume;
        }

        public float getEffectVolue()
        {
            return this.effectVolume;
        }

        public void setMusicVolume(int volume)
        {
            this.musicVolume = (float)Math.Max(0.001, Math.Min(volume, 1));
            this.refreshMusicVolume();
            LocalStorageManager.getInstance().setPlayerInfo<float>(LocalStorageType.CUR_USER_MUSIC_VOLUME, this.musicVolume, LocalStorageSaveType.FloatType);
        }
        public void setEffectVolume(int volume)
        {
            this.musicVolume = (float)Math.Max(0.001, Math.Min(volume, 1));
            this.refreshEffectVolume();
            LocalStorageManager.getInstance().setPlayerInfo<float>(LocalStorageType.CUR_USER_MUSIC_EFFECT_VOLUME, this.effectVolume, LocalStorageSaveType.FloatType);
        }

        public void setMusicEnable(bool isActive)
        {
            if(this.musicEnable != isActive)
            {
                this.musicEnable = isActive;
                if(isActive == true)
                {
                    LocalStorageManager.getInstance().setDataToLocal(LocalStorageType.CUR_USER_MUSIC_ENABLE, (System.Object)true, LocalStorageSaveType.BoolType);
                }
                else
                {
                    LocalStorageManager.getInstance().setDataToLocal(LocalStorageType.CUR_USER_MUSIC_ENABLE, (System.Object)false, LocalStorageSaveType.BoolType);
                }
                
            }
            this.refreshMusicActiveState();
        }

        public void setEffectEnable(bool isActive)
        {
            if (this.effectEnable != isActive)
            {
                this.effectEnable = isActive;
                if (isActive == true)
                {
                    LocalStorageManager.getInstance().setDataToLocal(LocalStorageType.CUR_USER_MUSIC_EFFECT_ENABLE, (System.Object)true, LocalStorageSaveType.BoolType);
                }
                else
                {
                    LocalStorageManager.getInstance().setDataToLocal(LocalStorageType.CUR_USER_MUSIC_EFFECT_ENABLE, (System.Object)false, LocalStorageSaveType.BoolType);
                }
            }
            this.refreshEffectActiveState();
        }

        public void dipose()
        {
            GameObject.Destroy(this.nodeObject);
        }
    }
}
