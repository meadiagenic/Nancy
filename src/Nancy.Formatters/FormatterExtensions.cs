namespace Nancy.Formatters
{
    using Responses;

    public static class FormatterExtensions
    {
        public static Response AsJson<TModel>(this IResponseFormatter formatter, TModel model)
        {
            return new JsonResponse<TModel>(model);
        }

        public static Response AsXml<TModel>(this IResponseFormatter formatter, TModel model)
        {
            return new XmlResponse<TModel>(model);
        }
    }
}