namespace TodoList.Presentation.Routes
{
    public static class TodoRoutes
    {
        public const string BaseRoute = "api/todo";


        public const string GetAll = BaseRoute + "/all";

        public const string GetAllPending = BaseRoute + "/pending";

        public const string Create = BaseRoute;

        public const string Update = BaseRoute + "/{id}";

        public const string Complete = BaseRoute + "/{id}/complete";

        public const string Delete = BaseRoute + "/{id}";
    }


}
