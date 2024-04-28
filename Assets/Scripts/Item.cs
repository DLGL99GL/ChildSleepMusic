using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField]
    Toggle isFavoriteToggle;
    [SerializeField]
    Button playBtn;
    public AudioClip clip;

    public bool isFavorite;

    public int itemID;
    private void Start()
    {
        // 设置按钮点击事件  
        playBtn.onClick.AddListener(PlayAudio);
        isFavoriteToggle.onValueChanged.AddListener(OnFavoriteChanged);
    }

    private void PlayAudio()
    {
        // 播放音频  
        PlayerManager.instance.PlayAudio(this);
    }

    private void OnFavoriteChanged(bool _isFavorite)
    {
        isFavorite = _isFavorite;

        PlayerManager.instance.isFavoriteAction.Invoke(this);
    }
}
