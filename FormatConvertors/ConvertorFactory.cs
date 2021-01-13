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
        public static IFileConvert GetConvertor(Format type)
        {
            switch (type)
            {
                case Format.JSON:
                    return new JSONConvertor();
                case Format.XML:
                    return new XMLConvertor();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
