using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public AudioSource audioSource; // 在Unity中分配你的AudioSource  

    public Action<Item> isFavoriteAction;
    public AudioClip[] clips;

    private int currentClipIndex = 0; // 当前播放音频片段的索引

    public Toggle moreLoopToggle;
    bool isMoreLoop = false;

    List<Item> items = new List<Item>();
    [SerializeField]
    Transform 所有Content;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        isFavoriteAction += IsFavoriteAction;
        audioSource = audioSource != null ? audioSource : GetComponent<AudioSource>();

        if (clips.Length > 0)
        {
            audioSource.clip = clips[currentClipIndex]; // 设置初始音频片段
        }
        moreLoopToggle.onValueChanged.AddListener((val) => {
            MoreLoopToggle(val);
            int it = val ? 999 : 1000;
            PlayerPrefs.SetInt("lastID", it);
        });

        Item[] temp = 所有Content.GetComponentsInChildren<Item>();
        items.AddRange(temp);

        // 读取整数值，如果不存在则返回0
        int score = PlayerPrefs.GetInt("lastID", 0);
        if(score != 0)
        {
            if(score == 999)
            {
                moreLoopToggle.isOn = true;
                MoreLoopToggle(true);
            }else if(score == 1000)
            {
                moreLoopToggle.isOn = false;
                MoreLoopToggle(false);
            }
            else
            {
                Item index = items.Find(x => x.itemID == score);
                if (index != null)
                {
                    PlayAudio(index);
                }
            }
        }
    }

    void MoreLoopToggle(bool val)
    {
        audioSource.loop = !val;
        isMoreLoop = val;
    }

    void IsFavoriteAction(Item item)
    {
        if (item.isFavorite)
        {
            //item
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && isMoreLoop) // 如果当前音频片段播放结束
        {
            currentClipIndex = (currentClipIndex + 1) % clips.Length; // 移动到下一个音频片段，循环列表
            audioSource.clip = clips[currentClipIndex]; // 更新音频片段
            audioSource.Play(); // 播放新的音频片段
        }
    }

    public void PlayAudio(Item it)
    {
        // 播放音频  
        audioSource.clip = it.clip;
        audioSource.Play();
        MoreLoopToggle(false);
        PlayerPrefs.SetInt("lastID", it.itemID);
        PlayerPrefs.Save(); // 确保数据被写入到磁盘
    }
}
