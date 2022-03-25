using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public enum LanguageType
    {
        zh_cn,
        zh_us,
    }

    public class LanguageUtil
    {
        private static LanguageUtil languageUtil;
        private zh_language curLanguageBag;
        private LanguageType curType;

        private Dictionary<UnityEngine.SystemLanguage, zh_language> systemLanguageList = new Dictionary<UnityEngine.SystemLanguage, zh_language>();

        public static LanguageUtil getInstance()
        {
            if(languageUtil == null)
            {
                languageUtil = new LanguageUtil();
                languageUtil.loadLanguageBag();
            }
            return languageUtil;
        }

        private void loadLanguageBag()
        {
            systemLanguageList.Add(UnityEngine.SystemLanguage.Chinese, new zh_cn());
            systemLanguageList.Add(UnityEngine.SystemLanguage.English, new zh_cn());
            //systemLanguageList.Add(UnityEngine.SystemLanguage.ChineseSimplified, new zh_cn());
            //systemLanguageList.Add(UnityEngine.SystemLanguage.ChineseTraditional, new zh_cn());

            UnityEngine.SystemLanguage sys = UnityEngine.Application.systemLanguage;
            if(sys == UnityEngine.SystemLanguage.Chinese) //暂时这样写
            {
                curType = LanguageType.zh_cn;
                curLanguageBag = systemLanguageList[UnityEngine.SystemLanguage.Chinese];
            }
            else
            {
                curType = LanguageType.zh_us;
                curLanguageBag = systemLanguageList[UnityEngine.SystemLanguage.Chinese];
            }
        }

        public LanguageType getCurLanguageType()
        {
            return curType;
        }

        public String getString(int id)
        {
            return curLanguageBag.getString(id);
        }

        public String getFormatString(int id, string one)
        {
            return String.Format(curLanguageBag.getString(id), one);
        }

        public String getFormatString(int id, string one, string two)
        {
            return String.Format(curLanguageBag.getString(id), one, two);
        }
        public String getFormatString(int id, string one, string two, string three)
        {
            return String.Format(curLanguageBag.getString(id), one, two, three);
        }
        public String getFormatString(int id, string[] list)
        {
            return String.Format(curLanguageBag.getString(id), list);
        }
    }
}
