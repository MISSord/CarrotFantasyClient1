using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

namespace ETModel
{
    public class TipUI
    {
        private Text txtShowTip;
        private Image imgBg;
        private GameObject nodeTran;
        private Transform transform;
        private Tween[] mainPanelTween;//0.上，1.下

        private Vector3 defaultPosition;
        private int scheId;

        private int PREFAB_HEIGHT = 200;

        public TipUI(GameObject tran)
        {
            this.nodeTran = tran;
            this.transform = tran.transform;
            this.txtShowTip = this.transform.Find("Text").GetComponent<Text>();
            this.imgBg = this.transform.GetComponent<Image>();

            this.scheId = -1;

            Vector2 curSize = UIUtil.getInstance().currentScreenSize;
            this.defaultPosition = new Vector3(0, -(curSize.y/2 + PREFAB_HEIGHT), 0.5f);
            this.transform.localPosition = this.defaultPosition;

            mainPanelTween = new Tween[2];
            mainPanelTween[0] = transform.DOLocalMoveY(0, 0.5f);
            mainPanelTween[0].SetAutoKill(false);
            mainPanelTween[0].Pause();
            mainPanelTween[1] = transform.DOLocalMoveY(curSize.y/2 + PREFAB_HEIGHT, 0.5f);
            mainPanelTween[1].SetAutoKill(false);
            mainPanelTween[1].Pause();
            mainPanelTween[1].OnComplete(() => { 
                this.transform.localPosition = defaultPosition; 
                this.scheId = -1;
                this.mainPanelTween[1].Restart();
                this.mainPanelTween[1].Pause();
                this.mainPanelTween[0].Restart();
                this.mainPanelTween[0].Pause();
            });
        }

        public void refreshTip(String tip)
        {
            if (this.scheId != -1) return;
            this.txtShowTip.text = tip;
            mainPanelTween[0].Play();
            this.scheId = Sche.delayExeOnceTimes(() => { this.mainPanelTween[1].Play();},2f);
        }

        public void showTip(String tip)
        {
            if (this.scheId != -1) return;
            this.txtShowTip.text = tip;
            mainPanelTween[0].Play();
        }

        public void fadeTip()
        {
            this.mainPanelTween[1].Play();
        }
    }
}
