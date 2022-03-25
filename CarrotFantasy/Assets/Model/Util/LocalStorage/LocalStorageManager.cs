using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class LocalStorageManager
    {
        private static LocalStorageManager localStorageManager;
        private string account;

        public static LocalStorageManager getInstance()
        {
            if (localStorageManager == null)
            {
                localStorageManager = new LocalStorageManager();
                localStorageManager.account = AccountServer.getInstance().getAccountId();
            }
            return localStorageManager;
        }

        private String getPlayerStorageData(String value)
        {
            return String.Format("{0}_{1}", this.account, value);
        }

        public void setDataToLocal(String key, System.Object value, LocalStorageSaveType valueType)
        {
            if (valueType == LocalStorageSaveType.IntType)
            {
                PlayerPrefs.SetInt(key, (int)value);
            }
            else if (valueType == LocalStorageSaveType.StringType)
            {
                PlayerPrefs.SetString(key, (String)value);
            }
            else if (valueType == LocalStorageSaveType.FloatType)
            {
                PlayerPrefs.SetFloat(key, (float)value);
            }
            else if (valueType == LocalStorageSaveType.BoolType)
            {
                if (value is Boolean) //判断是不是Boolean类型
                {
                    if ((Boolean)value == true)
                    {
                        PlayerPrefs.SetString(key, "true");
                    }
                    else if ((Boolean)value == false)
                    {
                        PlayerPrefs.SetString(key, "false");
                    }
                }

            }
        }

        public T getDataFromLocal<T>(String key, System.Object defaultValue, LocalStorageSaveType valueType)
        {
            if (valueType == LocalStorageSaveType.StringType)
            {
                String value = (String)defaultValue;
                return (T)(System.Object)PlayerPrefs.GetString(key, value);
            }
            else if (valueType == LocalStorageSaveType.IntType)
            {
                int value = (int)defaultValue;
                return (T)(System.Object)PlayerPrefs.GetInt(key, value);
            }
            else if (valueType == LocalStorageSaveType.FloatType)
            {
                float value = (float)defaultValue;
                return (T)(System.Object)PlayerPrefs.GetFloat(key, value);
            }
            else if (valueType == LocalStorageSaveType.BoolType)
            {
                String dfValue;
                if((bool)defaultValue == true)
                {
                    dfValue = "true";
                }
                else
                {
                    dfValue = "false";
                }

                String value = PlayerPrefs.GetString(key, dfValue);
                if (string.Equals(value, "true"))
                {
                    return (T)(System.Object)true;
                }
                else if (string.Equals(value, "false"))
                {
                    return (T)(System.Object)false;
                }
            }
            return default(T);
        }

        public T getPlayerInfo<T>(String key, System.Object defaultValue, LocalStorageSaveType valueType)
        {
            if (key == null || defaultValue == null)
            {
                Debug.Log("本地信息获取失败");
                return default(T);
            }
            return getDataFromLocal<T>(getPlayerStorageData(key), defaultValue, valueType);
        }

        public void setPlayerInfo<T>(String key, System.Object defaultValue, LocalStorageSaveType valueType)
        {
            if (key == null || defaultValue == null)
            {
                Debug.Log("本地信息设置失败");
            }
            this.setDataToLocal(getPlayerStorageData(key), defaultValue, valueType);
        }
    }
}
