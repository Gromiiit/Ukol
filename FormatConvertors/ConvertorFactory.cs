using System;

namespace Notino.Homework.FormatConvertors
{
    public enum Format
    {
        JSON,
        XML
    }

    public static class ConvertorFactory
    {
        public static IFileConvert GetConverter(Format type)
        {
            switch (type)
            {
                case Format.JSON:
                    return new JSONConverter();
                case Format.XML:
                    return new XMLConverter();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
