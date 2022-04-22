using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace ETModel
{
    /// <summary>
    /// 道具
    /// </summary>
    public class Item : MonoBehaviour
    {
        public BattleUnitView_Item itemView;

        private void OnMouseDown()
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            this.itemView.refreshTarget();
        }

    }
}


