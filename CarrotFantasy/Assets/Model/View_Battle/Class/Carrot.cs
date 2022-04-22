using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



/// <summary>
/// 萝卜
/// </summary>

namespace ETModel 
{
    public class Carrot : MonoBehaviour
    {

        //萝卜的不同状态
        private Sprite[] sprites;
        private Animator animator;
        private float timeVal;
        private SpriteRenderer sr;
        private Text hpText;

        private BattleDataComponent dataComponent;

        // Use this for initialization
        public void init()
        {
            sprites = new Sprite[7];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = ResourceLoader.getInstance().loadRes<Sprite>("Pictures/NormalMordel/Game/Carrot/" + i.ToString());
            }
            animator = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
            hpText = transform.Find("HpCanvas/txt_live").GetComponent<Text>();
            this.dataComponent = (BattleDataComponent)GameManager.getInstance().baseBattle.getComponent(BattleComponentType.DataComponent);

            this.dataComponent.eventDispatcher.addListener(BattleEvent.CARROT_LIVE_REDUCE, this.UpdateCarrotUI);
            this.UpdateCarrotUI();
        }

        // Update is called once per frame
        void Update()
        {
            if (timeVal >= 5)
            {
                animator.Play("Idle");
                timeVal = 0;
            }
            else
            {
                timeVal += Time.deltaTime;
            }
        }

        private void OnMouseDown()
        {
            if (dataComponent.carrotLive >= 10)
            {
                animator.Play("Touch");
                int randomNum = UnityEngine.Random.Range(1, 4);
                UIServer.getInstance().audioManager.playEffect(
                    String.Format("NormalMordel/Carrot/{0}",randomNum.ToString()));
            }
        }

        public void UpdateCarrotUI()
        {
            int hp = dataComponent.carrotLive;
            this.hpText.text = hp.ToString();

            if (dataComponent.carrotLive < 10)
            {
                animator.enabled = false;
            }

            if (hp >= 7 && hp < 10)
            {
                sr.sprite = sprites[6];
            }
            else if (hp < 7 && hp > 0)
            {
                sr.sprite = sprites[hp - 1];
            }
        }

        public void dispose()
        {
            this.dataComponent.eventDispatcher.removeListener(BattleEvent.CARROT_LIVE_REDUCE, this.UpdateCarrotUI);
        }
    }
}



