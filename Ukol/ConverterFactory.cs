using System;

namespace Notino.Homework
{
    public enum Format
    {
        JSON,
        XML
    }

    public static class ConverterFactory
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
