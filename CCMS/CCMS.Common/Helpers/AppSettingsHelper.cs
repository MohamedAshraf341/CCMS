using CCMS.Common.Dto;
using System;


namespace CCMS.Common.Helpers
{
    public  class AppSettingHelper
    {
        public static T GetValue<T>(AppSettingDto setting)
        {
            if(string.IsNullOrEmpty(setting.Value))
                return default(T);
            return (T)Convert.ChangeType(setting.Value, typeof(T));
        }

        public static void SetValue<T>(AppSettingDto setting, T value)
        {
            setting.Value = value.ToString();
            setting.ValueType = typeof(T).FullName;
        }
    }
}
