using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class HelpPanel : BasePanel
    {
        private SlideCanCoverScrollView helpSliderScrollView;
        private SlideCanCoverScrollView towerSliderScrollView;

        private GameObject nodeHelp;
        private GameObject nodeMonster;
        private GameObject nodeTower;

        private Button btnReturn;
        private Button btnHelp;
        private Button btnMonster;
        private Button btnTower;

        private Vector3 fadePosition = new Vector3(0, 3000, 0);
        private Vector3 showPosition = Vector3.zero;

        private int showId = 1;


        public HelpPanel(Dictionary<string, dynamic> param) : base(param)
        {
            this.prefabUrl = "Prefabs/Business/Help/HelpPanel";
        }

        public override void init()
        {
            base.init();
            this.panelManagerUnit.registerOnAssetReady(this.OnAssetReady);
            this.panelManagerUnit.registerOnDestroy(this.OnDestroy);
        }

        protected override void OnAssetReady()
        {
            base.OnAssetReady();
            this.nodeHelp = this.transform.Find("HelpPage").gameObject;
            this.nodeMonster = this.transform.Find("MonsterPage").gameObject;
            this.nodeTower = this.transform.Find("TowerPage").gameObject;

            this.btnReturn = this.transform.Find("node_top/Btn_Return").GetComponent<Button>();
            this.btnHelp = this.transform.Find("node_top/Btn_Help").GetComponent<Button>();
            this.btnMonster = this.transform.Find("node_top/Btn_Monster").GetComponent<Button>();
            this.btnTower = this.transform.Find("node_top/Btn_Tower").GetComponent<Button>();

            //this.helpSliderScrollView = this.nodeHelp.

            this.showId = 1;
            this.addListener();
            this.updateNodePosition();
        }

        private void addListener()
        {
            this.btnReturn.onClick.AddListener(this.finish);
            this.btnHelp.onClick.AddListener(this.showHelpPage);
            this.btnMonster.onClick.AddListener(this.showMonsterPage);
            this.btnTower.onClick.AddListener(this.showTowerPage);
        }

        private void updateNodePosition()
        {
            this.nodeHelp.transform.localPosition = this.showId == 1 ? this.showPosition : this.fadePosition;
            this.nodeMonster.transform.localPosition = this.showId == 2 ? this.showPosition : this.fadePosition;
            this.nodeTower.transform.localPosition = this.showId == 3 ? this.showPosition : this.fadePosition;
        }

        private void showHelpPage()
        {
            this.showId = 1;
            this.updateNodePosition();
        }

        private void showMonsterPage()
        {
            this.showId = 2;
            this.updateNodePosition();
        }

        private void showTowerPage()
        {
            this.showId = 3;
            this.updateNodePosition();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
