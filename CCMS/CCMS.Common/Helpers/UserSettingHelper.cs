using CCMS.Common.Dto;
using System;


namespace CCMS.Common.Helpers
{
    public  class UserSettingHelper
    {
        public static T GetValue<T>(UserSettingDto setting)
        {
            if(string.IsNullOrEmpty(setting.Value))
                return default(T);
            return (T)Convert.ChangeType(setting.Value, typeof(T));
        }

        public static void SetValue<T>(UserSettingDto setting, T value)
        {
            setting.Value = value.ToString();
            setting.ValueType = typeof(T).FullName;
        }
    }
}
